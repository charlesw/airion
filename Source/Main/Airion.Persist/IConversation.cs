// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Linq;

namespace Airion.Persist
{
	/// <summary>
	/// Description of IConversation.
	/// </summary>
	public interface IConversation : IDisposable
	{
		T Get<T>(object id);
		
		IQueryable<T> Linq<T>();
		
		void Update<T>(T entity);
		
		void Save<T>(T entity);
		
		void SaveOrUpdate<T>(T entity);
		
		void Delete<T>(T entity);
	}
}
