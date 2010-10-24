// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;

namespace Airion.Testing
{
	public class TestEventArgs : EventArgs
	{
		private readonly int _id;
		
		public static TestEventArgs FromId(int id)
		{
			return new TestEventArgs(id);
		}
		
		public TestEventArgs(int id)
		{
			_id = id;
		}
		
		public int Id
		{
			get { return _id; }
		}
		
		#region Equals and GetHashCode implementation
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * _id.GetHashCode();
			}
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			TestEventArgs other = obj as TestEventArgs;
			if (other == null)
				return false;
			return this._id == other._id;
		}

		public static bool operator ==(TestEventArgs lhs, TestEventArgs rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(TestEventArgs lhs, TestEventArgs rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
