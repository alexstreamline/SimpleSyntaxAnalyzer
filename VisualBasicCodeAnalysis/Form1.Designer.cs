﻿namespace VisualBasicCodeAnalysis
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFolderButton = new System.Windows.Forms.Button();
            this.startAnalysisButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileBitton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(12, 47);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(128, 54);
            this.openFolderButton.TabIndex = 0;
            this.openFolderButton.Text = "Open Code\'s Folder";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // startAnalysisButton
            // 
            this.startAnalysisButton.Location = new System.Drawing.Point(12, 224);
            this.startAnalysisButton.Name = "startAnalysisButton";
            this.startAnalysisButton.Size = new System.Drawing.Size(128, 47);
            this.startAnalysisButton.TabIndex = 1;
            this.startAnalysisButton.Text = "Start Analysis";
            this.startAnalysisButton.UseVisualStyleBackColor = true;
            this.startAnalysisButton.Click += new System.EventHandler(this.startAnalysisButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(174, 47);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(949, 306);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // openFileBitton
            // 
            this.openFileBitton.Location = new System.Drawing.Point(12, 139);
            this.openFileBitton.Name = "openFileBitton";
            this.openFileBitton.Size = new System.Drawing.Size(128, 50);
            this.openFileBitton.TabIndex = 3;
            this.openFileBitton.Text = "Open Project File";
            this.openFileBitton.UseVisualStyleBackColor = true;
            this.openFileBitton.Click += new System.EventHandler(this.openFileBitton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 298);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "DBTest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 414);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.openFileBitton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.startAnalysisButton);
            this.Controls.Add(this.openFolderButton);
            this.Name = "MainWindow";
            this.Text = "VBCodeAnalysis";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button startAnalysisButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button openFileBitton;
        private System.Windows.Forms.Button button1;
    }
}

