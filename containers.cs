using System;
using System.Collections.Generic;
using System.Linq;

namespace containers
{
    public class Node
    {
        public Node parent;
        public bool visited;
        public int x, y;
        public double f, g, h;
        public List<Edge> neighbours = new List<Edge>();

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Node(int x, int y, double f)
        {
            this.x = x;
            this.y = y;
            this.f = f;
        }
    }

    public class Edge
    {
        public Node node { get; set; }
        public Edge(Node v, string street)
        {
            node = v;
        }
    }

    public class SimplePriorityQueue<TValue> : SimplePriorityQueue<TValue, int> { }
    public class SimplePriorityQueue<TValue, TPriority> where TPriority : IComparable
    {
        private SortedDictionary<TPriority, Queue<TValue>> dict = new SortedDictionary<TPriority, Queue<TValue>>();

        public int Count { get; private set; }
        public bool Empty { get { return Count == 0; } }

        public void Enqueue(TValue val)
        {
            Enqueue(val, default(TPriority));
        }

        public void Enqueue(TValue val, TPriority pri)
        {
            ++Count;
            if (!dict.ContainsKey(pri)) dict[pri] = new Queue<TValue>();
            dict[pri].Enqueue(val);
        }

        public TValue Dequeue()
        {
            --Count;
            var item = dict.Last();
            if (item.Value.Count == 1) dict.Remove(item.Key);
            return item.Value.Dequeue();
        }
    }
}

