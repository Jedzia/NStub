namespace NStub.Gui
{
    partial class GeneratorConfig
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.tbConfig = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chklbParameters = new System.Windows.Forms.CheckedListBox();
            this.lviewBuilderTypes = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 644);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propGrid);
            this.panel2.Controls.Add(this.tbConfig);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 196);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(772, 448);
            this.panel2.TabIndex = 3;
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(772, 263);
            this.propGrid.TabIndex = 2;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropGridPropertyValueChanged);
            // 
            // tbConfig
            // 
            this.tbConfig.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbConfig.Location = new System.Drawing.Point(0, 263);
            this.tbConfig.Multiline = true;
            this.tbConfig.Name = "tbConfig";
            this.tbConfig.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbConfig.Size = new System.Drawing.Size(772, 185);
            this.tbConfig.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chklbParameters);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 196);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // chklbParameters
            // 
            this.chklbParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chklbParameters.Location = new System.Drawing.Point(323, 16);
            this.chklbParameters.Name = "chklbParameters";
            this.chklbParameters.Size = new System.Drawing.Size(446, 169);
            this.chklbParameters.TabIndex = 0;
            this.chklbParameters.SelectedIndexChanged += new System.EventHandler(this.ChkLbParametersSelectedIndexChanged);
            this.chklbParameters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkLbParametersItemCheck);
            // 
            // lviewBuilderTypes
            // 
            this.lviewBuilderTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lviewBuilderTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lviewBuilderTypes.Location = new System.Drawing.Point(3, 16);
            this.lviewBuilderTypes.Name = "lviewBuilderTypes";
            this.lviewBuilderTypes.Size = new System.Drawing.Size(314, 103);
            this.lviewBuilderTypes.TabIndex = 1;
            this.lviewBuilderTypes.UseCompatibleStateImageBehavior = false;
            this.lviewBuilderTypes.View = System.Windows.Forms.View.Details;
            this.lviewBuilderTypes.SelectedIndexChanged += new System.EventHandler(this.ListViewBuilderTypesSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 163;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lviewBuilderTypes);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(3, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 177);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRemove);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 119);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(314, 55);
            this.panel3.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(256, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 34);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(195, 16);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(55, 34);
            this.btnRemove.TabIndex = 0;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // GeneratorConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 644);
            this.Controls.Add(this.panel1);
            this.Name = "GeneratorConfig";
            this.Text = "GeneratorConfig";
            this.Load += new System.EventHandler(this.GeneratorConfigLoad);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chklbParameters;
        private System.Windows.Forms.TextBox tbConfig;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lviewBuilderTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
    }
}