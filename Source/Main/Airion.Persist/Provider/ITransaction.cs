// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of ITransaction.
	/// </summary>
	public interface ITransaction : IDisposable
	{
		void Commit();
		void Rollback();
	}
}
