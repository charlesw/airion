// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS
{
	/// <summary>
	/// Marker interface for a query handler.
	/// </summary>
	public interface IQueryHandler
	{
		
	}
	
	/// <summary>
	/// Handles the execution of a given query.
	/// </summary>
	public interface IQueryHandler<TQuery, TQueryResult> : IQueryHandler
	{
		TQueryResult Execute(TQuery query);
	}
}
