// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.TransientProvider
{
	/// <summary>
	/// Description of TransientTransaction.
	/// </summary>
	public class TransientTransaction : LightDisposableBase, ITransaction
	{
		public TransientTransaction()
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
