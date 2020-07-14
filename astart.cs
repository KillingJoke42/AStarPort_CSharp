using System;
using containers;
using utility;
using System.Collections.Generic;

namespace astarsolver
{
    public class astartsolver
    {
        public string heuristic;
        public static int ROW, COL;
        public string path_desc;
        utils utility;
        public astartsolver(string Heuristic, int num_row, int num_col)
        {
            heuristic = Heuristic;
            ROW = num_row;
            COL = num_col;
            utility = new utils(ROW, COL);
        }
        
        double CalculateHeuristic(int x, int y, pair goal, string heuristic)
        {
            if (heuristic.Equals("manhattan"))
            {
                return Convert.ToDouble(Math.Abs(x - goal.first) + Math.Abs(y - goal.second));
            }
            else if (heuristic.Equals("diagonal"))
            {
                return Convert.ToDouble(Math.Max(Math.Abs(x - goal.first), Math.Abs(y - goal.second)));
            }
            else if (heuristic.Equals("eucledian"))
            {
                return Convert.ToDouble(Math.Sqrt(Math.Pow(x - goal.first, 2) + Math.Pow(y - goal.second, 2)));
            }
            else
            {
                Console.WriteLine("Invalid Heuristic. Exiting...");
                Environment.Exit(-1);
                return 0;
            }
            //g = other.g + Math.Sqrt(Math.Pow((x - other.x), 2) + Math.Pow((y - other.y), 2));
        }
        bool successorSolver(int i_index, int j_index, int i, int j, double const_offfset, pair dest, 
                                    int[,] grid, ref bool[,] closedList,
                                    ref Queue<pPair> openList, ref cell[,] nodeDetails)
        {
            if (utility.isValid(i_index, j_index))
            {
                if (utility.isDestination(i_index, j_index, dest))
                {
                    nodeDetails[i_index, j_index].parent_i = i;
                    nodeDetails[i_index, j_index].parent_j = j;
                    Console.WriteLine("The destination cell is found!\n");
                    path_desc = utility.tracePath(nodeDetails, dest);
                    return true;
                    
                }
                else if (!closedList[i_index, j_index] && utility.isUnblocked(grid, i_index, j_index))
                {
                    double gNew = nodeDetails[i_index, j_index].g + const_offfset;
                    double hNew = CalculateHeuristic(i_index, j_index, dest, "manhattan");
                    double fNew = gNew + hNew;
                    if (nodeDetails[i_index, j_index].f == double.MaxValue || nodeDetails[i_index, j_index].f > fNew)
                    {
                        openList.Enqueue(new pPair((double)fNew, new pair(i_index, j_index)));
                        nodeDetails[i_index, j_index].f = fNew;
                        nodeDetails[i_index, j_index].g = gNew;
                        nodeDetails[i_index, j_index].h = hNew;
                        nodeDetails[i_index, j_index].parent_i = i;
                        nodeDetails[i_index, j_index].parent_j = j;
                    }
                }
            }
            return false;
        }
        public void aStarSearch(int[,] grid, pair src, pair dest)
        {
            if (!utility.isValid(src.first, src.second))
            {
                Console.WriteLine("Source is Invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!utility.isValid(dest.first, dest.second))
            {
                Console.WriteLine("Destination is invalid! Exiting...\n");
                Environment.Exit(-1);
            }
            if (!utility.isUnblocked(grid, src.first, src.second) || !utility.isUnblocked(grid, dest.first, dest.second))
            {
                Console.WriteLine("Source or the destination is blocked\n");
            }
            if (utility.isDestination(src.first, src.second, dest))
            {
                Console.WriteLine("We are already at the destination\n");
            }

            bool[,] closedList = new bool[ROW, COL];
            for (int z = 0; z < ROW * COL; z++) closedList[z % ROW, z / ROW] = false;
            cell[,] nodeDetails = new cell[ROW, COL];
            int i, j;
            for (i = 0; i < ROW; i++)
            {
                for (j = 0; j < COL; j++)
                {
                    //Console.WriteLine(double.MaxValue);
                    nodeDetails[i, j] = new cell(-1, -1);
                    nodeDetails[i, j].f = (double)double.MaxValue;
                    nodeDetails[i, j].g = (double)double.MaxValue;
                    nodeDetails[i, j].h = (double)double.MaxValue;
                }
            }

            i = src.first;
            j = src.second;
            nodeDetails[i, j] = new cell(i, j);
            nodeDetails[i, j].f = 0.0;
            nodeDetails[i, j].g = 0.0;
            nodeDetails[i, j].h = 0.0;

            Queue<pPair> openList = new Queue<pPair>();
            openList.Enqueue(new pPair((double)0.0, new pair(i, j)));
            bool foundDest = false;

            while (!(openList.Count == 0))
            {
                pPair p = openList.Dequeue();

                i = p.second.first;
                j = p.second.second;
                closedList[i, j] = true;

                foundDest = successorSolver(i - 1, j, i, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i + 1, j, i, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i, j + 1, i, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i, j - 1, i, j, 1.0, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i - 1, j + i, j, 1, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i - 1, j - 1, i, j, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i + 1, j + 1, i, j, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
                foundDest = successorSolver(i + 1, j - 1, i, j, 1.414, dest, grid, ref closedList, ref openList, ref nodeDetails);
                if (foundDest) return;
            }
            if (!foundDest)
                Console.WriteLine("Failed to find the Destination\n");
            return;

        }
    }
}
