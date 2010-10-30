// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using NUnit.Framework;
using Autofac;

namespace Airion.Parallels.Tests
{
	[TestFixture]
	public class TaskWorkerSpec : ParallelsAutofacTestBase
	{
		#region Fakes
		
		public class FakeWorkItem : IWorkItem
		{		
			public int ExecuteCount { get; private set; }
			public Thread SchedulingThread { get; private set; }
			
			public FakeWorkItem(Thread schedulingThread)
			{
				SchedulingThread = schedulingThread;
				ExecuteCount = 0;
			}
				
			public void Execute()
			{
				Assert.That(Thread.CurrentThread, Is.Not.EqualTo(SchedulingThread));
				ExecuteCount++;
			}
		}
		
		#endregion
		
		[Test(Description=@"Should be able to specify thread apartment")]
		[TestCase(ApartmentState.MTA)]
		[TestCase(ApartmentState.STA)]
		public void SpecifyThreadApartment(ApartmentState apartmentState) 
		{
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(apartmentState)) {
				taskWorker.Start();
				
				var taskHandle = taskWorker.ExecuteAction(
					() => {
						Assert.That(Thread.CurrentThread.GetApartmentState(), Is.EqualTo(apartmentState));
					});
				
				taskHandle.Wait();
			}
		}
		
		[Test(Description=@"Should be able to execute workitem on different thread")]
		public void ExecuteWorkItem()
		{
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				var workItem = new FakeWorkItem(Thread.CurrentThread);
				var taskHandle = taskWorker.ExecuteWorkItem(workItem);
				
				taskHandle.Wait();
				
				Assert.That(workItem.ExecuteCount, Is.EqualTo(1));
			}
		}
		
		[Test(Description=@"Should be able to execute action on different thread")]
		public void ExecuteAction()
		{
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				bool executed = false;
				Thread currentThread = Thread.CurrentThread;
				var taskHandle = taskWorker.ExecuteAction(
					() => {
						Assert.That(currentThread, Is.Not.SameAs(Thread.CurrentThread));
						executed = true;
					});
				
				taskHandle.Wait();
				
				Assert.That(executed, Is.True);
			}
		}
		
		[Test(Description=@"Should be able to cancel action.")]
		public void CancelAction() 
		{
			CancellationTokenSource source = new CancellationTokenSource();
			
			var token = source.Token;
			
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				var taskHandle = taskWorker.ExecuteAction(
					() => {
						while(!token.IsCancellationRequested) {
							
						}
					}, token);
				
				source.Cancel();
				
				taskHandle.Wait();
			
				Assert.That(taskHandle.IsCancelled, Is.True);
				Assert.That(taskHandle.IsFinished, Is.True);
			}
		}
		
		[Test(Description=@"Should be able to execute function on different thread")]
		public void ExecuteFunction()
		{
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				bool executed = false;
				Thread currentThread = Thread.CurrentThread;
				var taskHandle = taskWorker.ExecuteFunction(
					() => {
						Assert.That(currentThread, Is.Not.SameAs(Thread.CurrentThread));
						executed = true;
						return true;
					});
				
				Assert.That(taskHandle.Result, Is.True);
				Assert.That(executed, Is.True);
				
				Assert.That(taskHandle.IsFinished, Is.True);
				Assert.That(taskHandle.IsCancelled, Is.False);
			}
		}
		
		[Test(Description=@"Should be able to cancel function.")]
		public void CancelFunction()
		{			
			CancellationTokenSource source = new CancellationTokenSource();
			
			var token = source.Token;
			var container = BuildContainer();
			var taskWorkerFactory = container.Resolve<TaskWorkerFactory>();
			using(var taskWorker = taskWorkerFactory(ApartmentState.MTA)) {
				taskWorker.Start();
				
				var taskHandle = taskWorker.ExecuteFunction(
					() => {
						while(!token.IsCancellationRequested) {
							
						}
						return true;
					}, token);
				
				source.Cancel();
				
				Assert.That(taskHandle.Result, Is.False);
				
				Assert.That(taskHandle.IsFinished, Is.True);
				Assert.That(taskHandle.IsCancelled, Is.True);
			}
		}
//		
//		[Test(Description=@"Should be able to cancel function.")]
//		public void CancelFunction() 
//		{
//			CancellationTokenSource source = new CancellationTokenSource();
//			
//			var token = source.Token;
//			
//			var container = BuildContainer();
//			using(var taskWorker = container.Resolve<TaskWorker>()) {
//				taskWorker.Start();
//				
//				var taskHandle = taskWorker.ExecuteAction(
//					() => {
//						while(!token.IsCancellationRequested) {
//							
//						}
//					}, token);
//				
//				source.Cancel();
//				
//				taskHandle.Wait();
//			
//				Assert.That(taskHandle.IsCancelled, Is.True);
//				Assert.That(taskHandle.IsFinished, Is.True);
//			}
//		}
	}
}
