/*
 * Created by SharpDevelop.
 * User: Charles Weld
 * Date: 4/22/2011
 * Time: 10:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using ConfOrm;
using ConfOrm.NH;
using NHibernate.Cfg.MappingSchema;

namespace Airion.Persist.Provider
{
	public interface IDomainMapping
	{
		void DomainDefinition(ObjectRelationalMapper orm);
		void RegisterPatterns(Mapper mapper);
		void Customize(Mapper mapper);
		IEnumerable<Type> GetEntities();
	}
}
