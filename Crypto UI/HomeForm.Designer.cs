
namespace Crypto_UI
{
    partial class HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.aesRadioBtn = new System.Windows.Forms.RadioButton();
            this.trippleDesRadioBtn = new System.Windows.Forms.RadioButton();
            this.sbFIleBtn = new System.Windows.Forms.Button();
            this.colIndexTxtBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.delimiterTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rowsSkipTxtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fileNameTxtBox = new System.Windows.Forms.TextBox();
            this.browseBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.selectFnOnFileCmBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTabPage = new System.Windows.Forms.TabPage();
            this.aesTxtRadioBtn = new System.Windows.Forms.RadioButton();
            this.trippleDesTxtRadioBtn = new System.Windows.Forms.RadioButton();
            this.sbmitOnTxtBtn = new System.Windows.Forms.Button();
            this.outputTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.enterTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectFnOnTxtCmBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.txtTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.txtTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(475, 226);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.aesRadioBtn);
            this.tabPage2.Controls.Add(this.trippleDesRadioBtn);
            this.tabPage2.Controls.Add(this.sbFIleBtn);
            this.tabPage2.Controls.Add(this.colIndexTxtBox);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.delimiterTxtBox);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.rowsSkipTxtBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.fileNameTxtBox);
            this.tabPage2.Controls.Add(this.browseBtn);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.selectFnOnFileCmBox);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(467, 198);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File Based";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // aesRadioBtn
            // 
            this.aesRadioBtn.AutoSize = true;
            this.aesRadioBtn.Location = new System.Drawing.Point(415, 10);
            this.aesRadioBtn.Name = "aesRadioBtn";
            this.aesRadioBtn.Size = new System.Drawing.Size(45, 19);
            this.aesRadioBtn.TabIndex = 21;
            this.aesRadioBtn.Text = "AES";
            this.aesRadioBtn.UseVisualStyleBackColor = true;
            // 
            // trippleDesRadioBtn
            // 
            this.trippleDesRadioBtn.AutoSize = true;
            this.trippleDesRadioBtn.Checked = true;
            this.trippleDesRadioBtn.Location = new System.Drawing.Point(329, 10);
            this.trippleDesRadioBtn.Name = "trippleDesRadioBtn";
            this.trippleDesRadioBtn.Size = new System.Drawing.Size(80, 19);
            this.trippleDesRadioBtn.TabIndex = 20;
            this.trippleDesRadioBtn.TabStop = true;
            this.trippleDesRadioBtn.Text = "TrippleDES";
            this.trippleDesRadioBtn.UseVisualStyleBackColor = true;
            // 
            // sbFIleBtn
            // 
            this.sbFIleBtn.Enabled = false;
            this.sbFIleBtn.Location = new System.Drawing.Point(101, 112);
            this.sbFIleBtn.Name = "sbFIleBtn";
            this.sbFIleBtn.Size = new System.Drawing.Size(75, 23);
            this.sbFIleBtn.TabIndex = 9;
            this.sbFIleBtn.Text = "Submit";
            this.sbFIleBtn.UseVisualStyleBackColor = true;
            this.sbFIleBtn.Click += new System.EventHandler(this.sbFIleBtn_Click);
            // 
            // colIndexTxtBox
            // 
            this.colIndexTxtBox.Enabled = false;
            this.colIndexTxtBox.Location = new System.Drawing.Point(356, 64);
            this.colIndexTxtBox.Name = "colIndexTxtBox";
            this.colIndexTxtBox.Size = new System.Drawing.Size(34, 23);
            this.colIndexTxtBox.TabIndex = 8;
            this.colIndexTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "Column Index";
            // 
            // delimiterTxtBox
            // 
            this.delimiterTxtBox.Enabled = false;
            this.delimiterTxtBox.Location = new System.Drawing.Point(214, 64);
            this.delimiterTxtBox.Name = "delimiterTxtBox";
            this.delimiterTxtBox.Size = new System.Drawing.Size(34, 23);
            this.delimiterTxtBox.TabIndex = 7;
            this.delimiterTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "Delimiter";
            // 
            // rowsSkipTxtBox
            // 
            this.rowsSkipTxtBox.Enabled = false;
            this.rowsSkipTxtBox.Location = new System.Drawing.Point(101, 64);
            this.rowsSkipTxtBox.Name = "rowsSkipTxtBox";
            this.rowsSkipTxtBox.Size = new System.Drawing.Size(34, 23);
            this.rowsSkipTxtBox.TabIndex = 6;
            this.rowsSkipTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Rows To Skip";
            // 
            // fileNameTxtBox
            // 
            this.fileNameTxtBox.Location = new System.Drawing.Point(101, 35);
            this.fileNameTxtBox.Name = "fileNameTxtBox";
            this.fileNameTxtBox.Size = new System.Drawing.Size(267, 23);
            this.fileNameTxtBox.TabIndex = 0;
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(374, 35);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(86, 23);
            this.browseBtn.TabIndex = 4;
            this.browseBtn.Text = "Browse";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Select File";
            // 
            // selectFnOnFileCmBox
            // 
            this.selectFnOnFileCmBox.FormattingEnabled = true;
            this.selectFnOnFileCmBox.Items.AddRange(new object[] {
            "Decrypt",
            "Encrypt"});
            this.selectFnOnFileCmBox.Location = new System.Drawing.Point(101, 6);
            this.selectFnOnFileCmBox.Name = "selectFnOnFileCmBox";
            this.selectFnOnFileCmBox.Size = new System.Drawing.Size(121, 23);
            this.selectFnOnFileCmBox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "Select Function";
            // 
            // txtTabPage
            // 
            this.txtTabPage.Controls.Add(this.aesTxtRadioBtn);
            this.txtTabPage.Controls.Add(this.trippleDesTxtRadioBtn);
            this.txtTabPage.Controls.Add(this.sbmitOnTxtBtn);
            this.txtTabPage.Controls.Add(this.outputTxtBox);
            this.txtTabPage.Controls.Add(this.label3);
            this.txtTabPage.Controls.Add(this.enterTxtBox);
            this.txtTabPage.Controls.Add(this.label2);
            this.txtTabPage.Controls.Add(this.selectFnOnTxtCmBox);
            this.txtTabPage.Controls.Add(this.label1);
            this.txtTabPage.Location = new System.Drawing.Point(4, 24);
            this.txtTabPage.Name = "txtTabPage";
            this.txtTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.txtTabPage.Size = new System.Drawing.Size(467, 198);
            this.txtTabPage.TabIndex = 0;
            this.txtTabPage.Text = "Text Based";
            this.txtTabPage.UseVisualStyleBackColor = true;
            // 
            // aesTxtRadioBtn
            // 
            this.aesTxtRadioBtn.AutoSize = true;
            this.aesTxtRadioBtn.Location = new System.Drawing.Point(415, 10);
            this.aesTxtRadioBtn.Name = "aesTxtRadioBtn";
            this.aesTxtRadioBtn.Size = new System.Drawing.Size(45, 19);
            this.aesTxtRadioBtn.TabIndex = 23;
            this.aesTxtRadioBtn.Text = "AES";
            this.aesTxtRadioBtn.UseVisualStyleBackColor = true;
            // 
            // trippleDesTxtRadioBtn
            // 
            this.trippleDesTxtRadioBtn.AutoSize = true;
            this.trippleDesTxtRadioBtn.Checked = true;
            this.trippleDesTxtRadioBtn.Location = new System.Drawing.Point(329, 10);
            this.trippleDesTxtRadioBtn.Name = "trippleDesTxtRadioBtn";
            this.trippleDesTxtRadioBtn.Size = new System.Drawing.Size(80, 19);
            this.trippleDesTxtRadioBtn.TabIndex = 22;
            this.trippleDesTxtRadioBtn.TabStop = true;
            this.trippleDesTxtRadioBtn.Text = "TrippleDES";
            this.trippleDesTxtRadioBtn.UseVisualStyleBackColor = true;
            // 
            // sbmitOnTxtBtn
            // 
            this.sbmitOnTxtBtn.Location = new System.Drawing.Point(101, 64);
            this.sbmitOnTxtBtn.Name = "sbmitOnTxtBtn";
            this.sbmitOnTxtBtn.Size = new System.Drawing.Size(86, 23);
            this.sbmitOnTxtBtn.TabIndex = 3;
            this.sbmitOnTxtBtn.Text = "Submit";
            this.sbmitOnTxtBtn.UseVisualStyleBackColor = true;
            this.sbmitOnTxtBtn.Click += new System.EventHandler(this.sbmitOnTxtBtn_Click);
            // 
            // outputTxtBox
            // 
            this.outputTxtBox.Location = new System.Drawing.Point(101, 129);
            this.outputTxtBox.Name = "outputTxtBox";
            this.outputTxtBox.ReadOnly = true;
            this.outputTxtBox.Size = new System.Drawing.Size(359, 23);
            this.outputTxtBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output Text";
            // 
            // enterTxtBox
            // 
            this.enterTxtBox.Location = new System.Drawing.Point(101, 35);
            this.enterTxtBox.Name = "enterTxtBox";
            this.enterTxtBox.Size = new System.Drawing.Size(359, 23);
            this.enterTxtBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter Text";
            // 
            // selectFnOnTxtCmBox
            // 
            this.selectFnOnTxtCmBox.FormattingEnabled = true;
            this.selectFnOnTxtCmBox.Items.AddRange(new object[] {
            "Decrypt",
            "Encrypt"});
            this.selectFnOnTxtCmBox.Location = new System.Drawing.Point(101, 6);
            this.selectFnOnTxtCmBox.Name = "selectFnOnTxtCmBox";
            this.selectFnOnTxtCmBox.Size = new System.Drawing.Size(121, 23);
            this.selectFnOnTxtCmBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Function";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 249);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crypto Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.txtTabPage.ResumeLayout(false);
            this.txtTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage txtTabPage;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button sbmitOnTxtBtn;
        private System.Windows.Forms.TextBox outputTxtBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox enterTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox selectFnOnTxtCmBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox selectFnOnFileCmBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox rowsSkipTxtBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fileNameTxtBox;
        private System.Windows.Forms.TextBox delimiterTxtBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox colIndexTxtBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button sbFIleBtn;
        private System.Windows.Forms.RadioButton aesRadioBtn;
        private System.Windows.Forms.RadioButton trippleDesRadioBtn;
        private System.Windows.Forms.RadioButton aesTxtRadioBtn;
        private System.Windows.Forms.RadioButton trippleDesTxtRadioBtn;
    }
}

