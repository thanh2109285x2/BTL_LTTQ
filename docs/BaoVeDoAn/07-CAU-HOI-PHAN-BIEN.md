# 07 — NGÂN HÀNG CÂU HỎI PHẢN BIỆN (3 mức trả lời)

> Cách dùng: **Mức 1** trả lời trong 15–30 giây; **Mức 2** trong 45–90 giây; **Mức 3** khi giảng viên đào sâu.
> File chi tiết theo module: `03-NGHIEP-VU.md`, `04-SMART-TIMETABLE.md`, `05-AUTOMATION-RULESENGINE.md`, `06-BULK-IMPORT.md`.

---

## NHÓM A — TỔNG QUAN

### A1. Đề tài giải quyết vấn đề gì?
- **Mức 1**: Quản lý lớp học ngắn hạn (Tin học & Ngoại ngữ): tạo lớp, gán giáo viên/môn/phòng/ca, cho học viên đăng ký, xếp thời khóa biểu tự động, và tự động hoá một số quy tắc nghiệp vụ.
- **Mức 2**: Bài toán gốc là "trung tâm đào tạo" dùng Excel + nhắc nhở miệng nên vi phạm trùng lịch GV/phòng, vượt sĩ số, và không kiểm soát được "lớp nào sắp khai giảng mà chưa đủ học viên". Ứng dụng gom về 1 CSDL với ràng buộc được DB ép buộc, và có 2 module nâng cao: xếp lịch tự động (CP-SAT) và nhập hàng loạt.
- **Mức 3**: Phạm vi nghiệp vụ em hiện có trong DB: `LopHoc` (3), `HocVien` (9), `GiaoVien` (3), `DangKy` (10), `BuoiHoc` (51), `TaiKhoan` (3), `PhanQuyen` (54), `ChucNang` (32). Ba module "đắt giá" nhất là: (1) xếp lịch tự động — giải bài toán trùng lịch GV/phòng/học viên; (2) phân quyền 3 tầng (UI → Service/Repo → SQL filter); (3) Bulk Import all-or-nothing.

### A2. Đối tượng sử dụng là ai?
- **Mức 1**: 3 vai trò: Admin (quản trị toàn bộ), Giáo viên (xem lịch dạy của mình, quản lý lớp của mình), Học viên (xem lịch học của mình, tự đăng ký lớp).
- **Mức 2**: Khác biệt thể hiện bằng `CurrentUser` (TenDangNhap, VaiTro, MaGV, MaHocVien) và `PermissionService`. Quyền lưu ở bảng `PhanQuyen(MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)` — 32 mã chức năng, 54 dòng cấp quyền. Ngoài quyền "xem/thêm/sửa/xoá", có cờ **"chỉ của mình"** (`ChiCuaMinh`) dùng để lọc dữ liệu.
- **Mức 3**: Lọc phạm vi dữ liệu nằm ở `Common/PhamViDuLieu.cs`: giáo viên ⇒ `l.MaGV = @PV_MaGV`; học viên ⇒ `EXISTS (SELECT 1 FROM DangKy ...)`; không đủ quyền ⇒ điều kiện `1 = 0`. Cùng cơ chế này áp cho `frmThoiKhoaBieu` (GV xem lịch mình, HV xem lịch lớp mình đăng ký — `ThoiKhoaBieuRepository.LayLich`).

### A3. Lý do chọn đề tài?
- **Mức 1**: Vì nó đủ cả phần "quản lý nghiệp vụ" lẫn phần "thuật toán" (xếp lịch) và phần "tự động hoá quy tắc" — đủ để demo đầy đủ kỹ thuật WinForms + SQL Server + ADO.NET.
- **Mức 2**: Bài toán xếp lịch là CSP thật (biến, domain, ràng buộc cứng/mềm) — phù hợp để dùng solver thay vì if/else. Phần tự động hoá bằng rule cho thấy mở rộng được nghiệp vụ mà không sửa code.
- **Mức 3**: Em chọn **ADO.NET thuần** thay vì EF vì cần SQL cụ thể: `UPDLOCK/HOLDLOCK` chống vượt sĩ số, `OUTPUT INSERTED` lấy khóa vừa insert, unique index làm lớp bảo vệ cuối, và transaction nhiều bước. Đồ án cũng yêu cầu nắm SQL/ADO.NET.

### A4. Điểm nổi bật của phần mềm?
- **Mức 1**: Ba điểm: (1) xếp thời khóa biểu tự động bằng CP-SAT với ràng buộc cứng + mềm; (2) nhập hàng loạt có preview + all-or-nothing + xuất báo cáo lỗi; (3) Workflow Automation cấu hình rule trên UI, có lịch sử thực thi.
- **Mức 2**: Mỗi điểm đều có "lớp bảo vệ" ở DB chứ không chỉ ở code: xếp lịch có unique index chống trùng GV/phòng; nhập hàng loạt có transaction + PK; rule có CHECK constraint + dedup unique index.
- **Mức 3**: Điểm em tự đánh giá mạnh nhất là **tính nhất quán**: mọi đường ghi dữ liệu đều dùng `SqlParameter`, mọi thao tác cần toàn vẹn đều có transaction, và mọi bảng lớn đều có CHECK/FK/unique phù hợp. Điểm yếu em tự biết: 4 nhóm chức năng chưa có UI và 2 trigger Automation chưa có nơi phát sự kiện.

---

## NHÓM B — KIẾN TRÚC

### B1. Vì sao chia tầng?
- **Mức 1**: Để tách trách nhiệm: Form lo UI, Service lo quy tắc nghiệp vụ, Repository lo SQL, DbHelper lo kỹ thuật connection. Nhờ đó đổi câu SQL không phải sửa Form.
- **Mức 2**: Ví dụ cụ thể: `frmLopHoc` gọi `LopHocService.Them` (validate + check trùng + dịch exception thành chuỗi lỗi) → `LopHocRepository.Them` (check quyền + INSERT) → `DbHelper.ExecuteNonQuery`. Không tầng nào làm việc của tầng khác.
- **Mức 3**: Kiến trúc phân tầng nhưng **không phải clean architecture** — dự án không có interface cho từng repository, Service gọi thẳng `new LopHocRepository()` (DI thủ công), và `Models` đóng vai trò DTO lẫn entity. Đây là quyết định "đủ dùng, dễ hiểu" cho đồ án; nếu mở rộng thì nên thêm interface + DI container.

### B2. Vì sao không truy cập DB trực tiếp trong Form?
- **Mức 1**: Vì SQL trong Form không tái sử dụng được, khó kiểm thử, và dễ dẫn tới việc kiểm tra quyền bị lọt.
- **Mức 2**: Trong toàn bộ `Forms/` không có câu `SELECT/INSERT` nào; Form chỉ nhận `DataTable`/model và hiển thị. Ngoại lệ duy nhất là `PhamViDuLieu` trả về **điều kiện SQL dạng chuỗi** cho Repository ghép vào query — điều kiện đó do code sinh ra, không phải do người dùng nhập.
- **Mức 3**: Nhờ vậy, khi em thêm module Bulk Import, Form mới chỉ cần gọi `ImportService.XemTruoc/ThucHien` — không phải viết SQL trong Form, và quyền được kiểm tra lại ở Service (lớp bảo vệ cuối) chứ không phụ thuộc việc Form có ẩn nút hay không.

### B3. Service khác Repository thế nào?
- **Mức 1**: Service chứa quy tắc nghiệp vụ và trả về thông báo lỗi thân thiện; Repository chứa SQL và là nơi chặn quyền cuối cùng.
- **Mức 2**: `LopHocService.Them` trả `string`: rỗng = thành công, khác rỗng = lỗi để hiển thị. Còn `LopHocRepository.Them` **ném** `UnauthorizedAccessException` khi thiếu quyền. ⇒ Quy ước: Service bắt lỗi và chuyển thành thông điệp; Repository để lỗi nổi lên.
- **Mức 3**: Lý do tách: (1) Service có thể test bằng mock repository; (2) nhiều Service có thể dùng chung 1 repository; (3) quy tắc "1 lần ghi = 1 transaction" nằm ở Service/Repository chứ không phải UI. Hạn chế: hiện Service **không có interface** ⇒ khó mock ⇒ em mới test được bằng harness tích hợp với DB thật.

### B4. Vì sao tách Model và DTO?
- **Mức 1**: Vì dữ liệu hiển thị (DataTable 30 cột) và dữ liệu nghiệp vụ (model 10 trường) khác nhau; tách ra để không lẫn.
- **Mức 2**: Phần cũ trả `DataTable` cho UI (nhanh, tiện bind grid). Phần mới (Bulk Import, Automation) có DTO rõ ràng: `ImportDong`, `ImportLoi`, `KetQuaPreview/Import`, `AutomationRule/Condition/Action/Execution/ExecutionLog`, `AutomationContext`.
- **Mức 3**: `AutomationContext` là ví dụ đắn giá nhất của việc tách DTO: nó **chụp lại trạng thái lớp** tại thời điểm phát sự kiện, nên việc đánh giá rule không phụ thuộc trạng thái DB lúc chạy, và rule có thể test lại offline bằng nút "Test Rule".

### B5. Nếu thay database thì phải thay đổi phần nào?
- **Mức 1**: Chủ yếu tầng Data Access: `DbHelper` + các câu SQL trong `Repositories/**`. Form và phần lớn Service không đổi.
- **Mức 2**: Nhưng phải nói thẳng: `Services/Import/ImportService.cs` và `Repositories/Import/ImportLookupRepository.cs` dùng trực tiếp `Microsoft.Data.SqlClient` (`SqlConnection`, `SqlTransaction`, `SqlDbType`) ⇒ khi đổi DB phải sửa 2 file này nữa, và `ImportLookupRepository.TonTai` đang nội suy tên bảng/cột.
- **Mức 3**: Nếu muốn "chạy được với mọi DB" thì phải tạo abstraction (`IDbConnectionFactory`, `ISqlDialect`) và bỏ `SqlDbType` cụ thể. Đây là nợ kỹ thuật em biết và sẽ làm nếu có yêu cầu đa DB.

### B6. Vì sao có `DbHelper` mà không dùng trực tiếp `SqlConnection` ở mọi nơi?
- **Mức 1**: Để connection luôn được mở/đóng đúng cách và không lặp code.
- **Mức 2**: `DbHelper` gom 5 thao tác: `ExecuteQuery` (DataTable), `ExecuteNonQuery`, `ExecuteScalar`, `ExecuteTransaction`, `TestConnection`. Nó **không** che mất SQL — mỗi Repository vẫn tự viết SQL của mình, nên vẫn kiểm soát được index, câu truy vấn và tham số.
- **Mức 3**: `DbHelper` là "bản mỏng" (thin wrapper) chứ không phải ORM; ưu điểm là ít abstraction, dễ debug SQL thật; nhược điểm là không có unit-testability cho DAL và mọi repository đều dùng chung connection string tĩnh.

---

## NHÓM C — DATABASE

### C1. Tại sao thiết kế bảng như vậy?
- **Mức 1**: Tách danh mục khỏi nghiệp vụ: `MonHoc`, `GiaoVien`, `PhongHoc`, `CaHoc`, `ThoiGian` là danh mục; `LopHoc` tham chiếu chúng; `DangKy` và `BuoiHoc` là bảng trung gian/sự kiện.
- **Mức 2**: Bảng `DangKy` là N-N giữa học viên và lớp, có thuộc tính riêu `NgayDangKy`; `BuoiHoc` là N-N giữa lớp và (ngày, ca, phòng, GV) — PK `MaBuoi`, FK sang lớp/GV/ca/phòng, và 2 unique index để chống trùng lịch.
- **Mức 3**: Quan hệ đã được chuẩn hóa ở phần nghiệp vụ; chỗ chưa chuẩn hóa là `HocVien.MaLop` còn tồn tại song song với `DangKy` (script `Database/ThoiKhoaBieu.sql` đã có đoạn migrate `HocVien.MaLop → DangKy`; code đã chuyển sang dùng `DangKy` làm nguồn sự thật, kể cả khi tính sĩ số và lọc phạm vi dữ liệu).

### C2. Có vi phạm chuẩn hóa không?
- **Mức 1**: Có hai điểm: cột `HocVien.MaLop` (nhân bản quan hệ N-N) và cột `LopHoc.SiSo` (giá trị dẫn xuất).
- **Mức 2**: `LopHoc.SiSo` tồn tại nhưng **không được dùng** — mọi nơi tính sĩ số bằng `COUNT(*) FROM DangKy` (`DangKyRepository.DemSiSo`, subquery trong `LopHocRepository.LayDanhSach`). Nên rủi ro dữ liệu không nhất quán thực tế đã được tránh bằng cách **không tin** cột đó.
- **Mức 3**: `HocVien.MaLop` còn gây hiệu ứng phụ: FK NO ACTION ⇒ xoá lớp sẽ bị chặn (547) nếu còn học viên tham chiếu ⇒ đó chính là nguồn thông báo "Lớp này đang có học viên… không thể xóa" trong `LopHocService.Xoa`. Bước khắc phục: migrate xong thì drop cột và bỏ FK.

### C3. Làm sao đảm bảo Foreign Key hợp lệ?
- **Mức 1**: DB có FK thật cho mọi quan hệ (29 bảng, hàng chục FK); ngoài ra code kiểm tra trước ở Service/Validator.
- **Mức 2**: Hai lớp: (1) kiểm tra tồn tại ở tầng nghiệp vụ — ví dụ `ImportValidator` + `ImportLookupRepository.TonTai`, `LopHocService.KiemTra` bắt buộc chọn môn/GV/ca/phòng; (2) FK ở DB là chốt chặn cuối, lỗi 547 được bắt và chuyển thành thông báo dễ hiểu.
- **Mức 3**: FK không đủ ⇒ em còn kiểm tra ở tầng UI/Service để người dùng không phải nhập dữ liệu rác mới biết sai; và có `ON DELETE CASCADE` có chọn lọc: `DangKy.MaLop`, `DangKy.MaHocVien`, `BuoiHoc.MaLop`, `ThoiGian_Thu.MaThoiGian` cascade (dữ liệu con theo vòng đời cha); còn `LopHoc.MaMon/MaGV/...` NO ACTION (không xoá danh mục đang được dùng).

### C4. Khi hai người thao tác cùng lúc thì sao?
- **Mức 1**: Ba lớp: khóa dòng trong transaction (`UPDLOCK, HOLDLOCK`), unique index, và transaction cô lập.
- **Mức 2**: Ví dụ đăng ký lớp: `DangKyRepository.Them` mở transaction, `SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK)` ⇒ phiên thứ hai **chờ** ⇒ khi đếm `COUNT(DangKy)` sẽ thấy số đã cập nhật ⇒ chặn đúng, không vượt sĩ số. Nếu cả hai cùng đăng ký 1 học viên vào 1 lớp ⇒ `PK_DangKy(MaHocVien, MaLop)` chặn (2627).
- **Mức 3**: Còn 2 chỗ chưa có chống race: (1) `PhanQuyenDAL.Luu` dùng `UPDLOCK` nhưng đọc–ghi theo kiểu "đọc tất cả → xoá/ghi lại" nên vẫn có thể mất cập nhật nếu 2 admin sửa cùng lúc; (2) xếp lịch: 2 phiên cùng xếp sẽ bị unique index chặn nhưng **không có bước thử lại** ⇒ người dùng phải bấm "Tạo thời khóa biểu" lại.

### C5. Nếu xoá một bản ghi thì các bảng liên quan xử lý thế nào?
- **Mức 1**: Tùy FK: `ON DELETE CASCADE` thì xoá luôn dữ liệu con; `NO ACTION` thì DB chặn và code bắt lỗi 547 thành thông báo.
- **Mức 2**: Xoá lớp ⇒ xoá luôn `BuoiHoc` và `DangKy` (cascade) nhưng bị chặn nếu `HocVien.MaLop` còn trỏ tới. Xoá môn học ⇒ bị chặn nếu còn lớp dùng ⇒ "Môn học này đang được sử dụng (lớp học hoặc giáo viên), không thể xóa".
- **Mức 3**: Xoá học viên ⇒ cascade xoá `DangKy` của học viên đó ⇒ sĩ số lớp tự giảm ⇒ không cần cập nhật `LopHoc.SiSo` (cột này vốn không dùng). Xoá tài khoản ⇒ `TaiKhoan.MaGV/MaHocVien` là NO ACTION ⇒ phải xoá tài khoản trước khi xoá giáo viên/học viên, hoặc đặt NULL (không cho phép vì `CK_TaiKhoan_HoSo` yêu cầu MaGV/MaHocVien khác NULL theo vai trò).

---

## NHÓM D — THUẬT TOÁN VÀ NGHIỆP VỤ

### D1. Vì sao dùng thuật toán này chứ không phải thuật toán khác?
- *Xem `04-SMART-TIMETABLE.md` §6 (Q1) — trả lời ở đây.*
- **Mức 1**: Vì bài toán là tối ưu ràng buộc với cả ràng buộc cứng lẫn mềm; CP-SAT giải nhanh và trả lời tốt nhất trong thời gian giới hạn.
- **Mức 2**: Mọi ràng buộc đều quy về "tổng các biến ≤ 1" (cardinality) trên biến nhị phân ⇒ ánh xạ thẳng sang `LinearExpr.Sum(...) <= 1`; ràng buộc mềm đi vào hàm mục tiêu `Minimize(WeightedSum(...))`.
- **Mức 3**: Về hình thức đây là ILP 0-1; CP-SAT giải bằng SAT(CDCL) + LP relaxation, có clause learning nên thực tế mạnh hơn backtracking thuần (O(D^N)). Nếu tự viết backtracking thì vừa chậm vừa khó tối ưu ràng buộc mềm.

### D2. Có cách nào khác không?
- **Mức 1**: Có: ILP/MIP (giải bằng solver tuyến tính), heuristic/metaheuristic (di truyền, tabu), hoặc bài toán "timetabling" chuyên dụng.
- **Mức 2**: Em chọn CP-SAT vì ràng buộc + mục tiêu đều là tổng tuyến tính trên biến nhị phân và cần lời giải **tốt** trong thời gian giới hạn 20 giây (UX trên desktop).
- **Mức 3**: Nếu bài toán lớn hơn nhiều, hướng đi là: giảm ràng buộc H5 (học viên), dùng **warm start** (gợi ý lời giải cũ), hoặc chuyển sang MIP/CP-SAT incremental. Nếu mục tiêu là "xếp nhanh, không cần tối ưu", thì heuristic (greedy + local search) có thể đủ và dễ giải thích hơn.

### D3. Độ phức tạp ra sao? Trường hợp xấu nhất?
- **Mức 1**: NP-hard; solver giới hạn 20 giây nên luôn trả kết quả trong thời gian chấp nhận được.
- **Mức 2**: Số biến ≈ `lớp × buổi/tuần × 7 × ca × phòng`; hiện 2 lớp có học viên ⇒ 378 biến. Ràng buộc H5 (học viên) là phần nhân tuyến tính theo số học viên học ≥ 2 lớp ⇒ **đây là điểm sẽ vỡ trước**.
- **Mức 3**: Trường hợp xấu nhất về mặt *nghiệp vụ* là "không tồn tại lời giải" (10 lớp, 1 GV, 1 ca, 1 phòng). Về mặt *thuật toán* là khi solver trả `UNKNOWN` do hết 20 giây — hiện code gộp `INFEASIBLE` và `UNKNOWN` thành một thông báo, đây là điểm em cần cải thiện.

### D4. Nghiệp vụ nào em tự tin nhất và phần nào em còn yếu?
- **Mức 1**: Tự tin nhất: luồng đăng ký lớp + transaction + chặn vượt sĩ số. Yếu nhất: UI của Học viên/Giáo viên/Danh mục/Báo cáo và phần kết nối Automation vào menu.
- **Mức 2**: Mình đã kiểm chứng phần nhập hàng loạt bằng harness: 51 assertion gồm preview, ghi thật vào SQL Server, rollback khi lỗi ở dòng 2, chặn vượt sức chứa, chặn lớp đã kết thúc, chặn trùng, chặn khi thiếu quyền, sinh tệp mẫu/báo cáo lỗi, và giả lập sự kiện kéo-thả.
- **Mức 3**: Thứ tự ưu tiên nếu còn thời gian: (1) gắn Automation vào menu + hoàn thiện trigger `BULK_IMPORT_COMPLETED` (mở CHECK constraint); (2) màn hình CRUD Học viên/Giáo viên; (3) Preview cho xếp lịch; (4) unit test trong project.

---

## NHÓM E — BẢO MẬT VÀ ĐỘ TIN CẬY

### E1. SQL Injection là gì và em xử lý thế nào?
- **Mức 1**: Là kiểu tấn công chèn mã SQL vào câu lệnh để đổi ý nghĩa truy vấn; em chặn bằng cách **mọi giá trị đều truyền qua `SqlParameter`**, không nối chuỗi vào SQL.
- **Mức 2**: Ví dụ: `WHERE MaLop = @MaLop` thay vì `"WHERE MaLop = '" + maLop + "'"`. Giá trị được gửi riêng qua driver nên engine SQL không "diễn giải" nó như mã.
- **Mức 3**: Nói thẳng 4 chỗ có nội suy chuỗi `$"..."`: 2 chỗ trong `LopHocRepository` (alias bảng + điều kiện phạm vi do `PhamViDuLieu` sinh), 1 chỗ trong `LopHocTraCuuRepository`, và `ImportLookupRepository.TonTai` nội suy **tên bảng/cột**. Tất cả đều là định danh do code sinh, không phải dữ liệu người dùng; `PhamViDuLieu.KiemTraAlias` còn chặn alias lạ. Nhưng đây là nợ kỹ thuật — nên chuyển sang bảng tra cứu mapping bảng→SQL cố định.

### E2. Phân quyền của em hoạt động thế nào?
- **Mức 1**: 3 lớp: ẩn/hiện nút ở UI, kiểm tra `PermissionService.YeuCau` ở Service/Repository, và lọc dữ liệu theo phạm vi người dùng trong SQL.
- **Mức 2**: `ChucNang` (32 mã) + `PhanQuyen` (54 dòng: `MaChucNang, VaiTro, CoQuyen, ChiCuaMinh`); Admin luôn có toàn quyền; `PhanQuyen` chỉ có 2 vai trò vì Admin đã đặc biệt hoá. `PhamViDuLieu` sinh điều kiện lọc: GV → `MaGV = @PV_MaGV`, HV → `EXISTS(... DangKy ...)`.
- **Mức 3**: `ChiCuaMinh` được dùng ở 2 nơi: lọc dữ liệu (`PhamViDuLieu`) và **giới hạn trường được sửa** — trong `LopHocRepository.Sua`, nếu người dùng chỉ sửa được lớp của mình thì cột `MaGV` bị loại khỏi `SET` ⇒ GV không tự đổi giáo viên phụ trách. Ngoài ra `ChucNang.ChiAdmin` đánh dấu chức năng chỉ Admin (phân quyền, tài khoản).

### E3. Validation ở client và ở server/service khác nhau thế nào?
- **Mức 1**: Form kiểm tra để báo lỗi nhanh; Service/Repository kiểm tra lại (không tin UI); DB kiểm tra lần cuối bằng CHECK/FK/unique.
- **Mức 2**: Ví dụ thêm lớp: Form kiểm tra quyền và bắt buộc; `LopHocService.KiemTra` kiểm tra nghiệp vụ (ngày, sĩ số, số buổi); `LopHocRepository.TonTai` chặn trùng; DB có PK + FK + CHECK.
- **Mức 3**: Nguyên tắc: **UI không bao giờ là hàng rào bảo mật**; mọi kiểm tra quan trọng phải lặp lại ở tầng dưới vì Form có thể bị bỏ qua (gọi Service trực tiếp, gọi API khác sau này). Ví dụ cụ thể: `ImportService.ThucHien` kiểm tra lại toàn bộ ràng buộc **trong transaction** vì dữ liệu có thể đã đổi sau lúc preview.

### E4. Transaction của em dùng ở đâu?
- **Mức 1**: 6 nơi: đăng ký lớp, xếp lịch, nhập hàng loạt, lưu Rule, sửa Rule, lưu phân quyền.
- **Mức 2**: Tất cả đi qua `DbHelper.ExecuteTransaction` — nơi duy nhất gọi `BeginTransaction()`; `Commit()` khi callback trả về, `Rollback()` khi callback ném lỗi rồi re-throw.
- **Mức 3**: Những thao tác **không** có transaction: thêm/sửa/xoá lớp, môn học (mỗi câu lệnh đơn lẻ, không cần tính toàn vẹn nhiều bước). Đây là lựa chọn có chủ ý: thêm lớp = 1 INSERT, đã là nguyên tử sẵn.

### E5. Concurrency (tranh chấp) đã xử lý ở mức nào?
- **Mức 1**: Khóa dòng `UPDLOCK, HOLDLOCK` khi đọc số liệu quyết định ghi (đăng ký lớp, import đăng ký); unique index làm lưới an toàn cuối.
- **Mức 2**: Đăng ký lớp: khóa dòng `LopHoc` ⇒ 2 phiên xếp hàng ⇒ không vượt sĩ số. Trùng cặp đăng ký: PK composite. Automation: `UX_AutomationExecution_Dedup`.
- **Mức 3**: Chưa xử lý: cập nhật cùng lúc trên cùng 1 bản ghi (không có `RowVersion`/optimistic concurrency ở bảng nghiệp vụ — chỉ `AutomationRule` có `RowVersion`); và phân quyền vẫn theo mô hình "đọc–ghi lại" nên có thể mất cập nhật.

### E6. Exception handling của em có gì?
- **Mức 1**: 3 tầng: Form bắt lỗi và hiện thông báo; Service chuyển exception thành chuỗi lỗi; Repository để lỗi nổi lên kèm mã lỗi SQL cụ thể (2627/2601/547).
- **Mức 2**: `LopHocService.Xoa` bắt `SqlException` và nếu mã 547 trả "Lớp này đang có học viên…"; `ImportService.ThucHien` bắt 2627/2601 → "trùng khóa", 547 → "tham chiếu không hợp lệ", còn lại → thông báo chung; `AutomationEventPublisher` **nuốt toàn bộ lỗi** để Automation không ảnh hưởng nghiệp vụ.
- **Mức 3**: Hạn chế phải nói: (1) `catch (Exception ex)` tổng quát cuối của `ImportService` có thể lộ `ex.Message` của SQL Server cho người dùng; (2) chưa xử lý riêng deadlock (1205) và timeout; (3) chưa có logging tập trung (hiện chỉ `Console.WriteLine` trong `ActionHandlerRegistry` và `AutomationExecutionLog`).

---

## NHÓM F — HẠN CHẾ VÀ HƯỚNG PHÁT TRIỂN

### F1. Phần nào còn hạn chế?
- **Mức 1**: (1) Chưa có UI cho Học viên/Giáo viên/Danh mục/Báo cáo; (2) `CLASS_CREATED`/`CLASS_STATUS_CHANGED` chưa có nơi phát sự kiện; (3) xếp lịch chưa có Preview; (4) chưa có unit test trong project.
- **Mức 2**: Chi tiết và cách khắc phục nằm ở `03-NGHIEP-VU.md` §3.7 và `05-AUTOMATION-RULESENGINE.md` §11.
- **Mức 3**: Nói thêm: các hạn chế này **đã được ghi nhận và có đường khắc phục cụ thể** — ví dụ Automation chỉ thiếu 1 nút mở form + 1 câu `ALTER TABLE` mở CHECK constraint; đó không phải là viết lại từ đầu.

### F2. Nếu mở rộng lên hàng nghìn người dùng?
- **Mức 1**: WinForms + SQL Server chịu được hàng nghìn người dùng nội bộ; vấn đề sẽ là **connection** và **UI 1 tiến trình/máy**.
- **Mức 2**: Connection pooling của SQL Server xử lý phần kết nối; điểm cần làm: cache danh mục (hiện `LookupRepository` đọc DB mỗi lần mở form), thêm paging cho các bảng lớn, và chuyển phần tra cứu/báo cáo sang web nếu cần truy cập từ nhiều nơi.
- **Mức 3**: Với hàng nghìn người dùng đồng thời, kiến trúc hiện tại (1 app client mở connection mỗi thao tác) là chấp nhận được nếu thao tác ngắn; nếu nghiệp vụ có bước dài (xếp lịch 20 giây) thì phải chuyển việc đó sang job/service background để không giữ connection.

### F3. Nếu dữ liệu tăng gấp 10 lần?
- **Mức 1**: Vấn đề chính là các truy vấn N+1: `LopHocRepository.LayDanhSach` tính sĩ số bằng subquery cho mỗi dòng.
- **Mức 2**: `BuoiHoc` sẽ lớn nhanh nhất (mỗi lớp × số buổi × số tuần). Index đã có: `(Ngay, MaCa, MaGV)` và `(Ngay, MaCa, MaPhong)` unique + `IX_BuoiHoc_Lop`.
- **Mức 3**: Cách khắc phục cụ thể: (1) thay subquery sĩ số bằng `COUNT` có group hoặc materialized column cập nhật bằng trigger; (2) đưa danh mục tra cứu vào cache trong bộ nhớ; (3) phân trang danh sách; (4) với báo cáo lớn thì dùng materialized view/SQL Server Reporting.

### F4. Nếu mất kết nối DB giữa chừng thì sao?
- **Mức 1**: Thao tác ghi sẽ ném `SqlException`; transaction của `DbHelper` rollback ⇒ **không để lại dữ liệu dở dang**.
- **Mức 2**: Ở Form, `catch` sẽ hiện thông báo lỗi; ở `ImportService` thì cả tệp bị từ chối (all-or-nothing) ⇒ người dùng thử lại được, không phải "import được một phần".
- **Mức 3**: `DbHelper.TestConnection(out message)` đã có để kiểm tra kết nối trước khi dùng. Điểm cần bổ sung: retry có backoff cho lỗi mạng tạm thời, và hiển thị thông báo thân thiện thay vì `ex.Message` kỹ thuật.

### F5. Có phát triển thành hệ thống web được không?
- **Mức 1**: Được, vì nghiệp vụ đã nằm ở Service/Repository, không phụ thuộc WinForms.
- **Mức 2**: Chuyển Presentation sang ASP.NET Core, giữ nguyên Service + Repository + DB. Phải xử lý thêm: phiên đăng nhập/token thay cho `CurrentUser` static, và chuyển `ExcelHelper` (COM Interop) sang thư viện xuất file không cần Excel cài trên server.
- **Mức 3**: Lưu ý: `Common/CurrentUser` là **static** ⇒ chỉ hợp với desktop 1 người/1 tiến trình; khi web phải chuyển sang `HttpContext`-based accessor. `PermissionService` cũng giữ state tĩnh ⇒ cần chuyển sang scope theo request (Scoped). Đây là hai chỗ phải sửa đầu tiên khi chuyển web.

---

## NHÓM G — CÂU HỎI XOÁY SÂU

### G1. Em nói đã có transaction — transaction bắt đầu ở đâu và kết thúc ở đâu?
- **Mức 1**: Bắt đầu trong `DbHelper.ExecuteTransaction` ngay sau `conn.Open()`; kết thúc bằng `Commit()` nếu callback chạy hết, `Rollback()` nếu callback ném lỗi.
- **Mức 2**: Toàn bộ phần ghi nằm trong callback ⇒ cùng 1 transaction. Ví dụ `DangKyRepository.Them`: khóa dòng lớp → đếm sĩ số → INSERT, tất cả trong callback; nếu INSERT lỗi ⇒ rollback cả transaction ⇒ không đăng ký được lớp nào.
- **Mức 3**: Chi tiết quan trọng: transaction **chỉ bọc phần ghi DB**, không bọc phần tính toán nặng (ví dụ giải CP-SAT) — vì giữ transaction trong lúc tính 20 giây sẽ khóa tài nguyên DB vô ích. Với xếp lịch, em giải trước (không transaction), rồi mới mở transaction để ghi.

### G2. Em nói đã kiểm tra trùng dữ liệu — kiểm tra bằng cách nào?
- **Mức 1**: 3 nơi: kiểm tra trong Service/Validator (trước khi ghi), kiểm tra lại trong transaction bằng `WITH (HOLDLOCK)`, và **ràng buộc duy nhất ở DB** (PK/unique index).
- **Mức 2**: Ví dụ trùng mã học viên: `ImportValidator.KiemTraMa` so với `HashSet` mã đã có trong DB + `HashSet` mã trong file; khi ghi thật, `DaTonTai(conn, tran, "HocVien", "MaHocVien", ma)` với `HOLDLOCK`; nếu vẫn trùng thì PK `HocVien.MaHocVien` chặn và `SqlException 2627` được bắt riêng.
- **Mức 3**: Vì sao cần cả 3: kiểm tra ở tầng trên cho **thông báo thân thiện** (biết dòng nào lỗi, trùng với dòng nào trong file); kiểm tra trong transaction để **chống race** (dữ liệu đổi sau preview); ràng buộc DB là **lưới an toàn cuối** không thể bị vòng qua.

### G3. Nếu hai người cùng đăng ký học viên vào một lớp ở hai máy khác nhau?
- **Mức 1**: Sẽ không xảy ra vượt sĩ số, vì lớp bị khóa trong transaction.
- **Mức 2**: `SELECT SiSoToiDa FROM LopHoc WITH (UPDLOCK, HOLDLOCK)` giữ khóa tới khi transaction kết thúc ⇒ phiên thứ hai phải chờ; khi nó đếm `COUNT(*)` sẽ thấy số mới ⇒ chặn đúng ("Lớp đã đủ sĩ số").
- **Mức 3**: Nếu cả hai đăng ký **cùng một học viên vào cùng một lớp** ⇒ `PK_DangKy(MaHocVien, MaLop)` chặn ở DB ⇒ lỗi 2627 ⇒ thông báo "Bạn đã đăng ký lớp này rồi.". Cùng cơ chế này được dùng lại cho Bulk Import (`ImportLookupRepository.DocLopDangKhoa`).

### G4. Nếu Service gọi Repository nhưng SQL lỗi thì tầng nào bắt lỗi?
- **Mức 1**: Repository để lỗi nổi lên; Service bắt và chuyển thành thông báo; Form hiển thị. Với Automation thì publisher nuốt lỗi.
- **Mức 2**: Ví dụ `LopHocService.Xoa` bắt `SqlException`; nếu `Number == 547` trả "Lớp này đang có học viên…", ngược lại trả "Không xóa được: " + message. `LopHocService.Them` bắt `SqlException | UnauthorizedAccessException`.
- **Mức 3**: Nguyên tắc đã chọn: **không nuốt lỗi ở tầng dưới** (trừ Automation vì lỗi đó không được ảnh hưởng nghiệp vụ). Lý do: nếu Repository nuốt lỗi thì Service/Form không biết thao tác đã thất bại ⇒ dữ liệu và UI lệch nhau. Đổi lại, lỗi luôn "đi" theo đúng hệ thống phân tầng.

### G5. Em có thực sự dùng thuật toán MRV hay chỉ đặt tên class là MRV?
- **Mức 1**: Không — dự án **không có** class MRV nào; em dùng CP-SAT của Google OR-Tools.
- **Mức 2**: Các tên `MRVSelector`, `ForwardChecker`, `BacktrackingSolver`… **không tồn tại** trong source (đã grep toàn bộ). Cái có thật: solver tự làm propagation (tương đương forward checking) và có chiến lược chọn biến theo activity/impact.
- **Mức 3**: Cách trả lời chuẩn: "Em không tự viết backtracking vì bài toán của em là 0-1 ILP với hàm mục tiêu tuyến tính; CP-SAT phù hợp và mạnh hơn. Nếu giảng viên muốn xem cách ánh xạ khái niệm CSP vào code của em thì: biến = buổi học, domain = (thứ, ca, phòng) lọc theo sức chứa, ràng buộc cứng = cardinality, mục tiêu = tổng phạt."

### G6. Em nói hệ thống tự động hoá — phần nào thực sự tự động?
- **Mức 1**: Phần **tự động hoá quy tắc**: khi có sự kiện nghiệp vụ, hệ thống tự tìm rule theo trigger, tự đánh giá điều kiện, tự chạy hành động và tự ghi lịch sử.
- **Mức 2**: Hiện **chỉ có 1 nơi phát sự kiện**: `FrmBulkImport.PhatSuKienSauCommitAsync` — sau khi import đăng ký thành công, phát `CLASS_ENROLLMENT_CHANGED` cho **mỗi lớp bị ảnh hưởng** và phát `BULK_IMPORT_COMPLETED`.
- **Mức 3**: Nói thẳng phần chưa có: `CLASS_CREATED` và `CLASS_STATUS_CHANGED` **chưa có nơi phát** ⇒ rule của 2 trigger đó chưa bao giờ chạy; và `BULK_IMPORT_COMPLETED` bị CHECK constraint DB chặn nên publisher no-op. Ngoài ra module Automation **đã có** lối vào từ menu ("Hệ thống → Tự động hóa") nhưng mới chỉ chạy được nhánh `CLASS_ENROLLMENT_CHANGED` từ Bulk Import. Đây là danh sách việc em đã lập.

### G7. Nếu RulesEngine không chạy thì nghiệp vụ chính có bị ảnh hưởng không?
- **Mức 1**: Không — Automation luôn chạy sau khi nghiệp vụ đã commit, và mọi lỗi Automation đều bị nuốt.
- **Mức 2**: `AutomationEventPublisher.PhatSuKienAsync` bọc toàn bộ trong `try/catch` rỗng; trong Bulk Import, publisher được gọi **sau** khi transaction đã commit và sau thông báo thành công ⇒ lỗi Automation không thể làm hỏng kết quả import.
- **Mức 3**: Thêm: kể cả khi RulesEngine ném exception, `RuleEvaluator` bắt và trả `HopLe=false` ⇒ execution được ghi `FAILED` với lý do ⇒ vẫn **có dấu vết** để điều tra, chứ không phải lỗi "biến mất".

### G8. Làm thế nào chứng minh chức năng thực sự hoạt động?
- **Mức 1**: Em đã kiểm chứng bằng harness chạy trên SQL Server thật (không dùng dữ liệu giả): 51 assertion.
- **Mức 2**: Các assertion tiêu biểu: import 2 học viên ⇒ DB có đúng 2 dòng, điểm/ngày sinh đúng; tạo trigger tạm ném lỗi ở dòng 2 ⇒ dòng 1 **không** được ghi (chứng minh all-or-nothing); đăng ký 2 dòng vào lớp còn 1 chỗ ⇒ bị chặn kèm thông điệp đúng; lớp `DaKetThuc` ⇒ bị chặn; preview lại ⇒ báo trùng; user không có quyền ⇒ không ghi được dòng nào; automation rule chạy 2 lần cùng event ⇒ chỉ **1** execution `SUCCESS` và log đúng CONDITION/ACTION.
- **Mức 3**: Trung thực: harness **không nằm trong repo** và project **chưa có unit test framework** ⇒ đây là kiểm thử tích hợp bằng console, không phải test tự động chạy trong CI. Việc em sẽ làm tiếp: đưa harness vào repo dưới dạng project test riêng và thêm test cho `LopHocService`, `ImportValidator`.

### G9. Trong dự án của em, điều gì em muốn giảng viên hỏi nhất? (câu hỏi "bẻ" của giảng viên)
- **Mức 1**: Em muốn được hỏi về phần **transaction + khóa dòng khi đăng ký lớp**, vì đó là nơi em tự tin nhất và cũng là nơi có câu hỏi hay nhất.
- **Mức 2**: Em cũng muốn được hỏi về **vì sao dùng CP-SAT thay vì tự viết backtracking**, để em trình bày quyết định thiết kế.
- **Mức 3**: Và em chủ động nói trước cả 2 hạn chế lớn nhất (UI Học viên/Giáo viên/Báo cáo chưa có; 2 trigger Automation chưa có nơi phát) — vì giảng viên sẽ nhìn ra ngay, và việc thừa nhận trước cho thấy em nắm được phạm vi dự án của mình.
