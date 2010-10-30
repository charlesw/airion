// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Diagnostics;
using System.Threading;
//using Autofac.Core;

namespace Airion.Parallels.Internal
{
	public abstract class BaseScheduledTask : IScheduledTask
	{
		private volatile bool _isFinished;
		private ManualResetEventSlim _finishedHandle;
		private readonly object _finishLock = new object();
		private readonly CancellationToken _cancellationToken;
		
		public BaseScheduledTask(CancellationToken token)
		{
			_cancellationToken = token;
			_isFinished = false;
		}
		
		#if DEBUG
		
		~BaseScheduledTask()
		{
			Debug.Assert(_finishedHandle == null);
		}
		
		#endif
		
		public void Execute()
		{
			if(IsCancelled) {
				Finish();
			} else {
				try {
					ExecuteTask();
				} catch (OperationCanceledException) {
				} finally {
					Finish();
				}
			}
		}
		
		public void Close()
		{
			// dispose of resources
			if(_finishedHandle != null) {
				_finishedHandle.Dispose();
				_finishedHandle = null;
			}
		}
		
		protected abstract void ExecuteTask();
		
		protected abstract void WaitCompleted();
		
		public void Wait()
		{
			if(!_isFinished) {
				FinishedHandleSlim.Wait();
				Close();
				Debug.Assert(_isFinished);
			}
			WaitCompleted();
		}
		
		private void Finish()
		{
			if(_finishedHandle == null) {
				lock(_finishLock) {
					_isFinished = true;
					if(_finishedHandle != null) {
						_finishedHandle.Set();
					} else {
						// nobody is waiting on the task and ever will so it's safe to dispose
						Close();
					}
				}
			} else {
				_isFinished = true;
				_finishedHandle.Set();
			}
		}
		
		public bool IsCancelled {
			get { return _cancellationToken.IsCancellationRequested; }
		}
		
		public bool IsFinished {
			get { return _isFinished; }
		}
		
		private ManualResetEventSlim FinishedHandleSlim
		{
			get {
				if(_finishedHandle == null) {
					lock(_finishLock) {
						if(_finishedHandle == null) {
							_finishedHandle = new ManualResetEventSlim(_isFinished);
						}
					}
				}
				return _finishedHandle;
			}
		}
		
		public WaitHandle FinishedHandle {
			get {
				return FinishedHandleSlim.WaitHandle;
			}
		}
	}
	
	public abstract class BaseScheduledTask<TResult> : BaseScheduledTask, IScheduledTask<TResult>
	{
		public BaseScheduledTask(CancellationToken cancellationToken)
			: base(cancellationToken)
		{
		}
		
		private TResult _result;
		
		public TResult Result {
			get {
				Wait();
				return _result;
			}
			protected set { _result = value; }
		}
	}
	
}
