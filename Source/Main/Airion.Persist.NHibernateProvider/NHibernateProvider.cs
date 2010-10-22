// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;
using NHibernate.Cfg;

namespace Airion.Persist.NHibernateProvider
{
	/// <summary>
	/// Description of NHibernateProvider.
	/// </summary>
	public class NHibernateProvider : LightDisposableBase, IPersistenceProvider
	{
		private ISessionFactory _sessionFactory;
		
		public NHibernateProvider(NHibernate.Cfg.Configuration configuration)
		{
			Configuration = configuration;
			_sessionFactory = Configuration.BuildSessionFactory();
		}
		
		public NHibernate.Cfg.Configuration Configuration { get; private set; }
		
		public event EventHandler<SessionEventArgs> SessionOpened;
		
		public Airion.Persist.Provider.ISession OpenSession()
		{
			CheckState();
			
			var nhSession = _sessionFactory.OpenSession();
			var session = new NHibernateSession(this, nhSession);
			OnSessionOpened(new SessionEventArgs(session));
			return session;
		}
		
		protected virtual void OnSessionOpened(SessionEventArgs args)
		{
			if(SessionOpened != null) {
				SessionOpened(this, args);
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_sessionFactory != null) {
					_sessionFactory.Dispose();
					_sessionFactory = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
