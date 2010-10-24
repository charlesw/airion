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
		public ScheduledActionTask(Action action, CancellationToken cancellationToken)
			: base(cancellationToken)
		{
			Action = action;
		}
		
		protected override void ExecuteTask()
		{
			Action();
		}
		
		public Action Action { get; private set; }
	}
}
