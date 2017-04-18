﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualBasicCodeAnalysis.Analyzer;

namespace VisualBasicCodeAnalysis
{
    public partial class MainWindow : Form
    {
        private string folderPath;
        private string filePath;
        FolderBrowserDialog fld = new FolderBrowserDialog();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {  
            fld.SelectedPath = folderPath;
            if (fld.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(fld.SelectedPath))
                {
                    folderPath = fld.SelectedPath;
                }
            }    
        }

       

        private void startAnalysisButton_Click(object sender, EventArgs e)
        {
            
            FullThirdLevelAnalyzer.StartSearching(filePath);
           // VisualBasicAnalysis vba = new VisualBasicAnalysis();
           //// folderPath = @"E:\abc.sln";
           ////var analisysResult = vba.GetProjectTest(filePath);
           //  //vba.TestWorkspaceChange(filePath);
           //  vba.GetFunctionList(filePath);
           // if (!string.IsNullOrEmpty(filePath))  //todo раскомментить после тестов
           //{
           //     DatabaseConnection dtb = new DatabaseConnection();
           //     dtb.NonExecuteQueryForInsertFuncToFunc(VisualBasicAnalysis.FuncToFuncLinkStructsList);
               
           // }
        }

        private void openFileBitton_Click(object sender, EventArgs e)
        {
            FileDialog fld = new OpenFileDialog();
            fld.FileName = filePath;
            fld.Filter = "Solution File .sln |*.sln";
            if (fld.ShowDialog() == DialogResult.OK)
            {
                filePath = fld.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           DatabaseConnection dtb = new DatabaseConnection();
           // dtb.CreateOrUpdateDatabase();
           dtb.CreateNewDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FuncUsingLogParser.ReadLogFile(@"D:\Atlaslog-20170406.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DynamicTest.UnPasteMarker(filePath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
           FullThirdLevelAnalyzer.PasteMarkersToProject();
        }
    }
}
