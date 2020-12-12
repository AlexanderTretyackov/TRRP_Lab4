
namespace Client
{
    partial class MainForm
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
            this.gbInput = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btSend = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.info = new System.Windows.Forms.GroupBox();
            this.lbLoading = new System.Windows.Forms.Label();
            this.gbInput.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.info.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInput
            // 
            this.gbInput.Controls.Add(this.info);
            this.gbInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbInput.Location = new System.Drawing.Point(0, 0);
            this.gbInput.Name = "gbInput";
            this.gbInput.Size = new System.Drawing.Size(423, 88);
            this.gbInput.TabIndex = 3;
            this.gbInput.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(9, 40);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(333, 40);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(75, 20);
            this.btSend.TabIndex = 3;
            this.btSend.Text = "Отправить";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // output
            // 
            this.output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.output.Location = new System.Drawing.Point(3, 16);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.Size = new System.Drawing.Size(417, 139);
            this.output.TabIndex = 4;
            this.output.Text = "";
            this.output.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbLoading);
            this.groupBox1.Controls.Add(this.output);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 158);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // info
            // 
            this.info.Controls.Add(this.btSend);
            this.info.Controls.Add(this.btnCancel);
            this.info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.info.Location = new System.Drawing.Point(3, 16);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(417, 69);
            this.info.TabIndex = 5;
            this.info.TabStop = false;
            this.info.Visible = false;
            // 
            // lbLoading
            // 
            this.lbLoading.AutoSize = true;
            this.lbLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbLoading.Location = new System.Drawing.Point(112, 3);
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new System.Drawing.Size(209, 42);
            this.lbLoading.TabIndex = 5;
            this.lbLoading.Text = "ЗАГРУЗКА";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 246);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbInput);
            this.Name = "MainForm";
            this.Text = "Вычисление точного вершинного покрытия";
            this.gbInput.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.info.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbInput;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox info;
        private System.Windows.Forms.Label lbLoading;
    }
}