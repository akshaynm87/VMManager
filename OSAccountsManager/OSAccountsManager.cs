using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace OSAccountsManager
{
    public partial class OSAccountsManager : ServiceBase
    {
        Server server;
        public OSAccountsManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debug.WriteLine(" Starting Account Manager Service");
            server = new Server();
/*            UserAccountManager accManager = new UserAccountManager();
            accManager.AddUserAccount();
  */      }

        protected override void OnStop()
        {
        }
    }
}
