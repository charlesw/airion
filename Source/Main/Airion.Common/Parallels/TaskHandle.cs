// Author: Charles Weld
// Date: 14/03/2010 7:38 AM

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Airion.Parallels.Internal;

namespace Airion.Parallels
{
	public static class TaskHandle
	{
		/// <summary>
		/// Waits until one of the tasks represented by <see cref="taskHandles"/> is finished, all remaining task will be aborted.
		/// </summary>
		/// <param name="taskHandles">The task handles to wait on.</param>
		/// <returns></returns>
		public static int WaitAny(params ITaskHandle[] taskHandles)
		{		
			//FIXME: The is broken, we should return if a scheduled task is finished not ignore it.			
			// create wait handle array
			int maxLength = taskHandles.Length;
			var scheduledTasks = new IScheduledTask[maxLength];
			int index = 0;
			for (int i = 0; i < maxLength; i++) {
				var scheduledTask = taskHandles[i] as IScheduledTask;
				if(scheduledTask != null) {
					if(!scheduledTask.IsFinished) {
						scheduledTasks[index] = scheduledTask;
						index++;
					} 
				} else {
					throw new InvalidOperationException(String.Format("Couldn't convert task handle \"{0}\" to a IScheduledTask.", taskHandles[i]));
				}
			}	
			
			int length = index;
			var waitHandles = new WaitHandle[index];
			for (int i = 0; i < length; i++) {
				waitHandles[i] = scheduledTasks[i].FinishedHandle;
			}
			int releasedIndex = WaitHandle.WaitAny(waitHandles);	
			// release resources
			for (int i = 0; i < index; i++) {
				scheduledTasks[i].Close();
			}
			return releasedIndex;
		}
		
		public static void WaitAll(params ITaskHandle[] taskHandles)
		{
			for (int i = 0; i < taskHandles.Length; i++) {
				taskHandles[i].Wait();
			}
		}
	}
}
