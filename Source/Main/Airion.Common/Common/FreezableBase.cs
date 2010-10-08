// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;

namespace Airion.Common
{
	/// <summary>
	/// Description of FreezableBase.
	/// </summary>
	public abstract class FreezableBase : IFreezable
	{
		private bool isFrozen = false;
		
		public FreezableBase()
		{
		}
		
		public bool IsFrozen {
			get {
				return isFrozen;
			}
		}
		
		public void Freeze()
		{
			isFrozen = true;
			OnFreeze();
		}
		
		protected virtual void OnFreeze()
		{
			
		}
		
		public object Thaw()
		{
			FreezableBase clone = (FreezableBase)this.MemberwiseClone();
			clone.isFrozen = false;
			TransferMembers(clone);
			return clone;
		}
		
		protected virtual void TransferMembers(FreezableBase clone)
		{
			
		}
		
		protected void CheckFrozenState() 
		{
			Guard.Operation(!isFrozen, "Cannot modify a frozen object.");
		}	
	}
}
