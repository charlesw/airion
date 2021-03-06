﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using Airion.Parallels;
using Airion.Parallels.Internal;
using Autofac;

namespace Airion.Common.Tests.Support
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
			
			// external
			builder.RegisterType<TaskWorker>().As<ITaskWorker>();
		}
	}
}
