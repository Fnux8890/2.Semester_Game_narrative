using System.Collections.Generic;
using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;

namespace GameSystems.Dialogue.DialogueGraphStructure
{
    public class DialogueGraph
    {
        public List<Vertex> AdjacencyList {get; set;}

        public DialogueGraph()
        {
            AdjacencyList = new List<Vertex>();
        }
        
        /// <summary>
        /// Adds a new vertex to the graph
        /// </summary>
        /// <param name="newVertex">Name of the new vertex</param>
        /// <returns>Returns the success of the operation</returns>
        public bool AddVertex(Node newVertex)
        {
            // We will keep the implementation simple and focus on the concepts
            // Ignore duplicate vertices.
            if (AdjacencyList.Find(v => v.Name == newVertex.node_name) != null) return true;

            // Add vertex to the graph
            AdjacencyList.Add(new Vertex(newVertex));
            return true;
        }
        
        /// <summary>
        /// Adds a new edge between two given vertices in the graph
        /// </summary>
        /// <param name="v1">Name of the first vertex</param>
        /// <param name="v2">Name of the second vertex</param>
        /// <returns>Returns the success of the operation</returns>
        public bool AddAnEdge(Node v1, Node v2)
        {
            // We will keep the implementation simple and focus on the concepts
            // Do not worry about handling invalid indexes or any other error cases.
            // We will assume all vertices are valid and already exist.
            
            // Add vertex v2 to the edges of vertex v1
            AdjacencyList.Find(v => v.Name == v1.node_name).Edges.Add(v2);
            // Add vertex v1 to the edges of vertex v2
            AdjacencyList.Find(v => v.Name == v2.node_name).Edges.Add(v1);

            return true;
        }
        
        /// <summary>
        /// Removes an edge between two given vertices in the graph
        /// </summary>
        /// <param name="v1">Name of the first vertex</param>
        /// <param name="v2">Name of the second vertex</param>
        /// <returns>Returns the success of the operation</returns>
        public bool RemoveAnEdge(Node v1, Node v2)
        {
            // We will keep the implementation simple and focus on the concepts
            // Do not worry about handling invalid indexes or any other error cases.
            // We will assume all vertices are valid and already exist.

            // Remove vertex v2 to the edges of vertex v1
            AdjacencyList.Find(v => v.VertexNode == v1).Edges.Remove(v2);

            // Remove vertex v1 to the edges of vertex v2
            AdjacencyList.Find(v => v.VertexNode == v2).Edges.Remove(v1);

            return true;
        }
        
         #region " DFS Traversal "

        /// <summary>
        /// Recursively traverse the graph and return an array of vertex names
        /// </summary>
        /// <param name="startVertex">Name for the starting vertex from where the traversal should start.</param>
        /// <returns>Returns array of strings</returns>
        public List<Vertex> DFSRecursive(Node startVertex)
        {
            Vertex start = AdjacencyList.Find(v => v.Name == startVertex.node_name);
            if (start == null) return null;

            // List to keep track of the result
            List<Vertex> result = new List<Vertex>();

            // Lookup for keep track of visited nodes
            HashSet<Node> visited = new HashSet<Node>();

            DFSR(start, result, visited);
            return result;
        }

        private void DFSR(Vertex startVertex, List<Vertex> result, HashSet<Node> visited)
        {
            if (startVertex == null || visited.Contains(startVertex.VertexNode)) return;

            //Add the vertex to the visited list
            result.Add(startVertex);

            // Mark the vertex as visited
            visited.Add(startVertex.VertexNode);

            // Traverse through the neighbors of the vertex
            foreach (var neighbor in startVertex.Edges)
            {
                // If the neighbor vertex is not visited already, perform DFS on the neighbor vertex
                if (!visited.Contains(neighbor))
                {
                    DFSR(AdjacencyList.Find(v => v.Name == neighbor.node_name), result, visited);
                }
            }
        }

        /// <summary>
        /// Iteratively traverse the graph and return an array of vertex names
        /// </summary>
        /// <param name="startVertex">Name for the starting vertex from where the traversal should start.</param>
        /// <returns>Returns array of strings</returns>
        public List<Node> DFSIterative(Node startVertex)
        {
            Vertex start = AdjacencyList.Find(v => v.Name == startVertex.node_name);
            if (start == null) return null;

            List<Node> result = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Stack<Vertex> stack = new Stack<Vertex>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (visited.Contains(current.VertexNode)) continue;
                result.Add(current.VertexNode);
                visited.Add(current.VertexNode);

                foreach (var neighbor in current.Edges)
                {
                    if (!visited.Contains(neighbor))
                    {
                        stack.Push(AdjacencyList.Find(v => v.Name == neighbor.node_name));
                    }
                }
            }
            return result;
        }

        #endregion
        
        /// <summary>
        /// Iteratively traverse the graph and return an array of vertex names
        /// </summary>
        /// <param name="startVertex">Name for the starting vertex from where the traversal should start.</param>
        /// <returns>Returns array of strings</returns>
        public List<Node> BFSTraversal(Node startVertex)
        {
            Vertex start = AdjacencyList.Find(v => v.VertexNode == startVertex);
            if (start == null) return null;

            List<Node> result = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(start);

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();
                // If current vertex is already visited, move to the next vertex in the queue
                if(visited.Contains(current.VertexNode)) continue;

                result.Add(current.VertexNode);
                visited.Add(current.VertexNode);

                foreach (var neighbor in current.Edges)
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(AdjacencyList.Find(v => v.VertexNode == neighbor));
                    }
                }
            }

            return result;

        }
        
    }

    public class Vertex
    {
        public string Name { get; set;}
        public Node VertexNode {get; set; }
        public List<Node> Edges { get; set;}

        public Vertex(Node node)
        {
            Name = node.node_name;
            VertexNode = node;
            Edges = new List<Node>();
        }

    }
    
}