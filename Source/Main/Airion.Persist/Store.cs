// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Diagnostics;
using Airion.Common;
using Airion.Persist.Internal;
using Airion.Persist.Provider;

namespace Airion.Persist
{
	/// <summary>
	/// Description of Store.
	/// </summary>
	public class Store : DisposableBase
	{
		private IConfiguration _configuration;
		private IPersistenceProvider _persistenceProvider;
		private IValueStore<IConversation> _currentConversation;
		
		public Store(IConfiguration configuration)
		{
			_configuration = configuration;
			_persistenceProvider = _configuration.BuildProvider();
			_currentConversation = _configuration.BuildValueStore();
		}
		
		public IPersistenceProvider PersistenceProvider
		{
			get { return _persistenceProvider; }
		}
		
		public IConversation BeginConversation()
		{
			Guard.Operation(CurrentConversation == null, "There is already an active conversation.");
			
			var conversation = new Conversation(_persistenceProvider);
			conversation.Disposed += ConversationDisposed;		
			CurrentConversation = conversation;
			return conversation;
		}

		public IConversation CurrentConversation
		{
			get { return _currentConversation.Value; }
			private set { _currentConversation.Value = value; }
		}
		
		
		private void ConversationDisposed(object sender, EventArgs e)
		{
			Debug.Assert(sender == CurrentConversation);
			var conversation = (Conversation)sender;
			conversation.Disposed -= ConversationDisposed;
			
			CurrentConversation = null;
		}
		
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing) {
				if(_persistenceProvider != null) {
					_persistenceProvider.Dispose();
					_persistenceProvider = null;
				}
			}
		}
	}
}
