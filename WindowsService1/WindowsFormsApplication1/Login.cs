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
    public partial class Login : Form
    {
        Controller controller = new Controller();
        public Login()
        {
            InitializeComponent();
        }

        /***
         * 
         */ 
        private void loginButton_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == null){
                txtUser.Focus();
            }

            if (txtPassword.Text == null) {
                txtPassword.Focus();
            }

            Boolean result = controller.Login(txtUser.Text, txtPassword.Text);
            if (result) { 
                // 
                Application.Run(new Main());
                this.Dispose();
            }
        }
    }
}
