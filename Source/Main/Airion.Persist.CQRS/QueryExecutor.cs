// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using Airion.Common;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Description of QueryExecutor.
	/// </summary>
	public class QueryExecutor
	{
		#region QuerySignature
		
		private struct QuerySignature
		{
			private Type _queryType;
			private Type _queryResultType;
			
			public QuerySignature(Type queryType, Type queryResultType)
			{
				_queryType = queryType;
				_queryResultType = queryResultType;
			}
			
			public Type QueryType {
				get { return _queryType; }
			}
			
			public Type QueryResultType {
				get { return _queryResultType; }
			}
			
			public static QuerySignature FromQueryHandler(Type queryHandlerType)
			{
				var queryHandlerInterfaceDef = queryHandlerType.GetGenericInterface(typeof(IQueryHandler<,>));
				var genericTypeArgs = queryHandlerInterfaceDef.GetGenericArguments();
				return new QuerySignature(genericTypeArgs[0], genericTypeArgs[1]);
			}
			
			#region Equals and GetHashCode implementation
			
			public override bool Equals(object obj)
			{
				return (obj is QuerySignature) && Equals((QuerySignature)obj);
			}
			
			public bool Equals(QuerySignature other)
			{
				return object.Equals(this._queryType, other._queryType) && object.Equals(this._queryResultType, other._queryResultType);
			}
			
			public override int GetHashCode()
			{
				int hashCode = 0;
				unchecked {
					if (_queryType != null)
						hashCode += 1000000007 * _queryType.GetHashCode();
					if (_queryResultType != null)
						hashCode += 1000000009 * _queryResultType.GetHashCode();
				}
				return hashCode;
			}
			
			public static bool operator ==(QuerySignature lhs, QuerySignature rhs)
			{
				return lhs.Equals(rhs);
			}
			
			public static bool operator !=(QuerySignature lhs, QuerySignature rhs)
			{
				return !(lhs == rhs);
			}
			#endregion

		}
		
		#endregion
		
		private Dictionary<QuerySignature, IQueryHandler> _queryHandlers;
		
		public QueryExecutor(IEnumerable<IQueryHandler> queryHandlers)
		{
			_queryHandlers = new Dictionary<QuerySignature, IQueryHandler>();
			foreach(var queryHandler in queryHandlers) {
				var queryHandlerType = queryHandler.GetType();
				var querySignature = QuerySignature.FromQueryHandler(queryHandlerType);
				Guard.Require("queryHandlers", !_queryHandlers.ContainsKey(querySignature), "A query handler is already registered for the query <{0}, {1}>.", querySignature.QueryType.Name, querySignature.QueryResultType.Name);
				_queryHandlers.Add(querySignature, queryHandler);
			}
		}
		
		public TQueryResult Execute<TQuery, TQueryResult>(TQuery query)
		{
			TQueryResult result;
			var signature = new QuerySignature(typeof(TQuery), typeof(TQueryResult));
			IQueryHandler handler;
			if(_queryHandlers.TryGetValue(signature, out handler)) {
				var typedHandler = (IQueryHandler<TQuery, TQueryResult>)handler;
				result = typedHandler.Execute(query);
			} else {
				throw new InvalidOperationException(String.Format("No query handler is registered for the query <{0}, {1}>.", signature.QueryType.Name, signature.QueryResultType.Name));
			}			
			return result;
			
		}
	}
}
