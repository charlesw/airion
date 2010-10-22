// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.Tests.Support.Domain
{
	public class Person : Entity<Guid>
	{
		public Person()
		{
		}
		
		public virtual string Name { get; set; }
		public virtual string Address { get; set; }
	}
}
