// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Desctiption of CommandValidationException.
	/// </summary>
	public class CommandValidationException : Exception, ISerializable
	{
		public IEnumerable<CommandError> CommandErrors { get; private set; }
		
		public CommandValidationException()
		{
		}
		
		public CommandValidationException(IEnumerable<CommandError> commandErrors)
			: base(CommandValidationException.CreateMessage(commandErrors))
		{
			CommandErrors = commandErrors;
		}

	 	public CommandValidationException(string message) : base(message)
		{
		}

		public CommandValidationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected CommandValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		
		private static string CreateMessage(IEnumerable<CommandError> commandErrors) 
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("The command execution failed due to validation errors.");
			builder.AppendLine("Errors:");
			foreach(var error in commandErrors) {
				if(String.IsNullOrWhiteSpace(error.PropertyName)) {
					builder.AppendFormat("\tError: {0}\n", error.Message);
				}
			}
			
			foreach(var error in commandErrors) {
				if(!String.IsNullOrWhiteSpace(error.PropertyName)) {
					builder.AppendFormat("\tProperty {0}: {1}\n", error.PropertyName, error.Message);
				}
			}
			
			return builder.ToString();			
		}
	}
}