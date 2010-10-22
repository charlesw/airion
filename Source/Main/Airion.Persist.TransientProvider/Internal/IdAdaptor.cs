// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Airion.Common;
using Airion.Persist.Provider;

namespace Airion.Persist.TransientProvider.Internal
{
	public class IdAdaptor
	{
		private PropertyInfo _propertyInfo;
		private object _defaultValue;
		
		public IdAdaptor(PropertyInfo propertyInfo, IIdGenerator generator)
		{
			_propertyInfo = propertyInfo;
			IdGenerator = generator;
			_defaultValue = PropertyType.GetDefaultValue();
		}
		
		/// <summary>
		/// Gets the property type of the id property.
		/// </summary>
		public Type PropertyType
		{
			get { return _propertyInfo.PropertyType; }
		}
		
		/// <summary>
		/// Gets the value of the specified entity's id property.
		/// </summary>
		public object GetId(object entity)
		{
			return _propertyInfo.GetValue(entity, null);
		}
		
		/// <summary>
		/// Sets the specified entity's id property to the specified value.
		/// </summary>
		public void SetId(object entity, object value)
		{
			_propertyInfo.SetValue(entity, value, null);
		}
		
		/// <summary>
		/// Gets a value indicating if the id has been assigned.
		/// </summary>
		public bool IsIdAssigned(object entity) 
		{
			var id = GetId(entity);
			return !Object.Equals(id, _defaultValue);
		}
		
		public IIdGenerator IdGenerator { get; private set; }
	}
}
