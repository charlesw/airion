// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Runtime.Remoting.Messaging;

namespace Airion.Common
{
	/// <summary>
	/// Description of CallLocalValueStore.
	/// </summary>
	public class CallLocalValueStore<T> : IValueStore<T>
	{
		private string _id;
		
		public CallLocalValueStore()
		{
			_id = Guid.NewGuid().ToString();
		}
		
		public T Value {
			get {
				return (T)CallContext.GetData(_id);
			}
			set {
				CallContext.SetData(_id, value);
			}
		}
	}
}
