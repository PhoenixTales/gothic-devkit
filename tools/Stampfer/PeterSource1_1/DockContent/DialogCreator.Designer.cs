/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2009 Alexander "Sumpfkrautjunkie" Ruppert

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************************/
namespace Peter
{
    partial class DialogCreator
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
            this._grprInfo = new Peter.Grouper();
            this.pInfo = new System.Windows.Forms.Panel();
            this._grprCondition = new Peter.Grouper();
            this.pCondition = new System.Windows.Forms.Panel();
            this._grprOptions = new Peter.Grouper();
            this.BtStandardCreate = new System.Windows.Forms.Button();
            this.CbStandard = new System.Windows.Forms.ComboBox();
            this.BtReset = new System.Windows.Forms.Button();
            this.BtCreate = new System.Windows.Forms.Button();
            this.BtBack = new System.Windows.Forms.Button();
            this.BtChoicesDelete = new System.Windows.Forms.Button();
            this.BtChoicesEdit = new System.Windows.Forms.Button();
            this.BtChoicesAdd = new System.Windows.Forms.Button();
            this.LbChoices = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TbDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ckbPermanent = new System.Windows.Forms.CheckBox();
            this.ckbImportant = new System.Windows.Forms.CheckBox();
            this.ckbTrade = new System.Windows.Forms.CheckBox();
            this.nNr = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.BtNPC = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TbDialogName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._grprInfo.SuspendLayout();
            this._grprCondition.SuspendLayout();
            this._grprOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nNr)).BeginInit();
            this.SuspendLayout();
            // 
            // _grprInfo
            // 
            this._grprInfo.BackgroundColor = System.Drawing.Color.White;
            this._grprInfo.BackgroundGradientColor = System.Drawing.Color.White;
            this._grprInfo.BackgroundGradientMode = Peter.Grouper.GroupBoxGradientMode.None;
            this._grprInfo.BorderColor = System.Drawing.Color.Black;
            this._grprInfo.BorderThickness = 1F;
            this._grprInfo.Controls.Add(this.pInfo);
            this._grprInfo.CustomGroupBoxColor = System.Drawing.Color.White;
            this._grprInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grprInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grprInfo.GroupImage = null;
            this._grprInfo.GroupTitle = "Info";
            this._grprInfo.Location = new System.Drawing.Point(0, 324);
            this._grprInfo.Name = "_grprInfo";
            this._grprInfo.Padding = new System.Windows.Forms.Padding(20);
            this._grprInfo.PaintGroupBox = false;
            this._grprInfo.RoundCorners = 10;
            this._grprInfo.ShadowColor = System.Drawing.Color.DarkGray;
            this._grprInfo.ShadowControl = true;
            this._grprInfo.ShadowThickness = 3;
            this._grprInfo.Size = new System.Drawing.Size(737, 456);
            this._grprInfo.TabIndex = 9;
            // 
            // pInfo
            // 
            this.pInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pInfo.Location = new System.Drawing.Point(20, 20);
            this.pInfo.Name = "pInfo";
            this.pInfo.Size = new System.Drawing.Size(697, 416);
            this.pInfo.TabIndex = 0;
            // 
            // _grprCondition
            // 
            this._grprCondition.BackgroundColor = System.Drawing.Color.White;
            this._grprCondition.BackgroundGradientColor = System.Drawing.Color.White;
            this._grprCondition.BackgroundGradientMode = Peter.Grouper.GroupBoxGradientMode.None;
            this._grprCondition.BorderColor = System.Drawing.Color.Black;
            this._grprCondition.BorderThickness = 1F;
            this._grprCondition.Controls.Add(this.pCondition);
            this._grprCondition.CustomGroupBoxColor = System.Drawing.Color.White;
            this._grprCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this._grprCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grprCondition.GroupImage = null;
            this._grprCondition.GroupTitle = "Condition";
            this._grprCondition.Location = new System.Drawing.Point(0, 149);
            this._grprCondition.Name = "_grprCondition";
            this._grprCondition.Padding = new System.Windows.Forms.Padding(20);
            this._grprCondition.PaintGroupBox = false;
            this._grprCondition.RoundCorners = 10;
            this._grprCondition.ShadowColor = System.Drawing.Color.DarkGray;
            this._grprCondition.ShadowControl = true;
            this._grprCondition.ShadowThickness = 3;
            this._grprCondition.Size = new System.Drawing.Size(737, 175);
            this._grprCondition.TabIndex = 8;
            // 
            // pCondition
            // 
            this.pCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCondition.Location = new System.Drawing.Point(20, 20);
            this.pCondition.Name = "pCondition";
            this.pCondition.Size = new System.Drawing.Size(697, 135);
            this.pCondition.TabIndex = 0;
            // 
            // _grprOptions
            // 
            this._grprOptions.BackgroundColor = System.Drawing.Color.White;
            this._grprOptions.BackgroundGradientColor = System.Drawing.Color.White;
            this._grprOptions.BackgroundGradientMode = Peter.Grouper.GroupBoxGradientMode.None;
            this._grprOptions.BorderColor = System.Drawing.Color.Black;
            this._grprOptions.BorderThickness = 1F;
            this._grprOptions.Controls.Add(this.BtStandardCreate);
            this._grprOptions.Controls.Add(this.CbStandard);
            this._grprOptions.Controls.Add(this.BtReset);
            this._grprOptions.Controls.Add(this.BtCreate);
            this._grprOptions.Controls.Add(this.BtBack);
            this._grprOptions.Controls.Add(this.BtChoicesDelete);
            this._grprOptions.Controls.Add(this.BtChoicesEdit);
            this._grprOptions.Controls.Add(this.BtChoicesAdd);
            this._grprOptions.Controls.Add(this.LbChoices);
            this._grprOptions.Controls.Add(this.label5);
            this._grprOptions.Controls.Add(this.TbDescription);
            this._grprOptions.Controls.Add(this.label4);
            this._grprOptions.Controls.Add(this.ckbPermanent);
            this._grprOptions.Controls.Add(this.ckbImportant);
            this._grprOptions.Controls.Add(this.ckbTrade);
            this._grprOptions.Controls.Add(this.nNr);
            this._grprOptions.Controls.Add(this.label3);
            this._grprOptions.Controls.Add(this.BtNPC);
            this._grprOptions.Controls.Add(this.label2);
            this._grprOptions.Controls.Add(this.TbDialogName);
            this._grprOptions.Controls.Add(this.label1);
            this._grprOptions.CustomGroupBoxColor = System.Drawing.Color.White;
            this._grprOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this._grprOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grprOptions.GroupImage = null;
            this._grprOptions.GroupTitle = "Basisdaten";
            this._grprOptions.Location = new System.Drawing.Point(0, 0);
            this._grprOptions.Name = "_grprOptions";
            this._grprOptions.Padding = new System.Windows.Forms.Padding(20);
            this._grprOptions.PaintGroupBox = false;
            this._grprOptions.RoundCorners = 10;
            this._grprOptions.ShadowColor = System.Drawing.Color.DarkGray;
            this._grprOptions.ShadowControl = true;
            this._grprOptions.ShadowThickness = 3;
            this._grprOptions.Size = new System.Drawing.Size(737, 149);
            this._grprOptions.TabIndex = 6;
            // 
            // BtStandardCreate
            // 
            this.BtStandardCreate.Location = new System.Drawing.Point(275, 115);
            this.BtStandardCreate.Name = "BtStandardCreate";
            this.BtStandardCreate.Size = new System.Drawing.Size(43, 21);
            this.BtStandardCreate.TabIndex = 16;
            this.BtStandardCreate.Text = "OK";
            this.BtStandardCreate.UseVisualStyleBackColor = true;
            this.BtStandardCreate.Click += new System.EventHandler(this.BtStandardCreate_Click);
            // 
            // CbStandard
            // 
            this.CbStandard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbStandard.FormattingEnabled = true;
            this.CbStandard.Items.AddRange(new object[] {
            "ENDE",
            "Beklauen"});
            this.CbStandard.Location = new System.Drawing.Point(180, 115);
            this.CbStandard.Name = "CbStandard";
            this.CbStandard.Size = new System.Drawing.Size(89, 21);
            this.CbStandard.TabIndex = 15;
            // 
            // BtReset
            // 
            this.BtReset.Location = new System.Drawing.Point(547, 109);
            this.BtReset.Name = "BtReset";
            this.BtReset.Size = new System.Drawing.Size(117, 22);
            this.BtReset.TabIndex = 14;
            this.BtReset.Text = "Neu";
            this.BtReset.UseVisualStyleBackColor = true;
            this.BtReset.Click += new System.EventHandler(this.BtReset_Click);
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(547, 34);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(117, 52);
            this.BtCreate.TabIndex = 12;
            this.BtCreate.Text = "Generieren";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // BtBack
            // 
            this.BtBack.Location = new System.Drawing.Point(466, 109);
            this.BtBack.Name = "BtBack";
            this.BtBack.Size = new System.Drawing.Size(75, 23);
            this.BtBack.TabIndex = 11;
            this.BtBack.Text = "Zurück";
            this.BtBack.UseVisualStyleBackColor = true;
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtChoicesDelete
            // 
            this.BtChoicesDelete.Location = new System.Drawing.Point(466, 84);
            this.BtChoicesDelete.Name = "BtChoicesDelete";
            this.BtChoicesDelete.Size = new System.Drawing.Size(75, 23);
            this.BtChoicesDelete.TabIndex = 10;
            this.BtChoicesDelete.Text = "Löschen";
            this.BtChoicesDelete.UseVisualStyleBackColor = true;
            this.BtChoicesDelete.Click += new System.EventHandler(this.BtChoicesDelete_Click);
            // 
            // BtChoicesEdit
            // 
            this.BtChoicesEdit.Location = new System.Drawing.Point(466, 59);
            this.BtChoicesEdit.Name = "BtChoicesEdit";
            this.BtChoicesEdit.Size = new System.Drawing.Size(75, 23);
            this.BtChoicesEdit.TabIndex = 9;
            this.BtChoicesEdit.Text = "Bearbeiten";
            this.BtChoicesEdit.UseVisualStyleBackColor = true;
            this.BtChoicesEdit.Click += new System.EventHandler(this.BtChoicesEdit_Click);
            // 
            // BtChoicesAdd
            // 
            this.BtChoicesAdd.Location = new System.Drawing.Point(466, 34);
            this.BtChoicesAdd.Name = "BtChoicesAdd";
            this.BtChoicesAdd.Size = new System.Drawing.Size(75, 23);
            this.BtChoicesAdd.TabIndex = 8;
            this.BtChoicesAdd.Text = "Neu";
            this.BtChoicesAdd.UseVisualStyleBackColor = true;
            this.BtChoicesAdd.Click += new System.EventHandler(this.BtChoicesAdd_Click);
            // 
            // LbChoices
            // 
            this.LbChoices.FormattingEnabled = true;
            this.LbChoices.Location = new System.Drawing.Point(324, 36);
            this.LbChoices.Name = "LbChoices";
            this.LbChoices.Size = new System.Drawing.Size(136, 95);
            this.LbChoices.TabIndex = 13;
            this.LbChoices.SelectedIndexChanged += new System.EventHandler(this.LbChoices_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(321, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Choices";
            // 
            // TbDescription
            // 
            this.TbDescription.Location = new System.Drawing.Point(15, 75);
            this.TbDescription.Name = "TbDescription";
            this.TbDescription.Size = new System.Drawing.Size(220, 20);
            this.TbDescription.TabIndex = 1;
            this.TbDescription.TextChanged += new System.EventHandler(this.TbDescription_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            // 
            // ckbPermanent
            // 
            this.ckbPermanent.AutoSize = true;
            this.ckbPermanent.Location = new System.Drawing.Point(241, 62);
            this.ckbPermanent.Name = "ckbPermanent";
            this.ckbPermanent.Size = new System.Drawing.Size(77, 17);
            this.ckbPermanent.TabIndex = 7;
            this.ckbPermanent.Text = "Permanent";
            this.ckbPermanent.UseVisualStyleBackColor = true;
            // 
            // ckbImportant
            // 
            this.ckbImportant.AutoSize = true;
            this.ckbImportant.Location = new System.Drawing.Point(241, 78);
            this.ckbImportant.Name = "ckbImportant";
            this.ckbImportant.Size = new System.Drawing.Size(70, 17);
            this.ckbImportant.TabIndex = 6;
            this.ckbImportant.Text = "Important";
            this.ckbImportant.UseVisualStyleBackColor = true;
            // 
            // ckbTrade
            // 
            this.ckbTrade.AutoSize = true;
            this.ckbTrade.Location = new System.Drawing.Point(241, 94);
            this.ckbTrade.Name = "ckbTrade";
            this.ckbTrade.Size = new System.Drawing.Size(54, 17);
            this.ckbTrade.TabIndex = 5;
            this.ckbTrade.Text = "Trade";
            this.ckbTrade.UseVisualStyleBackColor = true;
            // 
            // nNr
            // 
            this.nNr.Location = new System.Drawing.Point(36, 36);
            this.nNr.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nNr.Name = "nNr";
            this.nNr.Size = new System.Drawing.Size(56, 20);
            this.nNr.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nr";
            // 
            // BtNPC
            // 
            this.BtNPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtNPC.Location = new System.Drawing.Point(15, 115);
            this.BtNPC.Name = "BtNPC";
            this.BtNPC.Size = new System.Drawing.Size(159, 21);
            this.BtNPC.TabIndex = 3;
            this.BtNPC.Text = "...";
            this.BtNPC.UseVisualStyleBackColor = true;
            this.BtNPC.Click += new System.EventHandler(this.BtNPC_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "NPC";
            // 
            // TbDialogName
            // 
            this.TbDialogName.Location = new System.Drawing.Point(98, 36);
            this.TbDialogName.Name = "TbDialogName";
            this.TbDialogName.Size = new System.Drawing.Size(220, 20);
            this.TbDialogName.TabIndex = 0;
            this.TbDialogName.TextChanged += new System.EventHandler(this.TbDialogName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dialogname";
            // 
            // DialogCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 780);
            this.Controls.Add(this._grprInfo);
            this.Controls.Add(this._grprCondition);
            this.Controls.Add(this._grprOptions);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DialogCreator";
            this.TabText = "FileDifference";
            this.Text = "Dateivergleich";
            this._grprInfo.ResumeLayout(false);
            this._grprCondition.ResumeLayout(false);
            this._grprOptions.ResumeLayout(false);
            this._grprOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nNr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        public Grouper _grprOptions;
        public System.Windows.Forms.TextBox TbDialogName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button BtNPC;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox TbDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckbPermanent;
        private System.Windows.Forms.CheckBox ckbImportant;
        private System.Windows.Forms.NumericUpDown nNr;
        private System.Windows.Forms.Label label3;
        public Grouper _grprCondition;
        public Grouper _grprInfo;
        public System.Windows.Forms.Panel pCondition;
        public System.Windows.Forms.Panel pInfo;
        private System.Windows.Forms.Button BtChoicesDelete;
        private System.Windows.Forms.Button BtChoicesEdit;
        private System.Windows.Forms.Button BtChoicesAdd;
        public System.Windows.Forms.ListBox LbChoices;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtBack;
        private System.Windows.Forms.Button BtCreate;
        private System.Windows.Forms.Button BtReset;
        private System.Windows.Forms.CheckBox ckbTrade;
        private System.Windows.Forms.ComboBox CbStandard;
        private System.Windows.Forms.Button BtStandardCreate;
    }
}