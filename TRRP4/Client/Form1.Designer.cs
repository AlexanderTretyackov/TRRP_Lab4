namespace Client
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDb = new System.Windows.Forms.OpenFileDialog();
            this.StartNormalize = new System.Windows.Forms.Button();
            this.LbAnswer = new System.Windows.Forms.Label();
            this.BtCancel = new System.Windows.Forms.Button();
            this.TbNumber = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.TbNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите номер числа:";
            // 
            // openFileDb
            // 
            this.openFileDb.FileName = "D:\\4 курс\\ТРРП\\Входной\\sales.db";
            // 
            // StartNormalize
            // 
            this.StartNormalize.Location = new System.Drawing.Point(12, 58);
            this.StartNormalize.Name = "StartNormalize";
            this.StartNormalize.Size = new System.Drawing.Size(263, 29);
            this.StartNormalize.TabIndex = 4;
            this.StartNormalize.Text = "Узнать!";
            this.StartNormalize.UseVisualStyleBackColor = true;
            this.StartNormalize.Click += new System.EventHandler(this.StartNormalize_Click);
            // 
            // LbAnswer
            // 
            this.LbAnswer.AutoSize = true;
            this.LbAnswer.Location = new System.Drawing.Point(12, 125);
            this.LbAnswer.MaximumSize = new System.Drawing.Size(263, 0);
            this.LbAnswer.Name = "LbAnswer";
            this.LbAnswer.Size = new System.Drawing.Size(0, 17);
            this.LbAnswer.TabIndex = 5;
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(12, 93);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(263, 29);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "Отменить";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Visible = false;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // TbNumber
            // 
            this.TbNumber.Location = new System.Drawing.Point(15, 29);
            this.TbNumber.Maximum = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            this.TbNumber.Name = "TbNumber";
            this.TbNumber.Size = new System.Drawing.Size(152, 22);
            this.TbNumber.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(287, 355);
            this.Controls.Add(this.TbNumber);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.LbAnswer);
            this.Controls.Add(this.StartNormalize);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Число фибоначи";
            ((System.ComponentModel.ISupportInitialize)(this.TbNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDb;
        private System.Windows.Forms.Button StartNormalize;
        private System.Windows.Forms.Label LbAnswer;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.NumericUpDown TbNumber;
    }
}

