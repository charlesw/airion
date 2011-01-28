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
	public interface ICommandHandler
	{		
	}
	
	public interface ICommandHandler<TCommand> : ICommandHandler
	{
		void Handle(CommandContext<TCommand> commandContext);
	}
}
