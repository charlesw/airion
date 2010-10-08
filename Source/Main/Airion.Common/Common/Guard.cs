// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)
                
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Airion.Common
{
	/// <summary>
	/// Provides the ability to run standard conditional checks which if the specified condition is not met will
	/// throw either InvalidOperationException, NotSupportedException, or an ArgumentException.
	/// </summary>
	public static class Guard
	{
		#region Operation checks
		
        [DebuggerHidden]
		public static void Operation(bool condition) 
		{
			if(!condition) throw new InvalidOperationException();
		}

        [DebuggerHidden]
		public static void Operation(bool condition, string message) 
		{
			if(!condition) throw new InvalidOperationException(message);
		}

        [DebuggerHidden]
		public static void Operation(bool condition, string message, params object[] args)
		{
			if(!condition) throw new InvalidOperationException(String.Format(message, args));
		}
		
		#endregion
		
		#region Supported checks
		
		[DebuggerHidden]
		public static void Supported(bool condition) 
		{
			if(!condition) throw new NotSupportedException();
		}

        [DebuggerHidden]
		public static void Supported(bool condition, string message) 
		{
			if(!condition) throw new NotSupportedException(message);
		}

        [DebuggerHidden]
		public static void Supported(bool condition, string message, params object[] args)
		{
			if(!condition) throw new NotSupportedException(String.Format(message, args));
		}
		
		[DebuggerHidden]
		public static void NotSupported(string message) 
		{
			throw new NotSupportedException(message);
		}

		[DebuggerHidden]
		public static void NotSupported(string message, params object[] args)
		{
			throw new NotSupportedException(String.Format(message, args));
		}
		
		#endregion
		
		#region Argument checks

		[DebuggerHidden]
		public static void Require(string paramName, bool condition) 
		{
			if(!condition) throw new ArgumentException(string.Empty, paramName);
		}
		
		[DebuggerHidden]
		public static void RequirePositiveInteger(string paramName, int value) 
		{
			if(value < 0) throw new ArgumentOutOfRangeException(paramName, String.Format("The {0} must be positive.", paramName));
		}

        [DebuggerHidden]
		public static void Require(string paramName, bool condition, string message) 
		{
			if(!condition) throw new ArgumentException(message, paramName);
		}

        [DebuggerHidden]
		public static void Require(string paramName, bool condition, string message, params object[] args)
		{
			if(!condition) throw new ArgumentException(String.Format(message, args), paramName);
		}
		
        /// <summary>
        /// Throws an ArgumentNullException if argument is null.
        /// </summary>
        [DebuggerHidden]
        public static void RequireNotNull<T>(string paramName, T argument)
            where T: class
        {
            if (argument == null) {
                throw new ArgumentNullException(paramName,
        		                                String.Format(@"The argument ""{0}"" must not be null.", paramName));
            }
        }
        
        [DebuggerHidden]
        public static void RequireNotNullOrEmpty(string paramName, string argument)
        {
        	if (String.IsNullOrEmpty(argument)) {
                throw new ArgumentException(paramName,
        		                                String.Format(@"The argument ""{0}"" must not be null or empty.", paramName));
            }
        }
		
        [DebuggerHidden]
        public static void RequireBetween<T>(string paramName, T value, T min, T max, bool inclusiveMin, bool inclusiveMax)
            where T: IComparable<T>
        {
        	bool invalidMinValue = inclusiveMin ? value.CompareTo(min) < 0 : value.CompareTo(min) <= 0;
        	bool invalidMaxValue = inclusiveMax ? value.CompareTo(max) > 0 : value.CompareTo(max) >= 0;
        	
            if (invalidMinValue || invalidMaxValue)
                throw new ArgumentOutOfRangeException(paramName, value, 
            	                                      String.Format("Specified argument must be between {0} ({1}) and {2} ({3}), but was {4}.", 
            	                                                    min, inclusiveMin ? "inclusive" : "exclusive",
            	                                                    max, inclusiveMax ? "inclusive" : "exclusive",
            	                                                    value));
        }
        
        [DebuggerHidden]
        public static void RequireGreaterThan<T>(string paramName, T value, T min)
            where T: IComparable<T>
        {
        	RequireGreaterThan(paramName, value, min, false);
        }
        
        [DebuggerHidden]
        public static void RequireGreaterThan<T>(string paramName, T value, T min, bool inclusive)
            where T: IComparable<T>
        {
        	if (inclusive && value.CompareTo(min) < 0) {
                throw new ArgumentOutOfRangeException(paramName, value, String.Format("Specified argument must be greater than or equal to {0}, but was {1}.", min, value));
        	} else if (!inclusive && value.CompareTo(min) <= 0) {
                throw new ArgumentOutOfRangeException(paramName, value, String.Format("Specified argument must be greater than {0}, but was {1}.", min, value));
        	} 
        }
                
        [DebuggerHidden]
        public static void RequireLessThan<T>(string paramName, T value, T max)
            where T: IComparable<T>
        {
            if (value.CompareTo(max) >= 0)
                throw new ArgumentOutOfRangeException(paramName, value, String.Format("Specified argument must be less than {0}, but was {1}.", max, value));
        }
                
        [DebuggerHidden]
        public static void RequireLessThanOrEqualTo<T>(string paramName, T value, T max)
            where T: IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, value, String.Format("Specified argument must be less than or equal to {0}, but was {1}.", max, value));
        }

        [DebuggerHidden]
        public static void RequireEqual<T>(string paramName, T value, T expectedValue)
        {
        	if (!Object.Equals(value, expectedValue)) {
        		throw new ArgumentException(
        			String.Format("Specified argument must equal {0}, but was {1}.", expectedValue, value),
        			paramName);
        	}
        }

        [DebuggerHidden]
        public static void RequireNotEqual<T>(string paramName, T value, T expectedValue)
        {
        	if (Object.Equals(value, expectedValue)) {
        		throw new ArgumentException(
        			String.Format("Specified argument must not equal {0}.", expectedValue),
        			paramName);
        	}
        }
        
        [DebuggerHidden]
        public static void RequireExists<T>(string collectionName, string paramName, ICollection<T> collection, T value)
        {
        	if (!collection.Contains(value)) {
        		throw new ArgumentException(
        			String.Format("The specified {0} does not exist in {1}.", paramName, collectionName),
        			paramName);
        	}
        }
        
        [DebuggerHidden]
        public static void RequireExists<TKey, TValue>(string collectionName, string paramName, IDictionary<TKey, TValue> collection, TKey key)
        {
        	if (!collection.ContainsKey(key)) {
        		throw new ArgumentException(
        			String.Format("The specified {0} does not exist in {1}.", paramName, collectionName),
        			paramName);
        	}
        }
        
         [DebuggerHidden]
        public static void RequireDoesntExist<T>(string collectionName, string paramName, ICollection<T> collection, T value)
        {
        	if (collection.Contains(value)) {
        		throw new ArgumentException(
        			String.Format("The specified {0} already exists in {1}.", paramName, collectionName),
        			paramName);
        	}
        }
        
        [DebuggerHidden]
        public static void RequireDoesntExist<TKey, TValue>(string collectionName, string paramName, IDictionary<TKey, TValue> collection, TKey key)
        {
        	if (collection.ContainsKey(key)) {
        		throw new ArgumentException(
        			String.Format("The specified {0} already exists in {1}.", paramName, collectionName),
        			paramName);
        	}
        }
        
        
        [DebuggerHidden]
        public static void RequireIsInstanceOf<T>(string paramName, object value)
        {
        	if (!(value is T)) {
        		throw new ArgumentException(
        			String.Format("Specified argument must be an instance of {0}.", typeof(T).Name),
        			paramName);
        	}
        }
        
        #endregion
        
        #region Misc checks
        
		[DebuggerHidden]
        public static void DisposedState(INotifyDisposed obj)
        {
        	if(obj.IsDisposed) {
        		throw new ObjectDisposedException(obj.ToString());
        	}
        }
        
        #endregion
	}
}
