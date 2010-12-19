// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>


using System;
using Airion.Common.Tests.Support.Examples.ChatRoom;
using Airion.Parallels;
using Airion.Testing;
using NUnit.Framework;

namespace Airion.Common.Tests.Contracts.Parallels
{
	[TestFixture]
	public class MediatorSpec
	{
		[Test(Description=@"Should be able to register event handlers")]
		public void RegisterEventHandlers()
		{
			var  participants = new Participant[] {
				new Participant("Participant 1"),
				new Participant("Participant 2"),
				new Participant("Participant 3")
			};
			
			IMediator mediator = null;
			try {
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participants[0].OnRecieveMessage);
				Assert.That(mediator.IsRegistered<ChatMessageEventArgs>(participants[0].OnRecieveMessage), Is.True);
				
			} finally {
				for (int i = 0; i < participants.Length; i++) {
					participants[i].Dispose();
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
		
		[Test(Description=@"Should be able to determine if event handler is registered")]
		public void DetermineIfEventHandlerIsRegistered()
		{
			var  participants = new Participant[] {
				new Participant("Participant 1"),
				new Participant("Participant 2"),
				new Participant("Participant 3")
			};
			
			IMediator mediator = null;
			try {
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participants[0].OnRecieveMessage);
				Assert.That(mediator.IsRegistered<ChatMessageEventArgs>(participants[0].OnRecieveMessage), Is.True);
				Assert.That(mediator.IsRegistered<ChatMessageEventArgs>(participants[1].OnRecieveMessage), Is.False);
				
			} finally {
				for (int i = 0; i < participants.Length; i++) {
					participants[i].Dispose();
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
		
		[Test(Description=@"Should be able to post events")]
		public void PostEvent()
		{
			var  participants = new Participant[] {
				new Participant("Participant 1"),
				new Participant("Participant 2"),
				new Participant("Participant 3")
			};
			
			IMediator mediator = null;
			try {
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participants[0].OnRecieveMessage);
				
				var args = new ChatMessageEventArgs("Hello there!");
				mediator.Post(this, args);
				
				Assert.That(participants[0].HasReceivedMessageOnce(this, args), Is.True);
				
			} finally {
				for (int i = 0; i < participants.Length; i++) {
					participants[i].Dispose();
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
		
		[Test(Description=@"Should call registered event handlers of the same type (including parents)")]
		public void PostEventCallsHandlersForParentTypes()
		{
			Participant participant = null;
			IMediator mediator = null;
			try {
				participant = new Participant("Test");
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participant.OnRecieveMessage);
				mediator.Register<EventArgs>(participant.OnRecieveEvent);
				
				var args = new ChatMessageEventArgs("Hello there!");
				mediator.Post(this, args);
				
				Assert.That(participant.HasReceivedMessageOnce(this, args), Is.True);
				Assert.That(participant.HasReceivedEventOnce(this, args), Is.True);
				
			} finally {
				if(participant != null) {
					participant.Dispose();
					participant = null;
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
		
		[Test(Description=@"Should be able to register a finalizer that is called last.")]
		public void FinalizerEvent()
		{
			var  participants = new Participant[] {
				new Participant("Participant 1"),
				new Participant("Participant 2"),
				new Participant("Participant 3")
			};
			
			IMediator mediator = null;
			try {
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participants[0].InterceptMessage);
				mediator.RegisterFinalizer<ChatMessageEventArgs>(participants[2].OnRecieveMessage);
				
				var args = new ChatMessageEventArgs("Hello there!");
				mediator.Post(this, args);
				
				Assert.That(participants[0].HasInterceptedMessageOnce(this, new ChatMessageEventArgs("Hello there!")), Is.True);
				Assert.That(participants[2].HasReceivedMessageOnce(this, new ChatMessageEventArgs("Message has been intercepted.")), Is.True);
				
			} finally {
				for (int i = 0; i < participants.Length; i++) {
					participants[i].Dispose();
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
		
		[Test(Description=@"Should be able to determine if mediator has any registered handlers")]
		public void HasHandler()
		{
			var  participants = new Participant[] {
				new Participant("Participant 1"),
				new Participant("Participant 2"),
				new Participant("Participant 3")
			};
			
			IMediator mediator = null;
			try {
				mediator = new Mediator();
				mediator.Register<ChatMessageEventArgs>(participants[0].InterceptMessage);
				mediator.RegisterFinalizer<ChatMessageEventArgs>(participants[2].OnRecieveMessage);
				
				Assert.That(mediator.IsEmpty(), Is.False);
				mediator.Deregister<ChatMessageEventArgs>(participants[0].InterceptMessage);
				Assert.That(mediator.IsEmpty(), Is.False);
				mediator.DeregisterFinalizer<ChatMessageEventArgs>(participants[2].OnRecieveMessage);
				Assert.That(mediator.IsEmpty(), Is.True);
				
			} finally {
				for (int i = 0; i < participants.Length; i++) {
					participants[i].Dispose();
				}
				
				var disposableMediator = mediator as IDisposable;
				if(disposableMediator != null) {
					disposableMediator.Dispose();
				}
			}
		}
	}
}
