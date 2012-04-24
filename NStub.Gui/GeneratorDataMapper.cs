// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorDataMapper.cs" company="EvePanix">
//   Copyright (c) Jedzia 2001-2012, EvePanix. All rights reserved.
//   See the license notes shipped with this source and the GNU GPL.
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using NStub.Core;

    /// <summary>
    /// Maps from <see cref="TreeView"/> <see cref="TreeNode"/>s to <see cref="TestNode"/>s.
    /// </summary>
    public static class GeneratorDataMapper
    {
        /// <summary>
        /// Maps from <see cref="TreeNode"/> with test build data to <see cref="TestNode"/>.
        /// </summary>
        /// <param name="treeNode">The <see cref="TreeNode"/> to convert.</param>
        /// <returns>A new <see cref="TestNode"/> initialized with the data of the <paramref name="treeNode"/>.</returns>
        public static TestNode MapToNode(this TreeNode treeNode)
        {
            TestNodeType testNodeType;
            switch (treeNode.ImageIndex)
            {
                case 11:
                    testNodeType = TestNodeType.Root;
                    break;
                case 0:
                    testNodeType = TestNodeType.Assembly;
                    break;
                case 1:
                    testNodeType = TestNodeType.Method;
                    break;
                case 2:
                    testNodeType = TestNodeType.Module;
                    break;
                case 3:
                    testNodeType = TestNodeType.Class;
                    break;
                default:
                    testNodeType = TestNodeType.Root;
                    break;
            }

            var returnValue = new TestNode(treeNode.Text, testNodeType, treeNode.Tag)
                                  {
                                      Checked = treeNode.Checked,
                                      
                                      // Tag = treeNode.Tag,
                                      // Text = treeNode.Text,
                                      // Nodes = new List<TestNode>(treeNode.Nodes.Count)
                                  };
            foreach (TreeNode item in treeNode.Nodes)
            {
                {
                    // if (item.Checked)
                    returnValue.Nodes.Add(item.MapToNode());
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Maps from a <see cref="TreeView.Nodes"/> with test build data to a list of <see cref="TestNode"/>s.
        /// </summary>
        /// <param name="treeView">The tree view with the data.</param>
        /// <returns>A new list of <see cref="TestNode"/>s initialized with the data of the <paramref name="treeView"/>.</returns>
        public static IList<TestNode> MapToNodes(this TreeView treeView)
        {
            return treeView.Nodes.Cast<TreeNode>().MapToNodes();
        }

        /// <summary>
        /// Maps from a list of <see cref="TreeNode"/>s with test build data to a list of <see cref="TestNode"/>s.
        /// </summary>
        /// <param name="mainNodes">The <see cref="TreeNode"/>s to convert.</param>
        /// <returns>A new list of <see cref="TestNode"/>s initialized with the data of the <paramref name="mainNodes"/>.</returns>
        public static IList<TestNode> MapToNodes(this IEnumerable<TreeNode> mainNodes)
        {
            var returnValue = new List<TestNode>();
            foreach (var item in mainNodes)
            {
                {
                    // if (item.Checked)
                    returnValue.Add(item.MapToNode());
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Maps from <see cref="TestNode"/> to <see cref="TreeNode"/>.
        /// </summary>
        /// <param name="testNode">The test node to translate.</param>
        /// <returns>A new <see cref="TreeNode"/> populated with the data of <paramref name="testNode"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>testNode</c> is out of range.</exception>
        public static TreeNode MapToTree(this TestNode testNode)
        {
            object tag = testNode.MethodInfo;
            if (testNode.IsClass)
            {
                tag = testNode.ClrType;
            }

            var returnValue = new TreeNode(testNode.Text)
                                  {
                                      Checked = testNode.Checked,
                                      Tag = tag,
                                  };

            switch (testNode.TestNodeType)
            {
                case TestNodeType.Root:
                    returnValue.ImageIndex = 11;
                    break;
                case TestNodeType.Assembly:
                    returnValue.ImageIndex = 0;
                    returnValue.Expand();
                    break;
                case TestNodeType.Module:
                    returnValue.ImageIndex = 2;
                    returnValue.Expand();
                    break;
                case TestNodeType.Class:
                    returnValue.ImageIndex = 3;
                    break;
                case TestNodeType.Method:
                    returnValue.ImageIndex = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("testNode", "TestNode.TestNodeType is undefined");
            }

            foreach (TestNode item in testNode.Nodes)
            {
                {
                    // if (item.Checked)
                    returnValue.Nodes.Add(item.MapToTree());
                }
            }

            return returnValue;
        }
    }
}