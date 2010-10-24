// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Parallels.Internal
{
	/// <summary>
	/// Description of PlatformConstants.
	/// </summary>
	public static class PlatformConstants
	{
		/// <summary>
		/// The (assumed) cache line size, in bytes, this should be pessimistic for current mainstream processors.
		/// </summary>
		public const int CacheLineSize = 16; 
	}
}
