using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NStub.Core
{
    /// <summary>
    /// Load assemblies and build a reflection graph of the contained types.
    /// </summary>
    public class AssemblyFetcher
    {
        private readonly TestNode _assemblyGraphTreeView;// = new TestNode() { Text = "Root" };
        private readonly List<AssemblyName> _referencedAssemblies;// = new List<AssemblyName>();
        private readonly List<string> _inputAssemblyOpenFileDialog;// = new List<string>();
        private readonly MemberVisibility methodVisibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AssemblyFetcher"/> class.
        /// </summary>
        /// <param name="methodVisibility">The method visibility to parse.</param>
        /// <param name="assemblyName">Description text of the root assembly node.</param>
        /// <param name="inputAssemblies">The list of input assemblies.</param>
        public AssemblyFetcher(MemberVisibility methodVisibility, string assemblyName, IEnumerable<string> inputAssemblies)
        {
            Guard.NotNullOrEmpty(() => assemblyName, assemblyName);
            Guard.NotNull(() => inputAssemblies, inputAssemblies);

            _assemblyGraphTreeView = new TestNode() { Text = assemblyName };
            _inputAssemblyOpenFileDialog = inputAssemblies.ToList();
            _referencedAssemblies = new List<AssemblyName>();
            this.methodVisibility = methodVisibility;
        }
        /// <summary>
        /// Reflects through the currently selected assembly and reflects the type tree
        /// in tvAssemblyGraph.
        /// </summary>
        public TestNode LoadAssembly()
        {
            if (this._assemblyGraphTreeView.Nodes.Count > 0)
            {
                return this._assemblyGraphTreeView;
            }

            //this._assemblyGraphTreeView.Nodes.Clear();

            for (int theAssembly = 0; theAssembly < this._inputAssemblyOpenFileDialog.Count; theAssembly++)
            {
                // Load our input assembly and create its node in the tree
                Assembly inputAssembly =
                    Assembly.LoadFile(this._inputAssemblyOpenFileDialog[theAssembly]);
                TestNode assemblyTreeNode =
                    this.CreateTreeNode(
                        this._inputAssemblyOpenFileDialog[theAssembly],
                        TestNodeType.Assembly,
                        "imgAssembly");
                this._assemblyGraphTreeView.Nodes.Add(assemblyTreeNode);

                // Add our referenced assemblies to the project generator so we
                // can reference them later
                foreach (AssemblyName assemblyName in inputAssembly.GetReferencedAssemblies())
                {
                    this._referencedAssemblies.Add(assemblyName);
                }

                // Retrieve the modules from the assembly.  Most assemblies only have one
                // module, but it is possible for assemblies to possess multiple modules
                Module[] modules = inputAssembly.GetModules(false);

                // Add the namespaces in the DLL
                for (int theModule = 0; theModule < modules.Length; theModule++)
                {
                    // Add a node to the tree to represent the module
                    TestNode moduleTreeNode =
                        this.CreateTreeNode(modules[theModule].Name, TestNodeType.Module, "imgModule");
                    this._assemblyGraphTreeView.Nodes[theAssembly].Nodes.Add(moduleTreeNode);
                    Type[] containedTypes = modules[theModule].GetTypes();

                    // Add the classes in each type
                    for (int theClass = 0; theClass < containedTypes.Length; theClass++)
                    {
                        // Add a node to the tree to represent the class
                        var classType = containedTypes[theClass];
                        var classNode = this.CreateTreeNode(classType.FullName,TestNodeType.Class, classType);
                        this._assemblyGraphTreeView.Nodes[theAssembly].Nodes[theModule].Nodes.Add(
                            classNode);

                        // Create a test method for each method in this type
                        MethodInfo[] methods = containedTypes[theClass].GetMethods();
                        for (int theMethod = 0; theMethod < methods.Length; theMethod++)
                        {
                            this._assemblyGraphTreeView.Nodes[theAssembly].Nodes[theModule].Nodes[theClass].Nodes.Add(
                                this.CreateTreeNode(methods[theMethod].Name, TestNodeType.Method, methods[theMethod]));
                        }
                    }

                    //moduleTreeNode.Expand();
                }

                //assemblyTreeNode.Expand();
            }
            return this._assemblyGraphTreeView;
        }

        /// <summary>
        /// Creates a <see cref="TestNode">TreeNode</see> with the
        /// given text and image key.
        /// </summary>
        /// <param name="text">The text of the TreeNode.</param>
        /// <param name="testNodeType">Type of the test node.</param>
        /// <param name="tag">The tag used as custom information storage.</param>
        /// <returns>A new node of the provided data.</returns>
        private TestNode CreateTreeNode(string text, TestNodeType testNodeType, object tag)
        {
            TestNode treeNode = new TestNode(text, testNodeType, tag);
            // treeNode.ImageIndex = this._objectIconsImageList.Images.IndexOfKey(imageKey);

            return treeNode;
        }

    }
}
