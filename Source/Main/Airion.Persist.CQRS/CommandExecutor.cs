// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Airion.Common;
using Airion.Parallels;

namespace Airion.Persist.CQRS
{
	public class CommandExecutor
	{		
		private Dictionary<Type, ICommandHandler> _commandHandlers;
		
		public CommandExecutor(IEnumerable<ICommandHandler> commandHandlers)
		{
			_commandHandlers = new Dictionary<Type, ICommandHandler>();
			foreach(var commandHandler in commandHandlers) {
				var commandHandlerType = commandHandler.GetType();
				var commandType = commandHandlerType.GetGenericInterface(typeof(ICommandHandler<>)).GetGenericArguments()[0];
				if(_commandHandlers.ContainsKey(commandType)) {
					throw new ArgumentException("Cannot register more than one command handler for the same command type.", "commandHandlers");
				}
				
				_commandHandlers.Add(commandType, commandHandler);
			}
		}
		
		public void Execute<TCommand>(TCommand command)
		{
			var commandType = typeof(TCommand);
			ICommandHandler commandHandler;
			if(!_commandHandlers.TryGetValue(commandType, out commandHandler)) {
				throw new InvalidOperationException(String.Format("No command handler has been registered for the command type {0}.", commandType.Name));
			}
			var typedCommandHandler = (ICommandHandler<TCommand>)commandHandler;
			var commandContext = new CommandContext<TCommand>(command);
			typedCommandHandler.Handle(commandContext);
		}		
	}
}
