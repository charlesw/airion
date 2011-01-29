// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Persist.CQRS.Tests.Support;
using Airion.Testing;
using NUnit.Framework;

namespace Airion.Persist.CQRS.Tests.Contracts
{
	[TestFixture]
	public class QueryExecutorTests
	{
		#region Steps
		
		public class Steps : AbstractSteps
		{
			#region Data
			
			private TestQueryHandler _queryHandler;
			private QueryExecutor _queryExecutor;
			private Exception _exception;
			private TestQueryResponse _queryResult;
			
			#endregion
			
			#region Commands
			
			public void SetupExecutor()
			{
				_queryHandler = new TestQueryHandler();
				_queryExecutor = new QueryExecutor(new IQueryHandler[] { _queryHandler });
			}
			
			public void SetupExecutorWithDuplicateQueryHandlers()
			{
				_queryHandler = new TestQueryHandler();
				try {
					_queryExecutor = new QueryExecutor(new IQueryHandler[] { _queryHandler, _queryHandler });
				} catch(Exception e) {
					_exception = e;
				}
			}
			
			public void ExecuteTestQuery(int query)
			{
				_queryResult = _queryExecutor.Execute<TestQuery, TestQueryResponse>(new TestQuery() { Query = query });
			}
			
			public void ExecuteInvalidQuery(int query)
			{
				try {
					_queryExecutor.Execute<int, int>(query);
				} catch(Exception e) {
					_exception = e;
				}
			}
			
			#endregion
			
			#region Verification
			
			public void VerifyExceptionWasThrown<TException>()
				where TException : Exception
			{
				Assert.That(_exception, Is.InstanceOf<TException>());
			}
			
			public void VerifyTestQueryResult(int expectedResult)
			{
				Assert.That(_queryResult, Is.Not.Null, "Query was not executed.");
				Assert.That(_queryResult.Result, Is.EqualTo(expectedResult));
			}
			
			#endregion
		}
		
		#endregion
		
		#region Tests
		
		[Test]
		public void Execute_HasQueryHandler_QueryHandlerIsInvokedAndResultIsReturned()
		{
			using(var steps = new Steps()) {
				steps.SetupExecutor();
				steps.ExecuteTestQuery(2);
				steps.VerifyTestQueryResult(4);
			}
		}
		
		[Test]
		public void Execute_NoMatchingQueryHandler_ThrowsInvalidOperationException()
		{
			using(var steps = new Steps()) {
				steps.SetupExecutor();
				steps.ExecuteInvalidQuery(2);
				steps.VerifyExceptionWasThrown<InvalidOperationException>();
			}
		}
		
		[Test]
		public void Construct_MultipleQueryHandlersForSameQuery_ThrowsArgumentException()
		{
			using(var steps = new Steps()) {
				steps.SetupExecutorWithDuplicateQueryHandlers();
				steps.VerifyExceptionWasThrown<ArgumentException>();
			}
		}
		
		#endregion
		
	}
}
