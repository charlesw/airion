// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;
using System.Text;

using Airion.Persist.NHibernateProvider.Tests.Support;
using Airion.Persist.Provider;
using Airion.Persist.Tests.Contracts;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Airion.Persist.NHibernateProvider.Tests.Contract
{
	/// <summary>
	/// Description of NHibernateProviderTests.
	/// </summary>
	public class NHibernateConversationTests : ConversationTests
	{
		private string _dbSchema;
		
		protected override IConfiguration BuildConfiguration()
		{
			return new NHibernateConfiguration()
				.Database(SQLiteConfiguration.Standard.InMemory())
				.Mappings(x => x.FluentMappings
				          .AddFromAssemblyOf<PersonMap>());
		}
		
		protected override void OnStoreCreate(Store store)
		{
			var dbSchemaBuilder = new StringBuilder();
			var schemaExporter = new SchemaExporter((NHibernateProvider)store.PersistenceProvider);
			
			using(var writer = new StringWriter(dbSchemaBuilder)) {
				schemaExporter.Export(writer);
			}
			_dbSchema = dbSchemaBuilder.ToString();
			
			store.PersistenceProvider.SessionOpened += new EventHandler<SessionEventArgs>(PersistenceProvider_SessionOpened);
			
			base.OnStoreCreate(store);
		}

		void PersistenceProvider_SessionOpened(object sender, SessionEventArgs e)
		{
			var session = (NHibernateSession)e.Session;
			var provider = (NHibernateProvider)session.PersistenceProvider;
			using(var script = new StringReader(_dbSchema)) {
				var schemaExecuter = new ScriptExecuter(session);
				//schemaExecuter.Execute(script);
				
				new SchemaExport(provider.Configuration).Execute(true, true, false);
			}
		}
	}
}
