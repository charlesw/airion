// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using NHibernate;
using NHibernate.Cfg;

namespace Airion.Persist.Provider
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
				
		public ISession OpenSession()
		{
			CheckState();
			
			return _sessionFactory.OpenSession();
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
