// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using Airion.Common;
using Airion.Persist.Internal;
using Airion.Persist.Provider;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;

namespace Airion.Persist.Provider
{
	public delegate IValueStore<IConversation> ConversationStoreFactory();
	
	/// <summary>
	/// Description of NHibernateConfiguration.
	/// </summary>
	public class NHibernateConfiguration : IConfiguration
	{
		private Action<IDbIntegrationConfigurationProperties> _databaseIntegration;
		private List<Tuple<HbmMapping, string>> _mappings = new List<Tuple<HbmMapping, string>>();
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
			
		public NHibernateConfiguration Database(Action<IDbIntegrationConfigurationProperties> databaseIntegration) 
		{
			_databaseIntegration = databaseIntegration;
			return this;
		}
		
		public NHibernateConfiguration AddMapping(string modelName, HbmMapping mapping)
		{
			_mappings.Add(new Tuple<HbmMapping, string>(mapping, modelName));
			return this;
		}
		
		public NHibernateConfiguration AddMapping<TDomainMapping>(string modelName)
			where TDomainMapping : class, IDomainMapping, new()
		{
			var domainMapper = new DomainMapper<TDomainMapping>();
			var mapping = domainMapper.Map();
			return AddMapping(modelName, mapping);
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
			nhConfig.Proxy(x => x.ProxyFactoryFactory<NHibernate.ByteCode.LinFu.ProxyFactoryFactory>());
			nhConfig.DataBaseIntegration(_databaseIntegration);
			foreach(var mapping in _mappings) {
				nhConfig.AddDeserializedMapping(mapping.Item1, mapping.Item2);
			}
			return new NHibernateProvider(nhConfig);
		}
		
		IValueStore<IConversation> IConfiguration.BuildValueStore()
		{
			return _conversationStoreFactory();
		}
		
		// new SchemaExporter(nhConfig).OutputFile(/**/).Execute(IConversation)
	}
}
