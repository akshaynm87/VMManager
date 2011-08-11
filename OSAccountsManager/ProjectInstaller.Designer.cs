namespace OSAccountsManager
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.ServiceProcess.ServiceProcessInstaller OSAccManagerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller OSAccManagerServiceInstaller;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OSAccManagerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.OSAccManagerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // OSAccManagerServiceProcessInstaller
            // 
            this.OSAccManagerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.OSAccManagerServiceProcessInstaller.Password = null;
            this.OSAccManagerServiceProcessInstaller.Username = null;
            //            this.VMManagerServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VMManagerServiceProcessInstaller_AfterInstall);
            // 
            // FamServiceInstaller
            // 
            this.OSAccManagerServiceInstaller.DisplayName = "DOFS-Sandbox VM Manager";
            this.OSAccManagerServiceInstaller.ServiceName = "VMManagerService";
            //            this.VMManagerServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.OSAccManagerServiceProcessInstaller,
            this.OSAccManagerServiceInstaller});
        }

        #endregion
    }
}