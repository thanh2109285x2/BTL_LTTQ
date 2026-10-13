using BTL_LTTQ.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.QuanLy
{
    public partial class frmLopHoc : Form
    {
        public frmLopHoc()
        {
            InitializeComponent();
        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ok = DbHelper.TestConnection(out string msg);
            MessageBox.Show(msg, ok ? "OK" : "Lỗi");
        }
    }
}
