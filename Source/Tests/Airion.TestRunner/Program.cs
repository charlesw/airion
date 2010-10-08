// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.TestRunner
{
	/// <summary>
	/// Description of Program.
	/// </summary>
	public static class Program
	{
		public static void Main(string[] args)
		{
			// tests
			
			Pause();
		}
		
		private static void Pause()
		{
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}
