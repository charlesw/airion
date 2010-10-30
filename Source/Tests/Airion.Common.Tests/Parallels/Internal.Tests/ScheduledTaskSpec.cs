// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using System.Threading;
using NUnit.Framework;

namespace Airion.Parallels.Internal.Tests
{
	public abstract class ScheduledTaskSpec
	{
		protected IScheduledTask CreateScheduledTask(Action action, CancellationToken cancellationToken)
		{
			return CreateScheduledTask(action, null, cancellationToken);
		}
		protected abstract IScheduledTask CreateScheduledTask(Action action, Action callback, CancellationToken cancellationToken);
		
		[Test(Description=@"Should no be finished and cancelled before task is run.")]
		public void ValidInitialState()
		{
			var task = CreateScheduledTask(
				() => {},
				CancellationToken.None
			);
			Assert.That(task.IsFinished, Is.False);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should be able to execute task")]
		public void TaskExecution()
		{
			bool executed = false;
			var task = CreateScheduledTask(
				() => {
					Thread.Sleep(50); // simulate some work
					executed = true;
				},
				CancellationToken.None
			);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Start();
			
			// wait for task to finish
			task.Wait();
			
			// wait for thread to finish
			thread.Join();
			
			Assert.That(executed, Is.True); // task was executed
			Assert.That(task.IsFinished, Is.True);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should execute callback in wait.")]
		public void TaskExecuteCallbackInWait()
		{
			bool callbackExecuted = false;
			bool executed = false;
			var task = CreateScheduledTask(
				// Work
				() => {
					Thread.Sleep(50); // simulate some work
					executed = true;
				},
				// Callback
				() => {
					callbackExecuted = true;
				},
				// Cancellation token
				CancellationToken.None
			);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Start();
			
			// wait for task to finish
			Assert.That(callbackExecuted, Is.False);
			task.Wait();
			
			// wait for thread to finish
			thread.Join();
			
			Assert.That(callbackExecuted, Is.True);
			Assert.That(executed, Is.True); // task was executed
			Assert.That(task.IsFinished, Is.True);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should be able to cancel task.")]
		public void TaskCancellation()
		{
			var tokenSource = new CancellationTokenSource();
			var cancellationToken = tokenSource.Token;
			
			var task = CreateScheduledTask(
				() => {
					while(!cancellationToken.IsCancellationRequested) {
						
					}
				}, cancellationToken);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Start();
			
			// Cancel task
			tokenSource.Cancel();
			
			// wait until task is completed
			task.Wait();
			
			Assert.That(task.IsCancelled, Is.True);
			Assert.That(task.IsFinished, Is.True);
		}
		
		
	}

	public abstract class ScheduledTaskSpec<T>
	{
		protected IScheduledTask<T> CreateScheduledTask(System.Func<T> func, CancellationToken cancellationToken)
		{
			return CreateScheduledTask(func, null, cancellationToken);
		}
		
		protected abstract IScheduledTask<T> CreateScheduledTask(System.Func<T> func, Action callbackAction, CancellationToken cancellationToken);
		
		protected abstract T ExpectedResult { get; }
		
		[Test(Description=@"Should no be finished and cancelled before task is run.")]
		public void ValidInitialState()
		{
			var task = CreateScheduledTask(
				() => {return default(T);},
				CancellationToken.None
			);
			Assert.That(task.IsFinished, Is.False);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should be able to execute task")]
		public void TaskExecution()
		{
			bool executed = false;
			var task = CreateScheduledTask(
				() => {
					Thread.Sleep(50); // simulate some work
					executed = true;
					return ExpectedResult;
				},
				CancellationToken.None
			);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Start();
			
			// wait for task to finish
			task.Wait();
			
			// wait for thread to finish
			thread.Join();
			
			Assert.That(executed, Is.True); // task was executed
			Assert.That(task.Result, Is.EqualTo(ExpectedResult));
			Assert.That(task.IsFinished, Is.True);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should wait until task is done when result is requested.")]
		public void GetTaskResult()
		{			
			bool executed = false;
			var task = CreateScheduledTask(
				() => {
					Thread.Sleep(100); // simulate some work
					executed = true;
					return ExpectedResult;
				},
				CancellationToken.None
			);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Name = "WorkerThread";
			thread.Start();
			
			// wait for task to finish
			var result = task.Result;
			
			// wait for thread to finish
			thread.Join();
			
			Assert.That(executed, Is.True); // task was executed
			Assert.That(result, Is.EqualTo(ExpectedResult)); // task was executed
			
			
			Assert.That(task.IsFinished, Is.True);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should issue callback when task wait is done.")]
		public void IssueCallbackWhenDone()
		{			
			bool executed = false;
			bool callbackExecuted = false;
			var task = CreateScheduledTask(
				// WOrk
				() => {
					Thread.Sleep(100); // simulate some work
					executed = true;
					return ExpectedResult;
				},
				// callback
				() => {
					callbackExecuted = true;
				},
				CancellationToken.None
			);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Name = "WorkerThread";
			thread.Start();
			
			// wait for task to finish
			var result = task.Result;
			
			// wait for thread to finish
			thread.Join();
			
			Assert.That(callbackExecuted, Is.True); // callback was executed
			Assert.That(executed, Is.True); // task was executed
			Assert.That(result, Is.EqualTo(ExpectedResult)); // task was executed
			
			
			Assert.That(task.IsFinished, Is.True);
			Assert.That(task.IsCancelled, Is.False);
		}
		
		[Test(Description=@"Should be able to cancel task.")]
		public void TaskCancellation()
		{
			var tokenSource = new CancellationTokenSource();
			var cancellationToken = tokenSource.Token;
			
			var task = CreateScheduledTask(
				() => {
					while(!cancellationToken.IsCancellationRequested) {
						
					}
					return default(T);
				}, cancellationToken);
			
			// execute task asynchrously
			Thread thread = new Thread(task.Execute);
			thread.Start();
			
			// Cancel task
			tokenSource.Cancel();
			
			// wait until task is completed
			task.Wait();
			
			Assert.That(task.IsCancelled, Is.True);
			Assert.That(task.IsFinished, Is.True);
		}
	}
}
