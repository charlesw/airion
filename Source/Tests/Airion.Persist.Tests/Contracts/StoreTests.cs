// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Tests.Support;
using MbUnit.Framework;

namespace Airion.Persist.Tests.Contracts
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class StoreTests
	{
		public class StoreTestSteps : LightDisposableBase
		{
			private Store store;
			private IConversation conversation;
			
			public StoreTestSteps()
			{
				var config = new FakeConfiguration();
				store = new Store(config);
			}
			
			protected override void Dispose(bool disposing)
			{			
				if(disposing) {					
					if(conversation != null) {
						conversation.Dispose();
						conversation = null;
					}
					
					if(store != null) {
						store.Dispose();
						store = null;
					}
				}
				base.Dispose(disposing);
			}
			
			public void GivenIHaveAlreadyStartedAConversation()
			{
				conversation  = store.BeginConversation();
			}

			public void WhenIBeginAConversation()
			{
				conversation = store.BeginConversation();
			}
			
			public void WhenIEndTheConversation()
			{
				conversation.Dispose();
			}
			
			public void ThenTheCurrentConversationShouldBeTheConversationIJustBegan()
			{
				Assert.AreSame(conversation, store.CurrentConversation);
			}
			
			public void ThenTheCurrentConversationShouldBeNull()
			{
				Assert.IsNull(store.CurrentConversation);
			}
		}
		
		[Test]
		public void BeginConversation_NoActiveConversation_CurrentConversationIsConversationJustBegan()
		{
			using(var storeSteps = new StoreTestSteps()) {
				storeSteps.WhenIBeginAConversation();
				storeSteps.ThenTheCurrentConversationShouldBeTheConversationIJustBegan();
			}
		}
		
		[Test]
		public void BeginConversation_ActiveConversation_ThrowsInvalidOperationException()
		{
			using(var storeSteps = new StoreTestSteps()) {
				storeSteps.GivenIHaveAlreadyStartedAConversation();
				Assert.Throws<InvalidOperationException>(storeSteps.WhenIBeginAConversation);
			}
		}
		
		[Test]
		public void EndConversation_ActiveConversation_CurrentConversationIsCleared() 
		{
			using(var storeSteps = new StoreTestSteps()) {
				storeSteps.GivenIHaveAlreadyStartedAConversation();
				storeSteps.WhenIEndTheConversation();
				storeSteps.ThenTheCurrentConversationShouldBeNull();
			}
		}
		
		
		
	}
}
