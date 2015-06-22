using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\gsoc related\ClassDiagramRelated\Class-Diagram-for-Mono\ClassDiagram.sln";
            Solution solution = new Solution(path);
            RoslynRun r = new RoslynRun();
            UMLClass result = r.AnalyzeCode(solution);
            UMLTree tree = new UMLTree(result);
            tree.BuildGraph();
        }
    }
}
