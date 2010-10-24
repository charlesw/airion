// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;

namespace Airion.Parallels
{
	/// <summary>
	/// Description of IMediator.
	/// </summary>
	public interface IMediator
	{
		void Register<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs;
		
		void RegisterFinalizer<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs;
		
		void Deregister<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs;
		
		void DeregisterFinalizer<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs;
		
		bool IsRegistered<TEventArgs>(EventHandler<TEventArgs> eventHandler)
			where TEventArgs : EventArgs;
		
		void Post<TEventArgs>(object source, TEventArgs args)
			where TEventArgs : EventArgs;		
		
		bool IsEmpty();
	}
}
