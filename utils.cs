using System;
using containers;
using System.Collections.Generic;

namespace utility
{
    public class utils
    {
        int ROW, COL;
        public utils(int row, int col)
        {
            ROW = row;
            COL = col;

        }
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

            while (!(nodeDetails[row, col].parent.x == row && nodeDetails[row, col].parent.y == col))
            {
                Path.Push(new Node(row, col));
                int temp_row = nodeDetails[row, col].x;
                int temp_col = nodeDetails[row, col].y;
                row = temp_row;
                col = temp_col;
            }

            Path.Push(new Node(row, col));
            while (!(Path.Count == 0))
            {
                Node p = Path.Pop();
                Console.WriteLine("-> ({0},{1}) ", p.x, p.y);
            }
        }

    }
}
