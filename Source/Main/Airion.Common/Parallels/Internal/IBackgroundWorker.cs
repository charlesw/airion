// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Parallels.Internal
{
	/// <summary>
	/// Handles executing work asynchrounously.
	/// </summary>
	public interface IBackgroundWorker : IDisposable
	{
		void Start();
		
		IPendingWorkCollection<IScheduledTask> PendingWork { get; }
		
		bool IsStarted { get; }
		
		bool IsDisposed { get; }
		
		bool IsDisposing { get; }
	}
}
