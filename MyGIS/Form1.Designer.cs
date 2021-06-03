
namespace MyGIS
{
    partial class MyGIS
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
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelAttribute = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxAttribute = new System.Windows.Forms.TextBox();
            this.buttonAddPointEntity = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(12, 9);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(16, 17);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(140, 9);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(15, 17);
            this.labelY.TabIndex = 1;
            this.labelY.Text = "Y";
            // 
            // labelAttribute
            // 
            this.labelAttribute.AutoSize = true;
            this.labelAttribute.Location = new System.Drawing.Point(267, 9);
            this.labelAttribute.Name = "labelAttribute";
            this.labelAttribute.Size = new System.Drawing.Size(32, 17);
            this.labelAttribute.TabIndex = 2;
            this.labelAttribute.Text = "属性";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(34, 6);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 23);
            this.textBoxX.TabIndex = 3;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(161, 6);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 23);
            this.textBoxY.TabIndex = 4;
            // 
            // textBoxAttribute
            // 
            this.textBoxAttribute.Location = new System.Drawing.Point(305, 6);
            this.textBoxAttribute.Name = "textBoxAttribute";
            this.textBoxAttribute.Size = new System.Drawing.Size(100, 23);
            this.textBoxAttribute.TabIndex = 5;
            // 
            // buttonAddPointEntity
            // 
            this.buttonAddPointEntity.Location = new System.Drawing.Point(411, 6);
            this.buttonAddPointEntity.Name = "buttonAddPointEntity";
            this.buttonAddPointEntity.Size = new System.Drawing.Size(82, 23);
            this.buttonAddPointEntity.TabIndex = 6;
            this.buttonAddPointEntity.Text = "添加点实体";
            this.buttonAddPointEntity.UseVisualStyleBackColor = true;
            // 
            // MyGIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 452);
            this.Controls.Add(this.buttonAddPointEntity);
            this.Controls.Add(this.textBoxAttribute);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.labelAttribute);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MyGIS";
            this.Text = "MyGIS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelAttribute;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxAttribute;
        private System.Windows.Forms.Button buttonAddPointEntity;
    }
}

