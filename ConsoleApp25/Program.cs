using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BinaryTreeViewer
{
    /// <summary>
    /// Shows in an HTML document a graph of the tree.
    /// RECOMMENDATION: Use break-point on the line of the BinaryTreeViewer.View.
    /// </summary>
    public static class BTViewer
    {
        class Program
        {
            static void Main(string[] args)
            {

            }
        }
        private static int StartingTempCount = 1; //the starting temp count so we know how many
        //trees we've created.
        private static int tempCount = 1; // the number of temporary files we've created.
        private static readonly string BINTREE_CSS_FILENAME = "BINTREEINITIALIZER.css";
        private static string fileName => $"BINTREE{tempCount}.html"; //name structure of BINTREE files.

        /// <summary>
        /// Sets the value of tempCount according to the previous saved_trees.
        /// </summary>
        static BTViewer()
        {
            string directory = Directory.GetCurrentDirectory();

            if (!Directory.GetFiles(directory).Contains(BINTREE_CSS_FILENAME))
            {
                File.WriteAllText(BINTREE_CSS_FILENAME, @"#circle{
		border-radius: 50%;
		display: inline-block;
		border: 1px solid black;
	}
	.a{
		padding: 50px;
	}
	.b{
		width: 70px;
		height: 70px;
	}
	 .line{
width: 150px;
height: 150px;
border-bottom: 1px solid black;
position: absolute;
}");
            }

            Regex reg = new Regex(@"BINTREE\d+\.html"); //we check what is the latest binary tree file.

            List<string> fileNames = Directory.GetFiles(directory).ToList();
            fileNames = reg.Matches(string.Join(" ", fileNames)).Select(x => x.Value).ToList(); //Get the BINTREE files on the directory.

            if (fileNames.Count > 0)
            {
                //we find the next fileName as -> the latest file name count (BINTREE*Number*) + 1
                tempCount = fileNames.Select(x => int.Parse(new Regex(@"\d+").Match(x).Value)).Max() + 1; //the next tree to draw.
            }
            else
                tempCount = 1;

            StartingTempCount = tempCount;
        }

        /// <summary>
        /// Writes the full tree into a file by the head.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tree">The starting of the tree.</param>
        public static void View<T>(BinaryTree<T> tree)
        {
            // in case they entered an invalid tree.
            if (tree == null)
                return;

            // in case there is only one node in the tree (only the head).
            if (tree.GetRightNode() == null && tree.GetLeftNode() == null)
            {
                InitializeFileStructure(); // we initialize the file structure.
                DrawElement(tree, (0, 0), false);
                File.AppendAllText(fileName, "</html>");
                RunTree();

                tempCount++;
                return;
            }

            // how much left we take from the beginning (max value). -> max_left_offset
            int max_left_offset = tree.GetMaxLeft(); // the max left node.

            // we start by finding the position of the head of the tree.
            (int x, int y) head_position = (max_left_offset + 1, 0);

            // we initialize the file structure.
            InitializeFileStructure();

            // we draw the head of the tree.
            DrawElement(tree, head_position, false);

            // we draw the rest of the tree.
            DrawSubTree(tree.GetLeftNode(), (head_position.x - 2, head_position.y + 2), true);
            DrawSubTree(tree.GetRightNode(), (head_position.x + 2, head_position.y + 2), false);

            // we append the closing tags and save the file.
            File.AppendAllText(fileName, "</html>");

            // we run the tree in the default browser.
            RunTree();

            tempCount++;
        }

        /// <summary>
        /// Initializes the file structure by writing the HTML and CSS code.
        /// </summary>
        private static void InitializeFileStructure()
        {
            File.WriteAllText(fileName, @"<!DOCTYPE html>
<html>
<head>
<title>Binary Tree Viewer</title>
<link rel=""stylesheet"" type=""text/css"" href=""BINTREEINITIALIZER.css"">
</head>
<body>
");
        }

        /// <summary>
        /// Draws an element of the tree (node).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node to draw.</param>
        /// <param name="position">The position of the node.</param>
        /// <param name="isLeft">Whether the node is the left child of its parent.</param>
        private static void DrawElement<T>(BinaryTree<T> node, (int x, int y) position, bool isLeft)
        {
            string elementId = $"node{position.x}{position.y}";

            File.AppendAllText(fileName, $"<div id=\"{elementId}\" class=\"a b circle\" style=\"position: absolute; top: {position.y * 150}px; left: {position.x * 150}px\">{node.GetValue()}</div>");

            if (!isLeft)
            {
                File.AppendAllText(fileName, $"<div id=\"line{position.x}{position.y}\" class=\"line\" style=\"top: {position.y * 150}px; left: {(position.x - 1) * 150}px\"></div>");
            }
            else
            {
                File.AppendAllText(fileName, $"<div id=\"line{position.x}{position.y}\" class=\"line\" style=\"top: {position.y * 150}px; left: {(position.x + 1) * 150}px\"></div>");
            }
        }

        /// <summary>
        /// Draws a subtree of the tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subTree">The subtree to draw.</param>
        /// <param name="position">The position of the subtree's head.</param>
        /// <param name="isLeft">Whether the subtree is the left child of its parent.</param>
        private static void DrawSubTree<T>(BinaryTree<T> subTree, (int x, int y) position, bool isLeft)
        {
            if (subTree == null)
                return;

            DrawElement(subTree, position, isLeft);

            // recursively draw the left and right subtrees
            DrawSubTree(subTree.GetLeftNode(), (position.x - 2, position.y + 2), true);
            DrawSubTree(subTree.GetRightNode(), (position.x + 2, position.y + 2), false);
        }

        /// <summary>
        /// Runs the tree by opening the HTML file in the default browser.
        /// </summary>
        private static void RunTree()
        {
            Process.Start(fileName);
        }
    }
}
