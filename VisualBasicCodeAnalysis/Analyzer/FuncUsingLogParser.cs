using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VisualBasicCodeAnalysis.Analyzer
{
    public static class FuncUsingLogParser
    {
        /// <summary>
        /// Хэш таблица пар (ID функции - ее использование)
        /// 0 - не используется
        /// 1 - не используется, но в это функции есть дочерние (или родительские) функции
        /// 2 - используется (при тесте была запись в лог от маркера этой функции)
        /// </summary>
        public static Dictionary<int,int> IsUsingFunctionDictionary = new Dictionary<int, int>();

        public static void ReadLogFile(string filePath)
        {
            var allLinesFromLog = File.ReadAllLines(filePath);
            foreach (var logString in allLinesFromLog)
            {
                Regex regex = new Regex(@"Функция ""(.*)"" отработала");
                MatchCollection matches = regex.Matches(logString);
                int funcId = Convert.ToInt32(matches[0].Groups[1].Value);
                if (FullThirdLevelAnalyzer.FunctionStructList.ContainsKey(funcId)) //проверяем на всякий случай, если ID есть в логе - он обязан быть в таблице 
                {
                    var changedFuncStruct = FullThirdLevelAnalyzer.FunctionStructList[funcId];
                    changedFuncStruct.IsUseful = 2;
                    FullThirdLevelAnalyzer.FunctionStructList[funcId] = changedFuncStruct;
                    
                }
            }
        }
    }
}
