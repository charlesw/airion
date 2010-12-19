// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using NUnit.Framework;

namespace Airion.Common.Tests.Contracts.Common.Reflection
{
	[TestFixture]
	public class ReflectionUtiltitesTests
	{
		public string TestProperty { get; set; }
		
		[Test]
		public void GetPropertyName_ExpressionToProperty_ReturnsPropertyName()
		{
			var propertyName = ReflectionUtilities.GetPropertyName<ReflectionUtiltitesTests, string>(x => x.TestProperty);
			Assert.That(propertyName, Is.EqualTo("TestProperty"));
		}
	}
}
