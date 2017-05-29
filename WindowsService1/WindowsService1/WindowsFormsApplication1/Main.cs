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
            InitializeComponent();
            this.controller = new Controller();
            tableDevice = new DataTable();
            tableDevice.Columns.Add(Constant.DeviceTable.DEVICE_NAME);
            tableDevice.Columns.Add(Constant.DeviceTable.DEVICE_PID);
            tableDevice.Columns.Add(Constant.DeviceTable.DEVICE_VID);
            dataGridView1.DataSource = tableDevice;
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
            List<Device> lstDevice = controller.getAllDevice();
            if (lstDevice != null) { 
                foreach(Device device in lstDevice){
                    String[] data = new String[3];
                    data[0] = device.nameDevice;
                    data[1] = device.pidDevice;
                    data[2] = device.vidDevice;
                    tableDevice.Rows.Add(data);
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            String[] data = new String[3];
            data[0] = txtName.Text;
            data[1] = txtPid.Text;
            data[2] = txtVid.Text;
            tableDevice.Rows.Add(data);

        }

        private void remove_Click(object sender, EventArgs e)
        {
            tableDevice.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            //tableDevice.Clear();
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
            if (result)
            {
                MessageBox.Show(this, "success");
            }
            else {
                MessageBox.Show(this, "Error");
            }
        }

    }
}
