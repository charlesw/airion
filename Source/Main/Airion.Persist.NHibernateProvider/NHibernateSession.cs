// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;
using NHibernate.Linq;
using INHTransaction = NHibernate.ITransaction;

namespace Airion.Persist.NHibernateProvider
{
	/// <summary>
	/// Description of NHibernateTransaction.
	/// </summary>
	public class NHibernateSession : LightDisposableBase, Airion.Persist.Provider.ISession	
	{		
		public IPersistenceProvider PersistenceProvider { get; private set; }
		
		public NHibernateSession(IPersistenceProvider persistenceProvider, NHibernate.ISession nhSession)
		{
			PersistenceProvider = persistenceProvider;
			Session = nhSession;
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(Session != null) {
					Session.Dispose();
					Session = null;
				}
			}
			base.Dispose(disposing);
		}
		
		public Airion.Persist.Provider.ITransaction BeginTransaction()
		{
			var transaction = new NHibernateTransaction(Session.BeginTransaction());
			
			return transaction;
		}
		
		public NHibernate.ISession Session { get; private set; } 
		
		public T Get<T>(object id)
		{
			return Session.Get<T>(id);
		}
		
		public System.Linq.IQueryable<T> Linq<T>()
		{
			return Session.Linq<T>();
		}
		
		public void Update<T>(T entity)
		{
			Session.Update(entity);
		}
		
		public void Save<T>(T entity)
		{
			Session.Save(entity);
		}
		
		public void SaveOrUpdate<T>(T entity)
		{
			Session.SaveOrUpdate(entity);
		}
		
		public void Delete<T>(T entity)
		{
			Session.Delete(entity);
		}
		
		public void Flush()
		{
			Session.Flush();
		}
		
		public Airion.Persist.Provider.FlushMode FlushMode {
			get {
				return (Airion.Persist.Provider.FlushMode)Session.FlushMode;
			}
			set {
				Session.FlushMode = (NHibernate.FlushMode)value;
			}
		}
	}
}
