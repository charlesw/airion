// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using FluentNHibernate.Mapping;

namespace Airion.Persist.CQRS.Tests.Support.Mappings
{
	/// <summary>
	/// Description of MethodStepMap.
	/// </summary>
	public class MethodStepMap : ClassMap<MethodStep>
	{
		public MethodStepMap()
		{
			Id(x => x.Id);
			Map(x => x.StepDescription);
			Map(x => x.StepNumber);
			References(x => x.Rescipe);
		}
	}
}
