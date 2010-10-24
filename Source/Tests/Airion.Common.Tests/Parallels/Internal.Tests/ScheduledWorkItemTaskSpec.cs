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
	public class TestWorkItem : IWorkItem
	{		
		Action _action;
		
		public TestWorkItem(Action action)
		{
			_action = action;
		}
		
		public void Execute()
		{
			_action();
		}
		
	}
	
	[TestFixture]
	public class ScheduledWorkItemTaskSpec : ScheduledTaskSpec
	{
		protected override IScheduledTask CreateScheduledTask(Action action, CancellationToken cancellationToken)
		{
			var testWorkItem = new TestWorkItem(action);
			return new ScheduledWorkItemTask(testWorkItem, cancellationToken);
		}
	}
}
