using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDClassDiagramNR6
{
    interface INode
    {
        string GetName();
        bool isPartOfProject();
    }
}
