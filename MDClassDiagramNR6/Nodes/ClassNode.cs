using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    public class ClassNode : INode
    {
        public string ClassName;
        public List<Method> Methods;
        public List<Field> Fields;
        public List<String> Links;
        private bool inProject = true;
        public ClassNode(string ClassName)
        {
            this.ClassName = ClassName;
            Methods = new List<Method>();
            Fields = new List<Field>();
            Links = new List<string>();
        }
        public ClassNode() 
        {
            Methods = new List<Method>();
            Fields = new List<Field>();
            Links = new List<string>();
        }
        public string GetName()
        {
            return ClassName;
        }
        public bool isPartOfProject()
        {
            return inProject;
        }
    }
}
