// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using Airion.Common.Collections;
using Airion.Common.Tests.Support.Examples;
using NUnit.Framework;

namespace Airion.Common.Tests.Contracts.Common.Collections
{
	[TestFixture]
	public class MapBehaviour : CollectionBehaviour<Person>
	{		
		#region Map Tests
		
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ShouldNotAllowTheAdditionOfItemsWithSameKey()
		{
			IMap<string, Person> map = CreateMap();
			
			map.Add(CreateItem(0));
			map.Add(CreateItem(0));			
		}				
		
		[Test]
		public void ShouldBeAbleToRetreiveItemByKey()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Assert.That(person, Is.EqualTo(map[person.Name]));
		}
		
		[Test, ExpectedException(typeof(KeyNotFoundException))]
		public void ShouldThrowArgumentExceptionOnItemRetrevalIfNotExists()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Person nonExistingPerson = map[String.Empty];
		}
		
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowArgumentNullExceptionOnItemRetrevalIfKeyIsNull()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Person nonExistingPerson = map[null];
		}
		
		[Test]
		public void ShouldBeAbleToRemoveItemByKey()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Assert.That(map, Has.Member(person));
			
			map.Remove(person.Name);
			
			Assert.That(map, Has.No.Member(person));
		}
		
		[Test]
		public void ShouldBeAbleToRetreiveItemByKeyIfExists()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Person retreivedPerson;
			Assert.That(map.TryGetItem(person.Name, out retreivedPerson), Is.True);
			Assert.That(retreivedPerson, Is.EqualTo(person));
			
			Assert.That(map.TryGetItem("Non existant person", out retreivedPerson), Is.False);
			Assert.That(retreivedPerson, Is.Null);
		}
		
		[Test]
		public void ShouldBeAbleToTestIfItemExistsByKey()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			Assert.That(map.ContainsKey("Sam Brimmer"), Is.True);
			Assert.That(map.ContainsKey("John Brimmer"), Is.False);
		}
		
		#endregion
				
		#region DynamicMapping Tests
		
		[Test]
		public void ChangeKey_EntityExistsInMap_MapIsUpdatedWithNewKey()
		{
			Person person;
			IMap<string, Person> map = CreateMap(out person);
			
			person.Name = "JoeBlogs";
			
			Assert.That(person, Is.EqualTo(map[person.Name]));
		}
		
		#endregion

		#region Helpers
		
		protected override Person CreateItem(int itemIndex)
		{
			Person person = new Person();
			if(itemIndex == 0) {
				person.Name = "Sam Brimmer";
			} else if(itemIndex == 1) {
				person.Name = "Tim Brimmer";
			} else if(itemIndex == 2) {
				person.Name = "Jenny Brimmer";
			} else {
				throw new ArgumentOutOfRangeException("itemIndex", itemIndex, "itemIndex must be between 0 and 2 (inclusive).");
			}
			return person;
		}
		
		protected override System.Collections.Generic.ICollection<Person> CreateCollection()
		{
			return new Map<string, Person>(x => x.Name);
		}
				
		protected IMap<string, Person> CreateMap(out Person person)
		{
			IMap<string, Person> map = CreateMap();
			
			person = new Person();
			person.Name = "Sam Brimmer";
			person.Address = "10 Fake rd, some where.";
			person.DateOfBirth = new DateTime(1900, 10, 12);				
			
			map.Add(person);
			
			return map;
		}
		
		protected IMap<string, Person> CreateMap(out Person person1, out Person person2)
		{
			IMap<string, Person> map = CreateMap();
			
			person1 = new Person();
			person1.Name = "Sam Brimmer";
			person1.Address = "10 Fake rd, some where.";
			person1.DateOfBirth = new DateTime(1900, 10, 12);	
			map.Add(person1);			
			
			person2 = new Person();
			person2.Name = "John Brimmer";
			person2.Address = "10 Fake rd, some where.";
			person2.DateOfBirth = new DateTime(1900, 10, 12);	
			map.Add(person2);
			
			return map;
		}
		
		protected virtual IMap<string, Person> CreateMap()
		{
			return new Map<string, Person>(x => x.Name);
		}
		
		#endregion
	}
		
	
}
