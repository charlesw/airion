﻿using System;
using Airion.Persist.Tests.Contract;

namespace Airion.TestRunner
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var testFixture = new NHibernateConversationTests();
			testFixture.Save_EntityDoesntExists_EntityIsAddedAndIdIsAssigned();
			
		}
	}
}