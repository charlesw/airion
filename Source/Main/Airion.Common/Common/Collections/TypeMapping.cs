// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;
using System.Collections.Generic;

namespace Airion.Common.Collections
{
	public class TypeMapping<T> : DictionaryBase<Type, T>
	{
		private Dictionary<Type, T> typeMapping = new Dictionary<Type, T>();
		
		public TypeMapping()
		{
		}
		
		public override int Count {
			get {
				return typeMapping.Count;
			}
		}
		
		public override void Add(Type key, T value)
		{
			typeMapping.Add(key, value);
		}
		
		public override T this[Type key]
		{
			get {
				Guard.RequireNotNull("key", key);
				T result;
				if(!TryGetValue(key, out result)) {
					CollectionGuard.KeyNotFound();
				}
				return result;
			}
			set {
				typeMapping[key] = value;
			}
		}
		
		public override void Clear()
		{
			typeMapping.Clear();
		}
		
		public override bool Remove(Type key)
		{
			return typeMapping.Remove(key);
		}
		
		public override bool TryGetValue(Type key, out T value)
		{
			if(!typeMapping.TryGetValue(key, out value)) {
				// search entire mapping for generic definitions, base classes and finally interfaces
				
				bool foundMatch = false;
				if(key.IsGenericType) {
					Type genericTypeDefinition = key.GetGenericTypeDefinition();
					if(typeMapping.TryGetValue(genericTypeDefinition, out value)) {
						// matched to generic type mapping
						foundMatch = true;
					}
				}
				
				if(!foundMatch) {
					// search base classes
					Type baseType = key.BaseType;
					while(baseType != null) {
						if(typeMapping.TryGetValue(baseType, out value)) {
							foundMatch = true;
							break;
						}
						
						baseType = baseType.BaseType;
					}
				}
				
				if(!foundMatch) {
					// search interface classes
					foreach(Type interfaceType in key.GetInterfaces()) {
						if(typeMapping.TryGetValue(interfaceType, out value)) {
							foundMatch = true;
							break;
						}
					}
				}
				
				if(foundMatch) {
					// a match was found update the mapping.
					typeMapping[key] = value;
					return true;
				} else {
					value = default(T);
					return false;
				}
			}
			return true;
		}
		
		public override IEnumerator<KeyValuePair<Type, T>> GetEnumerator()
		{
			return typeMapping.GetEnumerator();
		}
		
	}
}
