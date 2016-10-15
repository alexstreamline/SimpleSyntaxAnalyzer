using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace VisualBasicCodeAnalysis.Analyzer
{
    public class VisualBasicAnalysis : CodeAnalysis
    {
        private Dictionary<string, string> CharBasicDictionary = new Dictionary<string, string>();

        public void DictInit()
        {
            CharBasicDictionary.Add("&","Long");
            CharBasicDictionary.Add("%", "Integer");
            CharBasicDictionary.Add("#", "Double");
            CharBasicDictionary.Add("!", "Single");
            CharBasicDictionary.Add("@", "Decimal");
            CharBasicDictionary.Add("$", "String");
        }
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

        public void FuncSearcher(SyntaxTree syntaxTree)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement))
                .OfType<MethodStatementSyntax>();
            var func_List = pattern.ToList();
            int a = 10;
            foreach (var func_description in func_List)
            {
                var x = syntaxTree.GetLineSpan(func_description.FullSpan);
              /*  new FuncStruct(func_description.Identifier.Text,
               func_description.AsClause.Type, func_description.ParameterList,func_description.)*/
            }

            

        }

        public List<FuncStruct> FuncSearcher1(SyntaxTree syntaxTree)
        {
            
            List<FuncStruct> functionDescList = new List<FuncStruct>();
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(m => (m.Kind() == SyntaxKind.FunctionBlock) /*|| (m.Kind() == SyntaxKind.ConstructorBlock)*/)
                ;
            var func_List = pattern.ToList();
            if (func_List.Count != 0)
            {


                foreach (var func_description in func_List)
                {
                    var x = syntaxTree.GetLineSpan(func_description.Span);
                    int count = x.Span.End.Line - x.Span.Start.Line;
                    int def_offset = x.Span.Start.Line + 1;
                    string def_path = x.Path;
                    var xxx =
                        (MethodStatementSyntax)
                        func_description.ChildNodes().First(m => m.Kind() == SyntaxKind.FunctionStatement);

                    

                    var qwerty = new FuncStruct(xxx.Identifier.Text, ReturnTypeToString(xxx) ,
                        ParamListToString(xxx.ParameterList), count,def_offset, def_path);
                    functionDescList.Add(qwerty);
                    /*  new FuncStruct(func_description.Identifier.Text,
                     func_description.AsClause.Type, func_description.ParameterList,func_description.)*/
                }
            }
            return functionDescList;


        }

        public string ParamListToString(ParameterListSyntax parameterListSyntax)
        {
            string parametrs = String.Empty;
            var temp = parameterListSyntax.Parameters.Select(x => x?.AsClause?.Type.ToString()).ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] == null)
                {
                    string paramName = parameterListSyntax.Parameters[i].Identifier.Identifier.Text;
                    string lastSignInParamName = paramName.Substring(paramName.Length - 1);
                    string returnParamName = String.Empty;
                    if (CharBasicDictionary.ContainsKey(lastSignInParamName))
                    {
                        returnParamName = CharBasicDictionary[lastSignInParamName];
                    }
                    parametrs += returnParamName;
                }
                else
                {
                    parametrs += temp[i];
                }
                if ((i > 0) && (i != temp.Count-1))
                {
                    parametrs += ";";
                }
            }
            return parametrs;
        }

        public string ReturnTypeToString(MethodStatementSyntax function)
        {
            string parametrs = string.Empty;
            if (function != null)
            {
                string returnTypeName = function.AsClause?.Type?.ToString();

                if (returnTypeName == null)
                {
                    string returnTypeFromFuncName = function.Identifier.Text;
                    //string paramName = parameterListSyntax.Parameters[i].Identifier.Identifier.Text;
                    string lastSignInTypeName = returnTypeFromFuncName.Substring(returnTypeFromFuncName.Length - 1);
                    if (CharBasicDictionary.ContainsKey(lastSignInTypeName))
                    {
                        parametrs += CharBasicDictionary[lastSignInTypeName];
                    }
                    else
                    {
                        parametrs += string.Empty;
                    }
                    
                }
                else
                {
                    parametrs += returnTypeName;
                }
            }

            return parametrs;
        }

        public void Var11Searcher(SyntaxTree syntaxTree)
        {
            var newTree = syntaxTree;
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.FunctionStatement)
                .Select(node => node.GetText().Replace(0,0,"111"))
                .Select(newTree.WithChangedText)
                ;
            var xxx = newTree.GetChanges(syntaxTree);

        }

        

        public List<FuncStruct> GetProjectTest(string folderPath) //todo не путь к папке, а путь к файлу проекта!!!
        {
            DictInit();
            List<FuncStruct> funcStructList = new List<FuncStruct>();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(folderPath).Result; //todo сюда должен приходить путь до sln файла!!!!
            var projects = solution.Projects.ToList();
            foreach (var project in projects)
            {
                var bxx = project.Documents.ToList();
                foreach (var doc in bxx)
                {
                    SyntaxTree synTree = doc.GetSyntaxTreeAsync().Result;
                  var x = FuncSearcher1(synTree);
                    funcStructList.AddRange(x);
                }
                
            }
            //var test1 = projects[13];
            //Compilation compilation = test1.GetCompilationAsync().Result;
            //SyntaxTree syntaxTree1 = test1.Documents.ToList()[0].GetSyntaxTreeAsync().Result;
            //FuncSearcher(syntaxTree1);
            //Var11Searcher(syntaxTree1);
            //foreach (var document in test1.Documents)
            //{
            //    SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;

            //    SemanticModel model = compilation.GetSemanticModel(syntaxTree);
            //    //VisualBasicSyntaxWalker a = new VisualBasicSyntaxWalker();             need override
            //  //  a.Visit();   

            //}
            //foreach (var project in projects)
            //{
                
            //}
           // var documets=  projects.Select(x => x.Documents.Where(y => y.SourceCodeKind == SourceCodeKind.Regular));
            //documets.Select(x => x.Select(y => y.TryGetSyntaxTree()))
            return funcStructList;
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

        public struct FuncStruct
        {
            public string Name;
            public string ReturnType;
            public string TypeParam;
            public int CountLine;
            public string DefFile;
            public int DefOffset;

            public FuncStruct(string name, string returnType, string typeParam, int countLine, int defOffset, string defFile)
            {
               Name = name;
               ReturnType = returnType;
               TypeParam = typeParam;
               CountLine = countLine;
               DefOffset = defOffset;
               DefFile = defFile;
            }
        }
    }
}
