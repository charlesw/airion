// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	public abstract class Cloneable : ICloneable
	{
		public Cloneable()
		{
		}
				
		public object Clone()
		{
			Cloneable clonedObject = (Cloneable)this.MemberwiseClone();
			TransferMembers(clonedObject);
			return clonedObject;
		}
		
		protected virtual void TransferMembers(Cloneable clone)
		{
			
		}
		
	}
}
