
Feature: Store - End conversation
	
Scenario: End existing conversation
		Given I have already started a conversation
		When I end the conversation
		Then the current conversation should be null