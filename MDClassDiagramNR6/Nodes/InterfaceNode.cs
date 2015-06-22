using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    public class InterfaceNode : INode
    {
        public string InterfaceName;
        public List<Method> Methods;
        public List<String> Links;
        private bool inProject;
        public InterfaceNode()
        {
            inProject = true;
            Methods = new List<Method>();
            Links = new List<string>();
        }
        public string GetName()
        {
            return InterfaceName;
        }
        public bool isPartOfProject()
        {
            return inProject;
        }
    }
}
