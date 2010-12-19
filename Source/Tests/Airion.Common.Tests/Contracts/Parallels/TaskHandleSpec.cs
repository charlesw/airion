//// <file>
////     <copyright see="prj:///doc/copyright.txt"/>
////     <license see="prj:///doc/license.txt"/>
////     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
//// </file>
//
//using System;
//using System.Threading;
//using NUnit.Framework;
//using SharpDebug.Common.Parallels.Internal;
//
//namespace SharpDebug.Common.Parallels.Tests
//{
//	[TestFixture]
//	public class TaskHandleSpec
//	{
//		[Test]
//		public void Should_be_able_to_wait_until_one_task_finishes_out_of_a_set_of_tasks()
//		{
//			const int TaskCount = 4;
//			const int LastTaskIndex = TaskCount - 1;
//			using(TaskManager taskManager = new TaskManager(new PendingWorkQueue<IScheduledTask>())) {
//				ITaskHandle[] handles = new ITaskHandle[TaskCount];
//				
//				// first three tasks don't ever return and should be aborted
//				for (int i = 0; i < LastTaskIndex; i++) {
//					handles[i] = taskManager.Execute(
//						() => {
//							while(!AbortMonitor.IsAborted) {
//							}
//						});
//				}
//				
//				// the last task does return after a while (simulate some successful work).
//				handles[LastTaskIndex] = taskManager.Execute(
//					() => {
//						Thread.Sleep(60);
//					});
//				
//				int result = TaskHandle.WaitAny(handles);
//				Assert.That(result, Is.EqualTo(LastTaskIndex));
//				Assert.That(handles[result].IsAborted, Is.False);
//				Assert.That(handles[result].IsFinished, Is.True);
//				
//				// Abort the remaining tasks
//				for (int i = 0; i < LastTaskIndex; i++) {
//					handles[i].Abort();
//					handles[i].Wait(); // still need to make sure that the task has finished.
//					Assert.That(handles[i].IsAborted, Is.True);
//					Assert.That(handles[i].IsFinished, Is.True);
//				}
//			}
//		}		
//	
//		[Test]
//		public void Should_be_able_to_wait_untill_all_tasks_are_finished() 
//		{
//			const int TaskCount = 4;
//			using(TaskManager taskManager = new TaskManager(new PendingWorkQueue<IScheduledTask>())) {
//				ITaskHandle[] handles = new ITaskHandle[TaskCount];
//				
//				// first three tasks don't ever return and should be aborted
//				for (int i = 0; i < TaskCount; i++) {
//					int index = i;
//					handles[i] = taskManager.Execute(
//						() => {
//							Thread.Sleep((index+1)*20);
//						});
//				}
//				
//				TaskHandle.WaitAll(handles);
//				
//				// Abort the remaining tasks
//				for (int i = 0; i < TaskCount; i++) {		
//					Assert.That(handles[i].IsAborted, Is.False);
//					Assert.That(handles[i].IsFinished, Is.True);
//				}
//			}
//		}
//	
//	}
//}
//