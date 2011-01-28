// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Description of Entity.
	/// </summary>
	public class Entity<TId>
	{
		public virtual TId Id { get; protected set; }
		
		public static TEntity Create<TEntity>(TId id)
			where TEntity : Entity<TId>, new()
		{
			var entity = new TEntity();
			entity.Id = id;
			return entity;
		}
		
	}
}
