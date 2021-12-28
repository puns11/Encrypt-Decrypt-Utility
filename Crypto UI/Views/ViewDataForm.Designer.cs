
namespace Crypto_UI.Views
{
    partial class ViewDataForm
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
            this.components = new System.ComponentModel.Container();
            this.viewDataGridView = new System.Windows.Forms.DataGridView();
            this.dataViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.viewDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // viewDataGridView
            // 
            this.viewDataGridView.AllowUserToAddRows = false;
            this.viewDataGridView.AllowUserToDeleteRows = false;
            this.viewDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewDataGridView.Location = new System.Drawing.Point(0, 0);
            this.viewDataGridView.Name = "viewDataGridView";
            this.viewDataGridView.ReadOnly = true;
            this.viewDataGridView.RowTemplate.Height = 25;
            this.viewDataGridView.Size = new System.Drawing.Size(428, 341);
            this.viewDataGridView.TabIndex = 0;
            // 
            // ViewDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(428, 341);
            this.Controls.Add(this.viewDataGridView);
            this.Name = "ViewDataForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview Data";
            this.Load += new System.EventHandler(this.ViewDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView viewDataGridView;
        private System.Windows.Forms.BindingSource dataViewBindingSource;
    }
}