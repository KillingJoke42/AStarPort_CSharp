using containers;
using astarsolver;

namespace a_star_algo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] grid = {
                { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 }, 
		        { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 }, 
		        { 1, 1, 1, 0, 1, 1, 0, 1, 0, 1 }, 
		        { 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 }, 
		        { 1, 1, 1, 0, 1, 1, 1, 0, 1, 0 }, 
		        { 1, 0, 1, 1, 1, 1, 0, 1, 0, 0 }, 
		        { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 }, 
		        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 }, 
		        { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1 }
            };
            pair src = new pair(0, 0);
            pair dest = new pair(8, 0);
            astartsolver algo = new astartsolver("eucledian", 9, 10);
            algo.aStarSearch(grid, src, dest);
        }
    }
}
