// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using Autofac;
using Airion.Parallels.Actors;
using Airion.Parallels.Actors.Internal;
using Airion.Parallels.Internal;

namespace Airion.Parallels
{
	/// <summary>
	/// Description of ParrallelsModule.
	/// </summary>
	public class ParrallelsModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// internal
			builder.RegisterType<PendingWorkQueue<IScheduledTask>>().As<IPendingWorkCollection<IScheduledTask>>();
			builder.RegisterType<BackgroundWorker>();
			builder.RegisterType<ActorBuilder>().As<IActorBuilder>();
			
			// external
			builder.RegisterType<TaskWorker>();
			builder.RegisterType<ActorHost>();
			builder.RegisterType<ActorHostBuilder>();
		}
	}
}
