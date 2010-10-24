// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Airion.Common;
using Airion.Testing;

namespace Airion.Parallels.Examples.ChatRoom
{
	public class Participant : LightDisposableBase
	{
		private string _name;
		
		private EventTracker<ChatMessageEventArgs> _onRecievedMessageTracker;
		private EventTracker<ChatMessageEventArgs> _onInterceptTracker;
		private EventTracker<EventArgs> _onEventTracker;
		
		private readonly Object _syncHandle = new Object();
		
		public Participant(string name)
		{
			_name = name;
			_onRecievedMessageTracker = new EventTracker<ChatMessageEventArgs>();
			_onInterceptTracker = new EventTracker<ChatMessageEventArgs>();
			_onEventTracker = new EventTracker<EventArgs>();
		}
		
		public void OnRecieveEvent(object sender, EventArgs args)
		{
			lock(_syncHandle) {
				_onEventTracker.OnFired(sender, args);
			}			
		}
		
		public void OnRecieveMessage(object sender, ChatMessageEventArgs args)
		{
			lock(_syncHandle) {
				_onRecievedMessageTracker.OnFired(sender, args);
			}
		}
				
		public void InterceptMessage(object sender, ChatMessageEventArgs args)
		{
			lock(_syncHandle) {			
				var argClone = new ChatMessageEventArgs(args.Message);
				_onInterceptTracker.OnFired(sender, argClone);	
				args.Message = "Message has been intercepted.";
			}
		}
		
		public bool HasReceivedEventOnce(object sender, EventArgs args)
		{
			lock(_syncHandle) {				
				return _onEventTracker.WasFiredOnce(sender, args);
			}
		}
		
		public bool HasReceivedMessageOnce(object sender, ChatMessageEventArgs args)
		{
			lock(_syncHandle) {				
				return _onRecievedMessageTracker.WasFiredOnce(sender, args);
			}
		}
		
		public bool HasInterceptedMessageOnce(object sender, ChatMessageEventArgs args)
		{
			lock(_syncHandle) {
				return _onInterceptTracker.WasFiredOnce(sender, args);
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				_onInterceptTracker.Dispose();
				_onRecievedMessageTracker.Dispose();
				_onEventTracker.Dispose();
			}
			base.Dispose(disposing);
		}
		
		public string Name
		{
			get { return _name; }
		}
	}
}
