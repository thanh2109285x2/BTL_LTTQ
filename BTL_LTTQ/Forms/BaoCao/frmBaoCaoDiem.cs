namespace BTL_LTTQ.Forms.BaoCao
{
    public partial class frmBaoCaoDiem : frmBaoCaoBase
    {
        public frmBaoCaoDiem() : base(true) { InitializeComponent(); GiaoDien.ApDung(this); }
        protected override ComboBox cboLop => _cboLop;
        protected override DataGridView dgvBaoCao => _dgvBaoCao;
        protected override Label lblTong => _lblTong;
        private void btnXemBaoCao_Click(object? sender, EventArgs e) => NapDuLieu();
    }
}
