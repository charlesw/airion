// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using Airion.Common;
using Airion.Common.Tests.Support;
using Airion.Parallels;
using Airion.Testing;
using Autofac;

namespace Airion.Common.Tests.Contracts.Parallels
{
	public class ParallelsAutofacTestBase : AutoFacTestBase
	{
		public ParallelsAutofacTestBase()
		{
		}
		
		protected override void RegisterComponents(ContainerBuilder builder)
		{
			//builder.RegisterModule(new CommonModule());
			builder.RegisterModule(new ParrallelsModule());
		}
	}
}
