/*
 * Created by SharpDevelop.
 * User: Charles Weld
 * Date: 4/22/2011
 * Time: 10:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using ConfOrm;
using ConfOrm.NH;
using NHibernate.Cfg.MappingSchema;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of DomainMapper.
	/// </summary>
	public class DomainMapper<TDomainMapping>
			where TDomainMapping : class, IDomainMapping, new()
	{
		public DomainMapper()
		{
		}
		
		public HbmMapping Map()
		{
			var orm = new ObjectRelationalMapper();
			_domainMapping.DomainDefinition(orm);
			var mapper = new Mapper(orm);
			_domainMapping.RegisterPatterns(mapper);
			_domainMapping.Customize(mapper);
			return mapper.CompileMappingFor(_domainMapping.GetEntities());
		}
		
	
		TDomainMapping _domainMapping = new TDomainMapping();
	}
}
