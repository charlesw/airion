// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Airion.Common;
using Airion.Parallels;

namespace Airion.Persist.CQRS
{
	public class CommandExecutor
	{		
		private Dictionary<Type, ICommandHandler> _commandHandlers;
		private static MethodInfo _executeInternalMethodInfo;
		
		static CommandExecutor() 
		{
			_executeInternalMethodInfo = typeof(CommandExecutor).GetMethod("ExecuteInternal", BindingFlags.NonPublic | BindingFlags.Instance);
			Guard.Operation(_executeInternalMethodInfo != null, "Couldn't find CommandExecutor.ExecuteInternal.");
		}
		
		
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
		
		public void Execute(object command)
		{
			//TODO: Implement this using a dynamicly generated delegate based approach rather than using reflection.
			
			ICommandHandler commandHandler = null;
			var commandType = command.GetType();
			while(commandType != null) {
				if(_commandHandlers.TryGetValue(commandType, out commandHandler)) {					
					break;
				}
				
				commandType = commandType.BaseType;
			}
			
			if(commandHandler != null) {
				try {
					var executeInternalMethod = _executeInternalMethodInfo.MakeGenericMethod(commandType);
					executeInternalMethod.Invoke(this, new object[] { commandHandler, command });
				} catch (TargetInvocationException e) {
					throw e.InnerException;
				}
			}
		}

		private void ExecuteInternal<TCommand>(ICommandHandler<TCommand> commandHandler, TCommand command)
		{
			var commandContext = new CommandContext<TCommand>(command);
			commandHandler.Handle(commandContext);
		}			
	}
}
