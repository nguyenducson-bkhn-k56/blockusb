using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        private Controller controller;
        private DataTable tableDevice;
        public Main()
        {
            this.controller = new Controller();
            InitializeComponent();
        }

        private void add_Click(object sender, EventArgs e)
        {
            DataRow row = new DataRow();
            String[] data = new String[3];
            data[0] = txtName.Text;
            data[1] = txtPid.Text;
            data[2] = txtVid.Text;
            tableDevice.Rows.Add(data);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tableDevice.Clear();
        }

        private Boolean fillGridData() {
            Boolean result = false;
            return result;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Device device = new Device();
            String nameDevice = txtName.Text;
            String pidDevice = txtPid.Text;
            String vidDevice = txtVid.Text;
            Boolean result = controller.EditDevice(tableDevice);
        }

    }
}
