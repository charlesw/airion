// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public abstract class ListCollectionWrapperBase : IList
	{	
		
		#region Fields
		
		protected int age = 0;
		protected TriState isReadonly = TriState.NotAssigned;
		protected TriState isFixedSize = TriState.NotAssigned;
		protected int count = 0;
		
		#endregion
		
		#region Constructors
		
		public ListCollectionWrapperBase()
		{
		}
		
		#endregion
		
		#region IList
		
		public object this[int index]
		{
			get {
				CheckIndex(index);
				int relativeIndex;
				IList currentList = ListAt(index, out relativeIndex);
				return currentList[relativeIndex];
			}
			set {
				age++;
				
				CheckIndex(index);
				int relativeIndex;
				IList currentList = ListAt(index, out relativeIndex);
				currentList[relativeIndex] = value;
			}
		}
		
		public virtual bool IsReadOnly {
			get {
				if(isReadonly == TriState.NotAssigned) {
					// readonly if any inner list is readonly or if there are no inner list
					bool hasReadonlyList = false;
					bool hasInnerList = false;
					foreach(IList innerList in GetWrappedLists()) {
						hasInnerList = true;
						if(innerList.IsReadOnly) {
							hasReadonlyList = true;
							break;
						}
					}
					
					isReadonly = (hasInnerList && !hasReadonlyList) ? TriState.False : TriState.True;
				}
				return isReadonly == TriState.True;
			}
		}
		
		public virtual bool IsFixedSize {
			get {
				if(isFixedSize == TriState.NotAssigned) {
					// readonly if any inner list is readonly or if there are no inner list
					bool hasFixedSizeList = false;
					bool hasInnerList = false;
					foreach(IList innerList in GetWrappedLists()) {
						hasInnerList = true;
						if(innerList.IsFixedSize) {
							hasFixedSizeList = true;
							break;
						}
					}
					
					isFixedSize = (hasInnerList && !hasFixedSizeList) ? TriState.False : TriState.True;
				}
				return isFixedSize == TriState.True;
			}
		}
		
		public virtual int Count {
			get {
				if(count == -1) {
					count = 0;
					foreach(IList innerList in GetWrappedLists()) {
						count += innerList.Count;
					}
				}
				return count;
			}
		}
		
		public object SyncRoot {
			get {
				throw new NotSupportedException("Synchronization not supported.");
			}
		}
		
		public bool IsSynchronized {
			get {
				throw new NotSupportedException("Synchronization not supported.");
			}
		}
		
		public int Add(object value)
		{
			CheckModifiablity(true);
			int offset;
			IList list;
			if(GetLastWrappedList(out list, out offset)) {
				age++;
				return offset + list.Add(value);
			} else {
				throw new NotSupportedException("Doesn't contain any internal lists.");
			}
		}
		
		public bool Contains(object value)
		{
			return IndexOf(value) != -1;
		}
		
		public virtual void Clear()
		{
			CheckModifiablity(true);
			
			int ageBefore = age;
			foreach(IList list in GetWrappedLists()) {
				list.Clear();
			}
			count = 0;
			CheckAge(ageBefore);
			age++;
		}
		
		public int IndexOf(object value)
		{
			int ageBefore = age;
			IList list;
			int index;
			if(FindList(value, out list, out index)) {
				CheckAge(ageBefore);
			   	return index;
			}
			return -1;
		}
		
		public void Insert(int index, object value)
		{
			CheckIndex(index);
			CheckModifiablity(true);
			
			int ageBefore = age;
			int relativeIndex;
			IList list;
			if(ListAt(index, out list, out relativeIndex)) {
				list.Insert(relativeIndex, value);
				CheckAge(ageBefore);
				age++;
			} else {
				throw new InvalidOperationException("Collection modified asynchronously.");
			}
		}
		
		public void Remove(object value)
		{
			CheckModifiablity(true);
			
			int indexOf = IndexOf(value);
			if(indexOf != -1) {
				RemoveAt(indexOf);
			}
		}
		
		public void RemoveAt(int index)
		{
			CheckModifiablity(true);
			CheckIndex(index);
			
			int ageBefore = age;
			int relativeIndex;
			IList list;
			if(ListAt(index, out list, out relativeIndex)) {
				list.RemoveAt(relativeIndex);
				CheckAge(ageBefore);
				age++;
			} else {
				throw new InvalidOperationException("Collection modified asynchronously.");
			}
		}
		
		public void CopyTo(Array array, int index)
		{
			// check conditions
			if(array == null) throw new ArgumentNullException("array");
			if(index < 0 || index >= array.Length) throw new ArgumentOutOfRangeException("index", index, String.Format("Index must be between 0 and the array's length ({1}).", array.Length));
			if(array.Rank != 1) throw new ArgumentException("Array must not be multi-dimensional.", "array");
			if(Count > (array.Length - index)) throw new ArgumentException("The specified array must be large enough to fit in all items.");
			
			int ageBefore = age;
			
			foreach(object item in this) {
				array.SetValue(item, index++);
			}
			CheckAge(ageBefore);
		}
		
		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}
		
		#endregion
		
		#region Underlying list access routines
						
		/// <summary>
		/// Forces the the list collection wrapper to reset it's internal trackers (i.e. resynchronize with the list).
		/// </summary>
		public void Refresh()
		{
			isReadonly = TriState.NotAssigned;
			isFixedSize = TriState.NotAssigned;
			count = -1;
			age++;
		}
		
		protected IList ListAt(int index)
		{
			int relativeIndex;
			return ListAt(index, out relativeIndex);
		}
		
		protected virtual IList ListAt(int index, out int relativeIndex)
		{			
			if(index >= 0) {
				int offset = 0;
				foreach(IList innerList in GetWrappedLists()) {
					if(index < offset + innerList.Count) {
						relativeIndex = index - offset;
						return innerList;
					}
				}
				throw new ArgumentOutOfRangeException("index", index, String.Format("Index must be between 0 and {0}.", Count));
			} else {
				throw new ArgumentOutOfRangeException("index", index, String.Format("Index must be between 0 and {0}.", Count));
			}
		}
		
		protected virtual bool ListAt(int index, out IList list, out int relativeIndex)
		{			
			list = null;
			relativeIndex = -1;
			if(index >= 0) {
				int offset = 0;
				foreach(IList innerList in GetWrappedLists()) {
					if(index < offset + innerList.Count) {
						list = innerList;
						relativeIndex = index - offset;
						return true;
					}
				}
				return false;
			} else {
				return false;
			}
		}		
		
		protected virtual bool FindList(object value, out IList list, out int index)
		{
			list = null;
			index = -1;
			
			int offset = 0;
			foreach(IList innerList in GetWrappedLists()) {
				int relativeIndex = innerList.IndexOf(value);
				if(relativeIndex != -1) {
					index = offset + relativeIndex;
					list = innerList;
					return true;
				}
				index += innerList.Count;
			}	
			
			
			return false;			
		}
		
		protected virtual bool GetLastWrappedList(out IList list, out int offset)
		{
			list = null;
			offset = -1;
			foreach(IList innerList in GetWrappedLists()) {
				list = innerList;	
				offset += innerList.Count;
			}
			
			if(list != null) {
				offset -= list.Count;
				return true;
			} else {
				return false;
			}			 
		}
		
		protected abstract IEnumerable<IList> GetWrappedLists();
				
		protected void CheckModifiablity(bool changingSize) {
			if(IsReadOnly) {
				throw new NotSupportedException("Cannot modify a readonly collection.");
			}
			
			if(changingSize && IsFixedSize) {
				throw new NotSupportedException("Cannot change a fixed size collection's size.");
			}
		}
		
		protected void CheckIndex(int index)
		{
			if(index < 0 || index >= Count) 
				throw new ArgumentOutOfRangeException("index", index, String.Format("Index must be between 0 and {0}.", Count));
		}
		
		protected void CheckAge(int ageBefore)
		{
			if(ageBefore != age) {
				throw new InvalidOperationException("Collection was modified in a unsychronized manner.");
			}
		}
		
		#endregion
		
		#region Inner Classes
		
		protected enum TriState
		{
			True,
			False,
			NotAssigned
		}
		
		public class Enumerator : IEnumerator
		{
			
			#region Fields
			
			private ListCollectionWrapperBase listCollection;
			private int index;
			private int startAge;
			
			#endregion
			
			#region Constructors			
			
			public Enumerator(ListCollectionWrapperBase listCollection)
			{
				this.listCollection = listCollection;
				this.index = -1;
				this.startAge = listCollection.age;
			}
			
			#endregion
			
			#region Members
			
			public object Current {
				get {
					listCollection.CheckIndex(index);
					listCollection.CheckAge(startAge);
					
					int relativeIndex;
					IList list;
					if(listCollection.ListAt(index, out list, out relativeIndex)) {
						return list[relativeIndex];
					} else {
						throw new InvalidOperationException("Collection was modified.");
					}
				}
			}
			
			public bool MoveNext()
			{
				listCollection.CheckAge(startAge);
				if(index <= listCollection.Count) {
					index++;
				}
				return index >= 0 && index < listCollection.Count;
			}
			
			public void Reset()
			{
				listCollection.CheckAge(startAge);
				index = -1;
			}
			
			#endregion
			
		}
		
		#endregion
	}
	
}
