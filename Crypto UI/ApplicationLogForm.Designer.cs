
namespace Crypto_UI
{
    partial class ApplicationLogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.appLogPanel = new System.Windows.Forms.Panel();
            this.appLogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.appLogCloseBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.appLogPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // appLogPanel
            // 
            this.appLogPanel.Controls.Add(this.appLogRichTextBox);
            this.appLogPanel.Location = new System.Drawing.Point(12, 12);
            this.appLogPanel.Name = "appLogPanel";
            this.appLogPanel.Size = new System.Drawing.Size(730, 344);
            this.appLogPanel.TabIndex = 0;
            // 
            // appLogRichTextBox
            // 
            this.appLogRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appLogRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.appLogRichTextBox.Name = "appLogRichTextBox";
            this.appLogRichTextBox.ReadOnly = true;
            this.appLogRichTextBox.Size = new System.Drawing.Size(730, 344);
            this.appLogRichTextBox.TabIndex = 0;
            this.appLogRichTextBox.Text = "";
            this.appLogRichTextBox.WordWrap = false;
            // 
            // appLogCloseBtn
            // 
            this.appLogCloseBtn.Location = new System.Drawing.Point(667, 362);
            this.appLogCloseBtn.Name = "appLogCloseBtn";
            this.appLogCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.appLogCloseBtn.TabIndex = 1;
            this.appLogCloseBtn.Text = "Close";
            this.appLogCloseBtn.UseVisualStyleBackColor = true;
            this.appLogCloseBtn.Click += new System.EventHandler(this.appLogCloseBtn_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(586, 362);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(75, 23);
            this.clearBtn.TabIndex = 2;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // ApplicationLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 390);
            this.ControlBox = false;
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.appLogCloseBtn);
            this.Controls.Add(this.appLogPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplicationLogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Logs";
            this.Load += new System.EventHandler(this.ApplicationLogForm_Load);
            this.appLogPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel appLogPanel;
        private System.Windows.Forms.Button appLogCloseBtn;
        private System.Windows.Forms.RichTextBox appLogRichTextBox;
        private System.Windows.Forms.Button clearBtn;
    }
}