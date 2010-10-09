// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Persist.Provider;
using Airion.Persist.Tests.Support;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Moq;

namespace Airion.Persist.Tests.Steps
{
    [Binding]
    public class StoreSteps
    {
    	private Store store;
    	private IConversation orignialConversation;
    	private IConversation conversation;
    	private Exception beginConversationException;
    	
    	[BeforeScenario]
    	public void BeforeScenario()
    	{
    		var config = new FakeConfiguration();
    		store = new Store(config);
    	}
    	
    	[AfterScenario]
    	public void AfterScenario()
    	{
    		if(orignialConversation != null) {
    			orignialConversation.Dispose();
    			orignialConversation = null;
    		}
    		
    		if(conversation != null) {
    			conversation.Dispose();
    			conversation = null;
    		}
    		
    		if(store != null) {
	    		store.Dispose();
	    		store = null;
    		}
    		
    		beginConversationException = null;
    	}
    	
       	[Given(@"I have already started a conversation")]
        public void GivenIHaveAlreadyStartedAConversation()
        {
        	orignialConversation  = store.BeginConversation();
        }

        [When(@"I begin a conversation")]
        public void WhenIBeginAConversation()
        {
        	try {
        		conversation = store.BeginConversation();
        	} catch (Exception e) {
        		beginConversationException = e;
        	}
        }
        
        [When(@"I end the conversation")]
        public void WhenIEndTheConversation()
        {
        	orignialConversation.Dispose();
        }
        
        [Then(@"the current conversation should be the conversation I just began")]
        public void ThenTheCurrentConversationShouldBeTheConversationIJustBegan()
        {
        	Assert.That(conversation, Is.SameAs(store.CurrentConversation));
        }
        
        [Then(@"throw InvalidOperationException")]
        public void ThenThrowInvalidOperationException()
        {
        	Assert.IsInstanceOf<InvalidOperationException>(beginConversationException);
        }
        
        [Then(@"the current conversation should be null")]
        public void ThenTheCurrentConversationShouldBeNull()
        {
        	Assert.That(store.CurrentConversation, Is.Null);
        }
    }
}
