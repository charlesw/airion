// Author: Charles Weld
// Date: 18/03/2010 12:05 PM

using System;
using System.Threading;
using NUnit.Framework;

namespace Airion.Parallels.Internal.Tests
{
	[TestFixture]
	public class QueuedPendingWorkSpec : PendingWorkSpec
	{
		protected override IPendingWorkCollection<int> CreatePendingWorkCollection()
		{
			return new PendingWorkQueue<int>();
		}
	}
}
