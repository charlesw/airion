// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using FluentNHibernate.Mapping;

namespace Airion.Persist.CQRS.Tests.Support.Mappings
{
	/// <summary>
	/// Description of RecipeMap.
	/// </summary>
	public class RecipeMap : ClassMap<Recipe>
	{
		public RecipeMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);
			Component(x => x.AuthorName);
			HasMany(x => x.MethodSteps).AsList();
			HasMany(x => x.Ingredients).AsSet();
		}
	}
}
