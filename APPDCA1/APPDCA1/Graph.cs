using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    class Graph
    {
        private int[,] adjacentMatrix;

        public Graph(int size)
        {
            adjacentMatrix = new int[size,size];
            for(int i=0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                { 
                    adjacentMatrix[i, j] = -1;
                }
            }
        }
       
        public void addEdge(int b, int a, int cost)
        {
            adjacentMatrix[b, a] = cost;
            adjacentMatrix[a, b] = cost;
        }
        public void removeEdge(int b, int a)
        {
            adjacentMatrix[b, a] = -1;
            adjacentMatrix[a, b] = -1;
        }

        public bool isEdge(int b, int a)
        {
            bool isEdge = false;
            if (adjacentMatrix[b, a] >= 0)
            {
                isEdge = true;
            }
            return isEdge;
        }

        public int edgeDistance(int b, int a)
        {
            return adjacentMatrix[b, a];
        }


    }
}
