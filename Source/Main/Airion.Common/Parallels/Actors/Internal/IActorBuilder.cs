// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;

namespace Airion.Parallels.Actors.Internal
{
	/// <summary>
	/// Description of IActorBuilder.
	/// </summary>
	public interface IActorBuilder
	{
		void CreateActorFactories(IEnumerable<Type> subjectTypes);
		IActor CreateActor(Type subjectType);
	}
}
