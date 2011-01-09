// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using FluentNHibernate.Mapping;

namespace Airion.Persist.CQRS.Tests.Support.Mappings
{
	/// <summary>
	/// Description of IngredientMap.
	/// </summary>
	public class IngredientMap : ClassMap<Ingredient>
	{
		public IngredientMap()
		{
			Id(x => x.Id);
			Map(x => x.IngredientName);
		}
	}
}
