/*
 * Created by SharpDevelop.
 * User: Charles
 * Date: 23/10/2010
 * Time: 08:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Airion.Persist.Provider
{
	/// <summary>
	/// Description of FlushMode.
	/// </summary>
	public enum FlushMode
	{
		Always = 20,
	    Auto = 10,
	    Commit = 5,
	    Never = 0,
	    Unspecified = -1
	}
}
