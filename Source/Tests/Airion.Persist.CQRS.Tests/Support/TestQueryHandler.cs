// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;

namespace Airion.Persist.CQRS.Tests.Support
{
	/// <summary>
	/// The TestQueryHandler takes a integer and doubles it.
	/// </summary>
	public class TestQueryHandler : IQueryHandler<TestQuery, TestQueryResponse>
	{
		public TestQueryHandler()
		{
			ExecutionCount = 0;
		}
		
		public int ExecutionCount { get; private set; }
			
		
		public TestQueryResponse Execute(TestQuery query)
		{
			ExecutionCount++;
			return new TestQueryResponse() { Result = query.Query * 2 };
		}
	}
}
