// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Threading;
using Airion.Common;
using Airion.Parallels.Extensions;
using Airion.Parallels.Internal;

namespace Airion.Parallels
{
	public delegate ITaskWorker TaskWorkerFactory(ApartmentState apartmentState);
	
	public interface ITaskWorker : IDisposable
	{
		void Start();
		ITaskHandle ExecuteWorkItem(IWorkItem workItem);
		ITaskHandle ExecuteAction(Action action);
		ITaskHandle ExecuteAction(Action action, CancellationToken cancellationToken);
		ITaskHandle<TResult> ExecuteFunction<TResult>(Func<TResult> function);
		ITaskHandle<TResult> ExecuteFunction<TResult>(Func<TResult> function, CancellationToken cancellationToken);
	}
}
