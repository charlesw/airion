// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;
using System.Text;
using Airion.Persist.Internal;
using Airion.Persist.Provider;
using Airion.Persist.Tests.Contracts;
using Airion.Persist.Tests.Support;
using Airion.Persist.Tests.Support.Domain;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Airion.Persist.Tests.Contract
{
	[TestFixture]
	public class NHibernateConversationTests : ConversationTests
	{
		private Store _store;
		
		protected override IConfiguration BuildConfiguration()
		{
			return new NHibernateConfiguration(provider => new TransientSessionAndTransactionManager(provider))
				.Database(database => {
					database.ConnectionString = @"Data Source=:memory:;Version=3;New=true"; // creates a Tempary database 
					database.Driver<SQLite20Driver>();
					database.Dialect<SQLiteDialect>();
				})
				.AddMapping<DomainMapping>("PersonModel");
		}
		
		protected override void OnStoreCreate(Store store)
		{
			_store = store;			
			
			base.OnStoreCreate(store);
		}
	}
}
