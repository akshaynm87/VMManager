using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VmManagerConfiguration
{
    public partial class AddEntry : Form
    {

        List <vmEntry>vmList;
        VMManagerConfigUI vmUI;
        
        public AddEntry(List<vmEntry> list,VMManagerConfigUI ui)
        {
            InitializeComponent();
            vmList = list;
            vmUI = ui;

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            vmEntry entry = new vmEntry();
            entry.vmName = textBoxName.Text;
            entry.vmFile = textBoxFile.Text;
            entry.snapshotName = textBoxSnapshotname.Text;
            entry.indexNum = vmList.Count+1;
            entry.busy = false;

            vmList.Add(entry);
            this.Dispose();
            vmUI.shouldSelect = vmList.Count-1;
            vmUI.updateList();
            

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.InitialDirectory = "c:\\";
                textBoxFile.Text = openFileDialog.FileName;
            }
        }
    }
}
