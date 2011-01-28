// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using Airion.Parallels;
using Airion.Persist;
using Airion.Persist.Internal;
using Airion.Persist.Provider;
using Airion.Testing;
using FluentNHibernate.Cfg.Db;
using Moq;
using NUnit.Framework;

namespace Airion.Persist.CQRS.Tests.Contracts
{
	[TestFixture]
	public class CommandBusTests
	{
		#region Steps
		
		public class Steps : AbstractSteps
		{			
			#region TestCommand
			
			private class TestCommand 
			{
				public TestCommand()
				{
					IsValid = true;
				}
				public bool IsValid { get; set; }
			}
			
			private class TestCommandHandler : AbstractCommandHandler<TestCommand>
			{
				public TestCommandHandler()
				{
					ExecutionCount = 0;
				}
				
				public int ExecutionCount { get; set; }
			
												
				protected override void Verify(CommandContext<TestCommand> commandContext)
				{					
					var command = commandContext.Command;
					if(!command.IsValid) {
						commandContext.AddError("IsValid", "The command is not valid.");
					}
				}
				
				
				protected override void HandleInternal(CommandContext<TestCommand> commandContext)
				{
					ExecutionCount++;					
				}				
			}
					
			#endregion
			
			#region Data
			
			private CommandBus _commandBus;
			
			// step data
			private TestCommandHandler _commandHandler;
			private TestCommand _command;
			private Exception _commandExecutionException;
			
			
			#endregion
			
			#region Lifecylce
			
			protected override void BeforeScenario()
			{
				_commandHandler = new TestCommandHandler();
				_commandBus = new CommandBus(new TestCommandHandler[] { _commandHandler });
			}
			
			protected override void AfterScenario()
			{
			}
			
			#endregion
			
			#region Commands
			
			public void ExecuteCommand()
			{
				try {
					_command = new TestCommand();
					_commandBus.Execute(_command);
				} catch (Exception e) {
					_commandExecutionException = e;
				}
			}
			
			public void ExecuteInvalidCommand()
			{
				try {
					_command = new TestCommand() { IsValid = false };
					_commandBus.Execute(_command);		
				} catch (Exception e) {
					_commandExecutionException = e;
				}
			}
			
			
			#endregion
			
			#region Verification
			
			public void VerifyCommandWasExecuted()
			{
				Assert.That(_commandHandler.ExecutionCount, Is.EqualTo(1));
			}
			
			public void VerifyCommandWasNotExecuted()
			{
				Assert.That(_commandHandler.ExecutionCount, Is.EqualTo(0));
			}
			
			public void VerifyValidationExeceptionWasThrown()
			{
				Assert.That(_commandExecutionException, Is.InstanceOf<CommandValidationException>());
			}
			
			#endregion
		}
		
		#endregion
		
		#region Tests
		
		[Test]
		public void ScheduleCommand_VerificationPasses_CommandIsExecuted()
		{
			using(var steps = new Steps()) {
				steps.ExecuteCommand();
				steps.VerifyCommandWasExecuted();
			}
		}
		
		[Test]
		public void ScheduleCommand_VerificationFails_CommandIsNotExecuted()
		{
			using(var steps = new Steps()) {
				steps.ExecuteInvalidCommand();
				steps.VerifyCommandWasNotExecuted();
				steps.VerifyValidationExeceptionWasThrown();
			}
		}
		
		#endregion
	}
}
