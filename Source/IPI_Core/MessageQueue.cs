using System.Collections.Generic;

namespace Covidien.Ipi.InformaticsCore
{
    /// <summary>
    /// A locking message queue intended for for passing requests between threads
    /// </summary>
    /// <typeparam name="T">Parameterize the queue for specific message types</typeparam>
    public class MessageQueue<T> where T : TBDMessage
    {
        List<T> mMessageQueue;
        public int Count { get { return (mMessageQueue.Count); } }
        public int HighWaterMark { get; private set; }
        public int TotalReceived { get; private set; }

        public MessageQueue()
        {
            mMessageQueue = new List<T>();
        }

        /// <summary>
        /// Queue up a message for processing
        /// </summary>
        /// <param name="msg">Message to be processed</param>
        public void Enqueue(T msg)
        {
            // IpiAssert.IsNotNull(msg, "Can not insert an empty message in the MessageQueue");
            int cnt = 0;
            lock (mMessageQueue)
            {
                mMessageQueue.Add(msg);
                cnt = mMessageQueue.Count;
            }
            if (cnt > HighWaterMark)
                HighWaterMark = cnt;
            // Note: we recognize there is a potential for a race condition that would allow the HighWaterMark to
            // be reset from its highest value.  However, the chances of this are slim and reasonably inconsequential
            // thus making the choice to unlock everything as fast as possible at the risk of this small inconsistency.
            TotalReceived++;
        }

        /// <summary>
        /// Pull the next FIFO message from the queue
        /// </summary>
        /// <returns>First message in the queue or null if none available</returns>
        public T Dequeue()
        {
            T msg = null;
            lock (mMessageQueue)  // lock this to be thread safe - only long enough to remove the first item
            {
                if (0 < mMessageQueue.Count)
                {
                    msg = mMessageQueue[0];
                    mMessageQueue.RemoveAt(0);
                }
            }
            return (msg);
        }
    }
}
