// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using Airion.Parallels;
using Airion.Persist;
using Airion.Persist.CQRS.Tests.Support.Mappings;
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
			
			private class TestCommand : ICommand
			{
				public int ExecuteCount { get; private set; }
				public bool IsValid { get; set; }
				
				public TestCommand()
				{
					ExecuteCount = 0;
					IsValid = true;
				}
				
				public void Execute(CommandContext commandContext)
				{
					if(Verify(commandContext)) {					
						ExecuteCount++;
					}
				}
				
				private bool Verify(CommandContext commandContext)
				{
					if(!IsValid) {
						commandContext.AddError("Validation failed");
					}
					
					return !commandContext.HasError;
				}
			}
					
			#endregion
			
			#region Data
			
			private Store _store;
			private CommandBus _commandBus;
			
			// step data
			private ITaskHandle _commandHandle;
			private TestCommand _command;
			private Exception _commandExecutionException;
			
			
			#endregion
			
			#region Lifecylce
			
			protected override void BeforeScenario()
			{
				var configuration = new NHibernateConfiguration(provider => new TransientSessionAndTransactionManager(provider))
					.Database(SQLiteConfiguration.Standard.InMemory())
					.Mappings(x => x.FluentMappings
					          .AddFromAssemblyOf<RecipeMap>());
				_store = new Store(configuration);
				_commandBus = new CommandBus(_store, appartmentState => new TaskWorker(appartmentState));
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
				_command = new TestCommand();
				_commandHandle = _commandBus.Schedule(_command);
			}
			
			public void ScheduleInvalidCommand()
			{
				_command = new TestCommand() { IsValid = false };
				_commandHandle = _commandBus.Schedule(_command);				
			}
			
			public void WaitForCommand()
			{
				try {
					_commandHandle.Wait();
				} catch(System.Reflection.TargetInvocationException e) {
					_commandExecutionException = e.InnerException;
				}
			}
			
			
			#endregion
			
			#region Verification
			
			public void VerifyCommandWasExecuted()
			{
				Assert.That(_command.ExecuteCount, Is.EqualTo(1));
			}
			
			public void VerifyCommandWasNotExecuted()
			{
				Assert.That(_command.ExecuteCount, Is.EqualTo(0));
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
				steps.ScheduleCommand();
				steps.WaitForCommand();
				steps.VerifyCommandWasExecuted();
			}
		}
		
		[Test]
		public void ScheduleCommand_VerificationFails_CommandIsNotExecuted()
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
