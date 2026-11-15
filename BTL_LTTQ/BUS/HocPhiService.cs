using BTL_LTTQ.Common;
using BTL_LTTQ.DAL;
using BTL_LTTQ.Models;
using System.Data;
using System.Globalization;
using System.Text;

namespace BTL_LTTQ.BUS
{
    public sealed class HocPhiService
    {
        private readonly HocPhiRepository _repository = new();

        public string LayHocVien(string tuKhoa, out DataTable danhSach)
        {
            danhSach = new DataTable();
            if (!PermissionService.Can(ChucNangConst.DONGHOCPHI)) return "Bạn không có quyền sử dụng chức năng đóng học phí.";
            if (CurrentUser.IsHocVien) return "Học viên không được tra cứu học viên khác.";
            if (!CurrentUser.IsAdmin) return "Chức năng lập phiếu giúp chỉ dành cho Admin.";
            try
            {
                danhSach = _repository.TimHocVien(tuKhoa.Trim());
                return "";
            }
            catch (Exception ex) { return "Không tìm được học viên: " + ex.Message; }
        }

        public string LayLopChuaLapPhieu(string? maHocVien, out DataTable danhSach)
        {
            danhSach = new DataTable();
            string loi = LayMaHocVienDuocPhep(maHocVien, out string ma);
            if (loi != "") return loi;
            try
            {
                danhSach = _repository.LayLopChuaLapPhieu(ma);
                return "";
            }
            catch (Exception ex) { return "Không tải được lớp chưa lập phiếu: " + ex.Message; }
        }

        public string TaoPhieu(string? maHocVien, IReadOnlyCollection<string> cacMaLop, out PhieuThuMoi? phieu)
        {
            phieu = null;
            string loi = LayMaHocVienDuocPhep(maHocVien, out string ma);
            if (loi != "") return loi;
            if (cacMaLop == null || cacMaLop.Count == 0) return "Vui lòng chọn ít nhất một lớp.";
            try
            {
                phieu = _repository.TaoPhieuThu(ma, cacMaLop);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string LayDanhSachPhieu(string? maHocVien, bool chiChoThanhToan, out DataTable danhSach)
        {
            danhSach = new DataTable();
            if (!PermissionService.Can(ChucNangConst.DONGHOCPHI)) return "Bạn không có quyền xem phiếu học phí.";
            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien)) return "Không xác định được mã học viên đăng nhập.";
                maHocVien = CurrentUser.MaHocVien;
            }
            else if (!CurrentUser.IsAdmin)
                return "Chức năng đóng học phí chỉ dành cho học viên và Admin.";

            try
            {
                danhSach = _repository.LayPhieuThu(maHocVien, chiChoThanhToan);
                return "";
            }
            catch (Exception ex) { return "Không tải được danh sách phiếu: " + ex.Message; }
        }

        public string HuyPhieu(int maPhieu)
        {
            if (!PermissionService.Can(ChucNangConst.DONGHOCPHI)) return "Bạn không có quyền hủy phiếu học phí.";
            if (!CurrentUser.IsHocVien || string.IsNullOrWhiteSpace(CurrentUser.MaHocVien))
                return "Chỉ học viên được hủy phiếu của chính mình.";
            try
            {
                _repository.HuyPhieu(maPhieu, CurrentUser.MaHocVien);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string XacNhanDaThu(int maPhieu)
        {
            if (!PermissionService.Can(ChucNangConst.HOCPHI_XACNHAN) || !CurrentUser.IsAdmin)
                return "Chỉ Admin có quyền xác nhận đã thu.";
            if (maPhieu <= 0) return "Mã phiếu không hợp lệ.";
            try
            {
                _repository.XacNhanDaThu(maPhieu, CurrentUser.TenDangNhap);
                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string TaoNoiDungVietQr(string bin, string soTaiKhoan, decimal soTien, string noiDung)
        {
            bin = bin?.Trim() ?? "";
            soTaiKhoan = soTaiKhoan?.Trim() ?? "";
            noiDung = noiDung?.Trim() ?? "";

            string beneficiaryInfo = Tlv("00", bin) + Tlv("01", soTaiKhoan);
            string merchantAccount = Tlv("00", "A000000727") + Tlv("01", beneficiaryInfo) + Tlv("02", "QRIBFTTA");
            string additional = Tlv("08", noiDung);

            var payload = new StringBuilder();
            payload.Append(Tlv("00", "01"));
            payload.Append(Tlv("01", "12"));
            payload.Append(Tlv("38", merchantAccount));
            payload.Append(Tlv("53", "704"));
            payload.Append(Tlv("54", soTien.ToString("0", CultureInfo.InvariantCulture)));
            payload.Append(Tlv("58", "VN"));
            payload.Append(Tlv("62", additional));
            payload.Append("6304");

            return payload + TinhCrc16CcittFalse(payload.ToString());
        }

        private string LayMaHocVienDuocPhep(string? maHocVien, out string ma)
        {
            ma = "";
            if (!PermissionService.Can(ChucNangConst.DONGHOCPHI)) return "Bạn không có quyền sử dụng chức năng đóng học phí.";
            if (CurrentUser.IsHocVien)
            {
                if (string.IsNullOrWhiteSpace(CurrentUser.MaHocVien)) return "Không xác định được mã học viên đăng nhập.";
                ma = CurrentUser.MaHocVien;
                return "";
            }
            if (!CurrentUser.IsAdmin) return "Chức năng này chỉ dành cho học viên và Admin.";
            if (string.IsNullOrWhiteSpace(maHocVien)) return "Admin vui lòng chọn học viên.";
            ma = maHocVien.Trim();
            return "";
        }

        private static string Tlv(string id, string value)
        {
            int length = Encoding.UTF8.GetByteCount(value);
            if (length > 99) throw new ArgumentOutOfRangeException(nameof(value), "Trường VietQR vượt quá độ dài TLV tối đa.");
            return id + length.ToString("D2", CultureInfo.InvariantCulture) + value;
        }

        private static string TinhCrc16CcittFalse(string data)
        {
            ushort crc = 0xFFFF;
            foreach (byte value in Encoding.ASCII.GetBytes(data))
            {
                crc ^= (ushort)(value << 8);
                for (int bit = 0; bit < 8; bit++)
                    crc = (ushort)((crc & 0x8000) != 0 ? (crc << 1) ^ 0x1021 : crc << 1);
            }
            return crc.ToString("X4", CultureInfo.InvariantCulture);
        }
    }
}
