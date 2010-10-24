// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.CQRS.Tests.Support
{
	/// <summary>
	/// The author 
	/// </summary>
	public class FullName
	{
		public FullName(string firstName, string lastName)
		{
			Guard.RequireNotNullOrEmpty("firstName", firstName);
			Guard.RequireNotNullOrEmpty("lastName", lastName);
			
			FirstName = firstName;
			LastName = lastName;
		}
		
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
				
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FullName other = obj as FullName;
			if (other == null)
				return false;
			return FirstName == other.FirstName && LastName == other.LastName;
		}
		
		public override int GetHashCode()
		{
			const int HashMultiplier = 37;
			return (FirstName.GetHashCode() * HashMultiplier) ^ LastName.GetHashCode();
		}
		
		public static bool operator ==(FullName lhs, FullName rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(FullName lhs, FullName rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
