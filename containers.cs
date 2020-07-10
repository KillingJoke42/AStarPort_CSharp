using System;

namespace containers
{
    public class cell
    {
        public int parent_i, parent_j;
        public double f, g, h;

        public cell(int x, int y)
        {
            this.parent_i = x;
            this.parent_j = y;
        }
        public void showNode()
        {
            Console.WriteLine("Node_parent_i: {0}; Node_parent_j: {1}; Node_f: {2}; Node_g: {3}; Node_h: {4}\n", parent_i, parent_j, f, g, h);
        }
    }

    public class pair
    {
        public int first;
        public int second;
        public pair(int x, int y)
        {
            this.first = x;
            this.second = y;
        }
    }

    public class pPair
    {
        public double first;
        public pair second;
        public pPair(double x, pair pair_obj)
        {
            this.first = x;
            this.second = pair_obj;
        }
    }
}