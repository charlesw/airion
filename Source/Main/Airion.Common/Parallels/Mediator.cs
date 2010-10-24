// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Airion.Common;
using Airion.Parallels.Internal;

namespace Airion.Parallels
{
	public class Mediator : IMediator
	{
		private readonly object _syncHandle = new object();
		private readonly Dictionary<Type, IEventGroup> _eventGroups;
		
		public Mediator()
		{
			_eventGroups = new Dictionary<Type, IEventGroup>();
		}
		
		public void Register<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("eventHandler", eventHandler);
			
			EventGroup<TEventArgs> typedEventGroup;
			if(!TryResolveEventGroup(out typedEventGroup)) {
				typedEventGroup = new EventGroup<TEventArgs>();
				AddEventGroup(typedEventGroup);
			}
			
			typedEventGroup.Handler += eventHandler;
		}
		
		public void RegisterFinalizer<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("eventHandler", eventHandler);
			
			EventGroup<TEventArgs> typedEventGroup;
			if(!TryResolveEventGroup(out typedEventGroup)) {
				typedEventGroup = new EventGroup<TEventArgs>();
				AddEventGroup(typedEventGroup);
			}
			
			typedEventGroup.Finalizers += eventHandler;
		}
		
		public void Deregister<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("eventHandler", eventHandler);
			
			EventGroup<TEventArgs> typedEventGroup;
			if(TryResolveEventGroup(out typedEventGroup)) {
				typedEventGroup.Handler -= eventHandler;
			}
		}
		
		public void DeregisterFinalizer<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("eventHandler", eventHandler);
			
			EventGroup<TEventArgs> typedEventGroup;
			if(TryResolveEventGroup(out typedEventGroup)) {
				typedEventGroup.Finalizers -= eventHandler;
			}
		}
		
		public bool IsRegistered<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("eventHandler", eventHandler);
			
			bool isRegistered;
			EventGroup<TEventArgs> typedEventGroup;
			if(TryResolveEventGroup(out typedEventGroup)) {
				isRegistered = typedEventGroup.IsRegistered(eventHandler);
			} else {
				isRegistered = false;
			}
			return isRegistered;
		}
		
		public void Post<TEventArgs>(object source, TEventArgs args)
			where TEventArgs : EventArgs
		{
			Guard.RequireNotNull("source", source);
			Guard.RequireNotNull("args", args);
						
			// find event groups of the specified type or base type.
			List<IEventGroup> eventGroups = new List<IEventGroup>();
			lock(_syncHandle) {
				// note: We don't need to add a special case for the 'object'
				// base type since no handler can be registered for this type.
				Type eventType = typeof(TEventArgs);
				while(eventType != null) {
					IEventGroup eventGroup;
					if(_eventGroups.TryGetValue(eventType, out eventGroup)) {
						eventGroups.Add(eventGroup);
					}
					eventType = eventType.BaseType;
				}
			}
			
			// invoke event groups
			foreach (var eventGroup in eventGroups) {
				eventGroup.Invoke(source, args);
			}
		}
		
		private bool TryResolveEventGroup<TEventArgs>(out EventGroup<TEventArgs> eventGroup)
			where TEventArgs : EventArgs
		{
			lock(_syncHandle) {
				bool success;
				IEventGroup untypedEventGroup;
				if(_eventGroups.TryGetValue(typeof(TEventArgs), out untypedEventGroup)) {
					eventGroup = untypedEventGroup as EventGroup<TEventArgs>;
					success = true;
				} else {
					eventGroup = null;
					success = false;
				}
				return success;
			}
		}
		
		public bool IsEmpty()
		{
			lock(_syncHandle) {
				bool isEmpty = true;
				foreach(var eventGroup in _eventGroups) {
					if(!eventGroup.Value.IsEmpty()) {
						isEmpty = false;
						break;
					}
				}
				return isEmpty;
			}
		}
		
		private void AddEventGroup<TEventArgs>(EventGroup<TEventArgs> eventGroup)
			where TEventArgs : EventArgs
		{
			lock(_syncHandle) {
				_eventGroups.Add(typeof(TEventArgs), eventGroup);
			}
		}
	}
}
