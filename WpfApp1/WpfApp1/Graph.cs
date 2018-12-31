using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Graph
    {
        private int[,] adjacentMatrix; //creates an int array 

        public Graph(int size) //Graph object
        {
            adjacentMatrix = new int[size, size]; //create a new array
            for (int i = 0; i < size; i++) //nested loop
            {
                for (int j = 0; j < size; j++) //inner for loop
                {
                    adjacentMatrix[i, j] = -1;
                }
            }
        }

        public void addEdge(int b, int a, int cost) //add an edge to the graph
        {
            adjacentMatrix[b, a] = cost;
            adjacentMatrix[a, b] = cost;
        }
        public void removeEdge(int b, int a) //remove an edge from the graph
        {
            adjacentMatrix[b, a] = -1;
            adjacentMatrix[a, b] = -1;
        }

        public bool isEdge(int b, int a) //determines whether input is an edge or not
        {
            bool isEdge = false;
            if (adjacentMatrix[b, a] >= 0)
            {
                isEdge = true; //set boolean variable isEdge to true
            }
            return isEdge; //return value
        }

        public int edgeDistance(int b, int a) //determines an edge's distance from another edge
        {
            return adjacentMatrix[b, a]; //return edge distance
        }
    }
}
