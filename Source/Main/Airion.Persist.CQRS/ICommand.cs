﻿// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Description of ICommand.
	/// </summary>
	public interface ICommand
	{		
		void Execute(CommandContext commandContext);
	}
}
