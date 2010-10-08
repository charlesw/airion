// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Airion.Common
{
	/// <summary>
	/// Description of ReflectionUtilities.
	/// </summary>
	public static class ReflectionUtilities
	{
		public static bool ImplementsInterface<T>(this Type type)
		{
			Type interfaceType = typeof(T);
			Guard.Require( "type", interfaceType.IsInterface,"The type \"{0}\" must be a interface.", type.Name);
			
			return type.GetInterfaces().Contains(interfaceType);
		}
		
		public static bool IsAssignableTo<T>(this Type type)
		{
			Type targetType = typeof(T);
			return IsAssignableTo(type, targetType);
		}

		/// <summary>
		/// Determines whether the specified type is assignable to the specified target type.
		/// </summary>
		public static bool IsAssignableTo(this Type type, Type targetType)
		{
			return targetType.IsAssignableFrom(type);
		}
		

		/// <summary>
		/// Determines whether the type is assignable from the specified target type (T).
		/// </summary>
		public static bool IsAssignableFrom<T>(this Type type)
		{
			Type sourceType = typeof(T);
			return type.IsAssignableFrom(sourceType);
		}

		public static List<System.Reflection.FieldInfo> GetInstanceFields(this Type type)
		{
			List<FieldInfo> fields = new List<FieldInfo>();
			while (type != null) {
				fields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
				type = type.BaseType;
			}
			return fields;
		}

		public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit)
			where T: Attribute
		{
			object[] attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
			T[] result = new T[attributes.Length];
			for (int i = 0; i < result.Length; ++i) {
				result[i] = (T)attributes[i];
			}
			return result;
		}

		public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit)
			where T : Attribute
		{
			return GetAttributes<T>(memberInfo, inherit).FirstOrDefault();
		}

		public static string GetAssemblyQualifiedName(string typeName, string assemblyName)
		{
			return String.Format("{0}, {1}", typeName, assemblyName);
		}
	}
}
