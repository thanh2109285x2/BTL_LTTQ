# 08 — KỊCH BẢN DEMO PHẦN MỀM

> Nguyên tắc: **chỉ demo những chức năng đã kiểm chứng chạy thật** (xem bảng trạng thái ở `00-TONG-QUAN-VA-TRANG-THAI.md`).
> Mọi chức năng chưa có UI **không** đưa vào kịch bản — nếu bị hỏi thì nói rõ là giới hạn phạm vi.

---

## 0. CHUẨN BỊ TRƯỚC BUỔI TRÌNH DIỄN

### 0.1 Kiểm tra môi trường (làm trước 30 phút)
| Việc | Cách kiểm tra | Nếu lỗi |
|---|---|---|
| SQL Server chạy | `sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -h -1 -W -Q "SELECT COUNT(*) FROM LopHoc"` | Khởi động SQL Express; kiểm tra đúng instance |
| Có dữ liệu mẫu | `LopHoc=3, HocVien=9, GiaoVien=3, DangKy=10, BuoiHoc=51, TaiKhoan=3` | Chạy `Database\ThoiKhoaBieu.sql` + `Database\PhanQuyen.sql` (idempotent) |
| Build đúng | `D:\VisualStudio\MSBuild\Current\Bin\MSBuild.exe BTL_LTTQ\BTL_LTTQ.csproj /t:Build /p:Configuration=Debug` — **không dùng `dotnet build`** (COM Excel → `MSB4803`) | Dùng MSBuild của VS |
| Excel có trên máy | Mở Excel từ Start Menu | Nếu không có Excel ⇒ **bỏ phần demo Xuất Excel** |
| Có sẵn file demo | Chuẩn bị 3 file ở `C:\BTL_Demo\` (xem 0.3) | — |
| Chụp ảnh màn hình | Có ảnh dự phòng của: màn hình xếp lịch, màn hình preview Bulk Import | — |

### 0.2 Tài khoản demo (đã kiểm tra mật khẩu khớp hash trong DB)
| Tài khoản | Mật khẩu | Vai trò | Dùng để demo |
|---|---|---|---|
| `admin` | `Admin@123` | Admin | Tạo lớp/môn, xếp lịch, phân quyền, Bulk Import |
| `gv01` | `Demo@123` | Giáo viên (GV01) | Xem **chỉ lớp của mình**, xem lịch dạy của mình |
| `hv01` | `Demo@123` | Học viên (HV01) | Tự đăng ký lớp, xem lịch học của mình |

> Mật khẩu được lưu bằng PBKDF2-SHA256 (600.000 vòng) + salt ⇒ không đọc trực tiếp được từ DB, nhưng script seed `Database/PhanQuyen.sql` ghi rõ ở comment.

### 0.3 Bộ dữ liệu demo chuẩn bị trước

**File 1 — `HocVien_OK.csv`** (UTF-8 có BOM, dùng tiêu đề tiếng Việt):
```
Mã học viên,Tên học viên,Ngày sinh,Giới tính,Điện thoại,Điểm
DEMO01,Nguyễn Minh Demo,2001-05-20,Nam,0901112222,8.5
DEMO02,Trần Thị Demo,15/03/2002,Nữ,0903334444,7.25
```
**File 2 — `HocVien_Loi.csv`** (cố tình 2 lỗi để demo báo cáo lỗi):
```
Mã học viên,Tên học viên,Ngày sinh,Điện thoại,Điểm
DEMO03,Lê Văn Demo,2000-01-01,0905556666,abc
DEMO04,,2001-02-02,0907778888,9
```
*(lỗi 1: "abc" không phải số; lỗi 2: thiếu tên học viên)*

**File 3 — `DangKy_DEMO.csv`**:
```
Mã học viên,Mã lớp,Ngày đăng ký
DEMO01,L01,2025-01-20
DEMO02,L01,2025-01-20
```
**File 4 — `GiaoVien_OK.csv`** (tùy chọn):
```
Mã giáo viên,Tên giáo viên,Ngày sinh,Giới tính,Điện thoại
DEMOGV01,Trần Demo Giáo Viên,1990-04-04,Nam,0911223344
```

**File 5 — `MauNhap_HocVien.xlsx`**: tạo sẵn bằng nút "Tải tệp mẫu" trong app (để demo nút này) — hoặc tạo trước để tiết kiệm thời gian.

---

## 1. KỊCH BẢN CHÍNH (15 phút) — theo 1 câu chuyện xuyên suốt

> Câu chuyện: **Admin mở khóa khóa học mới → học viên tự đăng ký → hệ thống xếp lịch → nhập thêm học viên hàng loạt cho lớp còn thiếu người.**

### Bước 1 — Đăng nhập & quan sát menu theo quyền (1,5 phút)
1. Mở app ⇒ đăng nhập `admin / Admin@123`.
2. **Chỉ vào**: menu được dựng lại theo quyền (`frmMain.ApDungQuyenMenu`), các nút Thêm/Sửa/Xoá trong từng form bị ẩn theo `LOPHOC_THEM/SUA/XOA`…
3. Nói 1 câu: *"Menu không cố định — nó dựng lại theo `PhanQuyen` của vai trò đang đăng nhập."*

### Bước 2 — Tạo môn học (1 phút)
1. Quản lý → Môn học → **Thêm**.
2. Nhập mã/tên/học phí → Lưu. (CRUD đã kiểm chứng; lỗi trùng mã và xoá môn đang dùng đều có thông báo.)
3. *(Tuỳ chọn)* Bấm **Xoá** ngay sau khi thêm để demo thông báo "Môn học này đang được sử dụng…" — **chỉ làm nếu đã gán môn đó cho lớp**.

### Bước 3 — Tạo lớp học (2 phút)
1. Quản lý → Lớp học → **Thêm**: mã `DEMO01`, tên "Lớp demo C#", Môn (vừa tạo), Giáo viên `GV01`, Ca `CA01`, Thời gian `TG02`, Phòng `PH02`, Ngày bắt đầu `2025-02-01`, Ngày kết thúc `2025-06-30`, Sĩ số tối đa `1`, Số buổi/tuần `3`.
2. **Cố tình bỏ trống Ngày kết thúc rồi Lưu** để demo validation ("Ngày kết thúc không được trước ngày bắt đầu") — rồi sửa lại cho đúng.
3. Nói: *"Validation nằm ở `LopHocService.KiemTra`, còn `LopHocRepository.TonTai` chặn trùng mã; tầng DB có thêm PK/FK."*

### Bước 4 — Học viên tự đăng ký (2 phút) ⭐ phần "wow"
1. **Đăng xuất**, đăng nhập `hv01 / Demo@123` ⇒ chỉ thấy menu phù hợp (không có Quản lý/Phân quyền).
2. Quản lý → Đăng ký lớp ⇒ danh sách lớp có sẵn với cờ "đã đăng ký" ⇒ chọn lớp **DEMO01** → Đăng ký ⇒ thông báo thành công.
3. **Cố đăng ký lại** cùng lớp đó ⇒ *"Bạn đã đăng ký lớp này rồi."* (do `PK_DangKy` + bắt lỗi 2627).
4. *Thêm tuỳ chọn:* đăng ký thêm vào lớp `L01` (đã có 5/30) — không bị chặn.
5. Nói: *"Sức chứa được kiểm tra trong transaction với khóa dòng `UPDLOCK, HOLDLOCK`, nên hai máy cùng đăng ký vẫn không vượt số."*

### Bước 5 — Xếp thời khóa biểu (2,5 phút) ⭐ điểm mạnh
1. Đăng lại `admin` ⇒ Tra cứu → Thời khóa biểu ⇒ bấm **Tạo thời khóa biểu**.
2. Nếu đã có lịch ⇒ hộp thoại xác nhận "Thời khóa biểu hiện tại sẽ được thay thế" ⇒ **Yes**.
3. Chờ (≤ vài giây) ⇒ thông báo "Đã xếp N buổi học cho M lớp" ⇒ lịch hiển thị theo tuần.
4. **Chỉ vào**: lịch của GV/HV được lọc theo vai trò (đăng nhập `gv01` ⇒ chỉ thấy lịch dạy của mình; `hv01` ⇒ chỉ lịch lớp mình đăng ký).
5. Nói: *"Bài toán là 0-1 ILP: biến = buổi học (thứ, ca, phòng); ràng buộc cứng = mỗi lớp 1 buổi/ngày, 1 GV / 1 phòng mỗi khung giờ, 1 học viên không ở 2 lớp; ràng buộc mềm = ưu tiên thứ/ca/phòng, tránh 2 buổi liền ngày."*

### Bước 6 — Bulk Import (3,5 phút) ⭐ module mới nhất
1. Quản lý → **Nhập dữ liệu hàng loạt**.
2. Chọn loại **Học viên** ⇒ bấm **Tải tệp mẫu** ⇒ mở file mẫu để giảng viên xem (tiêu đề chuẩn + sheet hướng dẫn).
3. **Kéo thả** `HocVien_Loi.csv` vào cửa sổ ⇒ preview tự chạy ⇒ 2 dòng đỏ ⇒ **nút Xác nhận nhập bị khoá** ⇒ bấm **Xuất báo cáo lỗi** ⇒ file `.xlsx`/`.csv` mở ra, thấy dòng/cột/lỗi.
4. Kéo thả `HocVien_OK.csv` ⇒ preview xanh ⇒ **Xác nhận nhập** ⇒ thông báo "Đã nhập thành công 2 dòng".
5. *Tuỳ chọn:* kéo thả `DangKy_DEMO.csv` (loại **Đăng ký lớp**) ⇒ import 2 dòng ⇒ sĩ số lớp `DEMO01` tăng 1→3. (Nhớ: lớp demo có `SiSoToiDa = 1` ⇒ **phải nâng lên ≥ 3 trước**, nếu không sẽ bị chặn — đây là cũng là một điểm demo hay: *"lớp chỉ còn 1 chỗ, tệp có 2 dòng ⇒ cả 2 dòng đều bị chặn, không có dòng nào lọt lẹt"*.)
6. Nói: *"Toàn bộ tệp là một transaction — all-or-nothing; trước khi ghi còn kiểm tra lại ràng buộc trong transaction vì dữ liệu có thể đã đổi sau lúc preview."*
7. *(Điểm demo quyền)*: nếu còn thời gian, đăng nhập `hv01` để chỉ ra **không có mục "Nhập dữ liệu hàng loạt"** trong menu — nhập hàng loạt là quản trị dữ liệu, học viên chỉ tự đăng ký lớp.

### Bước 7 — Trung tâm tự động hóa (1,5 phút)
1. Vẫn đang đăng nhập `admin` ⇒ nhóm **Hệ thống** ⇒ bấm **Tự động hóa** ⇒ mở `FrmAutomationDashboard` (thống kê rule/lần chạy, tab "Lần chạy gần đây", "Nhiệm vụ đang chờ", "Thông báo mới").
2. Bấm **Quản lý quy tắc** ⇒ `FrmRuleManagement` ⇒ **Thêm rule**: tên `DEMO_RULE`, trigger `Thay đổi số học viên đăng ký` (CLASS_ENROLLMENT_CHANGED), thêm điều kiện *Số học viên hiện tại > 0*, thêm hành động `Ghi nhật ký` (WRITE_AUDIT_LOG) với nội dung "demo".
3. Bấm **Test Rule** ⇒ hiện kết quả đánh giá từng điều kiện mà **không** chạy hành động.
4. Nói: *"Rule là dữ liệu trong DB, không phải code; biểu thức do hệ thống tự sinh từ field + toán tử + giá trị trong whitelist nên Admin không thể nhập C#/SQL tuỳ ý."*

### Bước 8 — Phân quyền (1,5 phút)
1. Hệ thống → Phân quyền ⇒ bỏ tick một quyền của vai trò Học viên (ví dụ `TIM_LOP`) ⇒ Lưu.
2. Đăng xuất, đăng nhập lại `hv01` ⇒ menu Tra cứu bị mất mục tương ứng **và không thấy mục "Nhập dữ liệu hàng loạt"** (nhập hàng loạt là chức năng quản trị dữ liệu, học viên chỉ tự đăng ký lớp).
3. Nói: *"Ba lớp kiểm soát: UI ẩn/hiện, `PermissionService.YeuCau` ở Service/Repository, và lọc phạm vi dữ liệu trong SQL."*

### Bước 9 — Tổng kết (30 giây)
Nêu 3 điểm mạnh (xếp lịch CP-SAT, bulk import all-or-nothing, phân quyền 3 tầng) + 2 hạn chế đã nói trước (UI Học viên/Giáo viên/Báo cáo; 2 trigger Automation chưa có nơi phát).

---

## 2. PHẦN CÓ THỂ DEMO THÊM (nếu còn thời gian)

| Phần | Cách demo | Lưu ý |
|---|---|---|
| Xuất Excel | Mở form Bất kỳ có dữ liệu → nút **Xuất Excel** trên `frmMain` | Cần **Microsoft Excel** trên máy; COM Interop |
| Tìm kiếm lớp | Tra cứu → Tìm lớp học, lọc theo môn/GV/sĩ số | GV có quyền "chỉ của mình" sẽ thấy phạm vi hẹp |
| Tìm giáo viên | Tra cứu → Tìm giáo viên, lọc theo trình độ/chuyên môn/môn | |
| Module Automation | **Có demo được**: Hệ thống → Tự động hóa (Admin) → thống kê, lần chạy gần đây, nhiệm vụ, thông báo → nút "Quản lý quy tắc" → tạo 1 rule mẫu + bấm **Test Rule** (chỉ đánh giá, không chạy action) | Nói rõ: rule mới chưa có dữ liệu thực tế nên số lần chạy sẽ vẫn là 0 cho tới khi có sự kiện phát |

---

## 3. THAO TÁC CẦN TRÁNH (rủi ro cao khi demo)

| ❌ Không nên | Lý do |
|---|---|
| Bấm các nút **Học viên / Giáo viên / Danh mục khác / Báo cáo** | Chúng **không có handler** ⇒ bấm không mở gì, giảng viên tưởng phần mềm treo/lỗi |
| Bấm **Tạo thời khóa biểu** nhiều lần liên tiếp | Mỗi lần **ghi đè toàn bộ** `BuoiHoc` (xóa rồi insert) |
| **Xoá lớp** đang có học viên | Bị DB chặn (FK) ⇒ thông báo lỗi đúng nhưng trông như lỗi phần mềm |
| **Xoá môn học** đang được lớp dùng | Tương tự |
| Import tệp lớn (>5.000 dòng) trong lúc demo | INSERT từng dòng ⇒ chậm, có thể giật |
| Sửa trực tiếp dữ liệu trong DB trong lúc demo | Dễ lệch với dữ liệu đang hiển thị trên UI |
| Đăng nhập `admin` rồi để màn hình đăng nhập mở lâu | Trông như treo |
| Xoá tài khoản `admin` | `UX_TaiKhoan_Admin` + `CK_TaiKhoan_HoSo` ⇒ không tạo được Admin thứ hai |

---

## 4. PHƯƠNG ÁN DỰ PHÒNG (khi demo hỏng)

| Sự cố | Dấu hiệu | Xử lý ngay tại chỗ |
|---|---|---|
| Không kết nối được DB | Báo lỗi SQL khi mở form | Kiểm tra SQL Express có chạy; **bỏ qua phần DB**, chuyển sang trình bày bằng slide/ảnh chụp + giải thích code |
| Máy không có Excel | Nút Xuất Excel báo lỗi COM | Bỏ phần này, nói "em xuất bằng Excel Interop nên cần Excel" |
| Xếp lịch lâu > 20 giây | Form bị "đơ" | Báo "solver giới hạn 20 giây rồi trả kết quả tốt nhất tìm được"; đợi và xem thông báo |
| Import báo "không nhận được tệp" | Sai mã hoá CSV | Lưu CSV bằng **UTF-8 có BOM** (Notepad: Save as → UTF-8 with BOM) hoặc dùng file `.xlsx` |
| File CSV tiếng Việt bị lỗi dấu | Header đọc sai | Đổi tên cột sang **không dấu** (vd `MaHocVien, TenHocVien`) — hệ thống nhận diện được cả hai |
| Preview báo thiếu cột bắt buộc | Sai tên cột | Dùng đúng tên trong tệp mẫu |
| Form Automation mở ra nhưng trống | Chưa có rule/execution nào | Bấm "Quản lý quy tắc" → tạo 1 rule mẫu, bấm **Test Rule** để minh hoạ cơ chế đánh giá điều kiện |
| Demo xong nhưng DB bị thay đổi nhiều | — | Chạy lại `Database\ThoiKhoaBieu.sql` + `Database\PhanQuyen.sql` (idempotent) để chuẩn hoá dữ liệu; xoá các mã `DEMO%` bằng SQL |

---

## 5. CHECKLIST TRƯỚC KHI VÀO PHÒNG BẢO VỆ

- [ ] SQL Server chạy, dữ liệu mẫu đủ (LopHoc ≥ 3, HocVien ≥ 9, BuoiHoc ≥ 51).
- [ ] Build bằng MSBuild của Visual Studio, không lỗi.
- [ ] App chạy được, đăng nhập `admin/Admin@123` thử 1 lần.
- [ ] 3 file CSV demo đặt ở `C:\BTL_Demo\`, mở sẵn để không mất thời gian.
- [ ] Tệp mẫu `.xlsx` có sẵn.
- [ ] Có ảnh chụp dự phòng: màn hình thời khóa biểu, màn hình preview Bulk Import, bảng `AutomationRule`.
- [ ] Điện thoại/sạc dự phòng; trình chiếu đã mở sẵn file `docs/BaoVeDoAn/` để trả lời khi cần tra cứu.
- [ ] Nhớ 3 hạn chế phải nói trước: UI Học viên/Giáo Viên/Danh mục/Báo cáo; 2 trigger Automation chưa có nơi phát; xếp lịch chưa có Preview và ghi đè toàn bộ lịch cũ.
