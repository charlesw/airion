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
		public ScheduledFunctionTask(Func<TResult> function, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			Function = function;
		}
		
		protected override void ExecuteTask()
		{
			Result = Function();
		}
		
		public Func<TResult> Function { get; private set; }
	}
}
