// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Diagnostics;

namespace Airion.Common
{
	/// <summary>
	/// Description of LightDisposableBase.
	/// </summary>
	public class LightDisposableBase : IDisposable
	{
		#region Constructors 

		public LightDisposableBase()
		{
		}

		~LightDisposableBase()
		{
			Dispose(false);
			Debug.Fail(String.Format("{0} should have been disposed of.", this.GetType().Name));
		}

		#endregion Constructors 

		#region Methods 

		// Public Methods 

		/// <summary>
		/// Releases all reasource used by the <see cref="ResourceBase"/>.
		/// </summary>
		public void Dispose()
		{			
			Dispose(true);
			IsDisposed = true;
			GC.SuppressFinalize(this);			
		}

		// Protected Methods 

		protected void CheckState()
		{
			if(IsDisposed) {
				throw new ObjectDisposedException(ToString());
			}
		}

		/// <summary>
		/// Releases the unmanaged resource used by the <see cref="ResourceBase"/> and optionally releases
		/// the managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed andd nmanaged resources; <c>false</c>
		/// to release only the unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
		}


		#endregion Methods 

		#region Properties 

		public bool IsDisposed {
			get;
			private set;
		}

		#endregion Properties 
	}
}
