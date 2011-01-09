// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Internal;
using Airion.Persist.Provider;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace Airion.Persist.Provider
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
		private CreateSessionAndTransactionManager _createSessionAndTransactionManager;
		
		public NHibernateConfiguration()
		{
			_conversationStoreFactory = () => new CallLocalValueStore<IConversation>();
			_createSessionAndTransactionManager = (IPersistenceProvider provider) => new SessionAndTransactionManager(provider);
		}
		
		public NHibernateConfiguration(CreateSessionAndTransactionManager createSessionAndTransactionManager)
		{
			_conversationStoreFactory = () => new CallLocalValueStore<IConversation>();
			_createSessionAndTransactionManager = createSessionAndTransactionManager;
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
		
		CreateSessionAndTransactionManager IConfiguration.CreateSessionAndTransactionManager 
		{
			get { return _createSessionAndTransactionManager; }
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
