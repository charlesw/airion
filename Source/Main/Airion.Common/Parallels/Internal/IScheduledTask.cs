// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;

namespace Airion.Parallels.Internal
{
	public interface IScheduledTask : IWorkItem, ITaskHandle
	{
		WaitHandle FinishedHandle { get; }
		
		void Close();
	}
	
	public interface IScheduledTask<TResult> : IScheduledTask, ITaskHandle<TResult>
	{
	}
}
