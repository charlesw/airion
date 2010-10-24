// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Linq;
using Airion.Common;

namespace Airion.Parallels.Internal
{
	public interface IEventGroup
	{		
		bool IsEmpty();
		
		void Invoke(object source, EventArgs args);
	}
	
	public class EventGroup<TEventArgs> : IEventGroup
		where TEventArgs: EventArgs
	{
		private EventHandler<TEventArgs> _eventHandler;
		private EventHandler<TEventArgs> _finalizers;
		private readonly object _syncRoot = new object();
		
		public EventGroup()
		{
		}
				
		public event EventHandler<TEventArgs> Handler
		{
			add {
				lock(_syncRoot) {
					_eventHandler += value;
				}
			}
			remove {
				lock(_syncRoot) {
					_eventHandler -= value;
				}
			}
		}
		
		public event EventHandler<TEventArgs> Finalizers
		{
			add {
				lock(_syncRoot) {
					_finalizers += value;
				}
			}
			remove {
				lock(_syncRoot) {
					_finalizers -= value;
				}
			}
		}
		
		public void Invoke(object source, TEventArgs args)
		{
			EventHandler<TEventArgs> eventHandlers;
			EventHandler<TEventArgs> finalizers;
			lock(_syncRoot) {
				if(_eventHandler != null) {
					eventHandlers = (EventHandler<TEventArgs>)_eventHandler.Clone();
				} else {
					eventHandlers =null;
				}
				
				if(_finalizers != null) {
					finalizers = (EventHandler<TEventArgs>)_finalizers.Clone();
				} else {
					finalizers = null;
				}
			}
			
			if(_eventHandler != null) {
				eventHandlers(source, args);
			}
			
			if(finalizers != null) {
				finalizers(source, args);
			}
		}
		
		public bool IsRegistered(EventHandler<TEventArgs> handler)
		{
			lock(_syncRoot) {
				return IsHandlerRegistered(handler) || IsFinalizerRegistered(handler);
			}
		}
		
		public bool IsEmpty()
		{
			lock(_syncRoot) {
				return _eventHandler == null && _finalizers == null;
			}
		}
		
		private bool IsHandlerRegistered(EventHandler<TEventArgs> handler)
		{
			bool isRegistered = false;
			if(_eventHandler != null) {
				var handlers = _eventHandler.GetInvocationList();
				isRegistered = handlers.Contains((Delegate)handler);
			}
			return isRegistered;
		}
		
		private bool IsFinalizerRegistered(EventHandler<TEventArgs> handler)
		{
			bool isRegistered = false;
			if(_finalizers != null) {
				var finalizers = _finalizers.GetInvocationList();
				isRegistered = finalizers.Contains((Delegate)handler);
			}
			return isRegistered;
		}
		
		void IEventGroup.Invoke(object source, EventArgs args)
		{
			Invoke(source, (TEventArgs)args);
		}
	}
}
