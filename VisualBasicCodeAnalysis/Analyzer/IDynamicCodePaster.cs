using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualBasicCodeAnalysis.Analyzer
{
     interface IDynamicCodePaster
    {
        /*
         код логгера для вставки в VB проект
         Imports Serilog

Public Class Logger
    Dim Shared logger As Serilog.ILogger = new LoggerConfiguration().WriteTo.RollingFile("D:\\Atlaslog.txt").CreateLogger()
     Dim Shared loggerLin As Serilog.ILogger = new LoggerConfiguration().WriteTo.RollingFile("D:\\AtlaslogLinSection.txt").CreateLogger()
   ' Dim Shared logger1 As Serilog.ILogger = new LoggerConfiguration().WriteTo.
Public Shared Sub Log(message As String)
        ' Dim logger As Serilog.ILogger = new LoggerConfiguration().WriteTo.RollingFile("D:\\logCalc.txt").CreateLogger()
        logger.Information("Функция {message} отработала",message)
End Sub
    Public Shared Sub LogLin(message As String)
        ' Dim logger As Serilog.ILogger = new LoggerConfiguration().WriteTo.RollingFile("D:\\logCalc.txt").CreateLogger()
        loggerLin.Information("Линейный участок {message} отработал",message)
End Sub
End Class
             */
        void PasteCode();

    }
}
