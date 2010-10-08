// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	public static class ArrayUtilities
	{
		public static void FillRange<T>(T[] array, int startIndex, int lastIndex, T value)
		{
			Guard.RequireNotNull("array", array);
			Guard.RequireBetween("startIndex", startIndex, 0, array.Length, true, false);
			Guard.RequireBetween("lastIndex", lastIndex, 0, array.Length, true, false);
			
			for (int i=startIndex;i<=lastIndex;i++) {
				array[i] = value;
			}
		}
	}
}
