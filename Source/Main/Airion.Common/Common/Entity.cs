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
		
		#region Equals and GetHashCode implementation
		
		public override bool Equals(object obj)
		{
			if(obj == null || obj.GetType() != GetType()) {
				return false;
			} else if(Object.ReferenceEquals(this, obj)) {
				return true;
			}
			
			var other = (Entity<TId>)obj;				
			return Object.Equals(Id, other.Id) && !Object.Equals(Id, default(TId));
		}
		
		public override int GetHashCode()
		{
			return GetType().GetHashCode() ^ Id.GetHashCode();
		}
		
		public static bool operator ==(Entity<TId> lhs, Entity<TId> rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(Entity<TId> lhs, Entity<TId> rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
		
	}
}
