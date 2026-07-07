
namespace LMS_Final
{
    partial class BorrowBook
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
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Borrow = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.DueDateTXTBX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.BorrowDateTXTBX = new System.Windows.Forms.TextBox();
            this.Year2 = new System.Windows.Forms.Label();
            this.YearTXTBX = new System.Windows.Forms.TextBox();
            this.Genre2 = new System.Windows.Forms.Label();
            this.GenreTXTBX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AuthorTXTBX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TitleTXBX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectCMBBX = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(672, 38);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(355, 464);
            this.pictureBox3.TabIndex = 51;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::LMS_Final.Properties.Resources.back_icon;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(7, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 56);
            this.pictureBox1.TabIndex = 50;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Borrow
            // 
            this.Borrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.Borrow.ForeColor = System.Drawing.Color.White;
            this.Borrow.Location = new System.Drawing.Point(430, 483);
            this.Borrow.Name = "Borrow";
            this.Borrow.Size = new System.Drawing.Size(136, 39);
            this.Borrow.TabIndex = 49;
            this.Borrow.Text = "Borrow";
            this.Borrow.UseVisualStyleBackColor = false;
            this.Borrow.Click += new System.EventHandler(this.Borrow_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(343, 391);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(97, 20);
            this.label6.TabIndex = 48;
            this.label6.Text = "Due Date";
            // 
            // DueDateTXTBX
            // 
            this.DueDateTXTBX.Location = new System.Drawing.Point(343, 421);
            this.DueDateTXTBX.Name = "DueDateTXTBX";
            this.DueDateTXTBX.Size = new System.Drawing.Size(209, 20);
            this.DueDateTXTBX.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(76, 391);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(163, 20);
            this.label7.TabIndex = 46;
            this.label7.Text = "Borrowing Date";
            // 
            // BorrowDateTXTBX
            // 
            this.BorrowDateTXTBX.Location = new System.Drawing.Point(77, 421);
            this.BorrowDateTXTBX.Name = "BorrowDateTXTBX";
            this.BorrowDateTXTBX.Size = new System.Drawing.Size(209, 20);
            this.BorrowDateTXTBX.TabIndex = 45;
            // 
            // Year2
            // 
            this.Year2.AutoSize = true;
            this.Year2.BackColor = System.Drawing.Color.Transparent;
            this.Year2.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.Year2.Location = new System.Drawing.Point(343, 304);
            this.Year2.Name = "Year2";
            this.Year2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Year2.Size = new System.Drawing.Size(174, 20);
            this.Year2.TabIndex = 44;
            this.Year2.Text = "Publishing Year";
            // 
            // YearTXTBX
            // 
            this.YearTXTBX.Location = new System.Drawing.Point(343, 333);
            this.YearTXTBX.Name = "YearTXTBX";
            this.YearTXTBX.Size = new System.Drawing.Size(209, 20);
            this.YearTXTBX.TabIndex = 43;
            // 
            // Genre2
            // 
            this.Genre2.AutoSize = true;
            this.Genre2.BackColor = System.Drawing.Color.Transparent;
            this.Genre2.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.Genre2.Location = new System.Drawing.Point(76, 304);
            this.Genre2.Name = "Genre2";
            this.Genre2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Genre2.Size = new System.Drawing.Size(64, 20);
            this.Genre2.TabIndex = 42;
            this.Genre2.Text = "Genre";
            // 
            // GenreTXTBX
            // 
            this.GenreTXTBX.Location = new System.Drawing.Point(77, 333);
            this.GenreTXTBX.Name = "GenreTXTBX";
            this.GenreTXTBX.Size = new System.Drawing.Size(209, 20);
            this.GenreTXTBX.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(343, 230);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 40;
            this.label3.Text = "Author";
            // 
            // AuthorTXTBX
            // 
            this.AuthorTXTBX.Location = new System.Drawing.Point(343, 258);
            this.AuthorTXTBX.Name = "AuthorTXTBX";
            this.AuthorTXTBX.Size = new System.Drawing.Size(207, 20);
            this.AuthorTXTBX.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(76, 230);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 38;
            this.label2.Text = "Title";
            // 
            // TitleTXBX
            // 
            this.TitleTXBX.Location = new System.Drawing.Point(77, 258);
            this.TitleTXBX.Name = "TitleTXBX";
            this.TitleTXBX.Size = new System.Drawing.Size(207, 20);
            this.TitleTXBX.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MS Gothic", 15F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(72, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Select Student:";
            // 
            // SelectCMBBX
            // 
            this.SelectCMBBX.FormattingEnabled = true;
            this.SelectCMBBX.Location = new System.Drawing.Point(76, 179);
            this.SelectCMBBX.Name = "SelectCMBBX";
            this.SelectCMBBX.Size = new System.Drawing.Size(474, 21);
            this.SelectCMBBX.TabIndex = 52;
            this.SelectCMBBX.SelectedIndexChanged += new System.EventHandler(this.SelectCMBBX_SelectedIndexChanged);
            // 
            // BorrowBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LMS_Final.Properties.Resources.borrowbookkkk;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 581);
            this.Controls.Add(this.SelectCMBBX);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Borrow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DueDateTXTBX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BorrowDateTXTBX);
            this.Controls.Add(this.Year2);
            this.Controls.Add(this.YearTXTBX);
            this.Controls.Add(this.Genre2);
            this.Controls.Add(this.GenreTXTBX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AuthorTXTBX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TitleTXBX);
            this.Controls.Add(this.label1);
            this.Name = "BorrowBook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Borrow Book";
            this.Load += new System.EventHandler(this.BorrowBook_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Borrow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox DueDateTXTBX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox BorrowDateTXTBX;
        private System.Windows.Forms.Label Year2;
        private System.Windows.Forms.TextBox YearTXTBX;
        private System.Windows.Forms.Label Genre2;
        private System.Windows.Forms.TextBox GenreTXTBX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AuthorTXTBX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TitleTXBX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SelectCMBBX;
    }
}