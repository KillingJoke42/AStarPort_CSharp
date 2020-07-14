using System;
using containers;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
        public bool isDestination(int row, int col, pair destination)
        {
            return (row == destination.first && col == destination.second);
        }
        public string tracePath(cell[,] nodeDetails, pair destination)
        {
            string path_desc = "";
            int row = destination.first;
            int col = destination.second;
            Stack<pair> Path = new Stack<pair>();

            while (!(nodeDetails[row, col].parent_i == row && nodeDetails[row, col].parent_j == col))
            {
                Path.Push(new pair(row, col));
                int temp_row = nodeDetails[row, col].parent_i;
                int temp_col = nodeDetails[row, col].parent_j;
                row = temp_row;
                col = temp_col;
            }

            Path.Push(new pair(row, col));
            path_desc = JsonConvert.SerializeObject(new a_star_soln(Path), Formatting.Indented);
            return path_desc;
        }

    }
}
