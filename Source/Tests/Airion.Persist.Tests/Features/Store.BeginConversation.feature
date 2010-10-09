Feature: Store - Begin conversation
	
Scenario: No existing conversation
	When I begin a conversation
	Then the current conversation should be the conversation I just began
	
Scenario: An existing conversation is active
	Given I have already started a conversation
	When I begin a conversation 
	Then throw InvalidOperationException
