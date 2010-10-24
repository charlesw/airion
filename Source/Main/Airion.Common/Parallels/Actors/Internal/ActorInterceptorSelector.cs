// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Airion.Parallels.Actors.Internal
{
	/// <summary>
	/// Description of ActorInterceptorSelector.
	/// </summary>
	public class ActorInterceptorSelector : IInterceptorSelector
	{
		public ActorInterceptorSelector()
		{
		}
		
		public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
		{
			if(method.DeclaringType == typeof(IActor)) {
				// don't intercept actor mixin calls.
				return new IInterceptor[0];
			} else {
				return interceptors;				
			}
		}
	}
}
