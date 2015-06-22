using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    public class UMLClass
    {
        public List<ClassNode> ClassNodes;
        public List<EnumNode> EnumNodes;
        public List<StructNode> StructNodes;
        public List<InterfaceNode> InterfaceNodes;
        public UMLClass()
        {
            ClassNodes = new List<ClassNode>();
            EnumNodes = new List<EnumNode>();
            StructNodes = new List<StructNode>();
            InterfaceNodes = new List<InterfaceNode>();
        }
    }
}
