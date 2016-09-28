using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace VisualBasicCodeAnalysis.Analyzer
{
    public class VisualBasicAnalysis : CodeAnalysis
    {
        public string[] ParseText(string path)
        {
            SyntaxTree tree;
            using (StreamReader str = new StreamReader(path))
            {
                 tree = VisualBasicSyntaxTree.ParseText(str.ReadToEnd());
            }
            SyntaxNode root = tree.GetRoot();
           // var workspace = MSBuildWorkspace.Create();
            //var y1 =
            //    root?.DescendantNodes();
            //foreach (var txx in y1)
            //{
            //    if (txx.Kind() == SyntaxKind.FunctionStatement)
            //    {
            //        var txx1 = (MethodStatementSyntax) txx;
            //        var s = txx1.Identifier.Text;
            //        string str1 = txx.GetText().ToString();
            //        var a = 0;
            //    }
            //}
            // var bb = y1.Where(m => m.Kind() == SyntaxKind.FunctionBlock).OfType<FunctionAggregationSyntax>();
            //var y =
            //    root?.DescendantNodes()
            //        .Where(m => m.Kind() == SyntaxKind.FunctionBlock)
            //        .OfType<FunctionAggregationSyntax>()
            //        .Select(m => m.FunctionName.ToString()).ToArray();
            var y = root?.DescendantNodes()
                .Where(m => (m.Kind() == SyntaxKind.FunctionStatement)||(m.Kind() == SyntaxKind.SubStatement))
                .OfType<MethodStatementSyntax>()
                .Select(m => m.Identifier.Text)
                .ToArray();
            return y;
        }

        public List<LabelToken> VarSearcher(SyntaxTree syntaxTree)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.ModifiedIdentifier)
                .Select(node => node.ChildTokens().First(x => x.Kind() == SyntaxKind.IdentifierToken));
            return pattern.Select(x => new LabelToken(x.Text, x.ValueText)).ToList();
            
        }

        public List<LabelToken> GetProjectTest(string folderPath) //todo не путь к папке, а путь к файлу проекта!!!
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
      //  public void Get

        public string[] ChooseVBFile(string folderPath)
        {
            var filePaths = Directory.GetFiles(folderPath, "*.vb", SearchOption.AllDirectories).ToArray();
            return filePaths;
        }

        public struct LabelToken
        {
            public string Text;
            public string Name;

            public LabelToken(string text, string name)
            {
                Text = text;
                Name = name;
            }
        }
    }
}
