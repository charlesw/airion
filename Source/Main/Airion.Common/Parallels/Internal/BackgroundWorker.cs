// Author: Charles Weld
// Date: 14/03/2010 1:37 PM

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Airion.Parallels.Extensions;
using Airion.Common;

namespace Airion.Parallels.Internal
{
	public sealed class BackgroundWorker : IBackgroundWorker
	{
		private readonly IPendingWorkCollection<IScheduledTask> _pendingWorkCollection;
		private Thread _workerThread;
		private ApartmentState _apartmentState;
		private volatile bool _isDisposing;
		private volatile bool _isDisposed;
		private volatile bool _isStarted;
		private CancellationTokenSource _cancellationTokenSource;
		
		public delegate BackgroundWorker Factory(IPendingWorkCollection<IScheduledTask> pendingWorkCollection, ApartmentState apartmentState);
		
		public BackgroundWorker(IPendingWorkCollection<IScheduledTask> pendingWorkCollection, ApartmentState apartmentState)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_pendingWorkCollection = pendingWorkCollection;
			_apartmentState = apartmentState;
			_isDisposing = false;
			_isDisposed = false;
			_isStarted = false;
		}
		
		~BackgroundWorker()
		{
			Debug.Fail(String.Format("The BackgroundWorker \"{0}\" was not disposed off.", this));
		}
		
		public void Start()
		{
			Guard.Operation(!_isStarted, "The Background Worker has already been started.");
			_workerThread = new Thread(ExecutePendingTasks);
			_workerThread.SetApartmentState(_apartmentState);
			_workerThread.Name = String.Format("BackgroundWorkerThread[{0}]", Guid.NewGuid());
			_workerThread.Start();
			_isStarted = true;
		}
		
		/// <summary>
		/// Releases all reasource used by the <see cref="BackgroundWorker"/>.
		/// </summary>
		public void Dispose()
		{
			_isDisposing = true;
			
			// cancel any pending operations
			_cancellationTokenSource.Cancel();
				
			// Ensure worker thread has finished.
			_workerThread.Join();
			
			// cleanup
			Dispose(true);
			
			_isDisposed = true;
			_isDisposing = false;
			GC.SuppressFinalize(this);
		}
		
		private void ExecutePendingTasks()
		{
			var cancellationToken = _cancellationTokenSource.Token;
			try {
				while(!_isDisposing) {
					IScheduledTask task = _pendingWorkCollection.Retrieve(cancellationToken);
					task.Execute();
				}
			} catch(OperationCanceledException) {
			}
		}
		
		private void Dispose(bool disposing)
		{
			if(disposing) {				
			}
		}
		
		public bool IsDisposed {
			get { return _isDisposed; }
		}
		
		public bool IsDisposing {
			get { return _isDisposing; }
		}
		
		public bool IsStarted {
			get { return _isStarted; }
		}
		
		public IPendingWorkCollection<IScheduledTask> PendingWork
		{
			get { return _pendingWorkCollection; }
		}
	}
}
