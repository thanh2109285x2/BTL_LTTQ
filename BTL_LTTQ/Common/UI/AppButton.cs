using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BTL_LTTQ
{
    public enum KieuNut
    {
        Phu,
        Chinh,
        NguyHiem,
        MenuCha,
        MenuCon,
        MenuTienIch,
        MenuThoat
    }

    [DefaultProperty(nameof(Kieu))]
    public class AppButton : Button
    {
        private KieuNut _kieu;

        [Category("Giao diện")]
        [DefaultValue(KieuNut.Phu)]
        public KieuNut Kieu
        {
            get => _kieu;
            set
            {
                if (_kieu == value)
                    return;

                _kieu = value;
                ApDungKieu();
            }
        }

        public bool ShouldSerializeKieu() => Kieu != KieuNut.Phu;

        public void ResetKieu() => Kieu = KieuNut.Phu;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApDungKieu();
        }

        private void ApDungKieu()
        {
            UseVisualStyleBackColor = false;
            FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;

            switch (Kieu)
            {
                case KieuNut.Chinh:
                    DinhDangMau(GiaoDien.MauChinh, GiaoDien.MauTrang, GiaoDien.MauChinh,
                        GiaoDien.MauChinhDam, GiaoDien.VienNutThuong);
                    break;
                case KieuNut.NguyHiem:
                    DinhDangMau(GiaoDien.MauNguyHiem, GiaoDien.MauTrang, GiaoDien.MauNguyHiem,
                        GiaoDien.MauNguyHiemDam, GiaoDien.VienNutThuong);
                    break;
                case KieuNut.MenuCha:
                    DinhDangMau(GiaoDien.MauSidebarCha, GiaoDien.MauChinhDam, GiaoDien.MauSidebarCha,
                        GiaoDien.MauSidebarHover, GiaoDien.VienNutMenu);
                    Font = new Font(Font, GiaoDien.ChuDam);
                    TextAlign = GiaoDien.CanTrai;
                    ImageAlign = GiaoDien.CanAnhMenuCha;
                    TextImageRelation = GiaoDien.AnhTruocChu;
                    Padding = GiaoDien.LeMenuCha;
                    break;
                case KieuNut.MenuCon:
                    DinhDangMau(GiaoDien.MauSidebar, GiaoDien.MauSidebarChu, GiaoDien.MauSidebar,
                        GiaoDien.MauSidebarHover, GiaoDien.VienNutMenu);
                    Font = new Font(Font, GiaoDien.ChuThuong);
                    TextAlign = GiaoDien.CanTrai;
                    Padding = GiaoDien.LeMenuCon;
                    break;
                case KieuNut.MenuTienIch:
                    DinhDangMau(GiaoDien.MauSidebar, GiaoDien.MauChinh, GiaoDien.MauSidebar,
                        GiaoDien.MauSidebarHover, GiaoDien.VienNutMenu);
                    Font = new Font(Font, GiaoDien.ChuThuong);
                    TextAlign = GiaoDien.CanGiua;
                    Padding = GiaoDien.LeMenuTienIch;
                    break;
                case KieuNut.MenuThoat:
                    DinhDangMau(GiaoDien.MauSidebar, GiaoDien.MauNguyHiem, GiaoDien.MauSidebar,
                        GiaoDien.MauThoatHover, GiaoDien.VienNutMenu);
                    Font = new Font(Font, GiaoDien.ChuThuong);
                    TextAlign = GiaoDien.CanGiua;
                    break;
                default:
                    DinhDangMau(GiaoDien.MauTrang, GiaoDien.MauChu, GiaoDien.MauVien,
                        GiaoDien.MauHangLe, GiaoDien.VienNutThuong);
                    break;
            }
        }

        private void DinhDangMau(Color nen, Color chu, Color vien, Color hover, int doDayVien)
        {
            BackColor = nen;
            ForeColor = chu;
            FlatAppearance.BorderColor = vien;
            FlatAppearance.BorderSize = doDayVien;
            FlatAppearance.MouseOverBackColor = hover;
            FlatAppearance.MouseDownBackColor = hover;
        }
    }
}
