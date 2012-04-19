using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using NStub.Core;
using NStub.CSharp.ObjectGeneration;
using System.IO;
using NStub.CSharp;
using System.Reflection;
using System.Windows.Forms;

namespace NStub.Gui
{

    public static class GeneratorDataMapper
    {
        public static IList<TestNode> MapToNodes(this TreeView treeView)
        {
            return treeView.Nodes.Cast<TreeNode>().MapToNodes();
        }

        public static IList<TestNode> MapToNodes(this IEnumerable<TreeNode> mainNodes)
        {
            var returnValue = new List<TestNode>();
            foreach (var item in mainNodes)
            {
                returnValue.Add(item.MapToNode());
            }
            return returnValue;
        }

        public static TestNode MapToNode(this TreeNode treeNode)
        {
            var returnValue = new TestNode()
            {
                Checked = treeNode.Checked,
                Tag = treeNode.Tag,
                Text = treeNode.Text
            };

            //if (treeNode.Nodes.Count > 0)
            {
                returnValue.Nodes = new List<TestNode>(treeNode.Nodes.Count);
                foreach (TreeNode item in treeNode.Nodes)
                {
                    returnValue.Nodes.Add(item.MapToNode());
                }
            }

            return returnValue;
        }
    }


}

