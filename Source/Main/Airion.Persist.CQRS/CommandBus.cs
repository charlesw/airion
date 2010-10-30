// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Threading;
using Airion.Common;
using Airion.Parallels;

namespace Airion.Persist.CQRS
{
	public class CommandBus : LightDisposableBase
	{
		private ITaskWorker _taskWorker;
		private Store _store;
		
		private class CommandHandler : IWorkItem, IWorkItemCallback
		{
			private Store _store;
			private ICommand _command;
			private Exception _commandExecutionException;
			
			public CommandHandler(Store store, ICommand command)
			{
				_store = store;
				_command = command;
			}
			
			public void Execute()
			{
				try {
					using(var conversation = _store.BeginConversation()) {
						var context = new CommandContext(conversation);
						_command.Execute(context);
						if(context.HasError) {
							throw new CommandValidationException(context.Errors);
						}
					}
				} catch (OperationCanceledException) {
					throw;
				} catch (Exception e) {
					// just record all other exceptions
					_commandExecutionException = e;
				}
			}
			
			public void NotifyCallback()
			{
				if(_commandExecutionException != null) {
					throw new System.Reflection.TargetInvocationException(_commandExecutionException);
				}
			}
		}
		
		public CommandBus(Store store, TaskWorkerFactory taskWorkerFactory)
		{
			_store = store;
			_taskWorker = taskWorkerFactory(ApartmentState.MTA);
			_taskWorker.Start();
		}
		
		public ITaskHandle Schedule(ICommand command)
		{
			var commandHandler = new CommandHandler(_store, command);
			return _taskWorker.ExecuteWorkItem(commandHandler);
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_taskWorker != null) {
					_taskWorker.Dispose();
					_taskWorker = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
