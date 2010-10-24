// Author: Charles Weld
// Date: 20/03/2010 3:12 PM

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Airion.Parallels.Internal
{	
	/// <summary>
	/// Description of PendingWorkQueue.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size=PlatformConstants.CacheLineSize*4, Pack=PlatformConstants.CacheLineSize)]
	public class PendingWorkQueue<T> : IPendingWorkCollection<T>
	{		
		[StructLayout(LayoutKind.Sequential, Size=PlatformConstants.CacheLineSize)]
		private class Node
		{
			public volatile Node _next;
			public T _value;
			
			public Node(T value)
			{
				_value = value;
			}
			
			public static Node Empty {
				get { return new Node(default(T)); }
			}
		}
		
		private Node _first;
		
		private int _consumerLock;
		
		private ManualResetEventSlim _retrieveWaitHandle;
		
		private Node _last;
		
		private int _producerLock; // 0 means lock is not held, 1 means it is
		
		private ManualResetEventSlim _addWaitHandle;
		
		private int _count;
		
		public PendingWorkQueue()
		{
			_first = _last = Node.Empty;
			_producerLock = _consumerLock = 0;
			_count = 0;
			_addWaitHandle = new ManualResetEventSlim(false);
			_retrieveWaitHandle = new ManualResetEventSlim(false);
		}
		
		public void Dispose()
		{
			// TODO: Dispose logic
		}
		
		/// <inheritdoc />
		public void Send(T item)
		{
			Node node = new Node(item);
			while(Interlocked.Exchange(ref _producerLock, 1) == 1)	// acquire exclusivity
			{}
			
			_last._next = node;											// pubulish to consumers
			_last = node;													// swing last forward
			Interlocked.Increment(ref _count);
			Thread.VolatileWrite(ref _producerLock, 0); 					// release exclusivity (producer)
			
			// signal change
			_addWaitHandle.Set();
		}

		/// <inheritdoc />
		public void Wait(CancellationToken cancellationToken)
		{
			while(Thread.VolatileRead(ref _count) > 0) {
				_retrieveWaitHandle.Wait(cancellationToken);
				_retrieveWaitHandle.Reset();
			}
		}

		/// <inheritdoc />
		public bool TryRetrieve(out T item)
		{
			while(Interlocked.Exchange(ref _consumerLock, 1) == 1)	{}	// acquire exclusivity (consumer)
			
			bool success;
			Node firstNode = _first;
			Node nextNode = _first._next;
			if(nextNode != null) {
				item = nextNode._value;									// get value
				_first = nextNode;											// swing first forward
				Interlocked.Decrement(ref _count);						// decrement count
				Thread.VolatileWrite(ref _consumerLock, 0);				// release exclusivity (consumer)
				_retrieveWaitHandle.Set();										// notify change
				success = true;
			} else {
				Thread.VolatileWrite(ref _consumerLock, 0);				// release exclusivity (consumer)
				item = default(T);
				success = false;
			}
			
			return success;
		}
		
		public T Retrieve(CancellationToken cancellationToken)
		{
			T item;
			while(!TryRetrieve(out item)) {
				_addWaitHandle.Wait(cancellationToken);
				_addWaitHandle.Reset();				
			}
			return item;
		}
		
		public int Count
		{
			get
			{
				return Thread.VolatileRead(ref _count);
			}
		}
	}
}
