using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MDClassDiagramNR6
{
    public class EnumNode : Microsoft.CodeAnalysis.Host.HostServices , INode 
    {
        public string EnumName;
        public List<String> Members;
        private bool inProject;
        public EnumNode()
        {
            Members = new List<String>();
        }
        public void AddMember(string member)
        {
            Members.Add(member);
        }
        public string GetName()
        {
            return EnumName;
        }
        public bool isPartOfProject()
        {
            return inProject;
        }
        protected override Microsoft.CodeAnalysis.Host.HostWorkspaceServices CreateWorkspaceServices(Microsoft.CodeAnalysis.Workspace w)
        {
            return null;
        }
    }
}
