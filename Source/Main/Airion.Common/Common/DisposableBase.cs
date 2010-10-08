// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Diagnostics;

namespace Airion.Common
{
	/// <summary>
	/// Description of DisposableBase.
	/// </summary>
	public class DisposableBase : IDisposable, INotifyDisposed, INotifyDisposing
	{
		#region Constructors 

		public DisposableBase()
		{
		}

		~DisposableBase()
		{
			string creationTrace = _creationTrace.ToString();
			Dispose(false);
			Debug.Fail(String.Format("{0} should have been disposed of.", this.GetType().Name));
		}

		#endregion Constructors 

		#region Properties 

		public bool IsDisposed {
			get;
			private set;
		}

		public bool IsDisposing {
			get; 
			private set;
		}

		#endregion Properties 

		#region Delegates and Events 

		// Events 

		public event EventHandler<EventArgs> Disposed;

		public event EventHandler<EventArgs> Disposing;

		// Event handlers 

		private void OnDisposed(EventArgs e)
		{
			IsDisposed = true;
			IsDisposing = false;
			
			if(Disposed != null) {
				Disposed(this, e);
			}
		}

		private void OnDisposing(EventArgs e)
		{
			IsDisposing = true;
			
			if(Disposing != null) {
				Disposing(this, e);
			}
		}

		#endregion Delegates and Events 

		#region Methods 

		// Public Methods 

		/// <summary>
		/// Releases all reasource used by the <see cref="ResourceBase"/>.
		/// </summary>
		public void Dispose()
		{
			OnDisposing(EventArgs.Empty);
			
			Dispose(true);
			GC.SuppressFinalize(this);
			
			OnDisposed(EventArgs.Empty);
		}

		// Protected Methods 

		protected void CheckState()
		{
			if(IsDisposing || IsDisposed) {
				throw new ObjectDisposedException(ToString(), "The object is either being disposed off or already disposed.");
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
		
		private StackTrace _creationTrace = new StackTrace();
	}
}
