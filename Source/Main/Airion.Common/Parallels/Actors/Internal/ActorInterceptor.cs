// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using System.Reflection;

using Castle.DynamicProxy;

namespace Airion.Parallels.Actors.Internal
{
	/// <summary>
	/// Description of ActorInterceptor.
	/// </summary>
	public class ActorInterceptor : IInterceptor
	{
		private IActor _actor;
		private MethodInfo _genericInvokeMethod;
		private Dictionary<Type, MethodInfo> _returnTypeToInvokeMapping;
		public ActorInterceptor(IActor actor)
		{
			_actor = actor;
			
			_returnTypeToInvokeMapping = new Dictionary<Type, MethodInfo>();
			var actorInterceptorType = GetType();
			_genericInvokeMethod = actorInterceptorType.GetMethod("Invoke", BindingFlags.NonPublic | BindingFlags.Instance);
		}
		
		public void Intercept(IInvocation invocation)
		{
			var method = invocation.Method;
			if(method.ReturnType != null) {
				MethodInfo invokeMethod;
				if(!_returnTypeToInvokeMapping.TryGetValue(method.ReturnType, out invokeMethod)) {
					invokeMethod = _genericInvokeMethod.MakeGenericMethod(method.ReturnType );
					_returnTypeToInvokeMapping[method.ReturnType] = invokeMethod;
				}
				// invoke generic Invoke method
				invocation.ReturnValue = invokeMethod.Invoke(this, new object[]{ method, invocation.Arguments});
			} else {
				
			}
			
//			invocation.ReturnValue = invocation.Method.Invoke(_actor.Subject, invocation.Arguments);
		}
	
		
		
		private TResult Invoke<TResult>(MethodInfo method, object[] args)
		{
			return  _actor.TaskWorker.ExecuteFunction<TResult>(
				() => {
					return (TResult)method.Invoke(_actor.Subject, args);
				}).Result;
		}
	}
}
