// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using Airion.Testing;
using NUnit.Framework;
using Moq;
using Airion.Parallels;

namespace Airion.Persist.CQRS.Tests.Contracts
{
	[TestFixture]
	public class CommandBusTests
	{
		#region Steps
		
		public class Steps : AbstractSteps
		{
			#region Data
			
			private Store _store;
			private CommandBus _commandBus;
			
			// step data
			private IWorkItem _commandWorkItem;
			private bool _commandExecuted = false;
			private Exception _commandExecutionException;
			
			#endregion
			
			#region Lifecylce
			
			protected override void BeforeScenario()
			{
				_store = new Store(new TransientConfiguration());
				_commandBus = new CommandBus(_store);
			}
			
			protected override void AfterScenario()
			{
				_commandBus.Dispose();
				_store.Dispose();
			}
			
			#endregion
			
			#region Commands
			
			public void ScheduleCommand()
			{
				
			}
			
			#endregion
			
			#region Verification
			#endregion
			
			#region Helpers
			
			private ICommand CreateMockCommand(bool isValid)
			{
				Mock<ICommand> mockCommand = new Mock<ICommand>();
				
//					.Callback(context => { this._commandExecuted = true; });
//				mockCommand.Setup(x => x.Verify)
//					.Callback(
//						context => {
//							if(!isValid) {
//								//context.x
//							}
//					});
			
				return mockCommand.Object;
			}
			
			#endregion
		}
		
		#endregion
		
		#region Tests
		
		[Test]
		public void ScheduleCommand_CommandIsExecuted()
		{
			using(var steps = new Steps()) {
				steps.ScheduleCommand();
				steps.WaitForCommand();
				steps.VerifyCommandWasExecuted();
			}
		}
		
		[Test]
		public void ScheduleCommand_VerificationFails_ReturnsAsyncHandleToCommandResult()
		{
			using(var steps = new Steps()) {
				steps.ScheduleInvalidCommand();
				steps.WaitForCommand();
				steps.VerifyCommandWasNotExecuted();
				steps.VerifyValidationExeceptionWasThrown();
			}
		}
		
		#endregion
	}
}
