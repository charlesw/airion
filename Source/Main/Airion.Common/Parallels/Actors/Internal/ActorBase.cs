// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Parallels.Actors.Internal
{
	public class Actor : IActor
	{
		public object Subject {
			get; set;
		}		
		
		public ITaskWorker TaskWorker { 
			get; set;
		}
	}
}
