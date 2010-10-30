// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;

namespace Airion.Parallels.Internal
{
	public sealed class ScheduledWorkItemTask : BaseScheduledTask
	{
		public ScheduledWorkItemTask(IWorkItem task, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			Task = task;
		}
		
		protected override void ExecuteTask()
		{
			Task.Execute();
		}
		
		protected override void WaitCompleted()
		{
			var callbackTask = Task as IWorkItemCallback;
			if(callbackTask != null) {
				callbackTask.NotifyCallback();
			}
		}
		
		public IWorkItem Task { get; private set; }
	}
}
