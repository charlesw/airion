// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using Autofac;

namespace Airion.Testing
{
	/// <summary>
	/// Description of AutoFacTestBase.
	/// </summary>
	public abstract class AutoFacTestBase
	{		
		protected IContainer BuildContainer()
		{
			var builder = new ContainerBuilder();
			RegisterComponents(builder);
			return builder.Build();
		}
		
		protected abstract void RegisterComponents(ContainerBuilder builder);
	}
}
