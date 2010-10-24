// Author: Charles Weld
// Date: 9/03/2010 6:48 AM

using System;

namespace Airion.Parallels
{
	public interface ITaskHandle
	{		
		void Wait();
		
		bool IsCancelled { get; }
		
		bool IsFinished { get; }
	}
	
	public interface ITaskHandle<TResult> : ITaskHandle
	{				
		TResult Result { get; }
	}
}
