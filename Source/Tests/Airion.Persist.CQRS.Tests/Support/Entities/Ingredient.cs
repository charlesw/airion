// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.CQRS.Tests.Support
{
	public class Ingredient : Entity<Guid>
	{
		public Ingredient()
		{
		}
				
		public virtual string IngredientName { get; set; }
	}
}
