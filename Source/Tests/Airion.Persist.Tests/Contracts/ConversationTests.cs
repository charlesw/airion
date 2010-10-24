// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using Airion.Common;
using Airion.Persist.Provider;
using Airion.Persist.Tests.Support.Domain;
using MbUnit.Framework;

namespace Airion.Persist.Tests.Contracts
{
	[TestFixture, Parallelizable(TestScope.All)]
	public abstract class ConversationTests
	{
		public class Steps : LightDisposableBase
		{
			private Store store;
			private IConversation conversation;
			private IQueryable<Person> personQuery;
			private Person retrievedPerson;
			
			public Steps(Store store)
			{
				this.store = store;
			}
			
			protected override void Dispose(bool disposing)
			{
				if(disposing) {
					if(conversation != null) {
						conversation.Dispose();
						conversation = null;
					}
				}
				base.Dispose(disposing);
			}
			
			public void BeginConversation()
			{
				conversation  = store.BeginConversation();
			}
			
			public void AddPeopleToStore(IEnumerable<Person> people)
			{
				foreach(var person in people) {
					conversation.Save(person);
				}
			}
			
			public void UpdatePerson(Person person)
			{
				conversation.Update(person);
			}
			
			public void SavePerson(Person person)
			{
				conversation.Save(person);
			}
			
			public void SaveOrUpdatePerson(Person person)
			{
				conversation.SaveOrUpdate(person);
			}
			
			public void IssuePersonQuery()
			{
				personQuery = conversation.Linq<Person>();
			}
			
			public void IssueGetPerson(Guid id)
			{
				retrievedPerson = conversation.Get<Person>(id);
			}
			
			public void DeletePerson(Person person)
			{
				conversation.Delete(person);
			}
			
			public void VerifyPersonQuery(IEnumerable<Person> expectedResult)
			{
				Assert.IsInstanceOfType<IQueryable<Person>>(personQuery);
				Assert.AreElementsEqualIgnoringOrder(expectedResult, personQuery);
			}

			public void VerifyRetrievedPerson(Person expectedPerson)
			{
				Assert.AreEqual(expectedPerson, retrievedPerson);
			}
			
			public void VerifyRetrievedPersonIsNull()
			{
				Assert.IsNull(retrievedPerson);
			}
			
			public void VerifyIdIsAssigned(Person person)
			{
				Guid unassignedId = Guid.Empty;
				Assert.AreNotEqual(unassignedId, person.Id);
			}
			
			public void VerifyPersonExists(Person person)
			{
				var persistedPerson = conversation.Get<Person>(person.Id);
				Assert.AreEqual(person, persistedPerson);
			}
			
			public void VerifyPersonDoesntExist(Person person)
			{
				var persistedPerson = conversation.Get<Person>(person.Id);
				Assert.IsNull(persistedPerson);
			}
		}
		
		protected abstract IConfiguration BuildConfiguration();
		
		protected virtual void OnStoreCreate(Store store)
		{
			
		}
		
		[Test]
		[Row(0)]
		[Row(10)]
		[NUnit.Framework.Test]
		[NUnit.Framework.TestCase(0)]
		[NUnit.Framework.TestCase(10)]
		public void Linq_ReturnsQueryableSetOfEntities(int count)
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var people = Enumerable.Range(1, count).Select<int, Person>(CreatePerson).ToList();
					
					// Given
					steps.BeginConversation();
					steps.AddPeopleToStore(people);
					
					// When
					steps.IssuePersonQuery();
					
					// Then
					steps.VerifyPersonQuery(people);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Update_EntityExists_EntityIsUpdated()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					steps.AddPeopleToStore(new Person[] { person });
					
					// Act
					steps.UpdatePerson(person);
					
					// Assert
					steps.VerifyPersonExists(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Update_EntityDoesntExists_ThrowInvalidOperationException()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					
					// Act
					Assert.Throws<InvalidOperationException>(() => steps.UpdatePerson(person));
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Save_EntityExists_ThrowInvalidOperationException()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					steps.AddPeopleToStore(new Person[] { person });
					
					// Act
					Assert.Throws<InvalidOperationException>(() => steps.SavePerson(person));
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Save_EntityDoesntExists_EntityIsAddedAndIdIsAssigned()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					
					// Act
					steps.SavePerson(person);
					
					// Assert
					steps.VerifyIdIsAssigned(person);
					steps.VerifyPersonExists(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void SaveOrUpdate_EntityExists_EntityIsUpdated()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					steps.AddPeopleToStore(new Person[] { person });
					
					// Act
					steps.SaveOrUpdatePerson(person);
					
					// Assert
					steps.VerifyPersonExists(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void SaveOrUpdate_EntityDoesntExists_EntityIsAddedAndIdIsAssigned()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					
					// Act
					steps.SaveOrUpdatePerson(person);
					
					// Assert
					steps.VerifyIdIsAssigned(person);
					steps.VerifyPersonExists(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Get_EntityExists_ReturnsCorrespondingEntity()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					steps.SavePerson(person);
					
					// Act
					steps.IssueGetPerson(person.Id);
					
					// Assert
					steps.VerifyRetrievedPerson(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Get_EntityDoesntExist_ReturnsNull()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					// Arrange
					steps.BeginConversation();
					
					// Act
					steps.IssueGetPerson(Guid.NewGuid());
					
					// Assert
					steps.VerifyRetrievedPersonIsNull();
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Delete_EntityExists_EntityIsRemoved()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					steps.SavePerson(person);
					
					// Act
					steps.DeletePerson(person);
					
					// Assert
					steps.VerifyPersonDoesntExist(person);
				}
			}
		}
		
		[Test]
		[NUnit.Framework.Test]
		public void Delete_EntityDoesntExists_NoChange()
		{
			using(var store = new Store(BuildConfiguration())) {
				OnStoreCreate(store);
				
				using(var steps = new Steps(store)) {
					var person = CreatePerson(1);
					
					// Arrange
					steps.BeginConversation();
					
					// Act
					steps.DeletePerson(person);
				}
			}
		}
		
		public static Person CreatePerson(int number)
		{
			return new Person() {
				Name = "Person" + number,
				Address = "Address" + number
			};
		}
		
	}
}
