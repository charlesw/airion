//// <file>
////     <copyright see="prj:///doc/copyright.txt"/>
////     <license see="prj:///doc/license.txt"/>
////     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
//// </file>
//
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Threading;
//using Airion.Parallels.Extensions;
//
//namespace Airion.Parallels.Internal
//{
//	/// <summary>
//	/// Description of MultiBackgroundWorker.
//	/// </summary>
//	public class MultiBackgroundWorker : IBackgroundWorker
//	{
//		private readonly IPendingWorkCollection<IScheduledTask> _pendingWorkCollection;
//		private readonly List<IBackgroundWorker> _backgroundWorkers = new List<IBackgroundWorker>();
//		private readonly Guid _id;
//		private Thread workerThread;
//		private volatile bool _isDisposing;
//		private volatile bool _isDisposed;
//		private volatile bool _isStarted;
//		
//		public MultiBackgroundWorker(Guid id, IPendingWorkCollection<IScheduledTask> pendingWorkCollection)
//		{
//			_pendingWorkCollection = pendingWorkCollection;
//			_id = id;
//			_isDisposing = false;
//			_isDisposed = false;
//			_isStarted = false;
//		}
//		
//		~MultiBackgroundWorker()
//		{
//			Debug.Fail(String.Format("The BackgroundWorker \"{0}\" was not disposed off.", this));
//		}
//		
//		public void Start()
//		{
//			Guard.Operation(!_isStarted, "The BackgroundWorker has already been started.");
//
//			_workerThread = new Thread(ExecuteWork);
//			_workerThread.Name = String.Format("WorkerSchedulerThread[{0}]", _id);
//			_workerThread.Start();
//			
//			_isStarted = true;
//		}
//		
//		private void ExecuteWork()
//		{
//			while(!_isDisposing) {
//				_pendingWorkCollection.Wait();
//			}
//		}
//		
//		/// <summary>
//		/// Releases all reasource used by the <see cref="BackgroundWorker"/>.
//		/// </summary>
//		public void Dispose()
//		{
//			_isDisposing = true;
//			
//			
//			_isDisposed = true;
//			_isDisposing = false;
//			GC.SuppressFinalize(this);
//		}
//	
//		public bool IsDisposed {
//			get { return _isDisposed; }
//		}
//		
//		public bool IsDisposing {
//			get { return _isDisposing; }
//		}
//		
//		public bool IsStarted {
//			get { return _isStarted; }
//		}
//		
//		public IPendingWorkCollection<IScheduledTask> PendingWork
//		{
//			get { return _pendingWorkCollection; }
//		}
//		
//		public Guid Id {
//			get { return _id; }
//		}
//		
//		public int WorkerCount {
//			get { return 0; }
//		}
//	}
//}
//