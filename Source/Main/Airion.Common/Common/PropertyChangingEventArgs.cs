// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common
{
	/// <summary>
	/// Provides data for the <see cref="INotifyPropertyChanging.PropertyChanging" /> event.
	/// </summary>
	public class PropertyChangingEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyChangingEventArgs" /> class.
		/// </summary>
		/// <param name="propertyName">The name of property that's changing.</param>
		/// <param name="oldValue">The old\current value of the property.</param>
		/// <param name="newValue">The expected new value of the property.</param>
		public PropertyChangingEventArgs(string propertyName, object oldValue, object newValue)
		{
			PropertyName = propertyName;
			OldValue = oldValue;
			NewValue = newValue;
		}
		
		/// <summary>
		/// Gets the name of the property that caused the <see cref="INotifyPropertyChanging.PropertyChaning" /> event.
		/// </summary>
		public string PropertyName { get; private set; }
		
		/// <summary>
		/// Gets the old\current value of the property.
		/// </summary>
		public object OldValue { get; private set; }
		
		/// <summary>
		/// Gets the expected new value of the property.
		/// </summary>
		public object NewValue { get; private set; }
	}
}
