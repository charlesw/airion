// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Linq;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of ISession.
	/// </summary>
	public interface ISession : IDisposable
	{
		IPersistenceProvider PersistenceProvider { get; }
		
		ITransaction BeginTransaction();
		
		T Get<T>(object id);
		
		IQueryable<T> Linq<T>();
		
		void Update<T>(T entity);
		
		void Save<T>(T entity);
		
		void SaveOrUpdate<T>(T entity);
		
		void Delete<T>(T entity);
		
		void Flush();
	}
}
