// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Airion.Parallels.Internal
{
	public sealed class ScheduledFunctionTask<TResult> : BaseScheduledTask<TResult>
	{
		public ScheduledFunctionTask(Func<TResult> function, Action callbackAction, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			Function = function;
			CallbackAction = callbackAction;
		}
		
		protected override void ExecuteTask()
		{
			Result = Function();
		}
		
		protected override void WaitCompleted()
		{
			if(CallbackAction != null) {
				CallbackAction();
			}
		}
		
		public Func<TResult> Function { get; private set; }
		public Action CallbackAction { get; private set; }
	}
}
