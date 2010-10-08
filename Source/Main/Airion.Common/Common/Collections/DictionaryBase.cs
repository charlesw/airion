// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public abstract class DictionaryBase<TKey, TValue> :
		IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, ICollection, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		#region Nested Classes (2) 

		private class KeyCollection : ReadonlyCollectionBase<TKey>
		{
		#region Nested Classes (1) 




			
			public class KeyCollectionEnumerator : IEnumerator<TKey>
			{
		#region Constructors (1) 

				public KeyCollectionEnumerator(DictionaryBase<TKey, TValue> dictionary)
				{
					_dictionaryEnumerator = dictionary.GetEnumerator();
				}

		#endregion Constructors 

		#region Methods (3) 

		// Public Methods (3) 

				public void Dispose()
				{
					_dictionaryEnumerator.Dispose();
				}

				public bool MoveNext()
				{
					return _dictionaryEnumerator.MoveNext();
				}

				public void Reset()
				{
					_dictionaryEnumerator.Reset();
				}

		#endregion Methods 

		#region Properties (2) 

				public TKey Current {
					get {
						return _dictionaryEnumerator.Current.Key;
					}
				}

				object System.Collections.IEnumerator.Current {
					get {
						return Current;
					}
				}

		#endregion Properties 

		#region Fields (1) 

				private IEnumerator<KeyValuePair<TKey, TValue>> _dictionaryEnumerator;

		#endregion Fields 
			}
		#endregion Nested Classes 

		#region Constructors (1) 

			public KeyCollection(DictionaryBase<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
			}

		#endregion Constructors 

		#region Methods (1) 

		// Public Methods (1) 

			public override IEnumerator<TKey> GetEnumerator()
			{
				return new KeyCollectionEnumerator(_dictionary);
			}

		#endregion Methods 

		#region Properties (1) 

			public override int Count {
				get {
					return _dictionary.Count;
				}
			}

		#endregion Properties 

		#region Fields (1) 

			private DictionaryBase<TKey, TValue> _dictionary;

		#endregion Fields 
		}
		private class ValueCollection : ReadonlyCollectionBase<TValue>
		{
		#region Nested Classes (1) 


			
			public class ValueCollectionEnumerator : IEnumerator<TValue>
			{
		#region Constructors (1) 

				public ValueCollectionEnumerator(DictionaryBase<TKey, TValue> dictionary)
				{
					_dictionaryEnumerator = dictionary.GetEnumerator();
				}

		#endregion Constructors 

		#region Methods (3) 

		// Public Methods (3) 

				public void Dispose()
				{
					_dictionaryEnumerator.Dispose();
				}

				public bool MoveNext()
				{
					return _dictionaryEnumerator.MoveNext();
				}

				public void Reset()
				{
					_dictionaryEnumerator.Reset();
				}

		#endregion Methods 

		#region Properties (2) 

				public TValue Current {
					get {
						return _dictionaryEnumerator.Current.Value;
					}
				}

				object System.Collections.IEnumerator.Current {
					get {
						return Current;
					}
				}

		#endregion Properties 

		#region Fields (1) 

				private IEnumerator<KeyValuePair<TKey, TValue>> _dictionaryEnumerator;

		#endregion Fields 
			}
		#endregion Nested Classes 

		#region Constructors (1) 

			public ValueCollection(DictionaryBase<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
			}

		#endregion Constructors 

		#region Methods (1) 

		// Public Methods (1) 

			public override IEnumerator<TValue> GetEnumerator()
			{
				return new ValueCollectionEnumerator(_dictionary);
			}

		#endregion Methods 

		#region Properties (1) 

			public override int Count {
				get {
					return _dictionary.Count;
				}
			}

		#endregion Properties 

		#region Fields (1) 

			private DictionaryBase<TKey, TValue> _dictionary;

		#endregion Fields 
		}
		#endregion Nested Classes 

		#region Constructors (1) 

		public DictionaryBase()
		{
			_keyCollection = new KeyCollection(this);
			_valueCollection = new ValueCollection(this);
		}

		#endregion Constructors 

		#region Methods (12) 

		// Public Methods (6) 

		/// <inheritdoc />
		public abstract void Add(TKey key, TValue value);

		/// <inheritdoc />
		public abstract void Clear();

		/// <inheritdoc />
		public virtual bool ContainsKey(TKey key)
		{
			TValue value;
			return TryGetValue(key, out value);
		}

		/// <inheritdoc />
		public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

		/// <inheritdoc />
		public abstract bool Remove(TKey key);

		/// <inheritdoc />
		public abstract bool TryGetValue(TKey key, out TValue value);
		
		// Private Methods (6)

		void ICollection.CopyTo(Array array, int index)
		{
			var collection = (ICollection<KeyValuePair<TKey, TValue>>)this;
			collection.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}
				
		/// <inheritdoc />
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}
		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ContainsKey(item.Key);
		}
		/// <inheritdoc />
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			Guard.RequireNotNull("array", array);
			Guard.RequirePositiveInteger("arrayIndex", arrayIndex);
			CollectionGuard.RequireArrayCanFitCollection("array", this, array, arrayIndex);
			
			foreach(var current in this) {
				array[arrayIndex] = current;
				arrayIndex++;
			}
		}
		
		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item.Key);
		}

		/// <inheritdoc />
		System.Collections.IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		#endregion Methods 

		#region Properties (10) 

		/// <inheritdoc />
		public abstract int Count {
			get;
		}
		
		/// <inheritdoc />
		public ICollection<TKey> Keys {
			get {
				return _keyCollection;
			}
		}

		/// <inheritdoc />
		public abstract TValue this[TKey key] {
			get;
			set;
		}

		/// <inheritdoc />
		public ICollection<TValue> Values {
			get { return _valueCollection; }
		}
		
		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return false; }
		}
		
		bool ICollection.IsSynchronized {
			get {
				return false;
			}
		}

		object ICollection.SyncRoot {
			get { 
				return _syncRoot;
			}
		}

		#endregion Properties 

		#region Fields (3) 

		private readonly KeyCollection _keyCollection;
		private object _syncRoot = new object();
		private readonly ValueCollection _valueCollection;

		#endregion Fields 
		
	}
}
