// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using Airion.Common;
using Airion.Parallels.Actors.Internal;

namespace Airion.Parallels.Actors
{
	/// <summary>
	/// Description of ActorHost.
	/// </summary>
	public class ActorHost : LightDisposableBase
	{
		private IActorBuilder _builder;
		private ITaskWorker _taskWorker;
		
		public delegate ActorHost Factory(ITaskWorker taskWorker, IActorBuilder builder);
		
		public ActorHost(ITaskWorker taskWorker, IActorBuilder builder)
		{
			_taskWorker = taskWorker;
			_builder = builder;
			_taskWorker.Start();
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				_taskWorker.Dispose();
				_taskWorker = null;
			}
			base.Dispose(disposing);
		}
		
		public TActor Host<TSubject, TActor>(TSubject subject)
			where TSubject : class, TActor
		{
			Guard.RequireNotNull("subject", subject);
			CheckState();
			
			var actor = _builder.CreateActor(typeof(TSubject));
			actor.Subject = subject;
			actor.TaskWorker = _taskWorker;
			return (TActor)actor;
		}
	}
}
