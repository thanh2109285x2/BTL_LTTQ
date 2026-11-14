# 00 — TỔNG QUAN, PHẠM VI ĐÃ LÀM & BẢNG TRẠNG THÁI TÍNH NĂNG

> Tài liệu chuẩn bị bảo vệ đồ án **Quản lý lớp học ngắn hạn (Tin học & Ngoại ngữ)**.
> Mọi khẳng định trong tài liệu này đều dẫn được về file/class/method/bảng cụ thể.
> Nguyên tắc: **không tính là "đã xong" nếu chỉ có tên file hoặc UI mà chưa nối với SQL Server**.

---

## 1. Tổng quan nghiệp vụ

### 1.1 Bài toán giải quyết
Quản lý vòng đời một lớp học ngắn hạn: **tạo lớp → gán môn/giáo viên/ca/thời gian/phòng → học viên đăng ký → xếp thời khóa biểu → theo dõi sĩ số và lịch dạy**.

### 1.2 Đối tượng sử dụng (3 vai trò, đã có dữ liệu thật trong DB)

| Vai trò | Tài khoản mẫu | Mã liên kết | Nguồn |
|---|---|---|---|
| Admin | `admin` | — | bảng `TaiKhoan` (3 dòng) |
| Giáo viên | `gv01` | `MaGV = GV01` | bảng `TaiKhoan` |
| Học viên | `hv01` | `MaHocVien = HV01` | bảng `TaiKhoan` |

Ràng buộc hồ sơ tài khoản do chính DB kiểm tra:
`CK_TaiKhoan_HoSo` — Admin không có `MaGV/MaHocVien`; Giáo viên bắt buộc có `MaGV`; Học viên bắt buộc có `MaHocVien`.
`UX_TaiKhoan_Admin` — **tối đa 1 Admin**.

### 1.3 Nghiệp vụ chính đã hiện thực
1. Đăng nhập → nạp quyền → ẩn/hiện menu theo quyền.
2. Quản lý **môn học** (CRUD đầy đủ, có export Excel).
3. Quản lý **lớp học** (CRUD, có phạm vi dữ liệu "chỉ của mình" cho giáo viên).
4. **Đăng ký / hủy đăng ký lớp** cho học viên, có chặn vượt sĩ số bằng transaction + khóa dòng.
5. **Xếp thời khóa biểu tự động** bằng Google OR-Tools CP-SAT.
6. Tra cứu lớp học / giáo viên, xuất Excel.
7. **Phân quyền theo vai trò** (bảng `PhanQuyen`, 54 dòng, 32 mã chức năng).
8. **Nhập hàng loạt** (Bulk Import) Học viên / Giáo viên / Đăng ký lớp từ `.xlsx`/`.csv`.
9. **Workflow Automation** (Rule → Condition → Action): đã có đầy đủ code, DB **và đã gắn vào menu** ("Hệ thống → Tự động hóa", quyền `AUTOMATION_XEM`/`AUTOMATION_QUANLY`).

### 1.4 Phạm vi hiện tại & giới hạn đã biết
- **Không có UI quản lý Học viên / Giáo viên / Danh mục khác / Báo cáo** — các nút `btnHocVien`, `btnGiaoVien`, `btnDanhMucKhac`, `btnBaoCaoDiem`, `btnDoanhThu`, `btnTop3`, `btnLopTheoPhong` có trong `frmMain.Designer.cs` nhưng **không được nối sự kiện `Click`** → bấm vào không mở form nào (bằng chứng: grep `\.Click \+=` trong `Forms/frmMain.Designer.cs` không có các nút này).
- Dữ liệu Học viên/Giáo viên vẫn **đọc/ghi được** ở tầng DB và qua Bulk Import; chỉ thiếu màn hình CRUD.
- Xếp lịch: **ghi đè toàn bộ** `BuoiHoc` khi bấm "Tạo thời khóa biểu" (`ThoiKhoaBieuRepository.LuuLich` → `DELETE FROM BuoiHoc` rồi INSERT trong 1 transaction).
- Automation: `BULK_IMPORT_COMPLETED` **không tạo được Rule** vì CHECK constraint DB chỉ cho 3 trigger lớp.

---

## 2. BẢNG TRẠNG THÁI TÍNH NĂNG (kèm bằng chứng)

> Quy ước: **Completed** = đã triển khai + kiểm chứng được; **Partially** = có một phần, thiếu phần khác; **Designed** = có model/DB nhưng chưa nối nghiệp vụ; **Not Implemented** = chưa có code.

| # | Tính năng | Trạng thái | Bằng chứng cụ thể |
|---|---|---|---|
| 1 | Đăng nhập / mật khẩu PBKDF2 | **Completed** | `Forms/HeThong/frmDangNhap.cs:36` gọi `TaiKhoanDAL.LayTheoTenDangNhap`, `:47` gọi `MatKhauHelper.KiemTra`; `Common/MatKhauHelper.cs` dùng `Rfc2898DeriveBytes.Pbkdf2(..., 600000, SHA256, 32)` + salt 16 byte + `CryptographicOperations.FixedTimeEquals` |
| 2 | Phân quyền theo vai trò | **Completed** | `Common/PermissionService.cs` (`Can`, `YeuCau`, `ChiCuaMinh`), `DataAccessLayer/PhanQuyenDAL.cs`, `Forms/HeThong/frmPhanQuyen.cs`; DB: bảng `PhanQuyen` (54 dòng), `ChucNang` (32 dòng) |
| 3 | Menu động theo quyền | **Completed** | `Forms/frmMain.cs:67` `SapXepMenu(panelCRUD, ...)`, `:112` `SapXepMenu`, `:132` `TogglePanel` |
| 4 | Quản lý môn học (CRUD) | **Completed** | `Forms/QuanLy/frmMonHoc.cs` → `Services/MonHocService.cs` (`Them/Sua/Xoa`) → `Repositories/MonHocRepository.cs` (`Them/Sua/Xoa/TonTai`) |
| 5 | Quản lý lớp học (CRUD) | **Completed** | `Forms/QuanLy/frmLopHoc.cs` → `Services/LopHocService.cs:18/38/56` → `Repositories/LopHocRepository.cs:52/64/91` |
| 6 | Phạm vi dữ liệu "chỉ của mình" | **Completed** | `Common/PhamViDuLieu.cs:9 ChoLopHoc` sinh điều kiện SQL `l.MaGV = @PV_MaGV` / `EXISTS(... DangKy ...)`; áp tại `LopHocRepository.cs:15,73` |
| 7 | Đăng ký / hủy đăng ký lớp | **Completed** | `Forms/QuanLy/frmDangKyLop.cs:112` → `Repositories/DangKyRepository.cs:60 Them` / `:120 Huy` |
| 8 | Chặn vượt sĩ số khi đăng ký | **Completed** | `DangKyRepository.cs:64-98`: `ExecuteTransaction` + `SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK)` + đếm `COUNT(*) FROM DangKy` |
| 9 | Xếp thời khóa biểu tự động | **Completed** | `Services/XepLichService.cs:26/34` (CP-SAT), `Repositories/ThoiKhoaBieuRepository.cs:13 LayDuLieuXepLich` / `:108 LuuLich`; DB có 51 dòng `BuoiHoc` |
| 10 | Xem TKB theo vai trò | **Completed** | `frmThoiKhoaBieu.cs:65-66` truyền `MaGV`/`MaHocVien`; `ThoiKhoaBieuRepository.cs:143 LayLich` lọc bằng `EXISTS (SELECT 1 FROM DangKy ...)` |
| 11 | Tra cứu lớp / giáo viên | **Completed** | `Forms/TraCuu/frmTimKiemLopHoc.cs`, `frmTimKiemGiaoVien.cs` → `Services/LopHocTraCuuService.cs`, `GiaoVienTraCuuService.cs` → `Repositories/*TraCuuRepository.cs` |
| 12 | Xuất Excel | **Completed** | `Common/ExcelHelper.cs:22 XuatTrangHienTai`, `:72 XuatDataGridView` (COM Interop) |
| 13 | Nhập hàng loạt (Bulk Import) | **Completed** | `Forms/Import/FrmBulkImport.cs` → `Services/Import/ImportService.cs` (`XemTruoc`/`ThucHien`) → `Services/Import/BangTinhReader.cs`, `ImportValidator.cs` → `Repositories/Import/ImportLookupRepository.cs` |
| 14 | Tạo tệp mẫu / báo cáo lỗi | **Completed** | `Services/Import/ImportTemplateWriter.cs`, `Services/Import/ImportErrorReportWriter.cs` (ClosedXML + CsvHelper) |
| 15 | Workflow Automation — tầng dữ liệu & rule | **Completed** | `Database/Automation.sql` (7 bảng), `Repositories/Automation/*.cs` (7 file), `Common/AutomationConst.cs` |
| 16 | Workflow Automation — đánh giá điều kiện | **Completed** | `Services/Automation/RuleEvaluator.cs:21 DanhGiaAsync` gọi `engine.ExecuteAllRulesAsync(tenWorkflow, new RuleParameter("ctx", context))`; `RuleDefinitionBuilder.XayDung` sinh Workflow; `RulesEngineProvider` cache engine |
| 17 | Workflow Automation — thực thi Action + lịch sử | **Completed** | `Services/Automation/AutomationEventPublisher.cs` (dedup SHA256, cooldown, `AutomationExecution`, `AutomationExecutionLog`), 4 handler trong `Services/Automation/*Handler.cs` |
| 18 | Automation — màn hình quản lý Rule | **Completed** | `Forms/Automation/FrmRuleManagement.cs`, `FrmRuleEditor.cs` (có nút `BtnTestRule_Click` dùng `RuleEvaluator` — chạy thử không thực thi action) — **đã gắn vào menu**: `frmMain.Designer.cs` có `btnTuDongHoa` trong `panelHeThong`, `frmMain.cs:btnTuDongHoa_Click` mở `FrmAutomationDashboard`; Dashboard có nút "Quản lý quy tắc" mở `FrmRuleManagement` |
| 19 | Automation — phát trigger từ nghiệp vụ lớp | **Partially** | Chỉ `FrmBulkImport.cs:314/331` phát trigger. `CLASS_CREATED` và `CLASS_STATUS_CHANGED` **không có nơi phát** trong toàn bộ source → Rule của 2 trigger này không bao giờ chạy |
| 20 | Trigger `BULK_IMPORT_COMPLETED` | **Designed** | Định nghĩa ở `Common/AutomationConst.cs` (`Trigger.BulkImportCompleted`) và được phát ở `FrmBulkImport.cs:331`, nhưng `CK_AutomationRule_TriggerType` chỉ cho `CLASS_CREATED/CLASS_ENROLLMENT_CHANGED/CLASS_STATUS_CHANGED` → **không tạo được Rule**; publisher sẽ no-op |
| 21 | Lịch sử nhập hàng loạt (Import History) | **Not Implemented (có chủ ý)** | Không có bảng `ImportBatch*`; chỉ dùng `KetQuaPreview.OperationId` (Guid trong bộ nhớ, `Models/Import/KetQuaPreview.cs:13`) |
| 22 | UI Quản lý Học viên | **Not Implemented** | Không tồn tại `frmHocVien`; nút `btnHocVien` không có `Click` handler. Dữ liệu vẫn dùng được qua Bulk Import (`ImportService.ThucHien` → `GhiHocVien`) |
| 23 | UI Quản lý Giáo viên | **Not Implemented** | Không tồn tại `frmGiaoVien`; nút `btnGiaoVien` không có `Click` handler. `LookupRepository.LayGiaoVien()` chỉ cấp danh sách chọn |
| 24 | UI Danh mục khác (nghề, xếp loại, chức danh…) | **Not Implemented** | Nút `btnDanhMucKhac` không có `Click` handler; `Repositories/DanhMucKhacRepository.cs` tồn tại nhưng tham chiếu ở UI rất hạn chế |
| 25 | UI Báo cáo (điểm, doanh thu, xếp hạng, lớp theo phòng) | **Not Implemented** | 4 nút báo cáo không có `Click` handler |
| 26 | Giao diện nhập hàng loạt: kéo thả tệp | **Completed** | `Forms/Import/FrmBulkImport.Designer.cs` (`AllowDrop = true`, nối `DragEnter/DragDrop/DragLeave`), `FrmBulkImport.cs` (`LayTepTha` lọc `.xlsx/.csv`) |
| 27 | Tự động hóa "mềm" (Soft constraint) trong xếp lịch | **Completed** | `XepLichService.cs:18-22` hệ số phạt; `:186 model.Minimize(LinearExpr.WeightedSum(chiPhi, trongSo))` |
| 28 | Chống trùng lịch ở tầng DB | **Completed** | Index unique `UX_BuoiHoc_GiaoVien`, `UX_BuoiHoc_Phong` (kiểm chứng từ `sys.indexes`) |
| 29 | Test tự động | **Partially** | Harness kiểm thử nằm ngoài repo (`%TEMP%\opencode\BulkImportTest`), 51 assertion cho Bulk Import + Automation + kéo thả; **chưa có unit test framework trong project** (`BTL_LTTQ.csproj` không tham chiếu xUnit/NUnit/MSTest) |

---

## 3. SƠ ĐỒ KIẾN TRÚC (đúng với code hiện tại)

```
┌──────────────────────── PRESENTATION (Forms) ────────────────────────┐
│ frmDangNhap → frmMain (menu động theo quyền)                        │
│   Quản lý : frmLopHoc, frmMonHoc, frmDangKyLop                       │
│   Tra cứu : frmTimKiemLopHoc, frmTimKiemGiaoVien, frmThoiKhoaBieu   │
│   Hệ thống: frmPhanQuyen, frmDangKy                                │
│   Import  : FrmBulkImport                                           │
│   Automat.: FrmRuleManagement, FrmRuleEditor, FrmAutomationDashboard  │
│              FrmAutomationExecutionDetail, FrmAutomationTaskDetail,   │
│              FrmDieuKienThem, FrmHanhDongThem                        │
│   ► KHÔNG có SQL trong Form (trừ SQL động của PhamViDuLieu trả về)  │
└───────────────────────────────┬──────────────────────────────────────┘
                                │ DataTable / Model
┌───────────────────────────────▼──────────────────────────────────────┐
│ BUSINESS (Services)                                                  │
│ LopHocService, MonHocService, LookupService,                         │
│ LopHocTraCuuService, GiaoVienTraCuuService,                          │
│ XepLichService (CP-SAT),                                            │
│ Services/Import/{ImportService, ImportValidator, BangTinhReader,    │
│                   ImportTemplateWriter, ImportErrorReportWriter}     │
│ Services/Automation/{AutomationEventPublisher, RuleEvaluator,        │
│                       RuleDefinitionBuilder, RulesEngineProvider,     │
│                       ConditionEvaluator, AutomationFieldCatalog,     │
│                       AutomationContextBuilder, ActionHandlerRegistry} │
└───────────────────────────────┬──────────────────────────────────────┘
                                │ Model / (conn, tran)
┌───────────────────────────────▼──────────────────────────────────────┐
│ DATA ACCESS (Repositories)                                           │
│ LopHocRepository, MonHocRepository, DangKyRepository,                 │
│ LookupRepository, DanhMucKhacRepository,                              │
│ LopHocTraCuuRepository, GiaoVienTraCuuRepository,                     │
│ ThoiKhoaBieuRepository, Repositories/Import/ImportLookupRepository,   │
│ Repositories/Automation/* (7 file)                                   │
└───────────────────────────────┬──────────────────────────────────────┘
                                │ SQL tham số hóa
┌───────────────────────────────▼──────────────────────────────────────┐
│ DATA ACCESS LAYER                                                    │
│ DbHelper (ExecuteQuery/ExecuteNonQuery/ExecuteScalar/                │
│           ExecuteTransaction/TestConnection/ConnectionString)         │
│ TaiKhoanDAL, PhanQuyenDAL                                           │
└───────────────────────────────┬──────────────────────────────────────┘
                                │
┌───────────────────────────────▼──────────────────────────────────────┐
│ SQL Server — DB: QuanLyLopTinHocVaNgoaiNguNganHan (localhost\SQLEXPRESS) │
│ 29 bảng: nghiệp vụ (LopHoc, HocVien, GiaoVien, DangKy, BuoiHoc…)   │
│           + phân quyền (TaiKhoan, PhanQuyen, ChucNang)              │
│           + Automation (AutomationRule/Condition/Action/Execution/…)  │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 4. BA LUỒNG DỮ LIỆU THẬT (điều giảng viên có thể yêu cầu vẽ lại)

### 4.1 Admin thêm một lớp học
```
frmLopHoc.btnThem_Click (kiểm tra PermissionService.Can(LOPHOC_THEM))
  → LopHocService.Them(LopHoc)            // Services/LopHocService.cs:18
      ├─ ChuanHoa(l)                      // trim mã/tên            :75
      ├─ KiemTra(l)                       // validate nghiệp vụ      :81
      ├─ LopHocRepository.TonTai(MaLop)   // trùng mã               :44
      └─ LopHocRepository.Them(l)         // PermissionService.YeuCau(LOPHOC_THEM) + INSERT
          → DbHelper.ExecuteNonQuery(sql, SqlParameter[])   // DataAccessLayer/DbHelper.cs:42
          → SQL Server
  ← chuỗi lỗi "" (rỗng = thành công) hoặc thông báo lỗi
```

### 4.2 Học viên đăng ký lớp
```
frmDangKyLop.btnDangKy_Click
  → DangKyRepository.Them(MaHocVien, MaLop)      // Repositories/DangKyRepository.cs:60
      → DbHelper.ExecuteTransaction((conn, tran) => {              // DbHelper.cs:53
            SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK) // khóa dòng lớp
            SELECT COUNT(*) FROM DangKy WHERE MaLop = @MaLop      // sĩ số hiện tại
            if (siSo >= siSoToiDa) throw InvalidOperationException(...)
            INSERT INTO DangKy (MaHocVien, MaLop) VALUES (...)    // tham số hóa
        })
      → COMMIT hoặc ROLLBACK tự động trong DbHelper.ExecuteTransaction
  ← "" hoặc "Lớp đã đủ sĩ số (x/y)." / "Bạn đã đăng ký lớp này rồi." (SqlException 2627/2601)
```

### 4.3 Nhập hàng loạt đăng ký lớp
```
FrmBulkImport.btnXemTruoc_Click → ImportService.XemTruoc(path, ImportLoai.DangKy)
  → BangTinhReader.Doc(path, loai)                 // đọc .xlsx/.csv → List<ImportDong>
  → ImportLookupRepository.Nap()                    // nạp mã tham chiếu + sĩ số lớp
  → ImportValidator.KiemTra(preview)               // báo lỗi theo dòng

FrmBulkImport.btnXacNhan_Click → ImportService.ThucHien(preview)
  → PermissionService.YeuCau(DANGKYLOP)            // ImportService.cs:155
  → DbHelper.ExecuteTransaction(...)               // MỌI dòng trong 1 transaction
      ├─ KiemTraLai(conn, tran, preview, loi)      // kiểm tra lại trước khi ghi
      │    └─ ImportLookupRepository.DocLopDangKhoa(conn, tran, maLop)  // UPDLOCK, HOLDLOCK
      └─ GhiDangKy(conn, tran, dongList)           // INSERT tham số hóa từng dòng
  → COMMIT (thành công) hoặc ROLLBACK (bất kỳ lỗi)
  → FrmBulkImport.PhatSuKienSauCommitAsync(ketQua) // phát Automation SAU COMMIT
```

---

## 5. ĐIỂM MẠNH VÀ ĐIỂM YẾU ĐÃ BIẾT (tự nhìn nhận trước khi giảng viên nói)

**Mạnh**
- Mọi SQL đều tham số hóa (`SqlParameter`), không nội suất chuỗi từ dữ liệu người dùng.
- Ràng buộc nghiệp vụ được bảo vệ ở **3 nơi**: UI (ẩn/hiện nút), Service/Repository (`PermissionService.YeuCau` — lớp bảo vệ cuối), Database (CHECK/FK/unique index).
- Có transaction thật ở nơi cần tính toàn vẹn: đăng ký lớp, xếp lịch, nhập hàng loạt, phân quyền.
- Nguyên tắc all-or-nothing của Bulk Import đã được kiểm chứng bằng test (lỗi ở dòng 2 → dòng 1 cũng không được ghi).

**Yếu (phải nói trước, không đợi giảng viên hỏi)**
- Chưa có unit test trong project; kiểm thử hiện là harness console ngoài repo (64 assertion cho Bulk Import + Automation + kéo thả + phân quyền menu).
- **4 nhóm chức năng (Học viên, Giáo viên, Danh mục khác, Báo cáo) có nút menu nhưng chưa có form** — bấm vào không mở gì.
- **Chỉ `CLASS_ENROLLMENT_CHANGED` có nơi phát sự kiện**; `CLASS_CREATED` và `CLASS_STATUS_CHANGED` chưa được phát ⇒ rule của 2 trigger đó không bao giờ chạy.
- Trigger `BULK_IMPORT_COMPLETED` khai báo trong code nhưng DB chưa cho phép tạo Rule (CHECK constraint) ⇒ publisher no-op.
- Xếp lịch ghi đè toàn bộ lịch cũ, chưa có bước "preview lịch trước khi lưu" (khác Bulk Import).
- Cột `HocVien.MaLop` còn tồn tại song song với bảng trung gian `DangKy` → hai nguồn sự thật về "học viên thuộc lớp nào".

---

## 6. BẢN ĐỒ TỚI CÁC TÀI LIỆU LIÊN QUAN

| File | Nội dung |
|---|---|
| `01-KIEN-TRUC-VA-ADO-NET.md` | Phân tầng, ADO.NET, transaction, exception handling, bảo mật |
| `02-DATABASE.md` | Schema thật, khóa, index, constraint, câu hỏi phản biện về DB |
| `03-NGHIEP-VU.md` | Từng module nghiệp vụ + Q&A riêng |
| `04-SMART-TIMETABLE.md` | Xếp lịch CP-SAT: biến, ràng buộc, hàm mục tiêu, giới hạn |
| `05-AUTOMATION-RULESENGINE.md` | Rule/Condition/Action, Microsoft RulesEngine, Execution History |
| `06-BULK-IMPORT.md` | Pipeline nhập hàng loạt, all-or-nothing, quan hệ với Automation |
| `07-CAU-HOI-PHAN-BIEN.md` | Ngân hàng câu hỏi A–G kèm 3 mức trả lời |
| `08-KICH-BAN-DEMO.md` | Kịch bản demo + dữ liệu chuẩn bị + phương án dự phòng |
| `09-MOCK-DEFENSE.md` | Quy trình luyện bảo vệ 8 vòng + rubric chấm điểm |
