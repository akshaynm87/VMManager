namespace VMManager
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
 
        private System.ServiceProcess.ServiceProcessInstaller VMManagerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller VMManagerServiceInstaller;
 
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
            this.VMManagerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.VMManagerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // FAMServiceProcessInstaller
            // 
            this.VMManagerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.VMManagerServiceProcessInstaller.Password = null;
            this.VMManagerServiceProcessInstaller.Username = null;
//            this.VMManagerServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VMManagerServiceProcessInstaller_AfterInstall);
            // 
            // FamServiceInstaller
            // 
            this.VMManagerServiceInstaller.DisplayName = "DOFS-Sandbox VM Manager";
            this.VMManagerServiceInstaller.ServiceName = "VMManagerService";
//            this.VMManagerServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.VMManagerServiceProcessInstaller,
            this.VMManagerServiceInstaller});

        }

        #endregion
    }
}