// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Airion.Common;

namespace Airion.Persist.TransientProvider.Internal
{
	/// <summary>
	/// Description of EntityStore.
	/// </summary>
	public class EntityStore
	{		
		private Dictionary<Type, IDictionary> _store = new Dictionary<Type, IDictionary>();
		private Dictionary<Type, IdAdaptor> _entityIdAdaptors = new Dictionary<Type, IdAdaptor>();
		
		public EntityStore()
		{
		}
		
		public Dictionary<object, T> GetSet<T>()
		{
			IDictionary collection;
			if(!_store.TryGetValue(typeof(T), out collection)) {
				collection = new Dictionary<object, T>();
				_store.Add(typeof(T), collection);
			}
			return (Dictionary<object, T>)collection;
		}
		
		public IdAdaptor GetIdAdaptor<T>()
		{
			IdAdaptor adaptor;
			if(!_entityIdAdaptors.TryGetValue(typeof(T), out adaptor)) {
				var propertyInfo = typeof(T).GetInstanceProperty(IdPropertyName);
				Guard.Operation(propertyInfo != null, "The property {0}.{1} doesn't exist.", typeof(T), IdPropertyName);
				
				var idGenerator = IdGeneratorFactory.Build(propertyInfo.PropertyType);
				
				adaptor = new IdAdaptor(propertyInfo, idGenerator);
				_entityIdAdaptors.Add(typeof(T), adaptor);
			}
			return adaptor;
		}
		
		public const string IdPropertyName = "Id";
	}
}
