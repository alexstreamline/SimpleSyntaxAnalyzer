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
    public class SecondLevelCSharp
    {
        public static List<LinearSection> LinearSections = new List<LinearSection>();

        public void StartTest()
        {
          
            
        }

        public void StartAnalysis()
        {
           //все найденные функции уже являются линейными участками 
           //добавляем их в список структур
        }

        public void LinearSectionSearcher(SyntaxNode node, KeyStatement keyStatement)
        {
            
        }

        public void GetMethodList(SyntaxTree syntaxTree)
        {
            var methodList = syntaxTree.GetRootAsync()
                 .Result.DescendantNodes()
                 .Where(node => node.Kind() == SyntaxKind.MethodDeclaration)
                 .ToList();
        }

        public void IfElseStatementAnalysis(SyntaxNode methodNode)
        {
           var methodList =  methodNode.DescendantNodes()
                .Where(node => node.Kind() == SyntaxKind.MethodDeclaration)
                .ToList();
        }

    }
    /// <summary>
    /// Структура линейного участка ( ЛУ ) (то есть объекта без ветвлений внутри)
    /// </summary>
    public struct LinearSection
    {
        public string ProjectName;
        public string FileName;
        /// <summary>
        /// имя функции(метода, процедуры), внутри которой находится ЛУ
        /// </summary>
        public string FuncName;
        public int StartLine;
        public int EndLine;

    }

    /// <summary>
    /// Ключевые объекты для поиска - то, что влияет на линейные участки
    /// </summary>
    public enum KeyStatement
    {
        If,
        Switch,
        While,
        Do,
        For,
        Return,
        Break,
        Goto,
        Throw,
        Continue,
        Foreach,
    }
}
