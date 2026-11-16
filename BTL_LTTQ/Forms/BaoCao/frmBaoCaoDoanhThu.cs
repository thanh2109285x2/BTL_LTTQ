namespace BTL_LTTQ.Forms.BaoCao
{
    public partial class frmBaoCaoDoanhThu : frmBaoCaoBase
    {
        public frmBaoCaoDoanhThu() : base(false) { InitializeComponent(); GiaoDien.ApDung(this); lblBannerTieuDe.ForeColor = Color.White; lblBannerMoTa.ForeColor = Color.FromArgb(225, 235, 248); }
        protected override ComboBox cboLop => _cboLop;
        protected override DataGridView dgvBaoCao => _dgvBaoCao;
        protected override Label lblTong => _lblTong;
        private void btnXemBaoCao_Click(object? sender, EventArgs e) => NapDuLieu();
    }
}
