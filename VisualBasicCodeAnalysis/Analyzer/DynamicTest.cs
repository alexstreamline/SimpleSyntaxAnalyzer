using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBasicCodeAnalysis.Analyzer;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using static VisualBasicCodeAnalysis.Analyzer.FullThirdLevelAnalyzer;

namespace VisualBasicCodeAnalysis.Analyzer
{
    /// <summary>
    /// проверка функциональной возможности вставки датчиков для динамики
    /// </summary>
    public static class DynamicTest 
    {
        /// <summary>
        ///вставка датчиков в заранее определнные строчки кода 
        /// </summary>
        public static void PasteMarker(Dictionary<int,FullThirdLevelAnalyzer.FunctionStruct> inputFuncList )
        {
            Dictionary<string,int> filesDictionary = new Dictionary<string, int>(); 
            foreach (var functionStruct in inputFuncList)
            {
                //так как вечно забываю, как это работает, описываю
                // если в словаре есть путь к файлу, в который сейчас будем вставлять маркер - увеличиваем его
                // value значение на 1 и вставляем маркер (ибо каждый новый сдвигает все последующие позиции вниз)
                if (filesDictionary.ContainsKey(functionStruct.Value.DefFile)) 
                {
                   ReWriteFile(functionStruct.Value.DefFile,functionStruct.Value.MarkerPosition + filesDictionary[functionStruct.Value.DefFile],functionStruct.Value.Id, 3);
                   filesDictionary[functionStruct.Value.DefFile]++;
                }
                else
                {
                    filesDictionary.Add(functionStruct.Value.DefFile,0);
                    ReWriteFile(functionStruct.Value.DefFile,functionStruct.Value.DefOffset,functionStruct.Value.Id, 3);
                }
               
            }
            
        }

        public static void PasteMarkerLinSection(Project project)
        {
            var docList = project.Documents.ToList();
            foreach (var document in docList)
            {
                if (document.Name != "Logger.vb")
                {
                    var linListInDoc = LinearSectionsList.Where(x => x.Value.FileName == document.FilePath)
                        .OrderBy(x => x.Value.StartLine)
                        .ToList();

                    int offset = 0;
                    foreach (var linSectionStruct in linListInDoc)
                    {



                        ReWriteFile(linSectionStruct.Value.FileName,
                            linSectionStruct.Value.StartLine + offset - 1,
                            linSectionStruct.Value.GlobalID, 2);
                        offset++;


                    }
                }
            }
        }
        /// <summary>
        /// удаление всех маркеров в проекте
        /// </summary>
        /// <param name="projectFilePath"></param>
        public static void UnPasteMarker(string projectFilePath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(projectFilePath).Result;
            var projects = solution.Projects.ToList();
            foreach (var project in projects)
            {
                var documentList = project.Documents.ToList();
                foreach (var document in documentList)
                {
                   DeleteMarkerFromFile(document.FilePath);
                   DeleteLinMarkerFromFile(document.FilePath);
                }
            }

        }

        /// <summary>
        /// добавление класса логгера в анализируемый проект
        /// ссылку на серилог все равно надо добавить самому через Nuget пакет
        /// </summary>
        /// <param name="filePath"> путь к файлу проекта </param>
        public static void AddLogClassToProject(string filePath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(filePath).Result;
            var projects = solution.Projects.ToList();
            foreach (var project in projects)
            {
                
               // project.AddDocument("", )
            }
            //var p = new Microsoft.Build.Evaluation.Project(@"C:\projects\BabDb\test\test.csproj");
            //p.AddItem("Folder", @"C:\projects\BabDb\test\test2");
            //p.AddItem("Compile", @"C:\projects\BabDb\test\test2\Class1.cs");
            //p.Save();
        }
       
        /// <summary>
        /// вставялет строку в файл в опреденное место
        /// </summary>
        /// <param name="filePath"> путь к файлу </param>
        /// <param name="line"> номер строки, куда будет вставлена необходимая строка</param>
        /// <param name="idFunction"> ID функции (или линейного участка), который запишется в логгер </param>
        public static void ReWriteFile(string filePath, int line, int idFunction, int analyzeLevel)
        {
            string[] allLines = File.ReadAllLines(filePath);
            List<string> allLinesToList = allLines.ToList();
            string messageLine = String.Empty;
            if (analyzeLevel == 3)
            {
                 messageLine = @"Logger.Log(""" + idFunction +
            @""")";
            }
            if (analyzeLevel == 2)
            {
                 messageLine = @"Logger.LogLin(""" + idFunction +
            @""")";
            }
            
            allLinesToList.Insert(line,messageLine);
            File.WriteAllLines(filePath, allLinesToList.ToArray());
        }

        /// <summary>
        /// Удаление маркеров из файла
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteMarkerFromFile(string filePath)
        {
            string[] allLines = File.ReadAllLines(filePath);
            List<string> allLinesToList = allLines.ToList();
            List<int> stringsNumToDelete = new List<int>();
            int key = 0;
            foreach (var line in allLinesToList)      //находим строки с маркерами и отмечаем их
            {
                Regex regex = new Regex(@"Logger.Log");
                MatchCollection matches = regex.Matches(line);
                if (matches.Count > 0)
                  {
                    stringsNumToDelete.Add(key);                    
                  }
                key++;
            }
           
                int correct = 0;
                foreach (var stringNum in stringsNumToDelete)
                {
                    allLinesToList.RemoveAt(stringNum-correct);
                    correct++;
                }
            
          
            
            File.WriteAllLines(filePath, allLinesToList.ToArray());  //перезаписываем файл уже без маркеров
        }

        public static void DeleteLinMarkerFromFile(string filePath)
        {
            string[] allLines = File.ReadAllLines(filePath);
            List<string> allLinesToList = allLines.ToList();
            List<int> stringsNumToDelete = new List<int>();
            int key = 0;
            foreach (var line in allLinesToList)      //находим строки с маркерами и отмечаем их
            {
                Regex regex = new Regex(@"Logger.LogLin");
                MatchCollection matches = regex.Matches(line);
                if (matches.Count > 0)
                {
                    stringsNumToDelete.Add(key);
                }
                key++;
            }

            int correct = 0;
            foreach (var stringNum in stringsNumToDelete)
            {
                allLinesToList.RemoveAt(stringNum - correct);
                correct++;
            }



            File.WriteAllLines(filePath, allLinesToList.ToArray());  //перезаписываем файл уже без маркеров
        }
    }
}
