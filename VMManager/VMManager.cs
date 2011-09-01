using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.ServiceProcess;
using System.Text;

namespace VMManager
{
    public partial class VMManager : ServiceBase
    {
        Server server;
        VMMonitor vmMonitor;
        VMActivityQueue activityQueue;
        string regMonDir = "HKEY_LOCAL_MACHINE\\SOFTWARE\\RetherNetworksInc\\DOFS-SandBox";

        public VMManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            activityQueue = new VMActivityQueue();

            server = new Server(activityQueue);
            vmMonitor = new VMMonitor(activityQueue);
            vmMonitor.Start();
            regMon();


        }
        public void regMon()
        {
            RegistryMonitor monitor = new RegistryMonitor(regMonDir);

            monitor.RegChanged += new EventHandler(OnRegChanged);
            monitor.Start();

        }
        private void OnRegChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("reg Changed");
            server.regChanged();
            vmMonitor.regChanged();


        }
        protected override void OnStop()
        {
            if (vmMonitor != null)
                vmMonitor.Stop();
        }
    }
}
