﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace VisualBasicCodeAnalysis.Analyzer
{
    /// <summary>
    /// класс, в котором находится полная логика анализа по третьему уровню НДВ
    /// </summary>
    public static class FullThirdLevelAnalyzer
    {

       // List<FuncStruct> funcStructList = new List<FuncStruct>();
        static List<VarStruct> varStructList = new List<VarStruct>();
        public static Dictionary<int,FunctionStruct> FunctionStructList = new Dictionary<int, FunctionStruct>();

        public static Dictionary<int, LinearSection> LinearSectionsList = new Dictionary<int, LinearSection>();
        private static readonly Dictionary<string, string> CharBasicDictionary = new Dictionary<string, string>
        {
            ["&"] = "Long",
            ["%"] = "Integer",
            ["#"] = "Double",
            ["!"] = "Single",
            ["@"] = "Decimal",
            ["$"] = "String"
        };

        /// <summary>
        ///  Переменная - используется для выделения уникалных ID найденным функциям
        /// </summary>
        public static int GlobalId;

        
        /// <summary>
        /// Запуск анализа проекта по 3 уровню НДВ
        /// поиск функций, переменных, расстановка датчиков для динамического анализа
        /// </summary>
        /// <param name="filePath"> путь к проекту для анализа </param>
        public static void StartSearching(string filePath)
        {
            GlobalId = 1;
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result;
            var projects = solution.Projects.ToList();
            foreach (var project in projects)
            {
                Compilation compilation = project.GetCompilationAsync().Result;
                var documentList = project.Documents.ToList();
                foreach (var document in documentList)
                {
                    SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
                    GetFunctionsList(syntaxTree,document,compilation, project); //заполнить список структур с описанием функциональных объектов
                    //GetConstructorsList(syntaxTree, document, compilation, project);
                }
            }
           
        }

        public static void PasteMarkersToProject()  //todo прям совсем тупое решение, подумать как сделать лучше
        {
            DynamicTest.PasteMarker(FunctionStructList);
        }


        /// <summary>
        /// Получить список функциональных объектов в этом файле
        /// </summary>
        /// <param name="syntaxTree">синтаксическое дерево этого файла</param>
        /// <param name="document">файл документа, нужен для заполнения полей в структуре функционального объекта</param>
        /// <param name="compilation">объект компиляции, нужен для семантической модели</param>
        /// <param name="project">объект проекта, нужен для работы поиска использования (FindUsage) </param>
        public static void GetFunctionsList(SyntaxTree syntaxTree, Document document, Compilation compilation, Project project)
        {
            var functionsList = syntaxTree.GetRootAsync().Result.DescendantNodes()
                .Where(node => (node.Kind() == SyntaxKind.FunctionBlock) || (node.Kind() == SyntaxKind.SubBlock))
                .ToList();

            if (functionsList.Count != 0)
            {
                foreach (var function in functionsList)
                    //todo переисеновать function в functionNode, а его в свою очередь в functionStatementNode
                  {
                    FindLinearSection(function);
                    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                    var lineSpan = syntaxTree.GetLineSpan(function.Span); //отступ в строках
                    int count = lineSpan.Span.End.Line - lineSpan.Span.Start.Line + 1; //размер функции в строках
                    int defOffset = lineSpan.Span.Start.Line + 1;
                    string defPath = lineSpan.Path;
                    int startLine = lineSpan.Span.Start.Line + 1; //строка, на которой начинается функция
                    int endLine = lineSpan.Span.End.Line + 1; //строка, на которой заканчивается
                    List<UsingStruct> usingStructList = new List<UsingStruct>();
                    var functionNode =
                        (MethodStatementSyntax)
                        function.ChildNodes()
                            .First(
                                m => (m.Kind() == SyntaxKind.FunctionStatement) || (m.Kind() == SyntaxKind.SubStatement));
                    if (functionNode != null)
                    {
                        var findReference =
                            SymbolFinder.FindReferencesAsync(semanticModel.GetDeclaredSymbol(functionNode),
                                project.Solution).Result.FirstOrDefault();
                        if (findReference != null)
                        {

                            foreach (var usingLocation in findReference.Locations)
                                //перечисление найденных использований функциональных объектов
                            {
                                //var locOffset = syntaxTree.GetLineSpan(location.Location.SourceSpan).Span;
                                var locOffset = usingLocation.Location.SourceSpan;
                                    // отступ (в виде спана, а не номера строки)
                                var name = usingLocation.Document.Name; // todo ?? имя документа, где находится ссылка 
                                UsingStruct usingStruct = new UsingStruct(locOffset.Start, locOffset.End, name);
                                usingStructList.Add(usingStruct);
                            }
                        }
                        var funcNodeForMarker = (SyntaxNode) functionNode;
                        int isUseful = 0; //при создании равно нулю, меняется в дальнейшем
                        //по итогам проверки маркеров/анализа использования функций
                        int markerPosition = syntaxTree.GetLineSpan(funcNodeForMarker.Span).Span.End.Line + 2; /*2 - дофига магическое число,
                        позволяет получить нужную позицию (сразу сигнатуры функции)*/
                        FunctionStructList.Add(GlobalId, new FunctionStruct(GlobalId,functionNode.Identifier.Text, ReturnTypeToString(functionNode),
                            ParamListToString(functionNode.ParameterList), count, defPath ,defOffset, isUseful, document.Name,
                                function.Span.Start, function.Span.End, markerPosition, usingStructList));  //заполнение структур функций
                        
                        
                        GlobalId++;
                    }
                    
                }
            }
        }

        /// <summary>
        /// Поиск в коде конструкторов - ибо они тоже являются функциональными объектами
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="document"></param>
        /// <param name="compilation"></param>
        /// <param name="project"></param>
        public static void GetConstructorsList(SyntaxTree syntaxTree, Document document, Compilation compilation, Project project)
        {
            var functionsList = syntaxTree.GetRootAsync().Result.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.ConstructorBlock)
                .ToList();

            if (functionsList.Count != 0)
            {
                foreach (var function in functionsList)
                //todo переисеновать function в functionNode, а его в свою очередь в functionStatementNode
                {
                    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
                    var lineSpan = syntaxTree.GetLineSpan(function.Span); //отступ в строках
                    int count = lineSpan.Span.End.Line - lineSpan.Span.Start.Line + 1; //размер функции в строках
                    int defOffset = lineSpan.Span.Start.Line + 1;
                    string defPath = lineSpan.Path;
                    int startLine = lineSpan.Span.Start.Line + 1; //строка, на которой начинается функция
                    int endLine = lineSpan.Span.End.Line + 1; //строка, на которой заканчивается
                    List<UsingStruct> usingStructList = new List<UsingStruct>();
                    var functionNode =
                        
                        function.ChildNodes()
                            .First(
                                m => m.Kind() == SyntaxKind.SubNewStatement);
                    if (functionNode != null)
                    {
                        var findReference =
                            SymbolFinder.FindReferencesAsync(semanticModel.GetDeclaredSymbol(functionNode),
                                project.Solution).Result.FirstOrDefault();
                        if (findReference != null)
                        {

                            foreach (var usingLocation in findReference.Locations)
                            //перечисление найденных использований функциональных объектов
                            {
                                //var locOffset = syntaxTree.GetLineSpan(location.Location.SourceSpan).Span;
                                var locOffset = usingLocation.Location.SourceSpan;
                                // отступ (в виде спана, а не номера строки)
                                var name = usingLocation.Document.Name; // todo ?? имя документа, где находится ссылка 
                                UsingStruct usingStruct = new UsingStruct(locOffset.Start, locOffset.End, name);
                                usingStructList.Add(usingStruct);
                            }
                        }
                        var funcNodeForMarker = (SyntaxNode)functionNode;
                        int markerPosition = syntaxTree.GetLineSpan(funcNodeForMarker.Span).Span.End.Line + 2; /*2 - дофига магическое число,
                        позволяет получить нужную позицию (сразу сигнатуры функции)*/
                        //functionStructList.Add(new FunctionStruct(GlobalId, string.Empty, string.Empty,
                        //    ParamListToString(functionNode.SubNewStatement.ParameterList), count, defPath, defOffset, document.Name,
                        //        function.Span.Start, function.Span.End, markerPosition, usingStructList));  //заполнение структур функций
                        GlobalId++;
                    }

                }
            }
        }
/// <summary>
/// поиск линейных участков внутри функции
/// </summary>
/// <param name="functionNode"></param>
        public static void FindLinearSection(SyntaxNode functionNode /*, int funcID*/)
        {
            //funcNode - сюда на вход приходит SubBlock или FuncBlock
            //в его чайлдах ищем if else конструкции 
            //Принцип: получаем ноду функции, иследуем на наличие if else конструкций 
            //если находим - создаем 2 новых линейных участка (по if и else ветке)
            //при этом рассматриваем каждую ветку на наличие внутри еще конструкций
            //
            var ifConstructionNodes = functionNode.ChildNodes().Where(node => node.Kind() == SyntaxKind.MultiLineIfBlock || node.Kind() == SyntaxKind.SingleLineIfStatement).ToList();


            //получить список всех первичных чайлд нод
            var nodeList = functionNode.ChildNodes().ToList();
            //начинается с описания - значит все правильно
            if (nodeList.First().IsKind(SyntaxKind.SubStatement) || nodeList.First().IsKind(SyntaxKind.FunctionStatement))
            {
                //заканчивается тоже правильно
                if (nodeList.Last().IsKind(SyntaxKind.EndSubStatement) ||
                    nodeList.Last().IsKind(SyntaxKind.EndFunctionStatement))
                {
                    //теперь свобода действий - ищем конструкции, которые будут разделять код на линейные участки
                    for (int i = 1; i < nodeList.Count-1; i++) //пройдем по всем нодам кроме стартовой и последней
                    {
                       // if (nodeList[i].IsKind(SyntaxKind.MultiLineIfBlock)) 
                    }
                }
            }
            //если нашли внутри функции if - else условия 
            if (ifConstructionNodes.Any())
            {
                foreach (var ifNode in ifConstructionNodes)
                {
                    if (ifNode.ChildNodes().ToList()[0].IsKind(SyntaxKind.IfStatement))
                    {
                        
                    }
                    else
                    {
                        int a = 0;
                    }
                }
                
            }
        }

        public static void IfElseStatement(SyntaxNode syntaxNode)
        {
            var nodeList = syntaxNode.ChildNodes().ToList(); //первая и последняя - опять описательные
            for (int i = 1; i < nodeList.Count - 1; i++) //пройдем по всем нодам кроме стартовой и последней
              {
                FindSplitStatement(nodeList[i]);

              }
        }
        /// <summary>
        /// Логика взаимодействия с неким разделителем (if/else/sitch/case, циклы)
        /// </summary>
        /// <param name="syntaxNode"></param>
        public static void FindSplitStatement(SyntaxNode syntaxNode)
        {
            switch (syntaxNode.Kind())
            {
                case SyntaxKind.MultiLineIfBlock:
                    //todo 
                    break;
                case SyntaxKind.SingleLineIfStatement:
                    //todo 
                    break;
                case SyntaxKind.DoWhileLoopBlock:
                    //todo 
                    break;
                case SyntaxKind.DoUntilLoopBlock:
                    //todo 
                    break;
            }
            
        }

        /// <summary>
        /// Get the return type of functions (include type that defined with end's name sign)
        /// </summary>
        /// <param name="function"> function's node from syntax tree </param>
        /// <returns>type of functions</returns>
        public static string ReturnTypeToString(MethodStatementSyntax function)
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

/// <summary>
/// создание строки с перечислением всех параметров функции
/// </summary>
/// <param name="parameterListSyntax"></param>
/// <returns></returns>
   public static string ParamListToString(ParameterListSyntax parameterListSyntax)
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
                if (i != temp.Count - 1)
                {
                    parametrs += ";";

                }
            }
            return parametrs;
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

            public FuncStruct(string name, string returnType, string typeParam, int countLine, int defOffset, string defFile, string docName, int id)
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
            /// <summary>
            /// Идентификатор - просто порядковый номер от 1 и далее
            /// </summary>
            public int Id;
            /// <summary>
            /// Имя функции
            /// </summary>
            public string Name;
            /// <summary>
            /// Тип возвращаемого значения функции
            /// </summary>
            public string ReturnType;
            /// <summary>
            /// тип(типы) параметров (в одну строку через ; )
            /// </summary>
            public string TypeParam;
            /// <summary>
            /// Размер функции в строках
            /// </summary>
            public int CountLine;
            /// <summary>
            /// Путь к файлу, где находится функция
            /// </summary>
            public string DefFile;
            /// <summary>
            /// величина отступа от начала файла до места, где находится функция
            /// </summary>
            public int DefOffset;
            /// <summary>
            /// используется ли функция
            /// 0 - нет
            /// 1 - нет, но есть родительские/дочерние функции
            /// 2 - да (проверяется срабатыванием маркеров)
            /// </summary>
            public int IsUseful;
          //  public string DocName;
            public string DocumentName;
            public int StartLine;
            public int FinishLine;
            /// <summary>
            /// позиция(номер строки), на которой нужно будет расположить маркер для динамического анализа
            /// </summary>
            public int MarkerPosition;
            public List<UsingStruct> UsingList;

            public FunctionStruct(int id, string name, string returnType, string typeParam, int countLine, string defFile,
                int defOffset, int isUseful, string documentName, int startLine, int finishLine, int markerPosition, List<UsingStruct> usingList)
            {
                Id = id;
                Name = name;
                ReturnType = returnType;
                TypeParam = typeParam;
                CountLine = countLine;
                DefFile = defFile;
                DefOffset = defOffset;
                IsUseful = isUseful;
                DocumentName = documentName;
                StartLine = startLine;
                FinishLine = finishLine;
                MarkerPosition = markerPosition;
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

        /// <summary>
        /// Структура линейного участка ( ЛУ ) (то есть объекта без ветвлений внутри)
        /// </summary>
        public struct LinearSection
        {
            /// <summary>
            /// ID функции, в которой находится локальный участок
            /// </summary>
            public int FunctionID;
            /// <summary>
            ///  ID участка внутри функции
            /// </summary>
            public int LocalID;
            /// <summary>
            /// Глобальный ID участка ( во всем проекте)
            /// </summary>
            public int ID;
            /// <summary>
            /// Размер линейного участка в строках
            /// </summary>
            public int Size;
            /// <summary>
            /// Используемость участка в ходе дин анализа
            /// 0 - не используется, 2 - используется
            /// </summary>
            public int IsUsing;
            public string ProjectName;
            public string FileName;
            /// <summary>
            /// имя функции(метода, процедуры), внутри которой находится ЛУ
            /// </summary>
            public string FuncName;
            public int StartLine;
            public int EndLine;

        }
        #endregion
    }
}