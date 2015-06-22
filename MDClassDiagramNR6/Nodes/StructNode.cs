using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    public class StructNode : INode
    {
        public string StructName;
        public List<Method> Methods;
        public List<Field> Fields;
        public List<String> Links;
        private bool inProject = true;
        public StructNode(string ClassName)
        {
            this.StructName = ClassName;
            Methods = new List<Method>();
            Fields = new List<Field>();
            Links = new List<string>();
        }
        public StructNode() 
        {
            Methods = new List<Method>();
            Fields = new List<Field>();
            Links = new List<string>();
        }
        public string GetName()
        {
            return StructName;
        }
        public bool isPartOfProject()
        {
            return inProject;
        }
    }
}
