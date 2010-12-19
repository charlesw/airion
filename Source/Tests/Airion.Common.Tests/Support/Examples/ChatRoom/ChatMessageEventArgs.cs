// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Common.Tests.Support.Examples.ChatRoom
{
	/// <summary>
	/// Description of ChatMessageEventArgs.
	/// </summary>
	public class ChatMessageEventArgs : EventArgs
	{
		private string _message;
		
		public ChatMessageEventArgs(string message)
		{
			_message = message;
		}
		
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
		
		public override string ToString()
		{
			return string.Format("[ChatMessageEventArgs Message={0}]", _message);
		}

		
		#region Equals and GetHashCode implementation
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (_message != null)
					hashCode += 1000000007 * _message.GetHashCode();
			}
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			ChatMessageEventArgs other = obj as ChatMessageEventArgs;
			if (other == null)
				return false;
			return this._message == other._message;
		}

		public static bool operator ==(ChatMessageEventArgs lhs, ChatMessageEventArgs rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ChatMessageEventArgs lhs, ChatMessageEventArgs rhs)
		{
			return !(lhs == rhs);
		}
		#endregion


	}
}
