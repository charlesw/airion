// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of IPersistenceProvider.
	/// </summary>
	public interface IPersistenceProvider : IDisposable
	{
		ISession OpenSession();
	}
}
