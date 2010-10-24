// Author: Charles Weld
// Date: 20/03/2010 3:06 PM

using System;
using System.Threading;

namespace Airion.Parallels.Internal
{
	public interface IPendingWorkCollection<T> : IDisposable
	{		
		/// <summary>
		/// Adds an item to the collection targeting a specific worker.
		/// </summary>
		/// <param name="workerId">The worker that should retrieve the item, Guid.Empty if any worker can.</param>
		/// <param name="item">The item to add.</param>
		void Send(T item);
				
		/// <summary>
		/// Attempts to retrieve an item from the collection.
		/// </summary>
		/// <param name="item">The item retrieved from the collection.</param>
		/// <returns></returns>
		bool TryRetrieve(out T item);
		
		T Retrieve(CancellationToken cancellationToken);
		
		/// <summary>
		/// Blocks the calling thread until the collection is empty.
		/// </summary>
		void Wait(CancellationToken cancellationToken);
				
		int Count { get; }		
	}
}
