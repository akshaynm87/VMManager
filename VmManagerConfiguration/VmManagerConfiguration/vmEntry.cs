using System;
using System.Collections.Generic;
using System.Text;

namespace VmManagerConfiguration
{
    public class vmEntry
    {
        public DateTime timestamp { get; set; }
        public string vmFile { get; set; }
        public string snapshotName { get; set; }
        public string vmName { get; set; }
        public bool busy { get; set; }
        public int indexNum { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public vmEntry()
        {
            busy = false;
            indexNum = 0;
        }

        public vmEntry(DateTime ts, string inVmPath, string inSnapshotName, string inVMName)
        {
            vmName = inVMName;
            timestamp = ts;
            vmFile = inVmPath;
            snapshotName = inSnapshotName;
            busy = false;
            indexNum = 0;
        }
    }
}
