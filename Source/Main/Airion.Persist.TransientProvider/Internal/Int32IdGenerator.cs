// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.TransientProvider.Internal
{
	public class Int32IdGenerator : IIdGenerator
	{
		int _idCount = 1;
		public object NextId()
		{
			return _idCount++;
		}
	}
}
