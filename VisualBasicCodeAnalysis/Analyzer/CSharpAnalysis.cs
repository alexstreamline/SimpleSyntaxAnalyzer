using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace VisualBasicCodeAnalysis.Analyzer
{
    class CSharpAnalysis
    {
        public List<string> GetProjectTest(string folderPath) //todo не путь к папке, а путь к файлу проекта!!!
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(folderPath).Result; //todo сюда должен приходить путь до sln файла!!!!
            var projects = solution.Projects.ToList();
            var test1 = projects[13];
            Compilation compilation = test1.GetCompilationAsync().Result;
            SyntaxTree syntaxTree1 = test1.Documents.ToList()[0].GetSyntaxTreeAsync().Result;
            foreach (var document in test1.Documents)
            {
                SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
                SemanticModel model = compilation.GetSemanticModel(syntaxTree);
                //VisualBasicSyntaxWalker a = new VisualBasicSyntaxWalker();             need override
                //  a.Visit();   

            }
            foreach (var project in projects)
            {

            }
            // var documets=  projects.Select(x => x.Documents.Where(y => y.SourceCodeKind == SourceCodeKind.Regular));
            //documets.Select(x => x.Select(y => y.TryGetSyntaxTree()))
            return VarSearcher(syntaxTree1);
        }

        public List<string> VarSearcher(SyntaxTree syntaxTree)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.MethodDeclaration)
                .Select(node => node.ChildTokens().First(x => x.Kind() == SyntaxKind.IdentifierToken));
            return pattern.Select(x => x.Text).ToList();

        }
    }
}
