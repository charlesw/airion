// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using System.Threading;
using NUnit.Framework;
using Airion.Parallels.Extensions;
using Airion.Parallels.Tests;
using Autofac;

namespace Airion.Parallels.Internal.Tests
{
	[TestFixture]
	public class BackgroundWorkerSpec : ParallelsAutofacTestBase
	{
		[Test]
		public void Should_start_worker_before_sending_it_items()
		{
			var container = BuildContainer();
			var workQueue = container.Resolve<IPendingWorkCollection<IScheduledTask>>();
			var backgroundWorkerFactory = container.Resolve<BackgroundWorker.Factory>();
			using(IBackgroundWorker worker = backgroundWorkerFactory(workQueue, ApartmentState.MTA)) {
				Assert.That(worker.IsStarted, Is.False);
				worker.Start();
				Assert.That(worker.IsStarted, Is.True);
			}
		}
				
		[Test]
		public void Should_be_able_to_execute_an_action_asynchnously()
		{
			var container = BuildContainer();
			var backgroundWorkerFactory = container.Resolve<BackgroundWorker.Factory>();
			
			var workQueue = container.Resolve<IPendingWorkCollection<IScheduledTask>>();
			using(IBackgroundWorker worker = backgroundWorkerFactory(workQueue, ApartmentState.MTA)) {
				worker.Start();
				
				bool executed = false;
				ManualResetEvent waitHandle = new ManualResetEvent(false);
				var future = workQueue.SendAction(
					() => {
						waitHandle.WaitOne();
						executed = true;
					}, null, CancellationToken.None);
				// the worker should be pending execution or waiting at the wait handle
				Assert.That(future.IsFinished, Is.False);
				Assert.That(executed, Is.False);
				
				// let the action proceed
				waitHandle.Set();
				
				// wait for it to complete
				future.Wait();
				
				// verify that action was successful
				Assert.That(future.IsFinished, Is.True);
				Assert.That(executed, Is.True);
			}
		}
		
		[Test]
		public void Should_be_able_to_have_multiple_workers_using_one_queue()
		{
			var container = BuildContainer();
			var backgroundWorkerFactory = container.Resolve<BackgroundWorker.Factory>();
			
			int WorkerCount = Environment.ProcessorCount*2;
			IBackgroundWorker[] workers = new IBackgroundWorker[WorkerCount];

			var workQueue = container.Resolve<IPendingWorkCollection<IScheduledTask>>();

			for (int i = 0; i < WorkerCount; i++) {
				workers[i] = backgroundWorkerFactory(workQueue, ApartmentState.MTA);
				workers[i].Start();
			}

			// schedule some work
			for (int i = 0; i < 50; i++) {
				workQueue.SendAction(() => { Thread.Sleep(50); }, null, CancellationToken.None);
			}

			// wait until work is done
			workQueue.Wait(CancellationToken.None);

			// dispose of workers
			for (int i = 0; i < WorkerCount; i++) {
				workers[i].Dispose();
			}
		}
		
		[Test]
		public void Should_finish_executing_task_when_disposed()
		{
			var container = BuildContainer();
			var backgroundWorkerFactory = container.Resolve<BackgroundWorker.Factory>();
			
			var workQueue = container.Resolve<IPendingWorkCollection<IScheduledTask>>();
			IBackgroundWorker worker = backgroundWorkerFactory(workQueue, ApartmentState.MTA);
			worker.Start();
			
			ManualResetEvent actionResetEvent = new ManualResetEvent(false);
			ManualResetEvent callerResetEvent = new ManualResetEvent(false);
			
			// request action to be executed but ensure that it is not
			bool executed = false;
			var future = workQueue.SendAction(
				() => {
					actionResetEvent.WaitOne(); // wait till we've scheduled the second event
					executed = true;
					
					// Lets the caller continue and dispose of the worker
					callerResetEvent.Set();
					
					// simulate some time consuming work
					Thread.Sleep(100);
				}, null, CancellationToken.None);
			
			
			
			// enqueue another worker this should not be executed
			bool secondActionExecuted = false;
			var secondFuture = workQueue.SendAction(
				() => { secondActionExecuted = true; }, null, CancellationToken.None
			);
			
			actionResetEvent.Set();
			callerResetEvent.WaitOne();
			
			// dispose of worker
			worker.Dispose();
			
			// verify that the executing action was successful
			Assert.That(future.IsFinished, Is.True);
			Assert.That(executed, Is.True);
			
			// verify that the second action was not executed
			Assert.That(secondFuture.IsFinished, Is.False);
			Assert.That(secondActionExecuted, Is.False);
		}
		
		private void ExampleWork()
		{
			// simulate some work
			Thread.Sleep(50);
		}
	}
}
