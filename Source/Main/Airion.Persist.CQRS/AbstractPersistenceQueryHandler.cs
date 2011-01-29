// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Represents a query handler that works with a database.
	/// </summary>
	public abstract class AbstractPersistenceQueryHandler<TQuery, TQueryResult> : IQueryHandler<TQuery, TQueryResult>
	{
		public Store Store { get; private set; }
		
		public AbstractPersistenceQueryHandler(Store store)
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
		
		public abstract TQueryResult Execute(TQuery query);
	}
}
