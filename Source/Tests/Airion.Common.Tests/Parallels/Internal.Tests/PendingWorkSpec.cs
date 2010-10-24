// Author: Charles Weld
// Date: 18/03/2010 11:43 AM


using System;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;

namespace Airion.Parallels.Internal.Tests
{
	public abstract class PendingWorkSpec
	{
		protected abstract IPendingWorkCollection<int> CreatePendingWorkCollection();
		
		[Test]
		public void Should_be_able_to_send_item()
		{
			IPendingWorkCollection<int> producer = CreatePendingWorkCollection();
			Assert.That(producer.Count, Is.EqualTo(0));
			
			ThreadStart action = () => {
				producer.Send(1); // Empty Guid
			};
			
			Thread thread1 = new Thread(action);
			Thread thread2 = new Thread(action);
			
			thread1.Start();
			thread2.Start();
			
			thread1.Join();
			thread2.Join();
			
			Assert.That(producer.Count, Is.EqualTo(2));
		}
				
		[Test]
		public void Should_be_able_to_retrieve_item()
		{
			IPendingWorkCollection<int> producer = CreatePendingWorkCollection();
			Assert.That(producer.Count, Is.EqualTo(0));
			
			ThreadStart action = () => {
				producer.Send(1);
				Thread.Sleep(100);
			};
			
			Thread thread1 = new Thread(action);
			Thread thread2 = new Thread(action);
			
			thread1.Start();
			thread2.Start();
			
			Assert.That(producer.Retrieve(CancellationToken.None), Is.EqualTo(1));
			Assert.That(producer.Retrieve(CancellationToken.None), Is.EqualTo(1));
			
			Assert.That(producer.Count, Is.EqualTo(0));
			
			thread1.Join();
			thread2.Join();
		}
		
		[Test]
		public void Should_be_able_to_try_and_retrieve_item()
		{
			IPendingWorkCollection<int> producer = CreatePendingWorkCollection();
			Assert.That(producer.Count, Is.EqualTo(0));
			
			int item;
			bool success;
			
			// collection is empty
			success = producer.TryRetrieve(out item);
			Assert.That(success, Is.False);
			Assert.That(item, Is.EqualTo(default(int)));
			
			// collection is not empty
			producer.Send(1);
			success = producer.TryRetrieve(out item);
			Assert.That(success, Is.True);
			Assert.That(item, Is.EqualTo(1));
		}
		
		[Test]
		public void Should_be_able_to_block_calling_thread_until_collection_is_empty()
		{
			IPendingWorkCollection<int> pendingWorkCollection = CreatePendingWorkCollection();
			pendingWorkCollection.Send(1);
			pendingWorkCollection.Send(1);
			
			Assert.That(pendingWorkCollection.Count, Is.EqualTo(2));
			
			ThreadStart action = () => {
				Thread.Sleep(60);
				pendingWorkCollection.Retrieve(CancellationToken.None);
				Thread.Sleep(60);
			};
			
			Thread thread1 = new Thread(action);
			Thread thread2 = new Thread(action);
			
			thread1.Start();
			thread2.Start();
			
			pendingWorkCollection.Wait(CancellationToken.None);
			Assert.That(pendingWorkCollection.Count, Is.EqualTo(0));
			
			thread1.Join();
			thread2.Join();
		}
		
		[Test]
		public void StressTest()
		{
			int RepeatCount = 100;
			int ThreadCount = Environment.ProcessorCount * 2;
			
			IPendingWorkCollection<int> pendingWorkCollection = CreatePendingWorkCollection();
			
			for (int repeatIndex = 0; repeatIndex < RepeatCount; repeatIndex++) {
				Thread[] senders = new Thread[ThreadCount];
				for (int senderIndex = 0; senderIndex < senders.Length; senderIndex++) {
					senders[senderIndex] = new Thread(() => { pendingWorkCollection.Send(1); });
					senders[senderIndex].Name = String.Format("Sender[{0}]", senderIndex);
					senders[senderIndex].Start();
				}
				
				Thread[] receivers = new Thread[ThreadCount];
				for (int receiverIndex = 0; receiverIndex < senders.Length; receiverIndex++) {
					receivers[receiverIndex] = new Thread(() => { pendingWorkCollection.Retrieve(CancellationToken.None); });
					receivers[receiverIndex].Name = String.Format("Receiver[{0}]", receiverIndex);
					receivers[receiverIndex].Start();
				}
				
				for (int threadIndex = 0; threadIndex < ThreadCount; threadIndex++) {
					senders[threadIndex].Join();
					receivers[threadIndex].Join();
				}
				
				Assert.That(pendingWorkCollection.Count, Is.EqualTo(0));
			}
		}
	}
}
