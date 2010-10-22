﻿using System;
using Airion.Persist.NHibernateProvider.Tests;
using Airion.Persist.NHibernateProvider.Tests.Contract;

namespace Airion.TestRunner
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var testFixture = new NHibernateConversationTests();
			testFixture.Update_EntityExists_EntityIsUpdated();
			
		}
	}
}