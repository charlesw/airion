// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using System.Threading.Tasks;
using Airion.Common;
using Airion.Parallels.Internal;

namespace Airion.Parallels.Extensions
{
	/// <summary>
	/// Description of PendingScheduledWorkExtensions.
	/// </summary>
	public static class PendingScheduledWorkExtensions
	{
		public static ITaskHandle SendTask(this IPendingWorkCollection<IScheduledTask> pendingWork, IWorkItem task)
		{
			return SendTask(pendingWork, task, CancellationToken.None);
		}
		
		public static ITaskHandle SendTask(this IPendingWorkCollection<IScheduledTask> pendingWork, IWorkItem task, CancellationToken cancellationToken)
		{
			Guard.RequireNotNull("task", task);
						
			var scheduledTask = new ScheduledWorkItemTask(task, cancellationToken);
			pendingWork.Send(scheduledTask);
			return scheduledTask;
		}
		
		public static ITaskHandle SendAction(this IPendingWorkCollection<IScheduledTask> pendingWork, Action action)
		{
			return SendAction(pendingWork, action, CancellationToken.None);
		}
		
		public static ITaskHandle SendAction(this IPendingWorkCollection<IScheduledTask> pendingWork, Action action, CancellationToken cancellationToken)
		{
			Guard.RequireNotNull("action", action);
			
			var scheduledTask = new ScheduledActionTask(action, cancellationToken);
			pendingWork.Send(scheduledTask);
			return scheduledTask;
		}
		
		public static ITaskHandle<TResult> SendFunction<TResult>(this IPendingWorkCollection<IScheduledTask> pendingWork, Func<TResult> function)
		{
			return SendFunction(pendingWork, function, CancellationToken.None);
		}
		
		public static ITaskHandle<TResult> SendFunction<TResult>(this IPendingWorkCollection<IScheduledTask> pendingWork, Func<TResult> function, CancellationToken cancellationToken)
		{
			Guard.RequireNotNull("function", function); 
			
			var scheduledTask = new ScheduledFunctionTask<TResult>(function, cancellationToken);
			pendingWork.Send(scheduledTask);
			return scheduledTask;
		}
		
	}
}
