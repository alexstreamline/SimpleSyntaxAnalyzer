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
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "DBTest";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 350);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "read log";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(238, 139);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(165, 50);
            this.button3.TabIndex = 6;
            this.button3.Text = "Delete markers";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(238, 47);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(165, 54);
            this.button4.TabIndex = 7;
            this.button4.Text = "Paste markers";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 503);
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
    }
}

