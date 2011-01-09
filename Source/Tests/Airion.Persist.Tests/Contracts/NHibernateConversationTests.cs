// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;
using System.Text;
using Airion.Persist.Internal;
using Airion.Persist.Provider;
using Airion.Persist.Tests.Contracts;
using Airion.Persist.Tests.Support;
using FluentNHibernate.Cfg.Db;
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
				.Database(SQLiteConfiguration.Standard.InMemory())
				.Mappings(x => x.FluentMappings
				          .AddFromAssemblyOf<PersonMap>());
		}
		
		protected override void OnStoreCreate(Store store)
		{
			_store = store;			
			
			base.OnStoreCreate(store);
		}
	}
}
