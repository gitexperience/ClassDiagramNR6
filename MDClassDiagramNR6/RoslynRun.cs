using System;
using System.Linq;
using System.Windows;
using System.IO;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;


using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;




namespace MDClassDiagramNR6
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    /// 


    public class RoslynRun
    {
        private IEnumerable<Field> GetFieldNodes(FieldDeclarationSyntax fd)
        {
            // For each variable in the field
            foreach (var variable in fd.Declaration.Variables)
            {
                Field field = new Field();
                field.Modifier = fd.Modifiers.ToString();
                field.Name = variable.Identifier.ToString();
                field.ReturnType = fd.Declaration.Type.ToString();
                yield return field;
            }
        }
        private Method GetMethodNode(MethodDeclarationSyntax method)
        {
            Method methodnode = new Method();
            methodnode.ReturnType = method.ReturnType.ToString();
            methodnode.Modifier = method.Modifiers.ToString();
            methodnode.Name = method.Identifier.ToString();

            foreach (var parameter in method.ParameterList.Parameters)
            {
                Parameter p = new Parameter();
                p.name = parameter.Identifier.ToString();
                p.type = parameter.Type.ToString();
                methodnode.Parameters.Add(p);
            }
            return methodnode;
        }
        private ClassNode GetClassNode(ClassDeclarationSyntax EachClass)
        {
            ClassNode classnode = new ClassNode();
            classnode.ClassName = EachClass.Identifier.ToString();
            // For each member in that class
            foreach (var member in EachClass.Members)
            {
                if (member is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fd = member as FieldDeclarationSyntax;

                    foreach (var field in GetFieldNodes(fd))
                    {
                        classnode.Fields.Add(field);
                    }
                }
                else if (member is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                    classnode.Methods.Add(GetMethodNode(method));
                }
            }
            if (EachClass.BaseList != null)
            {
                foreach (var baseType in EachClass.BaseList.Types)
                {
                    //string ss = baseType.ToFullString();
                    classnode.Links.Add(baseType.ToFullString());
                }
            }
            return classnode;
        }
        private StructNode GetStructNode(StructDeclarationSyntax EachStruct)
        {
            StructNode structnode = new StructNode();
            structnode.StructName = EachStruct.Identifier.ToString();
            // For each member in that class
            foreach (var member in EachStruct.Members)
            {
                if (member is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fd = member as FieldDeclarationSyntax;

                    foreach (var field in GetFieldNodes(fd))
                    {
                        structnode.Fields.Add(field);
                    }
                }
                else if (member is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                    structnode.Methods.Add(GetMethodNode(method));
                }
            }
            if (EachStruct.BaseList != null)
            {
                foreach (var baseType in EachStruct.BaseList.Types)
                {
                    structnode.Links.Add(baseType.ToString());
                }
            }
            return structnode;
        }
        private InterfaceNode GetInterfaceNode(InterfaceDeclarationSyntax EachInterface)
        {
            InterfaceNode interfacenode = new InterfaceNode();
            interfacenode.InterfaceName = EachInterface.Identifier.ToString();
            foreach (var member in EachInterface.Members)
            {
                MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                interfacenode.Methods.Add(GetMethodNode(method));
            }
            if (EachInterface.BaseList != null)
            {
                foreach (var baseType in EachInterface.BaseList.Types)
                {
                    interfacenode.Links.Add(baseType.ToString());
                }
            }
            return interfacenode;
        }

        private EnumNode GetEnumNode(EnumDeclarationSyntax EachEnum)
        {
            EnumNode enumnode = new EnumNode();
            enumnode.EnumName = EachEnum.Identifier.ToString();
            foreach (var member in EachEnum.Members)
            {
                enumnode.AddMember(member.Identifier.ToString());
            }
            return enumnode;
        }
        public UMLClass ParseFiles(Solution solution)
        {
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            foreach (var file in solution.AllFiles)
            {
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(file.OriginalText));
            }

            //var references = new MetadataReference[]
            //{
            //        MetadataReference.CreateFromAssembly(typeof(object).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.IO.File).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.String).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.Linq.Enumerable).Assembly),
            //};

            //var compilation = CSharpCompilation.Create("temporary",
            //                                             syntaxTrees,
            //                                            references);
            //var diagnostics = compilation.GetDiagnostics();

            //foreach (var item in diagnostics)
            //{
            //    if (item.Severity == DiagnosticSeverity.Error)
            //    {
            //        Console.WriteLine("Code has compile time errors. Kindly fix them");
            //        return new UMLClass();
            //    }
            //}


            UMLClass uml = new UMLClass();
            foreach (var st in syntaxTrees)
            {
                var AllClasses = st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                //For each class in file
                foreach (var EachClass in AllClasses)
                {
                    uml.ClassNodes.Add(GetClassNode(EachClass));
                }

                //For each struct in file
                var AllStructs = st.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();
                foreach (var EachStruct in AllStructs)
                {
                    uml.StructNodes.Add(GetStructNode(EachStruct));
                }

                // For reach interface in file
                var AllInterfaces = st.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
                foreach (var EachInterface in AllInterfaces)
                {
                    uml.InterfaceNodes.Add(GetInterfaceNode(EachInterface));
                }

                //For each enum in file
                var AllEnums = st.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>();
                foreach (var EachEnum in AllEnums)
                {
                    uml.EnumNodes.Add(GetEnumNode(EachEnum));
                }
            }
            return uml;
        }





        public UMLClass AnalyzeCode(Solution solution)
        {
            // string path = @"D:\gsoc related\Roslyn\RoslynExperiments\RoslynExperiments\TestCode";
            //string path = @"D:\gsoc related\Roslyn\RoslynExperiments\RoslynExperiments.sln";
            
            // var workspace = MSBuildWorkspace.Create().OpenSolutionAsync(path);
            //string[] files = Directory.GetFiles(path);
            //List<string> fileContent = new List<string>();
            //foreach (string fileName in files)
            //{
            //    //if (Path.GetExtension(fileName) == ".cs")
            //    //{
            //    //    string readText = File.ReadAllText(fileName);
            //    //    //   Console.WriteLine("Examining file " + fileName + "\n");
            //    //    fileContent.Add(readText);
            //    //    // ParseFiles(readText);
            //    //    // Console.Write("\n\n\n\n");
            //    //}
            //}
             return ParseFiles(solution);
            //return ParseFiles(fileContent);
          //  Console.ReadLine();
        }
    }

}