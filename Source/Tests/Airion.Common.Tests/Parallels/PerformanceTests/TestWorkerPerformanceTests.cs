// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using Autofac;
using Airion.Parallels.Tests;
using Airion.Testing;

namespace Airion.Parallels.PerformanceTests
{
	[TestFixture]
	public class TestWorkerPerformanceTests : ParallelsAutofacTestBase
	{
		/// <summary>
		/// Performance test for task execution.
		/// </summary>
		/// <remarks>
		/// <list type="number">
		/// 	<item>
		/// 		<description>Baseline - Execution time ~150ms</description>
		/// 		<description>Refactor to use ManualResetEventSlim - Execution time ~90ms</description>
		/// 	</item>
		/// </list>
		/// </remarks>
		[Test(Description=@"Should be able to execute many tasks.")]
		public void TaskExecution()
		{
			const int ActionCount = 100000;
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				long averageRunTime = PerformanceTestRunner.ExecuteAction(
					25,
					() => {
						ITaskHandle[] taskHandles = new ITaskHandle[ActionCount];
						for (int i = 0; i < ActionCount; i++) {
							taskHandles[i] = taskWorker.ExecuteAction(() => {	});
						}
						
						TaskHandle.WaitAll(taskHandles);
					});
				
				Console.WriteLine("TaskExecution Mean Runtime: {0}ms", averageRunTime);
			}			
		}
	}
}
