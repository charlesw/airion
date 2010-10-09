// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of FakeTransaction.
	/// </summary>
	public class FakeTransaction : LightDisposableBase, ITransaction
	{
		public FakeTransaction()
		{
		}
		
		public void Commit()
		{
		}
		
		public void Rollback()
		{
		}
	}
}
