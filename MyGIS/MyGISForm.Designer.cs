
namespace MyGIS
{
    partial class MainForm
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
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveLeft = new System.Windows.Forms.Button();
            this.buttonMoveRight = new System.Windows.Forms.Button();
            this.buttonShowFullMap = new System.Windows.Forms.Button();
            this.buttonOpenAttribute = new System.Windows.Forms.Button();
            this.buttonSaveFile = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(659, 15);
            this.buttonZoomIn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(112, 42);
            this.buttonZoomIn.TabIndex = 16;
            this.buttonZoomIn.Text = "放大";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(781, 15);
            this.buttonZoomOut.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(112, 42);
            this.buttonZoomOut.TabIndex = 17;
            this.buttonZoomOut.Text = "缩小";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(907, 15);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(112, 42);
            this.buttonMoveUp.TabIndex = 18;
            this.buttonMoveUp.Text = "上移";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(1029, 15);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(112, 42);
            this.buttonMoveDown.TabIndex = 19;
            this.buttonMoveDown.Text = "下移";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveLeft
            // 
            this.buttonMoveLeft.Location = new System.Drawing.Point(1155, 15);
            this.buttonMoveLeft.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonMoveLeft.Name = "buttonMoveLeft";
            this.buttonMoveLeft.Size = new System.Drawing.Size(112, 42);
            this.buttonMoveLeft.TabIndex = 20;
            this.buttonMoveLeft.Text = "左移";
            this.buttonMoveLeft.UseVisualStyleBackColor = true;
            this.buttonMoveLeft.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonMoveRight
            // 
            this.buttonMoveRight.Location = new System.Drawing.Point(1279, 15);
            this.buttonMoveRight.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonMoveRight.Name = "buttonMoveRight";
            this.buttonMoveRight.Size = new System.Drawing.Size(112, 42);
            this.buttonMoveRight.TabIndex = 21;
            this.buttonMoveRight.Text = "右移";
            this.buttonMoveRight.UseVisualStyleBackColor = true;
            this.buttonMoveRight.Click += new System.EventHandler(this.ButtonMapAction_Click);
            // 
            // buttonShowFullMap
            // 
            this.buttonShowFullMap.Location = new System.Drawing.Point(507, 15);
            this.buttonShowFullMap.Margin = new System.Windows.Forms.Padding(4);
            this.buttonShowFullMap.Name = "buttonShowFullMap";
            this.buttonShowFullMap.Size = new System.Drawing.Size(142, 42);
            this.buttonShowFullMap.TabIndex = 23;
            this.buttonShowFullMap.Text = "显示全图";
            this.buttonShowFullMap.UseVisualStyleBackColor = true;
            this.buttonShowFullMap.Click += new System.EventHandler(this.ButtonShowFullMap_Click);
            // 
            // buttonOpenAttribute
            // 
            this.buttonOpenAttribute.Enabled = false;
            this.buttonOpenAttribute.Location = new System.Drawing.Point(339, 15);
            this.buttonOpenAttribute.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonOpenAttribute.Name = "buttonOpenAttribute";
            this.buttonOpenAttribute.Size = new System.Drawing.Size(158, 42);
            this.buttonOpenAttribute.TabIndex = 24;
            this.buttonOpenAttribute.Text = "打开属性表";
            this.buttonOpenAttribute.UseVisualStyleBackColor = true;
            this.buttonOpenAttribute.Click += new System.EventHandler(this.ButtonOpenAttribute_Click);
            // 
            // buttonSaveFile
            // 
            this.buttonSaveFile.Location = new System.Drawing.Point(177, 15);
            this.buttonSaveFile.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonSaveFile.Name = "buttonSaveFile";
            this.buttonSaveFile.Size = new System.Drawing.Size(150, 42);
            this.buttonSaveFile.TabIndex = 25;
            this.buttonSaveFile.Text = "存储文件";
            this.buttonSaveFile.UseVisualStyleBackColor = true;
            this.buttonSaveFile.Click += new System.EventHandler(this.ButtonSaveFile_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(15, 15);
            this.buttonOpenFile.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(150, 42);
            this.buttonOpenFile.TabIndex = 26;
            this.buttonOpenFile.Text = "打开文件";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.ButtonOpenFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1827, 1078);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.buttonSaveFile);
            this.Controls.Add(this.buttonOpenAttribute);
            this.Controls.Add(this.buttonShowFullMap);
            this.Controls.Add(this.buttonMoveRight);
            this.Controls.Add(this.buttonMoveLeft);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "MainForm";
            this.Text = "MyGIS";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Main_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveLeft;
        private System.Windows.Forms.Button buttonMoveRight;
        private System.Windows.Forms.Button buttonShowFullMap;
        private System.Windows.Forms.Button buttonOpenAttribute;
        private System.Windows.Forms.Button buttonSaveFile;
        private System.Windows.Forms.Button buttonOpenFile;
    }
}

