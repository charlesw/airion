//// <file>
////     <copyright see="prj:///doc/copyright.txt"/>
////     <license see="prj:///doc/license.txt"/>
////     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
//// </file>
//
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading;
//using Airion.Common;
//using Airion.Parallels.Internal;
//
//namespace Airion.Parallels
//{
//	public class AsyncMediator : LightDisposableBase, IMediator
//	{
//		private readonly object _syncHandle = new object();
//		private readonly Dictionary<string, IEventGroup> _eventGroups;
//		private readonly ConcurrentQueue<Action> _pendingInvokes;
//		private Thread _workerThread;
//		private ManualResetEventSlim _enqueueSignal;
//		private ManualResetEventSlim _dequeueSignal;
//		
//		// error message constants
//		private const string NotTheSameEventHandlerTypeErrorMessage = "All event handlers in an event group must be the same type.";
//		
//				
//		public AsyncMediator()
//		{
//			_eventGroups = new Dictionary<string, IEventGroup>();
//			_pendingInvokes = new ConcurrentQueue<Action>();
//			_enqueueSignal = new ManualResetEventSlim(false);
//			_dequeueSignal = new ManualResetEventSlim(false);
//			_workerThread = new Thread(ExecuteActions);
//			_workerThread.Name = "AsyncMediator WorkerThread";
//			_workerThread.Start();
//		}
//		
//		public void Register<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNullOrEmpty("eventName", eventName);
//			Guard.RequireNotNull("eventHandler", eventHandler);
//			CheckState();
//			
//			EventGroup<TEventArgs> typedEventGroup;
//			if(!TryResolveEventGroup(eventName, out typedEventGroup)) {
//				typedEventGroup = new EventGroup<TEventArgs>(eventName);
//				AddEventGroup(typedEventGroup);
//			}
//			
//			typedEventGroup.Handler += eventHandler;
//		}
//		
//		public void RegisterFinalizer<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNullOrEmpty("eventName", eventName);
//			Guard.RequireNotNull("eventHandler", eventHandler);
//			CheckState();
//			
//			EventGroup<TEventArgs> typedEventGroup;
//			if(!TryResolveEventGroup(eventName, out typedEventGroup)) {
//				typedEventGroup = new EventGroup<TEventArgs>(eventName);
//				AddEventGroup(typedEventGroup);
//			}
//			
//			typedEventGroup.Finalizers += eventHandler;
//		}
//		
//		public void Deregister<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNullOrEmpty("eventName", eventName);
//			Guard.RequireNotNull("eventHandler", eventHandler);
//			CheckState();
//			
//			EventGroup<TEventArgs> typedEventGroup;
//			if(TryResolveEventGroup(eventName, out typedEventGroup)) {
//				typedEventGroup.Handler -= eventHandler;
//			}
//		}
//		
//		public void DeregisterFinalizer<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNullOrEmpty("eventName", eventName);
//			Guard.RequireNotNull("eventHandler", eventHandler);
//			CheckState();
//			
//			EventGroup<TEventArgs> typedEventGroup;
//			if(TryResolveEventGroup(eventName, out typedEventGroup)) {
//				typedEventGroup.Finalizers -= eventHandler;
//			}
//		}
//		
//		public bool IsRegistered<TEventArgs>(string eventName, EventHandler<TEventArgs> eventHandler)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNullOrEmpty("eventName", eventName);
//			Guard.RequireNotNull("eventHandler", eventHandler);
//			CheckState();
//			
//			bool isRegistered;
//			EventGroup<TEventArgs> typedEventGroup;
//			if(TryResolveEventGroup(eventName, out typedEventGroup)) {
//				isRegistered = typedEventGroup.IsRegistered(eventHandler);
//			} else {
//				isRegistered = false;
//			}
//			return isRegistered;
//		}
//		
//		public void Post<TEventArgs>(object source, TEventArgs args)
//			where TEventArgs : MessageEventArgs
//		{
//			Guard.RequireNotNull("source", source);
//			Guard.RequireNotNull("args", args);
//			
//			CheckState();
//						
//			// schedule the invocation
//			_pendingInvokes.Enqueue(
//				() => {
//					EventGroup<TEventArgs> eventGroup;
//					if(TryResolveEventGroup(args.EventName, out eventGroup)) {
//						eventGroup.Invoke(source, args);
//					}
//				});
//			_enqueueSignal.Set();
//		}
//		
//		public void Wait()
//		{
//			while(!_pendingInvokes.IsEmpty) {
//				_dequeueSignal.Wait();
//				_dequeueSignal.Reset();
//			}
//		}
//		
//		public bool IsEmpty()
//		{
//			bool isEmpty = true;
//			foreach(var eventGroup in _eventGroups) {
//				if(!eventGroup.Value.IsEmpty()) {
//					isEmpty = false;
//					break;
//				}
//			}
//			return isEmpty;
//		}
//		
//		protected override void Dispose(bool disposing)
//		{
//			if(disposing) {
//				Wait();
//				// wake up worker thread
//				_enqueueSignal.Set();
//				
//				_workerThread.Join();
//			}
//			base.Dispose(disposing);
//		}
//
//		private bool TryResolveEventGroup<TEventArgs>(string eventName, out EventGroup<TEventArgs> eventGroup)
//			where TEventArgs : MessageEventArgs
//		{
//			lock(_syncHandle) {
//				bool success;
//				IEventGroup untypedEventGroup;
//				if(_eventGroups.TryGetValue(eventName, out untypedEventGroup)) {
//					eventGroup = untypedEventGroup as EventGroup<TEventArgs>;
//					Guard.Operation(eventGroup != null, NotTheSameEventHandlerTypeErrorMessage);
//					success = true;
//				} else {
//					eventGroup = null;
//					success = false;
//				}
//				return success;
//			}
//		}
//		
//		private void AddEventGroup<TEventArgs>(EventGroup<TEventArgs> eventGroup)
//			where TEventArgs : MessageEventArgs
//		{
//			lock(_syncHandle) {
//				_eventGroups.Add(eventGroup.Name, eventGroup);
//			}
//		}
//		
//		private void ExecuteActions()
//		{
//			while(!IsDisposing) {
//				Action action;
//				if(_pendingInvokes.TryDequeue(out action)) {
//					action();
//					_dequeueSignal.Set();
//				} else {
//					_enqueueSignal.Wait();
//					_enqueueSignal.Reset(); // close signal
//				}
//			}
//		}
//	}
//}
//