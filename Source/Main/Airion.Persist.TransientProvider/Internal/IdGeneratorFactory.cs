// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.TransientProvider.Internal
{
	/// <summary>
	/// Description of IdGeneratorFactory.
	/// </summary>
	public static class IdGeneratorFactory
	{
		public static IIdGenerator Build(Type idType)
		{
			if(idType == typeof(Int32)) {
				return new Int32IdGenerator();
			} else if(idType == typeof(Guid)) {
				return new GuidIdGenerator();
			} else {
				throw new NotSupportedException(String.Format("The type {0} is not supported.", idType.Name));
			}
		}
	}
}
