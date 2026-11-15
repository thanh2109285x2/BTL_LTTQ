using BTL_LTTQ;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.NghiepVu
{
    public partial class frmDongHocPhi : Form
    {
        public frmDongHocPhi()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }
    }
}
