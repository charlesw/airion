// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Persist
{
	/// <summary>
	/// Description of Conversation.
	/// </summary>
	public class Conversation : DisposableBase, IConversation
	{
		public Conversation()
		{
		}
		
		public T Get<T>(object id)
		{
			throw new NotImplementedException();
		}
		
		public System.Linq.IQueryable<T> Linq<T>()
		{
			throw new NotImplementedException();
		}
		
		public void Update<T>(T entity)
		{
			throw new NotImplementedException();
		}
		
		public void Save<T>(T entity)
		{
			throw new NotImplementedException();
		}
		
		public void SaveOrUpdate<T>(T entity)
		{
			throw new NotImplementedException();
		}
		
		public void Delete<T>(T entity)
		{
			throw new NotImplementedException();
		}
	}
}
