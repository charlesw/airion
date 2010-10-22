// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.Tests.Support
{
	/// <summary>
	/// Description of FakeSession.
	/// </summary>
	public class FakeSession : LightDisposableBase, ISession
	{		
		public IPersistenceProvider PersistenceProvider { get; private set; }
		
		public FakeSession(FakePersistenceProvider provider)
		{
			PersistenceProvider = provider;
		}
		
		public ITransaction BeginTransaction()
		{
			return new FakeTransaction();
		}
		
		public T Get<T>(object id)
		{
			return default(T);
		}
		
		public IQueryable<T> Linq<T>()
		{
			return new List<T>().AsQueryable();
		}
		
		public void Update<T>(T entity)
		{
			
		}
		
		public void Save<T>(T entity)
		{
		}
		
		public void SaveOrUpdate<T>(T entity)
		{
		}
		
		public void Delete<T>(T entity)
		{
		}
		
		public void Flush()
		{
			throw new NotImplementedException();
		}
	}
}
