// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Represents a error with the command, all command errors are immutable and thus thread safe.
	/// </summary>
	public class CommandError
	{
		public string PropertyName { get; private set; }
		public string Message { get; private set; }
		
		public CommandError(string propertyName, string message)
		{
			PropertyName = propertyName;
			Message = message;
		}
	}
}
