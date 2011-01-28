// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)


using System;
using System.Collections.Generic;

namespace Airion.Persist.CQRS
{
	public class CommandContext<TCommand>
	{
		private List<CommandError> _errors;
		private TCommand _command;
		
		public CommandContext(TCommand command)
		{
			_errors = new List<CommandError>();
			_command = command;
		}		
		
		public TCommand Command
		{
			get { return _command; }
		}
		
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
