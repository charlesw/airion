// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.CQRS.Tests.Support
{
	/// <summary>
	/// Description of MethodStep.
	/// </summary>
	public class MethodStep : Entity<Guid>
	{		
		protected MethodStep() {}
		
		public MethodStep(Recipe recipe)
		{
			Rescipe = recipe;
		}
		
		public virtual Recipe Rescipe { get; private set; }
		public virtual int StepNumber { get; set; }
		public virtual string StepDescription { get; set; }
	}
}
