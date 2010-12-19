// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Common.Tests.Support.Examples
{
	public class Person : INotifyPropertyChanging
	{
		private string name;

		public string Address { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string Name {
			get { return name; }
			set {
				if(name != value) {
					NotifyPropertyChanged("Name", name, value);
					name = value;
				}
			}
		}
		
		private void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
		{
			if(PropertyChanging != null) {
				PropertyChanging(this, new PropertyChangingEventArgs(propertyName, oldValue, newValue));
			}
		}
		
		public event EventHandler<PropertyChangingEventArgs> PropertyChanging;
	}
}
