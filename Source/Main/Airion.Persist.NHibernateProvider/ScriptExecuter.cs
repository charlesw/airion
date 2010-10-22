// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;

namespace Airion.Persist.NHibernateProvider
{
	/// <summary>
	/// Description of ScriptExecuter.
	/// </summary>
	public class ScriptExecuter
	{
		public NHibernateSession Session { get; private set; }
		
		public ScriptExecuter(NHibernateSession session)
		{
			Session = session;
		}
		
		// TODO: Break script down into individual commands & execute.
		public void Execute(TextReader script)
		{
			var nhSession = Session.Session;
			using(var transaction = nhSession.BeginTransaction()) {
				var connection = nhSession.Connection;
				var command = connection.CreateCommand();
				transaction.Enlist(command);
				command.CommandText = script.ReadToEnd();
				command.ExecuteNonQuery();
				
				transaction.Commit();
			}
		}
	}
}
