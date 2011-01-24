// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Linq;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;
using NHibernate.Linq;

namespace Airion.Persist.Internal
{
	/// <summary>
	/// Description of Conversation.
	/// </summary>
	public class Conversation : DisposableBase, IConversation
	{
		private ISessionAndTransactionManager _sessionAndTransactionManager;
		
		public Conversation(IPersistenceProvider provider, CreateSessionAndTransactionManager createManager)
		{
			_sessionAndTransactionManager = createManager(provider);
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_sessionAndTransactionManager != null) {
					_sessionAndTransactionManager.Dispose();
					_sessionAndTransactionManager = null;
				}
			}
			base.Dispose(disposing);
		}
		
		public T Get<T>(object id)
		{
			return GetSession().Get<T>(id);
		}
		
		public IQueryable<T> Linq<T>()
		{
			return GetSession().Query<T>();
		}
		
		public void Update<T>(T entity)
		{
			GetSession().Update(entity);
		}
		
		public void Save<T>(T entity)
		{
			GetSession().Save(entity);
		}
		
		public void SaveOrUpdate<T>(T entity)
		{
			GetSession().SaveOrUpdate(entity);
		}
		
		public void Delete<T>(T entity)
		{
			GetSession().Delete(entity);
		}
		
		public ISession GetSession()
		{
			return _sessionAndTransactionManager.Session;
		}
	}
}
