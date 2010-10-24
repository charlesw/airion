// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using Airion.Common;
using Airion.Parallels.Extensions;
using Airion.Parallels.Internal;

namespace Airion.Parallels
{
	/// <summary>
	/// Description of TaskWorker.
	/// </summary>
	public class TaskWorker : LightDisposableBase
	{
		public delegate TaskWorker Factory(ApartmentState apartmentState);
		
		private IPendingWorkCollection<IScheduledTask> _pendingWorkCollection;
		private BackgroundWorker _backgroundWorker;
		
		public TaskWorker(ApartmentState apartmentState)
			: this(apartmentState,
			       new PendingWorkQueue<IScheduledTask>(),
			       (pendingWorkCollection, workerApartmentState) => new BackgroundWorker(pendingWorkCollection, workerApartmentState))
		{
		}
		
		public TaskWorker(ApartmentState apartmentState, IPendingWorkCollection<IScheduledTask> pendingWorkCollection, BackgroundWorker.Factory backgroundWorkerfactory)
		{
			_pendingWorkCollection = pendingWorkCollection;
			_backgroundWorker = backgroundWorkerfactory(pendingWorkCollection, apartmentState);
		}
		
		public void Start()
		{
			_backgroundWorker.Start();
		}
		
		public ITaskHandle ExecuteAction(Action action)
		{
			return _pendingWorkCollection.SendAction(action, CancellationToken.None);
		}
		
		public ITaskHandle ExecuteAction(Action action, CancellationToken cancellationToken)
		{
			return _pendingWorkCollection.SendAction(action, cancellationToken);
		}
		
		public ITaskHandle<TResult> ExecuteFunction<TResult>(Func<TResult> function)
		{
			return _pendingWorkCollection.SendFunction(function, CancellationToken.None);
		}
		
		public ITaskHandle<TResult> ExecuteFunction<TResult>(Func<TResult> function, CancellationToken cancellationToken)
		{
			return _pendingWorkCollection.SendFunction(function, cancellationToken);
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				_backgroundWorker.Dispose();
			}
			base.Dispose(disposing);
		}
		
	}
}
