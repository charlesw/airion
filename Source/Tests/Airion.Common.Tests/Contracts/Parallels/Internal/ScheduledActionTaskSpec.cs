// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using System.Threading;
using Airion.Parallels.Internal;
using NUnit.Framework;

namespace Airion.Common.Tests.Contracts.Parallels.Internal
{
	[TestFixture]
	public class ScheduledActionTaskSpec : ScheduledTaskSpec
	{
		protected override IScheduledTask CreateScheduledTask(Action action, Action callbackAction, CancellationToken cancellationToken)
		{
			return new ScheduledActionTask(action, callbackAction, cancellationToken);
		}		
	}
}
