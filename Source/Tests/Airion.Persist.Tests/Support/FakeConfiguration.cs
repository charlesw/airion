// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of FakeConfiguration.
	/// </summary>
	public class FakeConfiguration : IConfiguration
	{
		public FakeConfiguration()
		{
		}
		
		public IPersistenceProvider BuildProvider()
		{
			return new FakePersistenceProvider();
		}
		
		public IValueStore<IConversation> BuildValueStore()
		{
			return new CallLocalValueStore<IConversation>();
		}
	}
}
