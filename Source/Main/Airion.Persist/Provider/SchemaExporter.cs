// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.IO;
using NHibernate.Tool.hbm2ddl;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of SchemaExporter.
	/// </summary>
	public class SchemaExporter
	{
		public NHibernateProvider Provider { get; private set; }
		
		public SchemaExporter(NHibernateProvider provider)
		{
			Provider = provider;
		}
		
		public void Export(TextWriter exportOutput)
		{
			var config = Provider.Configuration;
			new SchemaExport(config).Execute(false, false, false, null, exportOutput);
		}
	}
}
