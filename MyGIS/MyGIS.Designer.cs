
namespace MyGIS
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddPointEntity = new System.Windows.Forms.Button();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxAttribute = new System.Windows.Forms.TextBox();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelAttribute = new System.Windows.Forms.Label();
            this.labelMinX = new System.Windows.Forms.Label();
            this.labelMinY = new System.Windows.Forms.Label();
            this.labelMaxX = new System.Windows.Forms.Label();
            this.labelMaxY = new System.Windows.Forms.Label();
            this.textBoxMinX = new System.Windows.Forms.TextBox();
            this.textBoxMinY = new System.Windows.Forms.TextBox();
            this.textBoxMaxX = new System.Windows.Forms.TextBox();
            this.textBoxMaxY = new System.Windows.Forms.TextBox();
            this.buttonUpdateMap = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveLeft = new System.Windows.Forms.Button();
            this.buttonMoveRight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddPointEntity
            // 
            this.buttonAddPointEntity.Location = new System.Drawing.Point(372, 6);
            this.buttonAddPointEntity.Name = "buttonAddPointEntity";
            this.buttonAddPointEntity.Size = new System.Drawing.Size(90, 23);
            this.buttonAddPointEntity.TabIndex = 0;
            this.buttonAddPointEntity.Text = "添加点实体";
            this.buttonAddPointEntity.UseVisualStyleBackColor = true;
            this.buttonAddPointEntity.Click += new System.EventHandler(this.ButtonAddPointEntity_Click);
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(32, 6);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(88, 23);
            this.textBoxX.TabIndex = 1;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(146, 6);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(88, 23);
            this.textBoxY.TabIndex = 2;
            // 
            // textBoxAttribute
            // 
            this.textBoxAttribute.Location = new System.Drawing.Point(278, 6);
            this.textBoxAttribute.Name = "textBoxAttribute";
            this.textBoxAttribute.Size = new System.Drawing.Size(88, 23);
            this.textBoxAttribute.TabIndex = 3;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(12, 9);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(16, 17);
            this.labelX.TabIndex = 4;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(126, 9);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(15, 17);
            this.labelY.TabIndex = 5;
            this.labelY.Text = "Y";
            // 
            // Attribute
            // 
            this.labelAttribute.AutoSize = true;
            this.labelAttribute.Location = new System.Drawing.Point(240, 9);
            this.labelAttribute.Name = "Attribute";
            this.labelAttribute.Size = new System.Drawing.Size(32, 17);
            this.labelAttribute.TabIndex = 6;
            this.labelAttribute.Text = "属性";
            // 
            // labelMinX
            // 
            this.labelMinX.AutoSize = true;
            this.labelMinX.Location = new System.Drawing.Point(12, 38);
            this.labelMinX.Name = "labelMinX";
            this.labelMinX.Size = new System.Drawing.Size(38, 17);
            this.labelMinX.TabIndex = 7;
            this.labelMinX.Text = "MinX";
            this.labelMinX.UseMnemonic = false;
            // 
            // labelMinY
            // 
            this.labelMinY.AutoSize = true;
            this.labelMinY.Location = new System.Drawing.Point(146, 38);
            this.labelMinY.Name = "labelMinY";
            this.labelMinY.Size = new System.Drawing.Size(37, 17);
            this.labelMinY.TabIndex = 8;
            this.labelMinY.Text = "MinY";
            // 
            // labelMaxX
            // 
            this.labelMaxX.AutoSize = true;
            this.labelMaxX.Location = new System.Drawing.Point(278, 38);
            this.labelMaxX.Name = "labelMaxX";
            this.labelMaxX.Size = new System.Drawing.Size(41, 17);
            this.labelMaxX.TabIndex = 9;
            this.labelMaxX.Text = "MaxX";
            // 
            // labelMaxY
            // 
            this.labelMaxY.AutoSize = true;
            this.labelMaxY.Location = new System.Drawing.Point(419, 38);
            this.labelMaxY.Name = "labelMaxY";
            this.labelMaxY.Size = new System.Drawing.Size(40, 17);
            this.labelMaxY.TabIndex = 10;
            this.labelMaxY.Text = "MaxY";
            // 
            // textBoxMinX
            // 
            this.textBoxMinX.Location = new System.Drawing.Point(52, 35);
            this.textBoxMinX.Name = "textBoxMinX";
            this.textBoxMinX.Size = new System.Drawing.Size(88, 23);
            this.textBoxMinX.TabIndex = 11;
            // 
            // textBoxMinY
            // 
            this.textBoxMinY.Location = new System.Drawing.Point(184, 35);
            this.textBoxMinY.Name = "textBoxMinY";
            this.textBoxMinY.Size = new System.Drawing.Size(88, 23);
            this.textBoxMinY.TabIndex = 12;
            // 
            // textBoxMaxX
            // 
            this.textBoxMaxX.Location = new System.Drawing.Point(325, 35);
            this.textBoxMaxX.Name = "textBoxMaxX";
            this.textBoxMaxX.Size = new System.Drawing.Size(88, 23);
            this.textBoxMaxX.TabIndex = 13;
            // 
            // textBoxMaxY
            // 
            this.textBoxMaxY.Location = new System.Drawing.Point(465, 35);
            this.textBoxMaxY.Name = "textBoxMaxY";
            this.textBoxMaxY.Size = new System.Drawing.Size(88, 23);
            this.textBoxMaxY.TabIndex = 14;
            // 
            // buttonUpdateMap
            // 
            this.buttonUpdateMap.Location = new System.Drawing.Point(559, 35);
            this.buttonUpdateMap.Name = "buttonUpdateMap";
            this.buttonUpdateMap.Size = new System.Drawing.Size(85, 23);
            this.buttonUpdateMap.TabIndex = 15;
            this.buttonUpdateMap.Text = "更新地图";
            this.buttonUpdateMap.UseVisualStyleBackColor = true;
            this.buttonUpdateMap.Click += new System.EventHandler(this.ButtonUpdateMap_Click);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(12, 64);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(56, 23);
            this.buttonZoomIn.TabIndex = 16;
            this.buttonZoomIn.Text = "放大";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(74, 64);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(56, 23);
            this.buttonZoomOut.TabIndex = 17;
            this.buttonZoomOut.Text = "缩小";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(136, 64);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(56, 23);
            this.buttonMoveUp.TabIndex = 18;
            this.buttonMoveUp.Text = "上移";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(198, 64);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(56, 23);
            this.buttonMoveDown.TabIndex = 19;
            this.buttonMoveDown.Text = "下移";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveLeft
            // 
            this.buttonMoveLeft.Location = new System.Drawing.Point(260, 64);
            this.buttonMoveLeft.Name = "buttonMoveLeft";
            this.buttonMoveLeft.Size = new System.Drawing.Size(56, 23);
            this.buttonMoveLeft.TabIndex = 20;
            this.buttonMoveLeft.Text = "左移";
            this.buttonMoveLeft.UseVisualStyleBackColor = true;
            this.buttonMoveLeft.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveRight
            // 
            this.buttonMoveRight.Location = new System.Drawing.Point(322, 64);
            this.buttonMoveRight.Name = "buttonMoveRight";
            this.buttonMoveRight.Size = new System.Drawing.Size(56, 23);
            this.buttonMoveRight.TabIndex = 21;
            this.buttonMoveRight.Text = "右移";
            this.buttonMoveRight.UseVisualStyleBackColor = true;
            this.buttonMoveRight.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 591);
            this.Controls.Add(this.buttonMoveRight);
            this.Controls.Add(this.buttonMoveLeft);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.Controls.Add(this.buttonUpdateMap);
            this.Controls.Add(this.textBoxMaxY);
            this.Controls.Add(this.textBoxMaxX);
            this.Controls.Add(this.textBoxMinY);
            this.Controls.Add(this.textBoxMinX);
            this.Controls.Add(this.labelMaxY);
            this.Controls.Add(this.labelMaxX);
            this.Controls.Add(this.labelMinY);
            this.Controls.Add(this.labelMinX);
            this.Controls.Add(this.labelAttribute);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.textBoxAttribute);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.buttonAddPointEntity);
            this.Name = "Main";
            this.Text = "MyGIS";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Main_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxAttribute;
        private System.Windows.Forms.TextBox textBoxMinX;
        private System.Windows.Forms.TextBox textBoxMinY;
        private System.Windows.Forms.TextBox textBoxMaxX;
        private System.Windows.Forms.TextBox textBoxMaxY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelAttribute;
        private System.Windows.Forms.Label labelMinX;
        private System.Windows.Forms.Label labelMinY;
        private System.Windows.Forms.Label labelMaxX;
        private System.Windows.Forms.Label labelMaxY;
        private System.Windows.Forms.Button buttonAddPointEntity;
        private System.Windows.Forms.Button buttonUpdateMap;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveLeft;
        private System.Windows.Forms.Button buttonMoveRight;
    }
}

