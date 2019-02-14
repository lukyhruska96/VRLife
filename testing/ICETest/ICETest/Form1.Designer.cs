namespace ICETest
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lEar_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rEar_label = new System.Windows.Forms.Label();
            this.cursor_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.azimLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 414);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "lEar: ";
            // 
            // lEar_label
            // 
            this.lEar_label.AutoSize = true;
            this.lEar_label.Location = new System.Drawing.Point(50, 430);
            this.lEar_label.Name = "lEar_label";
            this.lEar_label.Size = new System.Drawing.Size(22, 13);
            this.lEar_label.TabIndex = 2;
            this.lEar_label.Text = "0;0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 429);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "rEar:";
            // 
            // rEar_label
            // 
            this.rEar_label.AutoSize = true;
            this.rEar_label.Location = new System.Drawing.Point(126, 429);
            this.rEar_label.Name = "rEar_label";
            this.rEar_label.Size = new System.Drawing.Size(22, 13);
            this.rEar_label.TabIndex = 4;
            this.rEar_label.Text = "0;0";
            // 
            // cursor_label
            // 
            this.cursor_label.AutoSize = true;
            this.cursor_label.Location = new System.Drawing.Point(752, 430);
            this.cursor_label.Name = "cursor_label";
            this.cursor_label.Size = new System.Drawing.Size(22, 13);
            this.cursor_label.TabIndex = 5;
            this.cursor_label.Text = "0;0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 430);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "azim:";
            // 
            // azimLabel
            // 
            this.azimLabel.AutoSize = true;
            this.azimLabel.Location = new System.Drawing.Point(207, 429);
            this.azimLabel.Name = "azimLabel";
            this.azimLabel.Size = new System.Drawing.Size(13, 13);
            this.azimLabel.TabIndex = 7;
            this.azimLabel.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.azimLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cursor_label);
            this.Controls.Add(this.rEar_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lEar_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lEar_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label rEar_label;
        private System.Windows.Forms.Label cursor_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label azimLabel;
    }
}

