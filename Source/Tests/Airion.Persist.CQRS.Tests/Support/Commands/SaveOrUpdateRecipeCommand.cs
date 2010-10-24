// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using Airion.Common;

namespace Airion.Persist.CQRS.Tests.Support.Commands
{
	public class SaveOrUpdateRecipeCommand
	{
		private Recipe _recipe;	
		
		public SaveOrUpdateRecipeCommand(Recipe recipe)
		{
			_recipe = recipe;
		}
		
		public void Execute(CommandContext context)
		{			
			context.Conversation.SaveOrUpdate(_recipe);
		}
	
		// verification rules	
		
		public bool CanExecute(CommandContext context)
		{
			// global rules
			if(!VerifyNoRecipeAlreadyExistsWithSameName(context.Conversation)) {
				context.AddError("A recipe already exists with the name '{0}'.", _recipe.Name);
			}
			
			return !context.HasError;			
		}	
		
		private bool VerifyNoRecipeAlreadyExistsWithSameName(IConversation conversation)
		{
			return conversation.Linq<Recipe>().Where(x => x.Name == _recipe.Name && x != _recipe).Count() == 0;
		}
	}
}
