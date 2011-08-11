using System;

namespace VMManager
{
    public class vmEntry
    {
        public DateTime timestamp { get; set; }
        public string vmPath { get; set; }
        public string snapshotName { get; set; }
        public string vmName { get; set; }
        public bool busy { get; set; }
        public int indexNum { get; set; }

        public vmEntry()
        {
            busy = false;
            indexNum = 0;
        }

        public vmEntry(DateTime ts, string inVmPath, string inSnapshotName, string inVMName)
        {
            vmName = inVMName;
            timestamp = ts;
            vmPath = inVmPath;
            snapshotName = inSnapshotName;
            busy = false;
            indexNum = 0;
        }
    }

}