// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using System.Threading;
using NUnit.Framework;

namespace Airion.Parallels.Internal.Tests
{
	[TestFixture]
	public class ScheduledActionTaskSpec : ScheduledTaskSpec
	{
		protected override IScheduledTask CreateScheduledTask(Action action, CancellationToken cancellationToken)
		{
			return new ScheduledActionTask(action, cancellationToken);
		}		
	}
}
