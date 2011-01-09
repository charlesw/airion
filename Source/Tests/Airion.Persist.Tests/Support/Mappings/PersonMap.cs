// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Persist.Tests.Support.Domain;
using FluentNHibernate.Mapping;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of PersonMap.
	/// </summary>
	public class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);
			Map(x => x.Address);
		}
	}
}
