#nullable enable

namespace BTL_LTTQ.Forms.QuanLy
{
    partial class frmGiaoVien
    {
        private System.ComponentModel.IContainer? components = null;
        private TableLayoutPanel _root = null!;
        private FlowLayoutPanel _search = null!;
        private Label _lblTim = null!;
        private TextBox _txtTim = null!;
        private Button _btnTim = null!;
        private Button _btnTatCa = null!;
        private TableLayoutPanel _content = null!;
        private GroupBox _thongTin = null!;
        private TableLayoutPanel _thongTinLayout = null!;
        private Panel _fields = null!;
        private Label _lblMa = null!;
        private TextBox _txtMa = null!;
        private Label _lblTen = null!;
        private TextBox _txtTen = null!;
        private Label _lblNgaySinh = null!;
        private DateTimePicker _ngaySinh = null!;
        private Label _lblGioiTinh = null!;
        private FlowLayoutPanel _gioiTinh = null!;
        private RadioButton _nam = null!;
        private RadioButton _nu = null!;
        private Label _lblDienThoai = null!;
        private TextBox _txtDienThoai = null!;
        private Label _lblDiaChi = null!;
        private TextBox _txtDiaChi = null!;
        private Label _lblChucDanh = null!;
        private ComboBox _chucDanh = null!;
        private Label _lblChuyenMon = null!;
        private ComboBox _chuyenMon = null!;
        private Label _lblCoQuan = null!;
        private ComboBox _coQuan = null!;
        private Label _lblTrinhDo = null!;
        private FlowLayoutPanel _trinhDo = null!;
        private RadioButton _radioTrinhDo1 = null!;
        private RadioButton _radioTrinhDo2 = null!;
        private RadioButton _radioTrinhDo3 = null!;
        private Panel _anhPanel = null!;
        private Label _lblAnh = null!;
        private PictureBox _previewAnh = null!;
        private Button _btnChonAnh = null!;
        private GroupBox _monBox = null!;
        private CheckedListBox _monDay = null!;
        private FlowLayoutPanel _actions = null!;
        private Button _btnThem = null!;
        private Button _btnSua = null!;
        private Button _btnXoa = null!;
        private Button _btnMoi = null!;
        private DataGridView _grid = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            _root = new TableLayoutPanel();
            _search = new FlowLayoutPanel();
            _lblTim = new Label();
            _txtTim = new TextBox();
            _btnTim = new Button();
            _btnTatCa = new Button();
            _content = new TableLayoutPanel();
            _thongTin = new GroupBox();
            _thongTinLayout = new TableLayoutPanel();
            _fields = new Panel();
            _lblMa = new Label();
            _txtMa = new TextBox();
            _lblTen = new Label();
            _txtTen = new TextBox();
            _lblNgaySinh = new Label();
            _ngaySinh = new DateTimePicker();
            _lblGioiTinh = new Label();
            _gioiTinh = new FlowLayoutPanel();
            _nam = new RadioButton();
            _nu = new RadioButton();
            _lblDienThoai = new Label();
            _txtDienThoai = new TextBox();
            _lblDiaChi = new Label();
            _txtDiaChi = new TextBox();
            _lblChucDanh = new Label();
            _chucDanh = new ComboBox();
            _lblChuyenMon = new Label();
            _chuyenMon = new ComboBox();
            _lblCoQuan = new Label();
            _coQuan = new ComboBox();
            _lblTrinhDo = new Label();
            _trinhDo = new FlowLayoutPanel();
            _radioTrinhDo1 = new RadioButton();
            _radioTrinhDo2 = new RadioButton();
            _radioTrinhDo3 = new RadioButton();
            _anhPanel = new Panel();
            _lblAnh = new Label();
            _previewAnh = new PictureBox();
            _btnChonAnh = new Button();
            _monBox = new GroupBox();
            _monDay = new CheckedListBox();
            _actions = new FlowLayoutPanel();
            _btnThem = new Button();
            _btnSua = new Button();
            _btnXoa = new Button();
            _btnMoi = new Button();
            _grid = new DataGridView();
            _root.SuspendLayout();
            _search.SuspendLayout();
            _content.SuspendLayout();
            _thongTin.SuspendLayout();
            _thongTinLayout.SuspendLayout();
            _fields.SuspendLayout();
            _gioiTinh.SuspendLayout();
            _trinhDo.SuspendLayout();
            _anhPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_previewAnh).BeginInit();
            _monBox.SuspendLayout();
            _actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // _root
            // 
            _root.ColumnCount = 1;
            _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _root.Controls.Add(_search, 0, 0);
            _root.Controls.Add(_content, 0, 1);
            _root.Controls.Add(_actions, 0, 2);
            _root.Controls.Add(_grid, 0, 3);
            _root.Dock = DockStyle.Fill;
            _root.Location = new Point(0, 0);
            _root.Margin = new Padding(3, 4, 3, 4);
            _root.Name = "_root";
            _root.Padding = new Padding(9);
            _root.RowCount = 4;
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 392F));
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _root.Size = new Size(1349, 894);
            _root.TabIndex = 0;
            // 
            // _search
            // 
            _search.Controls.Add(_lblTim);
            _search.Controls.Add(_txtTim);
            _search.Controls.Add(_btnTim);
            _search.Controls.Add(_btnTatCa);
            _search.Dock = DockStyle.Fill;
            _search.Location = new Point(12, 13);
            _search.Margin = new Padding(3, 4, 3, 4);
            _search.Name = "_search";
            _search.Padding = new Padding(5);
            _search.Size = new Size(1325, 44);
            _search.TabIndex = 0;
            _search.WrapContents = false;
            // 
            // _lblTim
            // 
            _lblTim.AutoSize = true;
            _lblTim.Location = new Point(10, 13);
            _lblTim.Margin = new Padding(5, 8, 9, 0);
            _lblTim.Name = "_lblTim";
            _lblTim.Size = new Size(197, 20);
            _lblTim.TabIndex = 0;
            _lblTim.Text = "Tìm mã, tên hoặc điện thoại:";
            // 
            // _txtTim
            // 
            _txtTim.Location = new Point(219, 9);
            _txtTim.Margin = new Padding(3, 4, 3, 4);
            _txtTim.MaxLength = 100;
            _txtTim.Name = "_txtTim";
            _txtTim.Size = new Size(319, 27);
            _txtTim.TabIndex = 0;
            // 
            // _btnTim
            // 
            _btnTim.AutoSize = true;
            _btnTim.BackColor = Color.FromArgb(41, 98, 168);
            _btnTim.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            _btnTim.FlatStyle = FlatStyle.Flat;
            _btnTim.ForeColor = Color.White;
            _btnTim.Location = new Point(544, 9);
            _btnTim.Margin = new Padding(3, 4, 3, 4);
            _btnTim.Name = "_btnTim";
            _btnTim.Size = new Size(86, 32);
            _btnTim.TabIndex = 1;
            _btnTim.Text = "Tìm";
            _btnTim.UseVisualStyleBackColor = false;
            // 
            // _btnTatCa
            // 
            _btnTatCa.AutoSize = true;
            _btnTatCa.BackColor = Color.White;
            _btnTatCa.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            _btnTatCa.FlatStyle = FlatStyle.Flat;
            _btnTatCa.ForeColor = Color.FromArgb(41, 98, 168);
            _btnTatCa.Location = new Point(636, 8);
            _btnTatCa.Name = "_btnTatCa";
            _btnTatCa.Size = new Size(86, 32);
            _btnTatCa.TabIndex = 2;
            _btnTatCa.Text = "Tất cả";
            _btnTatCa.UseVisualStyleBackColor = false;
            // 
            // _content
            // 
            _content.ColumnCount = 2;
            _content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            _content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            _content.Controls.Add(_thongTin, 0, 0);
            _content.Controls.Add(_monBox, 1, 0);
            _content.Dock = DockStyle.Fill;
            _content.Location = new Point(12, 65);
            _content.Margin = new Padding(3, 4, 3, 4);
            _content.Name = "_content";
            _content.RowCount = 1;
            _content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _content.Size = new Size(1325, 384);
            _content.TabIndex = 1;
            // 
            // _thongTin
            // 
            _thongTin.Controls.Add(_thongTinLayout);
            _thongTin.Dock = DockStyle.Fill;
            _thongTin.Location = new Point(3, 4);
            _thongTin.Margin = new Padding(3, 4, 3, 4);
            _thongTin.Name = "_thongTin";
            _thongTin.Padding = new Padding(9);
            _thongTin.Size = new Size(921, 376);
            _thongTin.TabIndex = 0;
            _thongTin.TabStop = false;
            _thongTin.Text = "Thông tin giáo viên";
            // 
            // _thongTinLayout
            // 
            _thongTinLayout.ColumnCount = 2;
            _thongTinLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73F));
            _thongTinLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            _thongTinLayout.Controls.Add(_fields, 0, 0);
            _thongTinLayout.Controls.Add(_anhPanel, 1, 0);
            _thongTinLayout.Dock = DockStyle.Fill;
            _thongTinLayout.Location = new Point(9, 29);
            _thongTinLayout.Margin = new Padding(3, 4, 3, 4);
            _thongTinLayout.Name = "_thongTinLayout";
            _thongTinLayout.RowCount = 1;
            _thongTinLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _thongTinLayout.Size = new Size(903, 338);
            _thongTinLayout.TabIndex = 0;
            // 
            // _fields
            // 
            _fields.Controls.Add(_lblMa);
            _fields.Controls.Add(_txtMa);
            _fields.Controls.Add(_lblTen);
            _fields.Controls.Add(_txtTen);
            _fields.Controls.Add(_lblNgaySinh);
            _fields.Controls.Add(_ngaySinh);
            _fields.Controls.Add(_lblGioiTinh);
            _fields.Controls.Add(_gioiTinh);
            _fields.Controls.Add(_lblDienThoai);
            _fields.Controls.Add(_txtDienThoai);
            _fields.Controls.Add(_lblDiaChi);
            _fields.Controls.Add(_txtDiaChi);
            _fields.Controls.Add(_lblChucDanh);
            _fields.Controls.Add(_chucDanh);
            _fields.Controls.Add(_lblChuyenMon);
            _fields.Controls.Add(_chuyenMon);
            _fields.Controls.Add(_lblCoQuan);
            _fields.Controls.Add(_coQuan);
            _fields.Controls.Add(_lblTrinhDo);
            _fields.Controls.Add(_trinhDo);
            _fields.Dock = DockStyle.Fill;
            _fields.Location = new Point(3, 4);
            _fields.Margin = new Padding(3, 4, 3, 4);
            _fields.Name = "_fields";
            _fields.Size = new Size(653, 330);
            _fields.TabIndex = 0;
            // 
            // _lblMa
            // 
            _lblMa.Location = new Point(8, 17);
            _lblMa.Margin = new Padding(3, 4, 3, 4);
            _lblMa.Name = "_lblMa";
            _lblMa.Size = new Size(100, 27);
            _lblMa.TabIndex = 0;
            _lblMa.Text = "Mã giáo viên";
            _lblMa.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _txtMa
            // 
            _txtMa.Location = new Point(112, 17);
            _txtMa.Margin = new Padding(3, 4, 3, 4);
            _txtMa.MaxLength = 20;
            _txtMa.Name = "_txtMa";
            _txtMa.Size = new Size(205, 27);
            _txtMa.TabIndex = 0;
            // 
            // _lblTen
            // 
            _lblTen.Location = new Point(328, 17);
            _lblTen.Margin = new Padding(3, 4, 3, 4);
            _lblTen.Name = "_lblTen";
            _lblTen.Size = new Size(100, 27);
            _lblTen.TabIndex = 1;
            _lblTen.Text = "Họ tên";
            _lblTen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _txtTen
            // 
            _txtTen.Location = new Point(432, 17);
            _txtTen.Margin = new Padding(3, 4, 3, 4);
            _txtTen.MaxLength = 150;
            _txtTen.Name = "_txtTen";
            _txtTen.Size = new Size(208, 27);
            _txtTen.TabIndex = 0;
            // 
            // _lblNgaySinh
            // 
            _lblNgaySinh.Location = new Point(8, 87);
            _lblNgaySinh.Margin = new Padding(3, 4, 3, 4);
            _lblNgaySinh.Name = "_lblNgaySinh";
            _lblNgaySinh.Size = new Size(100, 27);
            _lblNgaySinh.TabIndex = 2;
            _lblNgaySinh.Text = "Ngày sinh";
            _lblNgaySinh.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _ngaySinh
            // 
            _ngaySinh.Anchor = AnchorStyles.Left;
            _ngaySinh.Checked = false;
            _ngaySinh.CustomFormat = "dd/MM/yyyy";
            _ngaySinh.Format = DateTimePickerFormat.Custom;
            _ngaySinh.Location = new Point(112, 71);
            _ngaySinh.Margin = new Padding(3, 4, 3, 4);
            _ngaySinh.Name = "_ngaySinh";
            _ngaySinh.ShowCheckBox = true;
            _ngaySinh.Size = new Size(190, 27);
            _ngaySinh.TabIndex = 1;
            // 
            // _lblGioiTinh
            // 
            _lblGioiTinh.Location = new Point(328, 87);
            _lblGioiTinh.Margin = new Padding(3, 4, 3, 4);
            _lblGioiTinh.Name = "_lblGioiTinh";
            _lblGioiTinh.Size = new Size(100, 27);
            _lblGioiTinh.TabIndex = 3;
            _lblGioiTinh.Text = "Giới tính";
            _lblGioiTinh.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _gioiTinh
            // 
            _gioiTinh.Controls.Add(_nam);
            _gioiTinh.Controls.Add(_nu);
            _gioiTinh.Location = new Point(432, 82);
            _gioiTinh.Margin = new Padding(3, 4, 3, 4);
            _gioiTinh.Name = "_gioiTinh";
            _gioiTinh.Size = new Size(208, 38);
            _gioiTinh.TabIndex = 4;
            _gioiTinh.WrapContents = false;
            // 
            // _nam
            // 
            _nam.AutoSize = true;
            _nam.Location = new Point(3, 8);
            _nam.Margin = new Padding(3, 8, 9, 4);
            _nam.Name = "_nam";
            _nam.Size = new Size(62, 24);
            _nam.TabIndex = 0;
            _nam.Text = "Nam";
            // 
            // _nu
            // 
            _nu.AutoSize = true;
            _nu.Location = new Point(77, 8);
            _nu.Margin = new Padding(3, 8, 9, 4);
            _nu.Name = "_nu";
            _nu.Size = new Size(50, 24);
            _nu.TabIndex = 1;
            _nu.Text = "Nữ";
            // 
            // _lblDienThoai
            // 
            _lblDienThoai.Location = new Point(8, 157);
            _lblDienThoai.Margin = new Padding(3, 4, 3, 4);
            _lblDienThoai.Name = "_lblDienThoai";
            _lblDienThoai.Size = new Size(100, 27);
            _lblDienThoai.TabIndex = 5;
            _lblDienThoai.Text = "Điện thoại";
            _lblDienThoai.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _txtDienThoai
            // 
            _txtDienThoai.Location = new Point(112, 157);
            _txtDienThoai.Margin = new Padding(3, 4, 3, 4);
            _txtDienThoai.MaxLength = 30;
            _txtDienThoai.Name = "_txtDienThoai";
            _txtDienThoai.Size = new Size(205, 27);
            _txtDienThoai.TabIndex = 0;
            // 
            // _lblDiaChi
            // 
            _lblDiaChi.Location = new Point(328, 157);
            _lblDiaChi.Margin = new Padding(3, 4, 3, 4);
            _lblDiaChi.Name = "_lblDiaChi";
            _lblDiaChi.Size = new Size(100, 27);
            _lblDiaChi.TabIndex = 6;
            _lblDiaChi.Text = "Địa chỉ";
            _lblDiaChi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _txtDiaChi
            // 
            _txtDiaChi.Location = new Point(432, 157);
            _txtDiaChi.Margin = new Padding(3, 4, 3, 4);
            _txtDiaChi.MaxLength = 200;
            _txtDiaChi.Name = "_txtDiaChi";
            _txtDiaChi.Size = new Size(208, 27);
            _txtDiaChi.TabIndex = 0;
            // 
            // _lblChucDanh
            // 
            _lblChucDanh.Location = new Point(8, 227);
            _lblChucDanh.Margin = new Padding(3, 4, 3, 4);
            _lblChucDanh.Name = "_lblChucDanh";
            _lblChucDanh.Size = new Size(100, 28);
            _lblChucDanh.TabIndex = 7;
            _lblChucDanh.Text = "Chức danh";
            _lblChucDanh.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _chucDanh
            // 
            _chucDanh.DropDownStyle = ComboBoxStyle.DropDownList;
            _chucDanh.FormattingEnabled = true;
            _chucDanh.Location = new Point(112, 227);
            _chucDanh.Margin = new Padding(3, 4, 3, 4);
            _chucDanh.Name = "_chucDanh";
            _chucDanh.Size = new Size(205, 28);
            _chucDanh.TabIndex = 8;
            // 
            // _lblChuyenMon
            // 
            _lblChuyenMon.Location = new Point(328, 227);
            _lblChuyenMon.Margin = new Padding(3, 4, 3, 4);
            _lblChuyenMon.Name = "_lblChuyenMon";
            _lblChuyenMon.Size = new Size(100, 28);
            _lblChuyenMon.TabIndex = 9;
            _lblChuyenMon.Text = "Chuyên môn";
            _lblChuyenMon.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _chuyenMon
            // 
            _chuyenMon.DropDownStyle = ComboBoxStyle.DropDownList;
            _chuyenMon.FormattingEnabled = true;
            _chuyenMon.Location = new Point(432, 227);
            _chuyenMon.Margin = new Padding(3, 4, 3, 4);
            _chuyenMon.Name = "_chuyenMon";
            _chuyenMon.Size = new Size(208, 28);
            _chuyenMon.TabIndex = 10;
            // 
            // _lblCoQuan
            // 
            _lblCoQuan.Location = new Point(8, 297);
            _lblCoQuan.Margin = new Padding(3, 4, 3, 4);
            _lblCoQuan.Name = "_lblCoQuan";
            _lblCoQuan.Size = new Size(100, 28);
            _lblCoQuan.TabIndex = 11;
            _lblCoQuan.Text = "Cơ quan";
            _lblCoQuan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _coQuan
            // 
            _coQuan.DropDownStyle = ComboBoxStyle.DropDownList;
            _coQuan.FormattingEnabled = true;
            _coQuan.Location = new Point(112, 297);
            _coQuan.Margin = new Padding(3, 4, 3, 4);
            _coQuan.Name = "_coQuan";
            _coQuan.Size = new Size(205, 28);
            _coQuan.TabIndex = 12;
            // 
            // _lblTrinhDo
            // 
            _lblTrinhDo.Location = new Point(328, 297);
            _lblTrinhDo.Margin = new Padding(3, 4, 3, 4);
            _lblTrinhDo.Name = "_lblTrinhDo";
            _lblTrinhDo.Size = new Size(100, 28);
            _lblTrinhDo.TabIndex = 13;
            _lblTrinhDo.Text = "Trình độ";
            _lblTrinhDo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _trinhDo
            // 
            _trinhDo.AutoScroll = true;
            _trinhDo.Controls.Add(_radioTrinhDo1);
            _trinhDo.Controls.Add(_radioTrinhDo2);
            _trinhDo.Controls.Add(_radioTrinhDo3);
            _trinhDo.Location = new Point(432, 292);
            _trinhDo.Margin = new Padding(3, 4, 3, 4);
            _trinhDo.Name = "_trinhDo";
            _trinhDo.Padding = new Padding(0, 8, 0, 0);
            _trinhDo.Size = new Size(348, 58);
            _trinhDo.TabIndex = 14;
            // 
            // _radioTrinhDo1
            // 
            _radioTrinhDo1.AutoSize = true;
            _radioTrinhDo1.Location = new Point(0, 8);
            _radioTrinhDo1.Margin = new Padding(0, 0, 5, 0);
            _radioTrinhDo1.Name = "_radioTrinhDo1";
            _radioTrinhDo1.Size = new Size(84, 24);
            _radioTrinhDo1.TabIndex = 0;
            _radioTrinhDo1.Text = "Cử nhân";
            // 
            // _radioTrinhDo2
            // 
            _radioTrinhDo2.AutoSize = true;
            _radioTrinhDo2.Location = new Point(89, 8);
            _radioTrinhDo2.Margin = new Padding(0, 0, 5, 0);
            _radioTrinhDo2.Name = "_radioTrinhDo2";
            _radioTrinhDo2.Size = new Size(75, 24);
            _radioTrinhDo2.TabIndex = 1;
            _radioTrinhDo2.Text = "Thạc sĩ";
            // 
            // _radioTrinhDo3
            // 
            _radioTrinhDo3.AutoSize = true;
            _radioTrinhDo3.Location = new Point(169, 8);
            _radioTrinhDo3.Margin = new Padding(0, 0, 5, 0);
            _radioTrinhDo3.Name = "_radioTrinhDo3";
            _radioTrinhDo3.Size = new Size(72, 24);
            _radioTrinhDo3.TabIndex = 2;
            _radioTrinhDo3.Text = "Tiến sĩ";
            // 
            // _anhPanel
            // 
            _anhPanel.Controls.Add(_lblAnh);
            _anhPanel.Controls.Add(_previewAnh);
            _anhPanel.Controls.Add(_btnChonAnh);
            _anhPanel.Dock = DockStyle.Fill;
            _anhPanel.Location = new Point(662, 4);
            _anhPanel.Margin = new Padding(3, 4, 3, 4);
            _anhPanel.Name = "_anhPanel";
            _anhPanel.Padding = new Padding(9);
            _anhPanel.Size = new Size(238, 330);
            _anhPanel.TabIndex = 1;
            // 
            // _lblAnh
            // 
            _lblAnh.Location = new Point(12, 0);
            _lblAnh.Name = "_lblAnh";
            _lblAnh.Size = new Size(214, 35);
            _lblAnh.TabIndex = 0;
            _lblAnh.Text = "Ảnh đại diện";
            _lblAnh.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _previewAnh
            // 
            _previewAnh.BackColor = Color.White;
            _previewAnh.BorderStyle = BorderStyle.FixedSingle;
            _previewAnh.Location = new Point(12, 35);
            _previewAnh.Margin = new Padding(3, 4, 3, 4);
            _previewAnh.MinimumSize = new Size(183, 211);
            _previewAnh.Name = "_previewAnh";
            _previewAnh.Size = new Size(214, 225);
            _previewAnh.SizeMode = PictureBoxSizeMode.Zoom;
            _previewAnh.TabIndex = 1;
            _previewAnh.TabStop = false;
            // 
            // _btnChonAnh
            // 
            _btnChonAnh.Anchor = AnchorStyles.Top;
            _btnChonAnh.AutoSize = true;
            _btnChonAnh.Location = new Point(71, 268);
            _btnChonAnh.Margin = new Padding(3, 4, 3, 4);
            _btnChonAnh.Name = "_btnChonAnh";
            _btnChonAnh.Size = new Size(103, 35);
            _btnChonAnh.TabIndex = 2;
            _btnChonAnh.Text = "Chọn ảnh...";
            _btnChonAnh.UseVisualStyleBackColor = true;
            // 
            // _monBox
            // 
            _monBox.Controls.Add(_monDay);
            _monBox.Dock = DockStyle.Fill;
            _monBox.Location = new Point(930, 4);
            _monBox.Margin = new Padding(3, 4, 3, 4);
            _monBox.Name = "_monBox";
            _monBox.Padding = new Padding(7);
            _monBox.Size = new Size(392, 376);
            _monBox.TabIndex = 1;
            _monBox.TabStop = false;
            _monBox.Text = "Môn giảng dạy";
            // 
            // _monDay
            // 
            _monDay.CheckOnClick = true;
            _monDay.Dock = DockStyle.Fill;
            _monDay.FormattingEnabled = true;
            _monDay.Location = new Point(7, 27);
            _monDay.Margin = new Padding(3, 4, 3, 4);
            _monDay.Name = "_monDay";
            _monDay.Size = new Size(378, 342);
            _monDay.TabIndex = 0;
            // 
            // _actions
            // 
            _actions.Controls.Add(_btnThem);
            _actions.Controls.Add(_btnSua);
            _actions.Controls.Add(_btnXoa);
            _actions.Controls.Add(_btnMoi);
            _actions.Dock = DockStyle.Fill;
            _actions.Location = new Point(12, 457);
            _actions.Margin = new Padding(3, 4, 3, 4);
            _actions.Name = "_actions";
            _actions.Padding = new Padding(5);
            _actions.Size = new Size(1325, 60);
            _actions.TabIndex = 2;
            // 
            // _btnThem
            // 
            _btnThem.BackColor = Color.FromArgb(41, 98, 168);
            _btnThem.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            _btnThem.FlatStyle = FlatStyle.Flat;
            _btnThem.ForeColor = Color.White;
            _btnThem.Location = new Point(8, 9);
            _btnThem.Margin = new Padding(3, 4, 3, 4);
            _btnThem.Name = "_btnThem";
            _btnThem.Size = new Size(120, 40);
            _btnThem.TabIndex = 0;
            _btnThem.Text = "Thêm";
            _btnThem.UseVisualStyleBackColor = false;
            // 
            // _btnSua
            // 
            _btnSua.BackColor = Color.FromArgb(41, 98, 168);
            _btnSua.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            _btnSua.FlatStyle = FlatStyle.Flat;
            _btnSua.ForeColor = Color.White;
            _btnSua.Location = new Point(134, 9);
            _btnSua.Margin = new Padding(3, 4, 3, 4);
            _btnSua.Name = "_btnSua";
            _btnSua.Size = new Size(120, 40);
            _btnSua.TabIndex = 1;
            _btnSua.Text = "Sửa";
            _btnSua.UseVisualStyleBackColor = false;
            // 
            // _btnXoa
            // 
            _btnXoa.BackColor = Color.FromArgb(192, 57, 43);
            _btnXoa.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
            _btnXoa.FlatStyle = FlatStyle.Flat;
            _btnXoa.ForeColor = Color.White;
            _btnXoa.Location = new Point(260, 9);
            _btnXoa.Margin = new Padding(3, 4, 3, 4);
            _btnXoa.Name = "_btnXoa";
            _btnXoa.Size = new Size(120, 40);
            _btnXoa.TabIndex = 2;
            _btnXoa.Text = "Xóa";
            _btnXoa.UseVisualStyleBackColor = false;
            // 
            // _btnMoi
            // 
            _btnMoi.BackColor = Color.White;
            _btnMoi.FlatAppearance.BorderColor = Color.FromArgb(41, 98, 168);
            _btnMoi.FlatStyle = FlatStyle.Flat;
            _btnMoi.ForeColor = Color.FromArgb(41, 98, 168);
            _btnMoi.Location = new Point(386, 9);
            _btnMoi.Margin = new Padding(3, 4, 3, 4);
            _btnMoi.Name = "_btnMoi";
            _btnMoi.Size = new Size(120, 40);
            _btnMoi.TabIndex = 3;
            _btnMoi.Text = "Làm mới";
            _btnMoi.UseVisualStyleBackColor = false;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            _grid.ColumnHeadersHeight = 29;
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(12, 525);
            _grid.Margin = new Padding(3, 4, 3, 4);
            _grid.MultiSelect = false;
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowHeadersWidth = 51;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new Size(1325, 356);
            _grid.TabIndex = 3;
            // 
            // frmGiaoVien
            // 
            AcceptButton = _btnTim;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1349, 894);
            Controls.Add(_root);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1026, 721);
            Name = "frmGiaoVien";
            Text = "Quản lý giáo viên";
            _root.ResumeLayout(false);
            _search.ResumeLayout(false);
            _search.PerformLayout();
            _content.ResumeLayout(false);
            _thongTin.ResumeLayout(false);
            _thongTinLayout.ResumeLayout(false);
            _fields.ResumeLayout(false);
            _fields.PerformLayout();
            _gioiTinh.ResumeLayout(false);
            _gioiTinh.PerformLayout();
            _trinhDo.ResumeLayout(false);
            _trinhDo.PerformLayout();
            _anhPanel.ResumeLayout(false);
            _anhPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_previewAnh).EndInit();
            _monBox.ResumeLayout(false);
            _actions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

    }
}
