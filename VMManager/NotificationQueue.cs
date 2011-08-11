using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

namespace VMManager
{
    public class VMActivityQueue
    {
        private Queue queue;

        public VMActivityQueue()
        {
            queue = new Queue();
        }

        public void Enqueue(object o)
        {
            lock (queue)
            {
                queue.Enqueue(o);
            }
        }

        public object Dequeue()
        {
            lock (queue)
            {
                return queue.Dequeue();
            }
        }

        public object Peek()
        {
            return queue.Peek();
        }

        public bool isEmpty()
        {
            return (queue.Count == 0);
        }
    }
}
