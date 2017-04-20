namespace VisualBasicCodeAnalysis
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
            this.openFileBitton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.writeToDBButton = new System.Windows.Forms.Button();
            this.readLinSectionLogButton = new System.Windows.Forms.Button();
            this.pasteLinSectionMarker = new System.Windows.Forms.Button();
            this.deleteLinSectionMarker = new System.Windows.Forms.Button();
            this.funcFuncLinkButton = new System.Windows.Forms.Button();
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
            this.button1.Size = new System.Drawing.Size(128, 54);
            this.button1.TabIndex = 4;
            this.button1.Text = "Create DB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(722, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 55);
            this.button2.TabIndex = 5;
            this.button2.Text = "Read func log ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(453, 139);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(165, 50);
            this.button3.TabIndex = 6;
            this.button3.Text = "Delete func markers";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(453, 47);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(165, 54);
            this.button4.TabIndex = 7;
            this.button4.Text = "Paste func markers";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // writeToDBButton
            // 
            this.writeToDBButton.Location = new System.Drawing.Point(12, 394);
            this.writeToDBButton.Name = "writeToDBButton";
            this.writeToDBButton.Size = new System.Drawing.Size(128, 47);
            this.writeToDBButton.TabIndex = 8;
            this.writeToDBButton.Text = "Write to DB";
            this.writeToDBButton.UseVisualStyleBackColor = true;
            this.writeToDBButton.Click += new System.EventHandler(this.writeToDBButton_Click);
            // 
            // readLinSectionLogButton
            // 
            this.readLinSectionLogButton.Location = new System.Drawing.Point(722, 139);
            this.readLinSectionLogButton.Name = "readLinSectionLogButton";
            this.readLinSectionLogButton.Size = new System.Drawing.Size(159, 50);
            this.readLinSectionLogButton.TabIndex = 9;
            this.readLinSectionLogButton.Text = "Read linSection log";
            this.readLinSectionLogButton.UseVisualStyleBackColor = true;
            // 
            // pasteLinSectionMarker
            // 
            this.pasteLinSectionMarker.Location = new System.Drawing.Point(453, 253);
            this.pasteLinSectionMarker.Name = "pasteLinSectionMarker";
            this.pasteLinSectionMarker.Size = new System.Drawing.Size(165, 47);
            this.pasteLinSectionMarker.TabIndex = 10;
            this.pasteLinSectionMarker.Text = "Paste LinSectionMarker";
            this.pasteLinSectionMarker.UseVisualStyleBackColor = true;
            this.pasteLinSectionMarker.Click += new System.EventHandler(this.pasteLinSectionMarker_Click);
            // 
            // deleteLinSectionMarker
            // 
            this.deleteLinSectionMarker.Location = new System.Drawing.Point(453, 355);
            this.deleteLinSectionMarker.Name = "deleteLinSectionMarker";
            this.deleteLinSectionMarker.Size = new System.Drawing.Size(165, 54);
            this.deleteLinSectionMarker.TabIndex = 11;
            this.deleteLinSectionMarker.Text = "Delete LinSectionMarker";
            this.deleteLinSectionMarker.UseVisualStyleBackColor = true;
            this.deleteLinSectionMarker.Click += new System.EventHandler(this.deleteLinSectionMarker_Click);
            // 
            // funcFuncLinkButton
            // 
            this.funcFuncLinkButton.Location = new System.Drawing.Point(191, 47);
            this.funcFuncLinkButton.Name = "funcFuncLinkButton";
            this.funcFuncLinkButton.Size = new System.Drawing.Size(173, 54);
            this.funcFuncLinkButton.TabIndex = 12;
            this.funcFuncLinkButton.Text = "Create FuncFuncLink";
            this.funcFuncLinkButton.UseVisualStyleBackColor = true;
            this.funcFuncLinkButton.Click += new System.EventHandler(this.funcFuncLinkButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 503);
            this.Controls.Add(this.funcFuncLinkButton);
            this.Controls.Add(this.deleteLinSectionMarker);
            this.Controls.Add(this.pasteLinSectionMarker);
            this.Controls.Add(this.readLinSectionLogButton);
            this.Controls.Add(this.writeToDBButton);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.openFileBitton);
            this.Controls.Add(this.startAnalysisButton);
            this.Controls.Add(this.openFolderButton);
            this.Name = "MainWindow";
            this.Text = "VBCodeAnalysis";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button startAnalysisButton;
        private System.Windows.Forms.Button openFileBitton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button writeToDBButton;
        private System.Windows.Forms.Button readLinSectionLogButton;
        private System.Windows.Forms.Button pasteLinSectionMarker;
        private System.Windows.Forms.Button deleteLinSectionMarker;
        private System.Windows.Forms.Button funcFuncLinkButton;
    }
}

