// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;

namespace Airion.Common
{
	public interface IFreezable
	{
		bool IsFrozen { get; }
		
		void Freeze();
		
		object Thaw();
	}
}
