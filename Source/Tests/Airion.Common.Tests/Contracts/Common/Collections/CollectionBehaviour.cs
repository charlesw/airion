// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Airion.Common.Tests.Contracts.Common.Collections
{
	public abstract class CollectionBehaviour<T>
	{
		#region Helpers
		
		protected abstract T CreateItem(int itemIndex);
		
		protected abstract ICollection<T> CreateCollection();
		
		#endregion
		
		#region Collection Behaviour Specification
		
		[Test]
		public void ShouldBeAbleToAddNonDuplicateItems()
		{
			ICollection<T> collection = CreateCollection();
			
			T item1 = CreateItem(0);
			collection.Add(item1);			
			Assert.That(collection, Has.Member(item1));
			
			T item2 = CreateItem(1);
			collection.Add(item2);
			Assert.That(collection, Has.Member(item2));
		}
		
		[Test]
		public void ShouldBeAbleToIterateOverItems()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			int matches = 0;
			foreach(T item in collection) {
				if(Object.Equals(item, item1)) {
					matches |= 0x01;
				} else if(Object.Equals(item, item2)) {
					matches |= 0x02;
				} else {
					Assert.Fail("Item {0}: Should not exist.", item);
				}
			}
			
			Assert.That(matches & 0x01, Is.EqualTo(0x01));
			Assert.That(matches & 0x02, Is.EqualTo(0x02));
		}
		
		[Test]
		public void ShouldBeAbleToCopyItemsToArray()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			T[] array = new T[2];
			collection.CopyTo(array, 0);
			
			int matches = 0;
			for(int i=0;i<array.Length;++i) {
				T item = array[i];
				if(Object.Equals(item, item1)) {
					matches |= 0x01;
				} else if(Object.Equals(item, item2)) {
					matches |= 0x02;
				} else {
					Assert.Fail("Item {0}: Should not exist.", item);
				}
			}
			
			Assert.That(matches & 0x01, Is.EqualTo(0x01), "Array doesn't contain item 1");
			Assert.That(matches & 0x02, Is.EqualTo(0x02), "Array doesn't contain item 2");
		}
		
		[Test]
		public void ShouldBeAbleToCopyItemsToArrayFromSpecifiedStartLocation()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			T[] array = new T[4];
			collection.CopyTo(array, 2);
			
			int matches = 0;
			for(int i=2;i<array.Length;++i) {
				T item = array[i];
				if(Object.Equals(item, item1)) {
					matches |= 0x01;
				} else if(Object.Equals(item, item2)) {
					matches |= 0x02;
				} else {
					Assert.Fail("Item {0}: Should not exist.", item);
				}
			}
			
			Assert.That(matches & 0x01, Is.EqualTo(0x01), "Array doesn't contain item 1");
			Assert.That(matches & 0x02, Is.EqualTo(0x02), "Array doesn't contain item 2");
		}
		
		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShouldThrowArgumentOutOfRangeExceptionWhenCopyingItemsToArrayAndSpecifiedStartIndexIsLessThanZero()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			T[] array = new T[4];
			collection.CopyTo(array, -1);
		}
		
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentOutOfRangeExceptionWhenCopyingItemsToArrayAndSpecifiedStartIndexIsGreaterThanOrEqualToArraySize()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			T[] array = new T[4];
			collection.CopyTo(array, 4);
		}
		
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentExceptionWhenCopyingItemsToArrayAndSpecifiedArrayIsTooSmall()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			T[] array = new T[1];
			collection.CopyTo(array, 0);
		}
		
		[Test]
		public void ShouldBeAbleDetermineIfItemExistsInCollection()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			Assert.That(collection.Contains(item1), Is.True);
			Assert.That(collection.Contains(item2), Is.True);
		}
		
				
		[Test]
		public void ShouldBeAbleToRemoveItem()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			Assert.That(collection.Remove(item1), Is.True);
			Assert.That(collection.Contains(item1), Is.False);
			Assert.That(collection.Contains(item2), Is.True);
			
			Assert.That(collection.Remove(item2), Is.True);
			Assert.That(collection.Contains(item1), Is.False);
			Assert.That(collection.Contains(item2), Is.False);
		}
				
		[Test]
		public void ShouldBeAbleToClearCollection()
		{
			T item1, item2;
			ICollection<T> collection = CreateCollection(out item1, out item2);
			
			Assert.That(collection.Count, Is.EqualTo(2));
			
			collection.Clear();
			
			Assert.That(collection.Count, Is.EqualTo(0));			
		}		
		
		[Test]
		public void ShouldBeAbleToDetermineSizeOfCollection()
		{
			ICollection<T> collection = CreateCollection();
			
			Assert.That(collection.Count, Is.EqualTo(0));
			
			T item1 = CreateItem(0);
			collection.Add(item1);			
			Assert.That(collection.Count, Is.EqualTo(1));
			
			T item2 = CreateItem(1);
			collection.Add(item2);
			Assert.That(collection.Count, Is.EqualTo(2));
			
			collection.Remove(item1);
			Assert.That(collection.Count, Is.EqualTo(1));	
			
			collection.Clear();
			Assert.That(collection.Count, Is.EqualTo(0));	
		}
		
		protected ICollection<T> CreateCollection(out T item1, out T item2)
		{
			ICollection<T> collection = CreateCollection();
			
			item1 = CreateItem(0);
			collection.Add(item1);			
			Assert.That(collection, Has.Member(item1));
			
			item2 = CreateItem(1);
			collection.Add(item2);
			Assert.That(collection, Has.Member(item2));
			
			return collection;
		}
		
		#endregion
	}
}
