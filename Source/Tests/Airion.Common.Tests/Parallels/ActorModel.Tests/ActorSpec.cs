// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Charles Weld" email="ceweld@users.sourceforge.net"/>
// </file>

using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Autofac;
using Airion.Common;
using Airion.Parallels.Actors;
using Airion.Parallels.Tests;

namespace Airion.Parallels.ActorModel.Tests
{
	[TestFixture]
	public class ActorSpec : ParallelsAutofacTestBase
	{
		#region Internal classes
		
		public interface ITestActor
		{
			bool IsThreadDifferent(Thread thread);
		}
		
		public interface IPerson
		{
			Guid Id { get; }
			string Name { get; set; }
			string Address { get; set; }
			DateTime DateOfBirth { get; set; }
			
			void AdoptChild(IPerson child);
			IPerson CreateChild();
			bool DisownChild(IPerson child);
			
			IEnumerable<IPerson> Children { get; }
		}
		
		public class TestActor : ITestActor
		{
			public TestActor()
			{
			}
			
			public bool IsThreadDifferent(Thread thread)
			{
				return Thread.CurrentThread != thread;
			}
		}
		
		public class Person : TestActor, IPerson
		{
			private HashSet<IPerson> _children;
			
			public Person()
			{
				_children = new HashSet<IPerson>();
			}
			
			public Guid Id {
				get; private set;
			}
			
			public string Name {
				get; set;
			}
			
			public string Address {
				get; set;
			}
			
			public DateTime DateOfBirth {
				get; set;
			}
			
			public IEnumerable<ActorSpec.IPerson> Children {
				get { return _children; }
			}
			
			public void AdoptChild(IPerson child)
			{
				Guard.RequireNotNull("child", child);
				_children.Add(child);
			}
			
			public IPerson CreateChild()
			{
				var child = new Person();
				_children.Add(child);
				return child;
			}
			
			public bool DisownChild(IPerson child)
			{
				Guard.RequireNotNull("child", child);
				return _children.Remove(child);
			}
		}
		
		
		#endregion
		
		[Test(Description=@"Create actor")]
		public void CreateActor()
		{
			var container = BuildContainer();
			var actorHostBuilder = container.Resolve<ActorHostBuilder>();
			actorHostBuilder.RegisterType<Person>();
			
			using(var actorHost = actorHostBuilder.BuildHost()) {
				var person = new Person();
				var actor = actorHost.Host<Person, IPerson>(person);
				Assert.That(actor, Is.Not.Null);
				Assert.That(actor, Is.InstanceOf(typeof(IPerson)));
			}
		}
		
		[Test(Description=@"Actor Behaviour - Execution thread is different")]
		public void ActorBehaviour_ExecutionThreadIsDifferent()
		{
			var container = BuildContainer();
			var actorHostBuilder = container.Resolve<ActorHostBuilder>();
			actorHostBuilder.RegisterType<Person>();
			
			using(var actorHost = actorHostBuilder.BuildHost()) {
				var person = new Person();
				var actor = actorHost.Host<Person, ITestActor>(person);
				
				Assert.That(actor.IsThreadDifferent(Thread.CurrentThread), Is.True);
			}			
		}
	}
}
