// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using System.Threading;
using Airion.Parallels.Actors.Internal;

namespace Airion.Parallels.Actors
{
	/// <summary>
	/// Description of ActorHostBuilder.
	/// </summary>
	public class ActorHostBuilder
	{
		private HashSet<Type> _subjectTypes;
		private ActorHost.Factory _hostFactory;
		private TaskWorkerFactory _workerFactory;
		
		public ActorHostBuilder(TaskWorkerFactory taskWorkerFactory, ActorHost.Factory hostFactory)
		{
			_subjectTypes = new HashSet<Type>();
			_hostFactory = hostFactory;
			_workerFactory = taskWorkerFactory;
		}
		
		public void RegisterType<T>()
		{			
			_subjectTypes.Add(typeof(T));
		}
		
		public ActorHost BuildHost()
		{
			var actorBuilder = new ActorBuilder();
			var taskWorker = _workerFactory(ApartmentState.MTA);
			actorBuilder.CreateActorFactories(_subjectTypes);
			return _hostFactory(taskWorker, actorBuilder);
		}
	}
}
