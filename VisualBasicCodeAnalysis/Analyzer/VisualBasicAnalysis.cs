using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace VisualBasicCodeAnalysis.Analyzer
{
    public class VisualBasicAnalysis : CodeAnalysis
    {
        private Dictionary<string, string> CharBasicDictionary = new Dictionary<string, string>();
        private List<SyntaxNode> _functionAndSubNodes = new List<SyntaxNode>();
        List<FuncStruct> funcStructList = new List<FuncStruct>();
        List<VarStruct> varStructList = new List<VarStruct>();
        List<FunctionStruct> functionStructList = new List<FunctionStruct>();
        Dictionary<string,string> _funcToFuncUsing = new Dictionary<string, string>();
        public static Dictionary<int, FunctionStruct> FunctionList = new Dictionary<int, FunctionStruct>();
        public static List<FuncToFuncLinkStruct> FuncToFuncLinkStructsList = new List<FuncToFuncLinkStruct>();


        public VisualBasicAnalysis()
        {
           DictInit(); 
        }

        public void DictInit()
        {
            CharBasicDictionary.Add("&","Long");
            CharBasicDictionary.Add("%", "Integer");
            CharBasicDictionary.Add("#", "Double");
            CharBasicDictionary.Add("!", "Single");
            CharBasicDictionary.Add("@", "Decimal");
            CharBasicDictionary.Add("$", "String");
        }

        public void FuncToFuncUsing(SyntaxNode syntaxNode)
        {
            
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

        //public List<VarStruct> Va1111rSearcher(SyntaxTree syntaxTree)
        //{
        //    SyntaxNode root = syntaxTree.GetRoot();
        //    var pattern = root.DescendantNodes()
        //        .Where(node => node.Kind() == SyntaxKind.ModifiedIdentifier)
        //        .Select(node => node.ChildTokens().First(x => x.Kind() == SyntaxKind.IdentifierToken));
        //    return pattern.Select(x => new VarStruct(x.Text, x.ValueText)).ToList();
            
        //}
        public void VarSearcher(SyntaxTree syntaxTree)
        {
            int def_offset  = 0;
            string def_file = string.Empty;
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.FieldDeclaration)
                .ToList();
            int id = 1;
            foreach (var variable in pattern)
            {
                var varLineSpan = syntaxTree.GetLineSpan(variable.Span);
                def_offset = varLineSpan.Span.Start.Line + 1;
                def_file = varLineSpan.Path;
                var varTypeDesc =
                    (VariableDeclaratorSyntax)
                    variable.ChildNodes().First(x => x.Kind() == SyntaxKind.VariableDeclarator);
                var varType = varTypeDesc?.AsClause?.Type().ToString();
                var varTypeDescForName =
                   (ModifiedIdentifierSyntax)variable.ChildNodes().First(x => x.Kind() == SyntaxKind.VariableDeclarator)
                   .ChildNodes().First(x => x.Kind() == SyntaxKind.ModifiedIdentifier);
               
               
                varStructList.Add(new VarStruct(varTypeDescForName.Identifier.Text, varType, def_file, def_offset,id));
                id++;
            }
                
            

        }

        public void FuncSearcher(SyntaxTree syntaxTree)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement))
                .OfType<MethodStatementSyntax>();
            var func_List = pattern.ToList();
            foreach (var func_description in func_List)
            {
                var x = syntaxTree.GetLineSpan(func_description.FullSpan);
              /*  new FuncStruct(func_description.Identifier.Text,
               func_description.AsClause.Type, func_description.ParameterList,func_description.)*/
            }

        }

        public void GetFunctionsAsSyntaxNodes(SyntaxTree syntaxTree)
        {
            
            var functionsList = syntaxTree.GetRootAsync().Result.DescendantNodes()
                .Where(node => (node.Kind() == SyntaxKind.FunctionBlock) || (node.Kind() == SyntaxKind.SubBlock))
                .ToList();

            if (functionsList.Count != 0)
            {
                foreach (var function in functionsList)
                {
                    var lineSpan = syntaxTree.GetLineSpan(function.Span);
                    int startLine = lineSpan.Span.Start.Line + 1;
                    int endLine = lineSpan.Span.End.Line + 1;
                   
                    var functionNode =
                        (MethodStatementSyntax)
                        function.ChildNodes().First(m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement));
                    if (functionNode != null)
                    {
                       
                    }
                     /*  new FuncStruct(func_description.Identifier.Text,
                     func_description.AsClause.Type, func_description.ParameterList,func_description.)*/
                }
            }

        }

        public void FuncSearcher(SyntaxTree syntaxTree, Document document)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            var funcNodeList = root.DescendantNodes() //ищем функции/процедуры
                    .Where(node => node.Kind() == SyntaxKind.FunctionBlock || node.Kind() == SyntaxKind.SubBlock);
            var funcList = funcNodeList.ToList();
            if (funcList.Count != 0)
            {
                int iterationId = 0;
                foreach (var func_description in funcList)
                {

                    var x = syntaxTree.GetLineSpan(func_description.Span);
                    int count = x.Span.End.Line - x.Span.Start.Line;
                    int def_offset = x.Span.Start.Line + 1;
                    string def_path = x.Path;
                    var functionNode =(MethodStatementSyntax)func_description
                        .ChildNodes().First(
                                m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement));
                    if (functionNode != null)
                    {
                        _functionAndSubNodes.Add(functionNode);
                        var y = (SyntaxNode) functionNode;
                        var z = syntaxTree.GetLineSpan(y.Span);
                        var markerPosition = z.Span.End.Line + 2;
                            //todo добавил положение маркеров, отрефачить теперь все это болото
                        var qwerty = new FuncStruct(functionNode.Identifier.Text, ReturnTypeToString(functionNode),
                            ParamListToString(functionNode.ParameterList), count, /*def_offset*/ markerPosition,
                            def_path, document.Name, iterationId);
                        // functionDescList.Add(qwerty);
                        funcStructList.Add(qwerty);
                    }
                    //new FuncStruct(func_description.,
                    //func_description.AsClause.Type, func_description.ParameterList,func_description.)
                    iterationId++;
                }
            }
        }


        public List<FuncStruct> FuncSearcher1(SyntaxTree syntaxTree, Document document)
        {
            List<FuncStruct> functionDescList = new List<FuncStruct>();
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern = root.DescendantNodes()
                .Where(node => (node.Kind() == SyntaxKind.FunctionBlock) || (node.Kind() == SyntaxKind.SubBlock))
                ;
            var funcList = pattern.ToList();
            if (funcList.Count != 0)
            {
                int iterationId = 0;
                foreach (var func_description in funcList)
                {
                    
                    var x = syntaxTree.GetLineSpan(func_description.Span);
                    int count = x.Span.End.Line - x.Span.Start.Line;
                    int def_offset = x.Span.Start.Line + 1;
                    string def_path = x.Path;
                    var functionNode =
                        (MethodStatementSyntax)
                        func_description.ChildNodes().First(m => (m.Kind() == SyntaxKind.FunctionStatement)||(m.Kind() == SyntaxKind.SubStatement));
                    if (functionNode != null)
                    {
                        _functionAndSubNodes.Add(functionNode);
                        var y = (SyntaxNode) functionNode;
                        var z = syntaxTree.GetLineSpan(y.Span);
                        var markerPosition = z.Span.End.Line + 2;  //todo добавил положение маркеров, отрефачить теперь все это болото
                       var qwerty = new FuncStruct(functionNode.Identifier.Text, ReturnTypeToString(functionNode),
                            ParamListToString(functionNode.ParameterList), count, /*def_offset*/ markerPosition, def_path,document.Name,iterationId);
                       // functionDescList.Add(qwerty);
                        funcStructList.Add(qwerty);
                    }
                    //new FuncStruct(func_description.,
                    //func_description.AsClause.Type, func_description.ParameterList,func_description.)
                    iterationId++;
                }
            }


            //todo обработчик для процедур

           /*  pattern = root.DescendantNodes()
                .Where(m => (m.Kind() == SyntaxKind.SubBlock)) 
                ;
            var sub_List = pattern.ToList();
            if (sub_List.Count != 0)
            {
                foreach (var sub_description in sub_List)
                {
                    var subPositionSpan = syntaxTree.GetLineSpan(sub_description.Span);
                    int count = subPositionSpan.Span.End.Line - subPositionSpan.Span.Start.Line;
                    int def_offset = subPositionSpan.Span.Start.Line + 1;
                    string def_path = subPositionSpan.Path;
                    var subStatement =
                        (MethodStatementSyntax)
                        sub_description.ChildNodes().First(m => m.Kind() == SyntaxKind.SubStatement);
                    if (subStatement != null)
                    {
                        _functionAndSubNodes.Add(subStatement);

                        //var qwerty = new FuncStruct(subStatement.Identifier.Text, "void",
                        //    ParamListToString(subStatement.ParameterList), count, def_offset, def_path,document.Name,subStatement);
                        //functionDescList.Add(qwerty);
                    }
                    /*  new FuncStruct(func_description.Identifier.Text,
                     func_description.AsClause.Type, func_description.ParameterList,func_description.)
                }
            }*/
            return functionDescList;


        }

        public string ParamListToString(ParameterListSyntax parameterListSyntax)
        {
            string parametrs = string.Empty;
            var temp = parameterListSyntax.Parameters.Select(x => x?.AsClause?.Type.ToString()).ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] == null)
                {
                    string paramName = parameterListSyntax.Parameters[i].Identifier.Identifier.Text;
                    string lastSignInParamName = paramName.Substring(paramName.Length - 1);
                    string returnParamName = string.Empty;
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
                if (i != temp.Count-1)
                {
                    parametrs += ";";

                }
            }
            return parametrs;
        }
        /// <summary>
        /// Get the return type of functions (include type that defined with end's name sign)
        /// </summary>
        /// <param name="function"> function's node from syntax tree </param>
        /// <returns>type of functions</returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">путь к файлу проекта</param>
        public void GetFunctionList(string filePath)    //todo объединить с заполнением фанк структур
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result;
            var project = solution.Projects.ToList().Single();
            Compilation compilation = project.GetCompilationAsync().Result;
            var documentList = project.Documents.ToList();
            int id = 1;
            SyntaxTree syntaxTree1 = documentList[0].GetSyntaxTreeAsync().Result;
            FuncSearcher1(syntaxTree1, documentList[0]);
           // DynamicTest dynTest1 = new DynamicTest();
           // dynTest1.PasteMarker(funcStructList);
            foreach (var document in documentList)
            {
                SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
                SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                FuncSearcher1(syntaxTree, document);
              //  DynamicTest dynTest = new DynamicTest();
              //  dynTest.PasteMarker(funcStructList);
                var functionsList = syntaxTree.GetRootAsync().Result.DescendantNodes()
                .Where(node => (node.Kind() == SyntaxKind.FunctionBlock) || (node.Kind() == SyntaxKind.SubBlock))
                .ToList();
                 
                if (functionsList.Count != 0)
                {
                    foreach (var function in functionsList)
                    {
                        var lineSpan = syntaxTree.GetLineSpan(function.Span);
                        int startLine = lineSpan.Span.Start.Line + 1;
                        int endLine = lineSpan.Span.End.Line + 1;
                        List<UsingStruct> usingStructList = new List<UsingStruct>();
                        var functionNode =
                            (MethodStatementSyntax)
                            function.ChildNodes().First(m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement));
                        if (functionNode != null)
                        {
                            var mod = semanticModel.GetDeclaredSymbol(functionNode);
                            var test = SymbolFinder.FindReferencesAsync(mod, project.Solution).Result.FirstOrDefault();
                            if (test != null)
                            {
                               
                                foreach (var location in test.Locations)
                                {
                                    //var locOffset = syntaxTree.GetLineSpan(location.Location.SourceSpan).Span;
                                    var locOffset = location.Location.SourceSpan;
                                    var name = location.Document.Name;
                                    UsingStruct usingStruct = new UsingStruct(locOffset.Start,locOffset.End, name);
                                    usingStructList.Add(usingStruct);
                                }
                            }
                            // functionStructList.Add(new FunctionStruct(functionNode.Identifier.Text,id,document.Name, startLine,endLine,usingStructList));
                            //functionStructList.Add(new FunctionStruct(functionNode.Identifier.Text, id, document.Name, function.Span.Start, function.Span.End, usingStructList)); //вариант для спанов вместо строк
                            FunctionList.Add(id, new FunctionStruct(functionNode.Identifier.Text, id, document.Name, function.Span.Start, function.Span.End, usingStructList));
                            id++;
                        }
                        /*  new FuncStruct(func_description.Identifier.Text,
                         func_description.AsClause.Type, func_description.ParameterList,func_description.)*/
                    }

                    //foreach (var function in functionsList)
                    //{
                    //    foreach (var functionFromCollection in functionStructList) //todo сравнение фукнций из списка с функциями из юзингов
                    //    {
                    //        if ((location.Document.Name == functionFromCollection.DocName) &&
                    //            (locOffset.Start.Line > functionFromCollection.DefOffset) &&
                    //             (locOffset.End.Line < functionFromCollection.DefOffset + functionFromCollection.CountLine))
                    //        {
                    //            outputText += $"Функция {location.Document.Name}  содержит в себе использовние функции {functionFromCollection.Name} \n Строка {locOffset.Start} , файл  {location.Document.FilePath} \n";
                    //        }
                    //    }
                    //}
                }
            }
            foreach (var funcStruct in FunctionList)  //для каждой найденной функции проверить, на какие функции приходятся ее вызовы
            {
                foreach (var usingFunc in funcStruct.Value.UsingList)
                {
                    var funcCompareList = FunctionList.Where(x => x.Value.DocumentName == usingFunc.DocumentName);
                    // funcCompareList.Where(x => (usingFunc.StartLine > x.StartLine) && (usingFunc.FinishLine < x.FinishLine)).Select(x => )
                    foreach (var f in funcCompareList)
                    {
                        if ((usingFunc.StartLine > f.Value.StartLine) && (usingFunc.FinishLine < f.Value.FinishLine))
                        {
                            if (!FuncToFuncLinkStructsList.Contains(new FuncToFuncLinkStruct(f.Key, funcStruct.Key)))
                            {
                                FuncToFuncLinkStructsList.Add(new FuncToFuncLinkStruct(f.Key, funcStruct.Key));
                            }
                            //if (!_funcToFuncUsing.ContainsKey(f.Value.Name))
                            //{
                            //    _funcToFuncUsing.Add(f.Value.Name, funcStruct.Value.Name);
                            //}
                            //usingFunc.FuncUsingName = f.Name;
                            //данный вызов функции происходит в функции f.Name
                        }
                    }
                }

            }           
            FuncToFuncLinkStructsList = FuncToFuncLinkStructsList.OrderBy(x => x.ParentFuncId).ToList();
        }

        public void GetFunctionUsingLine(Project project)
        {
            var documentList = project.Documents.ToList();
            foreach (var document in documentList)
            {
                var usingListFromCurrentDoc = functionStructList
                    .SelectMany(m => m.UsingList)
                    .Where(m => m.DocumentName == document.Name);
                SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
               // var lineSpan = syntaxTree.GetLineSpan(function.Span);
              //  var usingListWithLineNumber = usingListFromCurrentDoc.Select(m => {})
            }
           
        }
            

        public Tuple<List<FuncStruct>, List<VarStruct>> GetProjectTest(string filePath) 
        {
            // DictInit();
            funcStructList = new List<FuncStruct>();
            varStructList = new List<VarStruct>();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result; 
            var projects = solution.Projects.ToList();
            foreach (var project in projects)
            {
                Compilation compilation = project.GetCompilationAsync().Result;
                var bxx = project.Documents.ToList();
                //var doc = bxx[8];


                //SyntaxTree synTree = doc.GetSyntaxTreeAsync().Result;
                //SemanticModel semanticModel = compilation.GetSemanticModel(synTree);
                //var y = VarSearcher(synTree);
                //var x = FuncSearcher1(synTree,doc);
                //funcStructList.AddRange(x);
                //varStructList.AddRange(y);
                //WorkWithSemanticModel(semanticModel, project, synTree, funcStructList);


                foreach (var doc in bxx)
                {
                    SyntaxTree synTree = doc.GetSyntaxTreeAsync().Result;
                   // SemanticModel semanticModel = compilation.GetSemanticModel(synTree);
                   // var y = VarSearcher(synTree);
                    var x = FuncSearcher1(synTree,doc);
                    funcStructList.AddRange(x);
                   // varStructList.AddRange(y);
                    //WorkWithSemanticModel(semanticModel, project, synTree, funcStructList);
                }

            }
            WorkWithSemanticModel(filePath);
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
            return Tuple.Create(funcStructList,varStructList);
        }

        public void WorkWithSemanticModel(string filePath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result;
            var projects = solution.Projects.ToList();
            string output = string.Empty;
            foreach (var project in projects)
            {
                Compilation compilation = project.GetCompilationAsync().Result;
                var documentList = project.Documents.ToList();
                foreach (var doc in documentList)
                {
                    var funcFromDocList = funcStructList.Where(x => x.DocName == doc.Name).ToList();
                    SyntaxTree synTree = doc.GetSyntaxTreeAsync().Result;
                    SemanticModel semanticModel = compilation.GetSemanticModel(synTree);
                    foreach (var funcFromDoc in funcFromDocList)
                    {
                        //var mod = semanticModel.GetDeclaredSymbol(funcFromDoc.FuncSyntaxNode);
                        output = WorkWithSemanticModel(semanticModel, project, synTree, funcFromDoc);
                    }
                   
                }

            }
        }

        public string WorkWithSemanticModel(SemanticModel semanticModel, Project project, SyntaxTree syntaxTree, FuncStruct funcStruct)
        {
            string outputText = string.Empty;
            SyntaxNode root = syntaxTree.GetRoot();
            var pattern1 = root
               .DescendantNodes().Where(m => (m.Kind() == SyntaxKind.SubStatement) || (m.Kind() == SyntaxKind.FunctionStatement))
               .OfType<MethodStatementSyntax>()
               .FirstOrDefault(x => x.Identifier.Text == funcStruct.Name)
               ;
            //var pattern =  root.DescendantNodes()
            //    .Where(m => (m.Kind() == SyntaxKind.SubStatement) || (m.Kind() == SyntaxKind.FunctionStatement)).Select(m => m.ChildNodes().FirstOrDefault(x => x.Kind() == SyntaxKind.IdentifierToken)).ToList()
            //    ;
            var mod = semanticModel.GetDeclaredSymbol(pattern1);
            var test = SymbolFinder.FindReferencesAsync(mod, project.Solution).Result.Single();
                    if (test != null)
                    {
                        foreach (var location in test.Locations)
                        {
                            
                            var locOffset = syntaxTree.GetLineSpan(location.Location.SourceSpan).Span;
                    
                    var x = syntaxTree.GetLineSpan(location.Location.SourceSpan);
                    foreach (var func in funcStructList)
                            {
                                if ((location.Document.Name == func.DocName) &&
                                    (locOffset.Start.Line > func.DefOffset) &&
                                     (locOffset.End.Line < func.DefOffset + func.CountLine))
                                {
                                    outputText += $"Функция {location.Document.Name}  содержит в себе использовние функции {func.Name} \n Строка {locOffset.Start} , файл  {location.Document.FilePath} \n";
                                }
                            }
                        }
                       
                    }
                
            return outputText;
            
        }

        public string[] ChooseVBFile(string folderPath)
        {
            var filePaths = Directory.GetFiles(folderPath, "*.vb", SearchOption.AllDirectories).ToArray();
            return filePaths;
        }
        /// <summary>
        /// тестовый метод для проверки сохранения измененых проектов в солюшене
        /// </summary>
        /// <param name="filePath"></param>
        public void TestWorkspaceChange(string filePath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result;
            Solution newSolution = solution;
            foreach (var projectId in solution.ProjectIds)
            {
                Project project = newSolution.GetProject(projectId);
                foreach (var documentId in project.DocumentIds)
                {
                    Document document = newSolution.GetDocument(documentId);
                    Document newDocument = Formatter.FormatAsync(document).Result;
                    newSolution = newDocument.Project.Solution;
                }
            }
            if (workspace.TryApplyChanges(newSolution))
            {
                MessageBox.Show("Done!");  //тест успешно отработал
            }
            //var project = solution.Projects.ToList().Single();
        }

        public void TestMethodToPasteTextInSourceFile()
        {
            
        }

        #region Structs

        

        
        public struct VarStruct
        {
           public string Name;
           public string Type;
           public string DefFile;
           public int DefOffset;
            public int Id;

            public VarStruct(string name, string type, string defFile, int defOffset, int id)
            {
                Name = name;
                Type = type;
                DefFile = defFile;
                DefOffset = defOffset;
                Id = id;
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
            public string DocName;
            public int Id;

            public FuncStruct(string name, string returnType, string typeParam, int countLine, int defOffset, string defFile, string docName,int id)
            {
               Name = name;
               ReturnType = returnType;
               TypeParam = typeParam;
               CountLine = countLine;
               DefOffset = defOffset;
               DefFile = defFile;
               DocName = docName;
               Id = id;
            }
        }

        public struct FunctionStruct
        {
            public string Name;
            public int Id;
            public string DocumentName;
            public int StartLine;
            public int FinishLine;
            public List<UsingStruct> UsingList;

            public FunctionStruct(string name, int id, string documentName, int startLine, int finishLine, List<UsingStruct> usingList)
            {
                Name = name;
                Id = id;
                DocumentName = documentName;
                StartLine = startLine;
                FinishLine = finishLine;
                UsingList = usingList;
            }
        }

        public struct UsingStruct
        {
            public int StartLine;
            public int FinishLine;
            public string DocumentName;
            public string FuncUsingName;

            public UsingStruct(int startLine, int finishLine, string documentName)
            {
                StartLine = startLine;
                FinishLine = finishLine;
                DocumentName = documentName;
                FuncUsingName = string.Empty;
            }
        }

        public struct FuncToFuncLinkStruct
        {
            public int ParentFuncId;
            public int ChildFuncId;

            public FuncToFuncLinkStruct(int parentId, int childId)
            {
                ParentFuncId = parentId;
                ChildFuncId = childId;
            }
        }
        #endregion
    }
}
