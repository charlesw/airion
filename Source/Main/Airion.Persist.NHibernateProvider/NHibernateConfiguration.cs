// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace Airion.Persist.NHibernateProvider
{
	public delegate IValueStore<IConversation> ConversationStoreFactory();
	
	/// <summary>
	/// Description of NHibernateConfiguration.
	/// </summary>
	public class NHibernateConfiguration : IConfiguration
	{
		private IPersistenceConfigurer _persistenceConfigurer;
		private Action<MappingConfiguration> _mappings;
		private ConversationStoreFactory _conversationStoreFactory;
		
		public NHibernateConfiguration()
		{
			_conversationStoreFactory = () => new CallLocalValueStore<IConversation>();
		}
		
		public NHibernateConfiguration Database(IPersistenceConfigurer persistenceConfigurer) 
		{
			_persistenceConfigurer = persistenceConfigurer;
			return this;
		}
		
		public NHibernateConfiguration Mappings(Action<MappingConfiguration> mappings)
		{
			_mappings = mappings;
			return this;
		}
		
		public NHibernateConfiguration RegisterConversationStoreFactory(ConversationStoreFactory conversationStoreFactory)
		{
			_conversationStoreFactory = conversationStoreFactory;
			return this;
		}
		
		IPersistenceProvider IConfiguration.BuildProvider()
		{
			// NHibernate configuration
			var nhConfig = new Configuration();
			Fluently.Configure(nhConfig)
				.Database(_persistenceConfigurer)
				.Mappings(_mappings)
				.BuildConfiguration();
			
			return new NHibernateProvider(nhConfig);
		}
		
		IValueStore<IConversation> IConfiguration.BuildValueStore()
		{
			return _conversationStoreFactory();
		}
		
		// new SchemaExporter(nhConfig).OutputFile(/**/).Execute(IConversation)
	}
}
