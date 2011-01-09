// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using FluentNHibernate.Mapping;

namespace Airion.Persist.CQRS.Tests.Support.Mappings
{
	/// <summary>
	/// Description of FullNameMap.
	/// </summary>
	public class FullNameMap : ComponentMap<FullName>
	{
		public FullNameMap()
		{
			Map(x => x.FirstName);
			Map(x => x.LastName);
		}
	}
}
