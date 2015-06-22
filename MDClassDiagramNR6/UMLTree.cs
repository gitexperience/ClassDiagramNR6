using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    class UMLTree
    {
        private Dictionary<String, List<String> > Graph;
        private UMLClass diagram;
        private Dictionary<String, INode> Mapping;
        public UMLTree(UMLClass diagram)
        {
            this.diagram = diagram;
            Graph = new Dictionary<string,List<string>>();
            Mapping = new Dictionary<string,INode>();

            foreach (var classnode in diagram.ClassNodes){
                 Graph.Add(classnode.ClassName, new List<String>());
                 Mapping.Add(classnode.ClassName, classnode);
            }               

            foreach (var interfacenode in diagram.InterfaceNodes){
                 Graph.Add(interfacenode.InterfaceName, new List<String>());
                 Mapping.Add(interfacenode.InterfaceName, interfacenode);
            }
                

            foreach (var structnode in diagram.StructNodes){
                 Graph.Add(structnode.StructName, new List<String>());
                 Mapping.Add(structnode.StructName, structnode);
            }               

            foreach (var enumnode in diagram.EnumNodes){
                Graph.Add(enumnode.EnumName, new List<String>());
                Mapping.Add(enumnode.EnumName, enumnode);
            }
           

        }
        private void AddRelationship(INode from, string to)
        {
            Graph[from.GetName()].Add(to);
        }
        public void BuildGraph()
        {
            foreach(var node in diagram.ClassNodes)
            {
                foreach(var link in node.Links)
                {
                    //We do not add a link if the 'to' node is not present in the project
                    if(Graph.ContainsKey(link))
                    {
                        this.AddRelationship(node, link );
                    }
                }
            }
        }
        public INode GetNode(string name)
        {
            if (Mapping.ContainsKey(name))
                return Mapping[name];
            return null;
        }
    }
}
