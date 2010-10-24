// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using System.Threading;
using Airion.Common;

namespace Airion.Testing
{
	public class EventTracker<TEventArgs> : LightDisposableBase
		where TEventArgs : EventArgs
	{
		private readonly ICollection<Tuple<object, TEventArgs>> _firedEventSignitures;
		private readonly object _syncHandle;
		private AutoResetEvent _eventFired;
		
		public EventTracker()
		{
			_syncHandle = new object();
			_firedEventSignitures = new List<Tuple<object, TEventArgs>>();
			_eventFired = new AutoResetEvent(false);
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing) {
				_eventFired.Dispose();
				_eventFired = null;
			}
		}
		
		public void OnFired(object sender, TEventArgs args)
		{
			CheckState();
			var eventSig = Tuple.Create<object, TEventArgs>(sender, args);
			lock(_syncHandle) {
				_firedEventSignitures.Add(eventSig);
			}
			_eventFired.Set();
		}
		
		public void Reset()
		{
			CheckState();
			lock(_syncHandle) {
				_firedEventSignitures.Clear();
			}
		}
		
		public bool WasFiredOnce(object source, TEventArgs args)
		{
			CheckState();
			lock(_syncHandle) {
				return GetFiredCount(source, args) == 1;
			}
		}
		
		public int GetFiredCount(object source, TEventArgs args)
		{
			CheckState();
			var eventSig = Tuple.Create<object, TEventArgs>(source, args);
			
			lock(_syncHandle) {
				// count instances
				int count = 0;
				foreach (var firedEventSig in _firedEventSignitures) {
					if(Object.Equals(firedEventSig, eventSig)) {
						count++;
					}
				}
				return count;
			}
		}
		
		/// <summary>
		/// Waits until the EventTracker recieves the specified event.
		/// </summary>
		/// <param name="source">The event's source paramater.</param>
		/// <param name="args">The event's arguments.</param>
		/// <returns>Returns <c>True</c> if the event was recieved; otherwise <c>False</c>.</returns>
		public bool WaitUntilFired(object source, TEventArgs args)
		{
			return WaitUntilFired(source, args, -1);
		}
		
		/// <summary>
		/// Waits until the EventTracker recieves the specified event.
		/// </summary>
		/// <param name="source">The event's source paramater.</param>
		/// <param name="args">The event's arguments.</param>
		/// <param name="timeout">The period of time to wait for the event to occur (-1 for infinity).</param>
		/// <returns>Returns <c>True</c> if the event was recieved; otherwise <c>False</c>.</returns>
		public bool WaitUntilFired(object source, TEventArgs args, int timeout)
		{
			CheckState();
			bool success = true;
			while(success && GetFiredCount(source, args) == 0) {
				success = _eventFired.WaitOne(timeout);
			}
			return success;
		}
	}
}
