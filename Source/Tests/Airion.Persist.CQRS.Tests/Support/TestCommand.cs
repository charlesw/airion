// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS.Tests.Support
{			
	public class TestCommand
	{
		public TestCommand()
		{
			IsValid = true;
		}
		public bool IsValid { get; set; }
	}
}
