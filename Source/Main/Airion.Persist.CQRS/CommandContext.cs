// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using System.Collections.Generic;

namespace Airion.Persist.CQRS
{
	public class CommandContext
	{
		private List<CommandError> _errors;
		
		public CommandContext(IConversation conversation)
		{
			_errors = new List<CommandError>();
			Conversation = conversation;
		}
		
		public IConversation Conversation { get; private set; }
		
		public IEnumerable<CommandError> Errors { 
			get { return _errors; }
		}
		
		public bool HasError
		{
			get { return _errors.Count > 0; }
		}
		
		public void AddError(string errorMessage)
		{
			AddError(string.Empty, errorMessage);
		}
		
		public void AddError(string propertyName, string errorMessage)
		{
			AddError(new CommandError(propertyName, errorMessage));
		}
		
		public void AddError(CommandError error)
		{
			_errors.Add(error);
		}
	}
}
