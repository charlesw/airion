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
using Airion.Persist.CQRS.Tests.Support;

namespace Airion.Persist.CQRS.Tests.Contracts
{
	[TestFixture]
	public class CommandExecutorTests
	{
		#region Steps
		
		public class Steps : AbstractSteps
		{	
			
			#region Data
			
			private CommandExecutor _commandBus;
			
			// step data
			private TestCommandHandler _commandHandler;
			private TestCommand _command;
			private Exception _commandExecutionException;
			
			
			#endregion
			
			#region Lifecylce
			
			protected override void BeforeScenario()
			{
				_commandHandler = new TestCommandHandler();
				_commandBus = new CommandExecutor(new TestCommandHandler[] { _commandHandler });
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
		public void ExecuteCommand_VerificationPasses_CommandIsExecuted()
		{
			using(var steps = new Steps()) {
				steps.ExecuteCommand();
				steps.VerifyCommandWasExecuted();
			}
		}
		
		[Test]
		public void ExecuteCommand_VerificationFails_CommandIsNotExecuted()
		{
			using(var steps = new Steps()) {
				steps.ExecuteInvalidCommand();
				steps.VerifyCommandWasNotExecuted();
				steps.VerifyValidationExeceptionWasThrown();
			}
		}
		
		[Test]
		public void Construct_MultipleCommandHandlersForSameCommand_ThrowArgumentException()
		{
			// The command bus doesn't currently support multiple command handlers for the same command, make sure that it fails.
			var commandHandlers = new ICommandHandler[] {
				new TestCommandHandler(), 
				new TestCommandHandler()
			};
			
			Assert.Throws<ArgumentException>(() => new CommandExecutor(commandHandlers));
		}
		
		#endregion
	}
}
