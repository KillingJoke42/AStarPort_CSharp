//Remove Unwanted commits??
/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;*/
using containers;
//using utility;
using astarsolver;

namespace a_star_algo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] grid = new int[8, 8]
                                 {{1, 0, 1, 1, 1, 1, 1, 1},
                                  {0, 1, 1, 1, 0, 0, 0, 1},
                                  {1, 1, 1, 1, 1, 0, 0, 1},
                                  {1, 0, 0, 1, 1, 0, 0, 1},
                                  {1, 0, 0, 1, 1, 1, 1, 1},
                                  {0, 1, 1, 0, 0, 1, 1, 0},
                                  {0, 0, 0, 1, 1, 0, 0, 1},
                                  {1, 1, 1, 1, 1, 0, 0, 1}};
            Node src = new Node(7, 7);
            Node dest = new Node(0, 0);
            astartsolver algo = new astartsolver("manhattan", 8, 8);
            algo.aStarSearch(grid, src, dest);
        }
    }

    
}
