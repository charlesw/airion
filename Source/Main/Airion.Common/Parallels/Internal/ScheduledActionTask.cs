// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;

namespace Airion.Parallels.Internal
{
	public class ScheduledActionTask : BaseScheduledTask
	{
		public ScheduledActionTask(Action action, Action callbackAction, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			Action = action;
			CallbackAction = callbackAction;
		}
		
		protected override void ExecuteTask()
		{
			Action();
		}
		
		protected override void WaitCompleted()
		{
			if(CallbackAction != null) {
				CallbackAction();
			}
		}
		
		public Action Action { get; private set; }
		public Action CallbackAction { get; private set; }
	}
}
