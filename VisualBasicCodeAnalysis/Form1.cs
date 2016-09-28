using System;
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
        private string folderPath = null;
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

        public void AddText(string[] texts)
        {
            foreach (var text in texts)
            {
                richTextBox1.Text += text + "\n";
            }            
        }

        private void startAnalysisButton_Click(object sender, EventArgs e)
        {

            VisualBasicAnalysis vba = new VisualBasicAnalysis();
           var labels = vba.GetProjectTest(folderPath);
           // if (!string.IsNullOrEmpty(folderPath))  todo раскомментить после тестов
           {
               foreach (var label in labels)
               {
                   richTextBox1.Text += "Name - " + label.Name + "\n";
                   richTextBox1.Text += "Text(Value) - " + label.Text + "\n";
               }
              
                //    var temp = vba.ChooseVBFile(folderPath);
                //    richTextBox1.Text += "Начинаем анализ";
                //    int count = 0;
                //    foreach (var filePath in temp)
                //    {
                //        var vbfiles = vba.ParseText(filePath);
                //        if ((vbfiles != null) && (vbfiles.Length > 0))
                //        {
                //            richTextBox1.Text += "Файл " + filePath + "\n";
                //            count += vbfiles.Length;
                //            AddText(vbfiles);
                //        }
                //    }
                //    richTextBox1.Text += "\n" + "Всего найдено файлов .vb - " + temp.Length + "\n";
                //    richTextBox1.Text += "\n" + "Всего найдено функций - " + count + "\n";
                // AddText(vba.ChooseVBFile(folderPath));
            }
        }
    }
}
