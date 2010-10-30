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
	public class TestWorkItem : IWorkItem, IWorkItemCallback
	{		
		Action _action;
		Action _callbackAction;
		
		public TestWorkItem(Action action, Action callbackAction)
		{
			_action = action;
			_callbackAction = callbackAction;
		}
		
		public void Execute()
		{
			_action();
		}
		
		public void NotifyCallback()
		{
			if(_callbackAction != null) {
				_callbackAction();
			}
		}
	}
	
	[TestFixture]
	public class ScheduledWorkItemTaskSpec : ScheduledTaskSpec
	{
		protected override IScheduledTask CreateScheduledTask(Action action, Action callbackAction, CancellationToken cancellationToken)
		{
			var testWorkItem = new TestWorkItem(action, callbackAction);
			return new ScheduledWorkItemTask(testWorkItem, cancellationToken);
		}
	}
}
