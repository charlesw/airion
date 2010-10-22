// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.TransientProvider
{
	/// <summary>
	/// Description of TransientConfiguration.
	/// </summary>
	public class TransientConfiguration : IConfiguration
	{
		public TransientConfiguration()
		{
		}
		
		public IPersistenceProvider BuildProvider()
		{
			return new TransientPersistenceProvider();
		}
		
		public IValueStore<IConversation> BuildValueStore()
		{
			return new CallLocalValueStore<IConversation>();
		}
	}
}
