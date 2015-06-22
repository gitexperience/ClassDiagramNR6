// Copyright (c) AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Logging;



using Microsoft.CodeAnalysis.Syntax;

namespace MDClassDiagramNR6
{
	/// <summary>
	/// Represents a C# project (.csproj file)
	/// </summary>
	public class CSharpProject
	{
		/// <summary>
		/// Parent solution.
		/// </summary>
		public readonly Solution Solution;
		
		/// <summary>
		/// Title is the project name as specified in the .sln file.
		/// </summary>
		public readonly string Title;
		
		/// <summary>
		/// Name of the output assembly.
		/// </summary>
		public readonly string AssemblyName;
		
		/// <summary>
		/// Full path to the .csproj file.
		/// </summary>
		public readonly string FileName;
		
		public readonly List<CSharpFile> Files = new List<CSharpFile>();
		
		/// <summary>
		/// The resolved type system for this project.
		/// This field is initialized once all projects have been loaded (in Solution constructor).
		/// </summary>
		
		public CSharpProject(Solution solution, string title, string fileName)
		{
			// Normalize the file name
			fileName = Path.GetFullPath(fileName);
			
			this.Solution = solution;
			this.Title = title;
			this.FileName = fileName;
			
			// Use MSBuild to open the .csproj
			var msbuildProject = new Microsoft.Build.Evaluation.Project(fileName);
			// Figure out some compiler settings
			this.AssemblyName = msbuildProject.GetPropertyValue("AssemblyName");

            // Parse the C# code files
            foreach (var item in msbuildProject.GetItems("Compile"))
            {
                var file = new CSharpFile(this, Path.Combine(msbuildProject.DirectoryPath, item.EvaluatedInclude));
                Files.Add(file);
            }

		}
		
		IEnumerable<string> ResolveAssemblyReferences(Microsoft.Build.Evaluation.Project project)
		{
			// Use MSBuild to figure out the full path of the referenced assemblies
			var projectInstance = project.CreateProjectInstance();
			projectInstance.SetProperty("BuildingProject", "false");
			project.SetProperty("DesignTimeBuild", "true");
			
			projectInstance.Build("ResolveAssemblyReferences", new [] { new ConsoleLogger(LoggerVerbosity.Minimal) });
			var items = projectInstance.GetItems("_ResolveAssemblyReferenceResolvedFiles");
			string baseDirectory = Path.GetDirectoryName(this.FileName);
			return items.Select(i => Path.Combine(baseDirectory, i.GetMetadataValue("Identity")));
		}
		
		static bool? GetBoolProperty(Microsoft.Build.Evaluation.Project p, string propertyName)
		{
			string val = p.GetPropertyValue(propertyName);
			bool result;
			if (bool.TryParse(val, out result))
				return result;
			else
				return null;
		}
		
		public override string ToString()
		{
			return string.Format("[CSharpProject AssemblyName={0}]", AssemblyName);
		}
	}
}
