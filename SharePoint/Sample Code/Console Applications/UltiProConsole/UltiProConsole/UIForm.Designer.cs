namespace UltiProConsole
{
    partial class UIForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sessionToken = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.apiUser = new System.Windows.Forms.Label();
            this.lblApiUser = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.inputQuery = new System.Windows.Forms.TextBox();
            this.results = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.chkTerminated = new System.Windows.Forms.CheckBox();
            this.cboFTPT = new System.Windows.Forms.ComboBox();
            this.lblQueryHelper = new System.Windows.Forms.Label();
            this.addressFindResponseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.addressFindResponseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.status);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.sessionToken);
            this.groupBox1.Controls.Add(this.lblConnectionStatus);
            this.groupBox1.Controls.Add(this.apiUser);
            this.groupBox1.Controls.Add(this.lblApiUser);
            this.groupBox1.Location = new System.Drawing.Point(15, 254);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Web Service Debugging Info";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.BackColor = System.Drawing.SystemColors.Control;
            this.status.Location = new System.Drawing.Point(49, 21);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(79, 13);
            this.status.TabIndex = 5;
            this.status.Text = "Not Connected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Status:";
            // 
            // sessionToken
            // 
            this.sessionToken.AutoSize = true;
            this.sessionToken.Location = new System.Drawing.Point(88, 57);
            this.sessionToken.Name = "sessionToken";
            this.sessionToken.Size = new System.Drawing.Size(152, 13);
            this.sessionToken.TabIndex = 3;
            this.sessionToken.Text = "ERROR: No Token Retrieved.";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(8, 57);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(81, 13);
            this.lblConnectionStatus.TabIndex = 2;
            this.lblConnectionStatus.Text = "Session Token:";
            // 
            // apiUser
            // 
            this.apiUser.AutoSize = true;
            this.apiUser.Location = new System.Drawing.Point(61, 39);
            this.apiUser.Name = "apiUser";
            this.apiUser.Size = new System.Drawing.Size(94, 13);
            this.apiUser.TabIndex = 1;
            this.apiUser.Text = "ERROR: No User.";
            // 
            // lblApiUser
            // 
            this.lblApiUser.AutoSize = true;
            this.lblApiUser.Location = new System.Drawing.Point(8, 39);
            this.lblApiUser.Name = "lblApiUser";
            this.lblApiUser.Size = new System.Drawing.Size(52, 13);
            this.lblApiUser.TabIndex = 0;
            this.lblApiUser.Text = "API User:";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(384, 275);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // inputQuery
            // 
            this.inputQuery.Location = new System.Drawing.Point(15, 40);
            this.inputQuery.Name = "inputQuery";
            this.inputQuery.Size = new System.Drawing.Size(444, 20);
            this.inputQuery.TabIndex = 2;
            this.inputQuery.TextChanged += new System.EventHandler(this.inputQuery_TextChanged);
            // 
            // results
            // 
            this.results.Location = new System.Drawing.Point(12, 101);
            this.results.Multiline = true;
            this.results.Name = "results";
            this.results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.results.Size = new System.Drawing.Size(685, 127);
            this.results.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Location = new System.Drawing.Point(15, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(698, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Enter critera below to retrieve employee address information.";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(486, 275);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(584, 275);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // chkTerminated
            // 
            this.chkTerminated.Location = new System.Drawing.Point(15, 65);
            this.chkTerminated.Name = "chkTerminated";
            this.chkTerminated.Size = new System.Drawing.Size(682, 35);
            this.chkTerminated.TabIndex = 9;
            this.chkTerminated.Text = "Select only inactive employees";
            this.chkTerminated.UseVisualStyleBackColor = true;
            // 
            // cboFTPT
            // 
            this.cboFTPT.FormattingEnabled = true;
            this.cboFTPT.Items.AddRange(new object[] {
            "-- Make a Selection --",
            "Full Time Employees",
            "Part Time Employees",
            "Both Full Time and Part Time Employees"});
            this.cboFTPT.Location = new System.Drawing.Point(476, 39);
            this.cboFTPT.Name = "cboFTPT";
            this.cboFTPT.Size = new System.Drawing.Size(221, 21);
            this.cboFTPT.TabIndex = 10;
            this.cboFTPT.Text = "-- Make a Selection --";
            // 
            // lblQueryHelper
            // 
            this.lblQueryHelper.BackColor = System.Drawing.SystemColors.Window;
            this.lblQueryHelper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueryHelper.ForeColor = System.Drawing.Color.DarkGray;
            this.lblQueryHelper.Location = new System.Drawing.Point(114, 44);
            this.lblQueryHelper.Name = "lblQueryHelper";
            this.lblQueryHelper.Size = new System.Drawing.Size(280, 14);
            this.lblQueryHelper.TabIndex = 11;
            this.lblQueryHelper.Text = "Enter an ID number, full OR partial last name.";
            // 
            // addressFindResponseBindingSource
            // 
            this.addressFindResponseBindingSource.DataSource = typeof(UltiProConsole.EmployeeAddressService.AddressFindResponse);
            // 
            // UIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 348);
            this.Controls.Add(this.lblQueryHelper);
            this.Controls.Add(this.results);
            this.Controls.Add(this.cboFTPT);
            this.Controls.Add(this.inputQuery);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkTerminated);
            this.Name = "UIForm";
            this.Text = "UltiPro Web Service Demonstration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.addressFindResponseBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblApiUser;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label apiUser;
        private System.Windows.Forms.Label sessionToken;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox inputQuery;
        private System.Windows.Forms.BindingSource addressFindResponseBindingSource;
        private System.Windows.Forms.TextBox results;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox chkTerminated;
        private System.Windows.Forms.ComboBox cboFTPT;
        private System.Windows.Forms.Label lblQueryHelper;
    }
}