// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;
using Airion.Persist.Provider;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Airion.Persist.Internal
{
	/// <summary>
	/// Description of TransientSessionAndTransactionManager.
	/// </summary>
	public class TransientSessionAndTransactionManager : SessionAndTransactionManager
	{
		public TransientSessionAndTransactionManager(IPersistenceProvider provider)
			: base(provider)
		{
		}
		
		protected override void OnBeginTransaction(ITransaction transaction)
		{			
			using(var standardOutput = new StreamWriter(Console.OpenStandardOutput())) {
				 new SchemaExport(Provider.Configuration).Execute(false, true, false, Session.Connection, standardOutput);
			}
			base.OnBeginTransaction(transaction);
		}
	}
}
