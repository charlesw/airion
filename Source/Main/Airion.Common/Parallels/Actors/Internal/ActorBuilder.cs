// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Airion.Common;

namespace Airion.Parallels.Actors.Internal
{
	/// <summary>
	/// Description of ActorBuilder.
	/// </summary>
	public class ActorBuilder : IActorBuilder
	{
		private ProxyGenerator _generator;
		private Dictionary<Type, Func<IActor>> _actorFactories;
		
		public ActorBuilder()
		{
			_generator = new ProxyGenerator();
			_actorFactories = new Dictionary<Type, Func<IActor>>();
		}
		
		public void CreateActorFactories(IEnumerable<Type> subjectTypes)
		{
			foreach (var subjectType in subjectTypes) {
				_actorFactories[subjectType] = BuildActorFactory(subjectType);
			}
		}
		
		public Func<IActor> BuildActorFactory(Type subjectType)
		{
			Func<IActor> actorFactory;
			
			var subjectInterfaces = subjectType.GetInterfaces();
			if(subjectInterfaces.Length > 0) {
				var subjectInterface = subjectInterfaces[0];
				var otherSubjectInterfaces = new Type[subjectInterfaces.Length - 1];
				Array.Copy(subjectInterfaces, 1, otherSubjectInterfaces, 0, otherSubjectInterfaces.Length);
				
				actorFactory = delegate() {
					var actorMixin = new Actor();
					var options = new ProxyGenerationOptions {
						Selector  = new ActorInterceptorSelector()
					};
					options.AddMixinInstance(actorMixin);
					
					var actor = (IActor)_generator.CreateInterfaceProxyWithoutTarget(
						subjectInterface,
						otherSubjectInterfaces,
						options,
						new ActorInterceptor(actorMixin));
					return actor;
				};
			} else {
				throw new ArgumentException("The subject type must implement atlease one interface.");
			}
			return actorFactory;
		}
		
		
		public IActor CreateActor(Type subjectType)
		{
			Guard.RequireNotNull("subjectType", subjectType);
			Guard.Operation(_actorFactories.ContainsKey(subjectType), "The specified subject type  was not registered.");
			
			return _actorFactories[subjectType]();
		}
	}
}
