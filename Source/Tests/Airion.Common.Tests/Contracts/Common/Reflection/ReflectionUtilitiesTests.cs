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
		
		[Test]
		[TestCase(typeof(string), typeof(IEquatable<>), typeof(IEquatable<string>))]
		[TestCase(typeof(int), typeof(IComparable<>), typeof(IComparable<int>))]
		[TestCase(typeof(int), typeof(IComparable), null, ExpectedException = typeof(ArgumentException), 
		          Description = "Generic interface type definition must be a generic type definition")]
		[TestCase(typeof(int), typeof(IComparable<int>), null, ExpectedException = typeof(ArgumentException), 
		          Description = "Generic interface type definition must be a generic type definition")]
		[TestCase(typeof(int), typeof(int), null, ExpectedException = typeof(ArgumentException), 
		          Description = "Generic interface type definition must be interface")]
		[TestCase(typeof(int), null, null, ExpectedException = typeof(ArgumentNullException), 
		          Description = "Generic interface type definition cannot be null")]
		public void GetGenericInterface(Type type, Type genericInterfaceTypeDefinition, Type genericInterfaceType)
		{			
			var result = type.GetGenericInterface(genericInterfaceTypeDefinition);
		}
	}
}
