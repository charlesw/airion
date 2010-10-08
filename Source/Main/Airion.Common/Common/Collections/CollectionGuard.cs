// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public static class CollectionGuard
	{
		static CollectionGuard()
		{
			CannotModifyCollectionMessage = "The \"{0}\" collection is read-only and cannot be modified.";
			ArrayTooSmallMessage = "The array is too small to hold all the collection's items.";
			KeyNotFoundMessage = "The key was not found in the collection.";
		}
		
		public static void ModifiedReadonlyCollection(Type type)
		{
			throw new NotSupportedException(String.Format(CannotModifyCollectionMessage, type.Name));
		}
		
		public static void RequireArrayCanFitCollection<T>(string argumentName, ICollection<T> collection, T[] array, int arrayIndex)
		{
			if(array.Length - arrayIndex < collection.Count)
				throw new ArgumentException(ArrayTooSmallMessage, argumentName);
		}
		
		public static void KeyNotFound()
		{
			throw new KeyNotFoundException(KeyNotFoundMessage);
		}
		
		private static readonly string CannotModifyCollectionMessage;
		private static readonly string ArrayTooSmallMessage;	
		private static readonly string KeyNotFoundMessage;
	}
}
