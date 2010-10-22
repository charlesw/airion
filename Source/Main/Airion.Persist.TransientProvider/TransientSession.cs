// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Airion.Common;
using Airion.Persist.Provider;
using Airion.Persist.TransientProvider.Internal;

namespace Airion.Persist.TransientProvider
{
	/// <summary>
	/// Description of TransientSession.
	/// </summary>
	public class TransientSession : LightDisposableBase, ISession
	{		
		private EntityStore _entityStore = new EntityStore();
		public IPersistenceProvider PersistenceProvider { get; private set; }
		
		public TransientSession(IPersistenceProvider persistenceProvider)
		{
			PersistenceProvider = persistenceProvider;
		}
		
		public ITransaction BeginTransaction()
		{
			return new TransientTransaction();
		}
		
		public T Get<T>(object id)
		{
			var entitySet = _entityStore.GetSet<T>();
			T entity;
			if(!entitySet.TryGetValue(id, out entity)) {
				entity = default(T);
			}
			return entity;
		}
		
		public IQueryable<T> Linq<T>()
		{			
			return _entityStore.GetSet<T>().Values.ToList().AsQueryable();
		}
		
		public void Update<T>(T entity)
		{
			var idAdaptor = _entityStore.GetIdAdaptor<T>();
			var entitySet = _entityStore.GetSet<T>();
			
			var entityId = idAdaptor.GetId(entity);
			Guard.Operation(entitySet.ContainsKey(entityId), "No entity with the id {0} of type {1} exists in the stored set.", entityId, typeof(T).Name);
			entitySet[entityId] = entity;
		}
		
		public void Save<T>(T entity)
		{
			var idAdaptor = _entityStore.GetIdAdaptor<T>();
			var entitySet = _entityStore.GetSet<T>();
			
			var entityId = idAdaptor.GetId(entity);
			if(!idAdaptor.IsIdAssigned(entity)) {
				entityId = idAdaptor.IdGenerator.NextId();
				idAdaptor.SetId(entity, entityId);
			} else {	
				Guard.Operation(!entitySet.ContainsKey(entityId), "An entity with the id {0} of type {1} already exists in the stored set.", entityId, typeof(T).Name);
			}
			entitySet[entityId] = entity;			
		}
		
		public void SaveOrUpdate<T>(T entity)
		{
			var idAdaptor = _entityStore.GetIdAdaptor<T>();
			var entitySet = _entityStore.GetSet<T>();
			
			var entityId = idAdaptor.GetId(entity);		
			if(!idAdaptor.IsIdAssigned(entity)) {
				entityId = idAdaptor.IdGenerator.NextId();
				idAdaptor.SetId(entity, entityId);
			}
			entitySet[entityId] = entity;
		}
		
		public void Delete<T>(T entity)
		{
			var idAdaptor = _entityStore.GetIdAdaptor<T>();
			var entitySet = _entityStore.GetSet<T>();
			
			var entityId = idAdaptor.GetId(entity);
			entitySet.Remove(entityId);
		}
		
		public void Flush()
		{
		}
		
		public FlushMode FlushMode {
			get; set;
		}
	}
}
