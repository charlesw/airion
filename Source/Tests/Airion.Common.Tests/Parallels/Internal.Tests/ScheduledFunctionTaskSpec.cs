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
	public class ScheduledFunctionTaskSpec : ScheduledTaskSpec<int>
	{		
		protected override IScheduledTask<int> CreateScheduledTask(Func<int> func, Action callbackAction, CancellationToken cancellationToken)
		{
			return new ScheduledFunctionTask<int>(func, callbackAction, cancellationToken);
		}
		
		protected override int ExpectedResult {
			get {
				return 255;
			}
		}
	}
}
