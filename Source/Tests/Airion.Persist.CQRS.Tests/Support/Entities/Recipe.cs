// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using Airion.Common;

namespace Airion.Persist.CQRS.Tests.Support
{
	/// <summary>
	/// Description of Recipe.
	/// </summary>
	public class Recipe : Entity<Guid>
	{
		public Recipe()
		{
			MethodSteps = new List<MethodStep>();
			Ingredients = new HashSet<Ingredient>();
		}
		
		public virtual string Name { get; set; }
		public virtual FullName AuthorName { get; set; }
		public virtual IList<MethodStep> MethodSteps { get; private set; }
		public virtual ICollection<Ingredient> Ingredients { get; private set; }		
	}
}
