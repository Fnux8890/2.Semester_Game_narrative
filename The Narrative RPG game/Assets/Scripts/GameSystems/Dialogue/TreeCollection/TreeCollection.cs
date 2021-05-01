using GameSystems.Dialogue.Dialogue_Json_Classes;
using UnityEngine;

namespace GameSystems.Dialogue.TreeCollection
{
    public class TreeCollection
    {
        private TreeNode root;

        // public TreeNode Find(int data)
        // {
        //     
        // }

        // public TreeNode FindRecursive(int data)
        // {
        //     
        // }

        public void Insert(Node node)
        {
            if (root != null)
            {
                root.Insert(node);
            }
            else
            {
                root = new TreeNode(node);
            }
        }

        public void RemoveAt(TreeNode node)
        {
            
        }

        // private TreeNode GetSuccessor(TreeNode node)
        // {
        //     
        // }
        
        
    }
}