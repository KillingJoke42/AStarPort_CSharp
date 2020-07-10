using System;
using containers;
using utility;
using System.Linq;
using System.Collections.Generic;
namespace astarsolver
{
    public class astartsolver
    {
        public string heuristic;
        public static int ROW, COL;
        KeyValuePair<double, Node> pPair = new KeyValuePair<double, Node>();
        utils utility;
        public astartsolver(string Heuristic, int num_row, int num_col)
        {
            heuristic = Heuristic;
            ROW = num_row;
            COL = num_col;
            utility = new utils(ROW, COL);
        }
        
        double CalculateHeuristic(int x, int y, Node goal, string heuristic)
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
        bool successorSolver(int i_index, int j_index, double const_offfset, Node dest, int[,] grid, ref bool[,] closedList,
                                    ref SimplePriorityQueue<Node> openList, ref Node[,] nodeDetails)
        {
            if (utility.isValid(i_index, j_index))
            {
                if (utility.isDestination(i_index, j_index, dest))
                {
                    nodeDetails[i_index, j_index].parent.x = i_index;
                    nodeDetails[i_index, j_index].parent.y = j_index;
                    Console.WriteLine("The destination cell is found!\n");
                    utility.tracePath(nodeDetails, dest);
                    return true;
                }
                else if (!closedList[i_index, j_index] && utility.isUnblocked(grid, i_index, j_index))
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
        public void aStarSearch(int[,] grid, Node src, Node dest)
        {
            if (!utility.isValid(src.x, src.y))
            {
                Console.WriteLine("Source is Invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!utility.isValid(dest.x, dest.y))
            {
                Console.WriteLine("Destination is invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!utility.isUnblocked(grid, src.x, src.y) || !utility.isUnblocked(grid, dest.x, dest.y))
            {
                Console.WriteLine("Source or the destination is blocked\n");
            }
            if (utility.isDestination(src.x, src.y, dest))
            {
                Console.WriteLine("We are already at the destination\n");
            }

            bool[,] closedList = new bool[ROW, COL];
            Node[,] nodeDetails = new Node[ROW, COL];
            int i, j;

            for (i = 0; i < ROW; i++)
            {
                for (j = 0; j < COL; j++)
                {
                    //Console.WriteLine(double.MaxValue);
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

            while (!openList.Empty)
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
    }
}
