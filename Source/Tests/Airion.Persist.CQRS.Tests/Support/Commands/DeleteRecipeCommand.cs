// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS.Tests.Support.Commands
{
	public class DeleteRecipeCommand 
	{
		private Recipe _recipe;
		
		public DeleteRecipeCommand(Recipe recipe)
		{
			_recipe = recipe;
		}
		
		public void Execute(IConversation conversation)
		{
			conversation.Delete(_recipe);
		}
		
		public bool CanExecute(CommandContext context)
		{
			return true;
		}
	}
}
