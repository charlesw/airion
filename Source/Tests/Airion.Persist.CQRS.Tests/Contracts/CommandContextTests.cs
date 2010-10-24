// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using System.Linq;
using Airion.Persist.TransientProvider;
using NUnit.Framework;
using Airion.Testing;

namespace Airion.Persist.CQRS.Tests.Contracts
{
	[TestFixture]
	public class CommandContextTests
	{
		#region Steps
		
		public class Steps : AbstractSteps 
		{
			#region Data
			
			private Store _store;
			private IConversation _conversation;
			private CommandContext _context;
			
			#endregion
			
			#region Lifecylce
			
			protected override void BeforeScenario()
			{
				_store = new Store(new TransientConfiguration());
				_conversation = _store.BeginConversation();
				_context = new CommandContext(_conversation);
			}
			
			protected override void AfterScenario()
			{
				_conversation.Dispose();
				_store.Dispose();
			}
			
			#endregion
			
			#region Commands
						
			public void AddError(string errorMessage)
			{
				_context.AddError(errorMessage);
			}
			
			public void AddError(string propertyName, string errorMessage)
			{
				_context.AddError(propertyName, errorMessage);
			}
			
			#endregion
			
			#region Verification
			
			public void VerifyErrorExists(string errorMessage)
			{
				var errorExists = _context.Errors.Any(x => x.Message == errorMessage);
				
				Assert.That(errorExists, Is.True);
			}
			
			public void VerifyErrorExists(string propertyName, string errorMessage)
			{
				var errorExists = _context.Errors.Any(x =>  x.PropertyName == propertyName && x.Message == errorMessage);
				
				Assert.That(errorExists, Is.True);
			}
			
			public void VerifyHasError()
			{
				Assert.That(_context.HasError, Is.True);
			}
			
			#endregion
		}
			
		#endregion
		
		#region Tests
		
		[Test]
		public void AddError_ErrorIsAddedToErrorCollectionAndHasErrorIsTrue()
		{
			using(var steps = new Steps()) {
				steps.AddError("Test");
				
				steps.VerifyErrorExists("Test");
				steps.VerifyHasError();
			}
		}
		
		[Test]
		public void AddPropertyError_ErrorIsAddedToErrorCollectionAndHasErrorIsTrue()
		{
			using(var steps = new Steps()) {
				steps.AddError("Test Property", "Test");
				
				steps.VerifyErrorExists("Test Property", "Test");
				steps.VerifyHasError();
			}
		}
		
		#endregion
	}
}
