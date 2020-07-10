using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace a_star_algo
{
    public class Node
    {
        public Node parent { get; set; }
        public bool visited { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public double g { get; set; }
        public double h { get; set; }
        public double f { get; set; }
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

    class Program
    {
        public string heuristic = "manhattan";
        const int ROW = 8;
        const int COL = 8;
        KeyValuePair<double, Node> pPair = new KeyValuePair<double, Node>();
        public bool isUnblocked(int[,] grid, int row, int col)
        {
            if (grid[row, col] == 1)
                return (true);
            else
                return (false);
        }
        public bool isValid(int row, int col)
        {
            return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
        }
        public bool isDestination(int row, int col, Node destination)
        {
            return (row == destination.x && col == destination.y);
        }
        public void tracePath(Node[,] nodeDetails, Node destination)
        {
            int row = destination.x;
            int col = destination.y;
            Stack<Node> Path = new Stack<Node>();

            while(!(nodeDetails[row, col].parent.x == row && nodeDetails[row, col].parent.y == col))
            {
                Path.Push(new Node(row, col));
                int temp_row = nodeDetails[row, col].x;
                int temp_col = nodeDetails[row, col].y;
                row = temp_row;
                col = temp_col;
            }

            Path.Push(new Node(row, col));
            while (!(Path.Count() == 0))
            {
                Node p = Path.Pop();
                Console.WriteLine("-> ({0},{1}) ", p.x, p.y);
            }
        }
        public double CalculateHeuristic(int x, int y, Node goal, string heuristic)
        {
            if (heuristic.Equals("manhattan"))
            {
                return Convert.ToDouble(Math.Abs(x - goal.x) + Math.Abs(y - goal.y));
            }
            else if (heuristic.Equals("diagonal"))
            {
                return Convert.ToDouble(Math.Max(Math.Abs(x - goal.x), Math.Abs(y - goal.y)));
            }
            else if (heuristic.Equals("eucledian"))
            {
                return Convert.ToDouble(Math.Sqrt(Math.Pow(x - goal.x, 2) + Math.Pow(y - goal.y, 2)));
            }
            else
            {
                Console.WriteLine("Invalid Heuristic. Exiting...");
                Environment.Exit(-1);
                return 0;
            }
            //g = other.g + Math.Sqrt(Math.Pow((x - other.x), 2) + Math.Pow((y - other.y), 2));
        }
        public bool successorSolver(int i_index, int j_index, double const_offfset, Node dest, int[,] grid, ref bool[,] closedList, 
                                    ref SimplePriorityQueue<Node> openList, ref Node[,] nodeDetails)
        {
            if (isValid(i_index, j_index))
            {
                if (isDestination(i_index, j_index, dest))
                {
                    nodeDetails[i_index, j_index].parent.x = i_index;
                    nodeDetails[i_index, j_index].parent.y = j_index;
                    Console.WriteLine("The destination cell is found!\n");
                    tracePath(nodeDetails, dest);
                    return true;
                }
                else if (!closedList[i_index, j_index] && isUnblocked(grid, i_index, j_index))
                {
                    double gNew = nodeDetails[i_index, j_index].g + const_offfset;
                    double hNew = CalculateHeuristic(i_index, j_index, dest, "manhattan");
                    double fNew = gNew + hNew;
                    if (nodeDetails[i_index, j_index].f == double.MaxValue || nodeDetails[i_index, j_index].f > fNew)
                    {
                        openList.Enqueue(new Node(i_index, j_index, (double)0.0));
                        nodeDetails[i_index, j_index].f = fNew;
                        nodeDetails[i_index, j_index].g = gNew;
                        nodeDetails[i_index, j_index].h = hNew;
                        nodeDetails[i_index, j_index].parent.x = i_index;
                        nodeDetails[i_index, j_index].parent.y = j_index;
                    }
                }
            }
            return false;
        }
        void aStarSearch(int [,] grid, Node src, Node dest)
        {
            if (!isValid(src.x, src.y))
            {
                Console.WriteLine("Source is Invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!isValid(dest.x, dest.y))
            {
                Console.WriteLine("Destination is invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!isUnblocked(grid, src.x, src.y) || !isUnblocked(grid, dest.x, dest.y))
            {
                Console.WriteLine("Source or the destination is blocked\n");
            }
            if (isDestination(src.x, src.y, dest))
            {
                Console.WriteLine("We are already at the destination\n");
            }

            bool[,] closedList = new bool[ROW,COL];
            Node[,] nodeDetails = new Node[ROW, COL];
            int i, j;

            for (i = 0; i < ROW; i++)
            {
                for (j = 0; j < COL; j++)
                {
                    nodeDetails[i, j].f = double.MaxValue;
                    nodeDetails[i, j].g = double.MaxValue;
                    nodeDetails[i, j].h = double.MaxValue;
                    nodeDetails[i, j].x = -1;
                    nodeDetails[i, j].y = -1;
                }
            }

            i = src.x;
            j = src.y;
            nodeDetails[i, j].f = 0.0;
            nodeDetails[i, j].g = 0.0;
            nodeDetails[i, j].h = 0.0;
            nodeDetails[i, j].parent.x = i;
            nodeDetails[i, j].parent.y = j;

            SimplePriorityQueue<Node> openList = new SimplePriorityQueue<Node>();
            openList.Enqueue(new Node(i, j, (double)0.0));
            bool foundDest = false;

            while(!openList.Empty)
            {
                Node p = openList.Dequeue();

                i = p.x;
                j = p.y;
                closedList[i, j] = true;

                foundDest = successorSolver(i - 1, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i + 1, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i, j + 1, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i, j - 1, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i - 1, j + 1, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i - 1, j - 1, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i + 1, j + 1, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest) return;
                foundDest = successorSolver(i + 1, j - 1, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (!foundDest)
                    return;
                else
                    Console.WriteLine("Failed to find the Destination Cell\n");
                return;
            }

        }
        public void Main(string[] args)
        {
            int[,] grid = new int[8, 8]
                                 {{0, 0, 1, 1, 1, 1, 1, 1},
                                  {0, 1, 1, 1, 0, 0, 0, 1},
                                  {1, 1, 1, 1, 1, 0, 0, 1},
                                  {1, 0, 0, 1, 1, 0, 0, 1},
                                  {1, 0, 0, 1, 1, 1, 1, 1},
                                  {0, 1, 1, 0, 0, 1, 1, 0},
                                  {0, 0, 0, 1, 1, 0, 0, 1},
                                  {1, 1, 1, 1, 1, 0, 0, 1}};
            Node src = new Node(8, 0);
            Node dest = new Node(0, 0);
            aStarSearch(grid, src, dest);
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
