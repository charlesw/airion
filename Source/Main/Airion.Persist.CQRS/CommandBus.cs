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
	public class CommandBus
	{		
		private Dictionary<Type, List<ICommandHandler>> _commandHandlers;
		
		public CommandBus(IEnumerable<ICommandHandler> commandHandlers)
		{
			_commandHandlers = new Dictionary<Type, List<ICommandHandler>>();
			foreach(var commandHandler in commandHandlers) {
				var commandHandlerType = commandHandler.GetType();
				var commandType = commandHandlerType.GetGenericInterface(typeof(ICommandHandler<>)).GetGenericArguments()[0];
				List<ICommandHandler> commandHandlersForCommandType;
				if(!_commandHandlers.TryGetValue(commandType, out commandHandlersForCommandType)) {
					commandHandlersForCommandType = new List<ICommandHandler>();
					_commandHandlers.Add(commandType, commandHandlersForCommandType);
				}
				commandHandlersForCommandType.Add(commandHandler);
			}
		}
		
		public void Execute<TCommand>(TCommand command)
		{
			var commandType = typeof(TCommand);
			List<ICommandHandler> commandHandlersForCommandType;
			if(!_commandHandlers.TryGetValue(commandType, out commandHandlersForCommandType)) {
				throw new InvalidOperationException(String.Format("No command handlers have been registered for the command type {0}.", commandType.Name));
			}
			
			var commandContext = new CommandContext<TCommand>(command);
			foreach(var commandHandler in commandHandlersForCommandType.Cast<ICommandHandler<TCommand>>()) {
				commandHandler.Handle(commandContext);
			}
		}		
	}
}
