using BTL_LTTQ;
using System.Windows.Forms;

namespace BTL_LTTQ.Forms.NghiepVu
{
    public partial class frmNhapDiem : Form
    {
        public frmNhapDiem()
        {
            InitializeComponent();
            GiaoDien.ApDung(this);
        }
    }
}
