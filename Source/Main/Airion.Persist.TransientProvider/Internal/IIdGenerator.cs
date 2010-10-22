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
	public interface IIdGenerator
	{
		object NextId();
	}
}
