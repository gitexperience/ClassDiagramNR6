using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    public class Method
    {
        public string Name;
        public string Modifier;
        public string ReturnType;
        public List<Parameter> Parameters;
        public Method(string Name, string Modifier, string ReturnType){
            this.Name = Name;
            this.Modifier = Modifier;
            this.ReturnType = ReturnType;
            Parameters = new List<Parameter>();
        }
        public Method()
        {
            Parameters = new List<Parameter>();
        }

    }
    public class Parameter
    {
        public string name;
        public string type;
    }
}
