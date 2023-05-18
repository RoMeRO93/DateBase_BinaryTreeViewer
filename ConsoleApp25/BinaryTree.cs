using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeViewer
{
    public class BinaryTree<T>
    {
        private T value;
        private BinaryTree<T> left;
        private BinaryTree<T> right;

        public BinaryTree(T value)
        {
            this.value = value;
            left = null;
            right = null;
        }

        public T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            this.value = value;
        }

        public BinaryTree<T> GetLeftNode()
        {
            return left;
        }

        public BinaryTree<T> GetRightNode()
        {
            return right;
        }

        public void SetLeftNode(BinaryTree<T> node)
        {
            left = node;
        }

        public void SetRightNode(BinaryTree<T> node)
        {
            right = node;
        }

        public int GetMaxLeft()
        {
            int max = 0;
            int depth = 0;
            CalculateMaxLeft(this, ref max, ref depth);
            return max;
        }

        private void CalculateMaxLeft(BinaryTree<T> node, ref int max, ref int depth)
        {
            if (node == null)
                return;

            depth++;

            if (depth > max)
                max = depth;

            CalculateMaxLeft(node.GetLeftNode(), ref max, ref depth);
            CalculateMaxLeft(node.GetRightNode(), ref max, ref depth);

            depth--;
        }
    }
}
