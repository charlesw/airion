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
using System.Linq;

using Airion.Persist.Provider;
using Airion.Persist.Tests.Support.Domain;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Patterns;
using NHibernate.Cfg.MappingSchema;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of DomainMapping.
	/// </summary>
	public class DomainMapping : IDomainMapping
	{		
		public DomainMapping()
		{
		}

		public void DomainDefinition(ObjectRelationalMapper orm)
		{
			orm.TablePerClass<Person>();
			orm.Patterns.PoidStrategies.Add(new GuidPoidPattern());
		}

		public void RegisterPatterns(Mapper mapper)
		{

		}

		public void Customize(Mapper mapper)
		{

		}
		public IEnumerable<Type> GetEntities()
		{
			return new Type[] { typeof(Person) };
		}
	}
}
