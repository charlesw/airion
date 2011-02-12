// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS.Tests.Support
{
	public class TestChildCommandHandler : AbstractCommandHandler<TestChildCommand>
	{
		public TestChildCommandHandler()
		{
			ExecutionCount = 0;
		}
		
		public int ExecutionCount { get; set; }
	
										
		protected override void Verify(CommandContext<TestChildCommand> commandContext)
		{					
			var command = commandContext.Command;
			if(!command.IsValid) {
				commandContext.AddError("IsValid", "The command is not valid.");
			}
		}
		
		
		protected override void HandleInternal(CommandContext<TestChildCommand> commandContext)
		{
			ExecutionCount++;					
		}				
	}
}
