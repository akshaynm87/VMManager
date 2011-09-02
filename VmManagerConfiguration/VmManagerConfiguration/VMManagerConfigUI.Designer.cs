namespace VmManagerConfiguration
{
    partial class VMManagerConfigUI
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
            this.labelVMList = new System.Windows.Forms.Label();
            this.listBoxVMList = new System.Windows.Forms.ListBox();
            this.groupBoxDetail = new System.Windows.Forms.GroupBox();
            this.buttonCreateUser = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.buttonRestoreVM = new System.Windows.Forms.Button();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxTimestamp = new System.Windows.Forms.TextBox();
            this.labelTimestamp = new System.Windows.Forms.Label();
            this.labelSetActive = new System.Windows.Forms.Label();
            this.labelActive = new System.Windows.Forms.Label();
            this.textBoxVMSnapshotName = new System.Windows.Forms.TextBox();
            this.buttonVMFile = new System.Windows.Forms.Button();
            this.labelVMSnapshotName = new System.Windows.Forms.Label();
            this.labelVMFile = new System.Windows.Forms.Label();
            this.textBoxVMFile = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAddEntry = new System.Windows.Forms.Button();
            this.buttonDeleteEntry = new System.Windows.Forms.Button();
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.labelInterval = new System.Windows.Forms.Label();
            this.textBoxValid = new System.Windows.Forms.TextBox();
            this.labelValid = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.groupBoxDetail.SuspendLayout();
            this.groupBoxGeneralSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelVMList
            // 
            this.labelVMList.AutoSize = true;
            this.labelVMList.Location = new System.Drawing.Point(40, 39);
            this.labelVMList.Name = "labelVMList";
            this.labelVMList.Size = new System.Drawing.Size(55, 13);
            this.labelVMList.TabIndex = 0;
            this.labelVMList.Text = "VM IP List";
            // 
            // listBoxVMList
            // 
            this.listBoxVMList.FormattingEnabled = true;
            this.listBoxVMList.Location = new System.Drawing.Point(43, 66);
            this.listBoxVMList.Name = "listBoxVMList";
            this.listBoxVMList.Size = new System.Drawing.Size(157, 225);
            this.listBoxVMList.TabIndex = 1;
            this.listBoxVMList.SelectedIndexChanged += new System.EventHandler(this.listBoxVMList_SelectedIndexChanged);
            // 
            // groupBoxDetail
            // 
            this.groupBoxDetail.Controls.Add(this.textBoxIP);
            this.groupBoxDetail.Controls.Add(this.labelIP);
            this.groupBoxDetail.Controls.Add(this.buttonCreateUser);
            this.groupBoxDetail.Controls.Add(this.textBoxPassword);
            this.groupBoxDetail.Controls.Add(this.textBoxUsername);
            this.groupBoxDetail.Controls.Add(this.labelPassword);
            this.groupBoxDetail.Controls.Add(this.buttonRestoreVM);
            this.groupBoxDetail.Controls.Add(this.labelUsername);
            this.groupBoxDetail.Controls.Add(this.textBoxTimestamp);
            this.groupBoxDetail.Controls.Add(this.labelTimestamp);
            this.groupBoxDetail.Controls.Add(this.labelSetActive);
            this.groupBoxDetail.Controls.Add(this.labelActive);
            this.groupBoxDetail.Controls.Add(this.textBoxVMSnapshotName);
            this.groupBoxDetail.Controls.Add(this.buttonVMFile);
            this.groupBoxDetail.Controls.Add(this.labelVMSnapshotName);
            this.groupBoxDetail.Controls.Add(this.labelVMFile);
            this.groupBoxDetail.Controls.Add(this.textBoxVMFile);
            this.groupBoxDetail.Enabled = false;
            this.groupBoxDetail.Location = new System.Drawing.Point(262, 66);
            this.groupBoxDetail.Name = "groupBoxDetail";
            this.groupBoxDetail.Size = new System.Drawing.Size(575, 270);
            this.groupBoxDetail.TabIndex = 2;
            this.groupBoxDetail.TabStop = false;
            this.groupBoxDetail.Text = "Detail";
            // 
            // buttonCreateUser
            // 
            this.buttonCreateUser.Location = new System.Drawing.Point(487, 183);
            this.buttonCreateUser.Name = "buttonCreateUser";
            this.buttonCreateUser.Size = new System.Drawing.Size(75, 23);
            this.buttonCreateUser.TabIndex = 13;
            this.buttonCreateUser.Text = "Create User";
            this.buttonCreateUser.UseVisualStyleBackColor = true;
            this.buttonCreateUser.Click += new System.EventHandler(this.buttonCreateUser_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(160, 235);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.ReadOnly = true;
            this.textBoxPassword.Size = new System.Drawing.Size(202, 20);
            this.textBoxPassword.TabIndex = 12;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(160, 211);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.ReadOnly = true;
            this.textBoxUsername.Size = new System.Drawing.Size(202, 20);
            this.textBoxUsername.TabIndex = 11;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(12, 240);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(71, 16);
            this.labelPassword.TabIndex = 10;
            this.labelPassword.Text = "Password:";
            // 
            // buttonRestoreVM
            // 
            this.buttonRestoreVM.Location = new System.Drawing.Point(487, 232);
            this.buttonRestoreVM.Name = "buttonRestoreVM";
            this.buttonRestoreVM.Size = new System.Drawing.Size(75, 23);
            this.buttonRestoreVM.TabIndex = 9;
            this.buttonRestoreVM.Text = "Restore VM";
            this.buttonRestoreVM.UseVisualStyleBackColor = true;
            this.buttonRestoreVM.Click += new System.EventHandler(this.buttonRestoreVM_Click);
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.Location = new System.Drawing.Point(12, 211);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(74, 16);
            this.labelUsername.TabIndex = 9;
            this.labelUsername.Text = "Username:";
            // 
            // textBoxTimestamp
            // 
            this.textBoxTimestamp.Location = new System.Drawing.Point(160, 171);
            this.textBoxTimestamp.Name = "textBoxTimestamp";
            this.textBoxTimestamp.ReadOnly = true;
            this.textBoxTimestamp.Size = new System.Drawing.Size(202, 20);
            this.textBoxTimestamp.TabIndex = 8;
            // 
            // labelTimestamp
            // 
            this.labelTimestamp.AutoSize = true;
            this.labelTimestamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimestamp.Location = new System.Drawing.Point(9, 171);
            this.labelTimestamp.Name = "labelTimestamp";
            this.labelTimestamp.Size = new System.Drawing.Size(76, 16);
            this.labelTimestamp.TabIndex = 7;
            this.labelTimestamp.Text = "Timestamp";
            // 
            // labelSetActive
            // 
            this.labelSetActive.AutoSize = true;
            this.labelSetActive.Location = new System.Drawing.Point(157, 32);
            this.labelSetActive.Name = "labelSetActive";
            this.labelSetActive.Size = new System.Drawing.Size(21, 13);
            this.labelSetActive.TabIndex = 6;
            this.labelSetActive.Text = "No";
            // 
            // labelActive
            // 
            this.labelActive.AutoSize = true;
            this.labelActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActive.Location = new System.Drawing.Point(9, 32);
            this.labelActive.Name = "labelActive";
            this.labelActive.Size = new System.Drawing.Size(48, 16);
            this.labelActive.TabIndex = 5;
            this.labelActive.Text = "Active:";
            // 
            // textBoxVMSnapshotName
            // 
            this.textBoxVMSnapshotName.Location = new System.Drawing.Point(160, 129);
            this.textBoxVMSnapshotName.Name = "textBoxVMSnapshotName";
            this.textBoxVMSnapshotName.Size = new System.Drawing.Size(202, 20);
            this.textBoxVMSnapshotName.TabIndex = 4;
            this.textBoxVMSnapshotName.TextChanged += new System.EventHandler(this.textBoxVMSnapshotName_TextChanged);
            // 
            // buttonVMFile
            // 
            this.buttonVMFile.Location = new System.Drawing.Point(487, 100);
            this.buttonVMFile.Name = "buttonVMFile";
            this.buttonVMFile.Size = new System.Drawing.Size(75, 23);
            this.buttonVMFile.TabIndex = 3;
            this.buttonVMFile.Text = "Select";
            this.buttonVMFile.UseVisualStyleBackColor = true;
            this.buttonVMFile.Click += new System.EventHandler(this.buttonVMFile_Click);
            // 
            // labelVMSnapshotName
            // 
            this.labelVMSnapshotName.AutoSize = true;
            this.labelVMSnapshotName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVMSnapshotName.Location = new System.Drawing.Point(9, 129);
            this.labelVMSnapshotName.Name = "labelVMSnapshotName";
            this.labelVMSnapshotName.Size = new System.Drawing.Size(131, 16);
            this.labelVMSnapshotName.TabIndex = 2;
            this.labelVMSnapshotName.Text = "VM Snapshot Name:";
            // 
            // labelVMFile
            // 
            this.labelVMFile.AutoSize = true;
            this.labelVMFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVMFile.Location = new System.Drawing.Point(9, 100);
            this.labelVMFile.Name = "labelVMFile";
            this.labelVMFile.Size = new System.Drawing.Size(53, 16);
            this.labelVMFile.TabIndex = 1;
            this.labelVMFile.Text = "VMFile:";
            // 
            // textBoxVMFile
            // 
            this.textBoxVMFile.Location = new System.Drawing.Point(160, 103);
            this.textBoxVMFile.Name = "textBoxVMFile";
            this.textBoxVMFile.Size = new System.Drawing.Size(307, 20);
            this.textBoxVMFile.TabIndex = 0;
            this.textBoxVMFile.TextChanged += new System.EventHandler(this.textBoxVMFile_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(654, 342);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(749, 342);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonAddEntry
            // 
            this.buttonAddEntry.Location = new System.Drawing.Point(43, 316);
            this.buttonAddEntry.Name = "buttonAddEntry";
            this.buttonAddEntry.Size = new System.Drawing.Size(75, 23);
            this.buttonAddEntry.TabIndex = 3;
            this.buttonAddEntry.Text = "Add Entry";
            this.buttonAddEntry.UseVisualStyleBackColor = true;
            this.buttonAddEntry.Click += new System.EventHandler(this.buttonAddEntry_Click);
            // 
            // buttonDeleteEntry
            // 
            this.buttonDeleteEntry.Location = new System.Drawing.Point(125, 316);
            this.buttonDeleteEntry.Name = "buttonDeleteEntry";
            this.buttonDeleteEntry.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteEntry.TabIndex = 12;
            this.buttonDeleteEntry.Text = "Delete";
            this.buttonDeleteEntry.UseVisualStyleBackColor = true;
            this.buttonDeleteEntry.Click += new System.EventHandler(this.buttonDeleteEntry_Click);
            // 
            // groupBoxGeneralSettings
            // 
            this.groupBoxGeneralSettings.Controls.Add(this.textBoxInterval);
            this.groupBoxGeneralSettings.Controls.Add(this.labelInterval);
            this.groupBoxGeneralSettings.Controls.Add(this.textBoxValid);
            this.groupBoxGeneralSettings.Controls.Add(this.labelValid);
            this.groupBoxGeneralSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxGeneralSettings.Location = new System.Drawing.Point(422, 13);
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.Size = new System.Drawing.Size(415, 47);
            this.groupBoxGeneralSettings.TabIndex = 13;
            this.groupBoxGeneralSettings.TabStop = false;
            this.groupBoxGeneralSettings.Text = "Settings";
            // 
            // textBoxInterval
            // 
            this.textBoxInterval.Location = new System.Drawing.Point(327, 21);
            this.textBoxInterval.Name = "textBoxInterval";
            this.textBoxInterval.Size = new System.Drawing.Size(75, 22);
            this.textBoxInterval.TabIndex = 3;
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(197, 23);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(124, 16);
            this.labelInterval.TabIndex = 2;
            this.labelInterval.Text = "Check Interval(min):";
            // 
            // textBoxValid
            // 
            this.textBoxValid.Location = new System.Drawing.Point(116, 21);
            this.textBoxValid.Name = "textBoxValid";
            this.textBoxValid.Size = new System.Drawing.Size(75, 22);
            this.textBoxValid.TabIndex = 1;
            // 
            // labelValid
            // 
            this.labelValid.AutoSize = true;
            this.labelValid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelValid.Location = new System.Drawing.Point(6, 24);
            this.labelValid.Name = "labelValid";
            this.labelValid.Size = new System.Drawing.Size(109, 16);
            this.labelValid.TabIndex = 0;
            this.labelValid.Text = "Validty time(min):";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIP.Location = new System.Drawing.Point(9, 75);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(77, 16);
            this.labelIP.TabIndex = 14;
            this.labelIP.Text = "IP Address:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(160, 77);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(307, 20);
            this.textBoxIP.TabIndex = 15;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // VMManagerConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 388);
            this.Controls.Add(this.groupBoxGeneralSettings);
            this.Controls.Add(this.buttonDeleteEntry);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAddEntry);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxDetail);
            this.Controls.Add(this.listBoxVMList);
            this.Controls.Add(this.labelVMList);
            this.Name = "VMManagerConfigUI";
            this.Text = "VMManager Config";
            this.groupBoxDetail.ResumeLayout(false);
            this.groupBoxDetail.PerformLayout();
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.groupBoxGeneralSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVMList;
        private System.Windows.Forms.ListBox listBoxVMList;
        private System.Windows.Forms.GroupBox groupBoxDetail;
        private System.Windows.Forms.Label labelVMSnapshotName;
        private System.Windows.Forms.Label labelVMFile;
        private System.Windows.Forms.TextBox textBoxVMFile;
        private System.Windows.Forms.TextBox textBoxVMSnapshotName;
        private System.Windows.Forms.Button buttonVMFile;
        private System.Windows.Forms.Label labelSetActive;
        private System.Windows.Forms.Label labelActive;
        private System.Windows.Forms.TextBox textBoxTimestamp;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Button buttonRestoreVM;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAddEntry;
        private System.Windows.Forms.Button buttonDeleteEntry;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        private System.Windows.Forms.TextBox textBoxValid;
        private System.Windows.Forms.Label labelValid;
        private System.Windows.Forms.TextBox textBoxInterval;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.Button buttonCreateUser;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelIP;
    }
}

