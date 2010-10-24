// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Diagnostics;

namespace Airion.Testing
{
	/// <summary>
	/// Provides a set of utility methods to aid in performance testing.
	/// </summary>
	public static class PerformanceTestRunner
	{
		/// <summary>
		/// Executes the specified action a number of time, returning the average run time.
		/// </summary>
		/// <param name="count">The number of times to run the action.</param>
		/// <param name="action">The action to execute.</param>
		/// <returns>The average run time in milliseconds.</returns>
		public static long ExecuteAction(int count, Action action)
		{
			long totalRunTime=0;
			Stopwatch watch = new Stopwatch();
			for (int i = 0; i < count; i++) {
				watch.Reset();
				watch.Start();
				action();
				watch.Stop();
				totalRunTime += watch.ElapsedMilliseconds; 
			}
			return count > 0 ? totalRunTime / count : 0;
		}
	}
}
