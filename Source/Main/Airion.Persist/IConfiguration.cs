// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Internal;
using Airion.Persist.Provider;

namespace Airion.Persist
{
	/// <summary>
	/// Description of IConfiguration.
	/// </summary>
	public interface IConfiguration
	{
		IPersistenceProvider BuildProvider();
		
		IValueStore<IConversation> BuildValueStore();
		
		CreateSessionAndTransactionManager CreateSessionAndTransactionManager { get; }
	}
}
