// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Represents the base class that provides persistence capabilities to command handlers.
	/// </summary>
	public abstract class AbstractPersistenceCommandHandler<TCommand> : AbstractCommandHandler<TCommand>
	{
		public Store Store { get; private set; }
		
		public AbstractPersistenceCommandHandler(Store store)
		{
			Store = store;
		}
		
		protected IConversation Conversation 
		{
			get {
				var conversation = Store.CurrentConversation;
				Guard.Operation(conversation != null, "No conversation is currently active.");
				return conversation;
			}
		}
	}
}
