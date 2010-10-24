// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Parallels.Actors.Internal
{
	public interface IActor
	{			
		object Subject { get; set; }
		TaskWorker TaskWorker { get; set; }
	}
}
