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
	public abstract class AbstractCommandHandler<TCommand> : ICommandHandler<TCommand>
	{		
		public void Handle(CommandContext<TCommand> commandContext)
		{
			Verify(commandContext);
			if(commandContext.HasError) {
				throw new CommandValidationException(commandContext.Errors);
			} else {
				HandleInternal(commandContext);
			}
		}
		
		protected abstract void Verify(CommandContext<TCommand> commandContext);
		
		protected abstract void HandleInternal(CommandContext<TCommand> commandContext);
	}
}
