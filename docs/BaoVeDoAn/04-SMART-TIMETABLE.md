# 04 — SMART TIMETABLE (XẾP THỜI KHÓA BIỂU TỰ ĐỘNG)

> **Lưu ý quan trọng về phạm vi tài liệu**: tên các lớp `SchedulingProblemBuilder`, `DomainGenerator`, `ScheduleValidator`,
> `MRVSelector`, `ForwardChecker`, `BacktrackingSolver`, `SchedulingSolver`, `SessionGenerator`, `ScheduleExtensionService`
> **KHÔNG tồn tại trong source code của dự án** (đã grep toàn bộ `BTL_LTTQ/**/*.cs`).
> Dự án **không** tự viết backtracking/MRV. Nó dùng **Google OR-Tools CP-SAT** (bài toán tối ưu ràng buộc).
> Phần này trình bày đúng những gì code đang làm, rồi giải thích cách nó khớp với khung khái niệm CSP để bạn trả lời phản biện.

---

## 1. Bằng chứng: dùng thư viện nào

- `BTL_LTTQ/BTL_LTTQ.csproj`: `<PackageReference Include="Google.OrTools" Version="9.15.6755" />`
- `BTL_LTTQ/Services/XepLichService.cs:4`: `using Google.OrTools.Sat;`
- File thực hiện: `Services/XepLichService.cs` (284 dòng) — **một service duy nhất** chứa toàn bộ mô hình + ràng buộc + giải.
- Dữ liệu vào: `Repositories/ThoiKhoaBieuRepository.cs:13 LayDuLieuXepLich()`; ghi ra: `:108 LuuLich()`.

---

## 2. Bài toán xếp lịch trong dự án này

### 2.1 Đầu vào (đọc từ DB)
`ThoiKhoaBieuRepository.LayDuLieuXepLich()` gom 5 nhóm dữ liệu:

| Nhóm | Nguồn SQL | Model |
|---|---|---|
| Lớp cần xếp | `LopHoc` LEFT JOIN `MonHoc` | `LopXepLich` (MaLop, MaGV, MaMon, SoBuoiTuan, NgayBD, NgayKT, MaCaUuTien, MaPhongUuTien, ThuUuTien, HocVien) |
| Ca học | `CaHoc` (sắp theo `GioBatDau`) | `List<string> MaCa` (CA01=8h, CA02=14h, CA03=18h) |
| Phòng + sức chứa | `PhongHoc` | `PhongXepLich` (MaPhong, SucChua: PH01=30, PH02=25, PH03=40) |
| Học viên từng lớp | `DangKy` | `LopXepLich.HocVien` ⇒ `SiSo => HocVien.Count` (`Models/DuLieuXepLich.cs:33`) |
| Mẫu thời gian | `ThoiGian_Thu` | `ThuUuTien` (0=T2 … 6=CN) |

Dữ liệu thật hiện có: `LopHoc` 3 lớp, nhưng `L003` **không có học viên** ⇒ `SiSo = 0` ⇒ bị bỏ qua; `L01` (5 HV, GV01, CA03, TG02, PH01, 10/01–10/03/2025) và `L02` (5 HV, GV02, CA01, TG01, PH03, 15/01–15/03/2025) được xếp ⇒ `BuoiHoc` có 51 dòng.

### 2.2 Biến quyết định (CSP variable)
Mỗi lớp `c` cần `SoBuoiTuan` buổi/tuần. Mỗi buổi `s` phải chọn đúng 1 bộ **(thứ d, ca k, phòng r)**.

Mã biến trong code: `x_{c}_{s}_{d}_{k}_{r}` — biến Boolean của CP-SAT:
```csharp
IntVar v = model.NewBoolVar($"x_{c}_{s}_{d}_{k}_{r}");     // XepLichService.cs:107
```
Số biến = `Σ_lớp (SoBuoiTuan × 7 × |Ca| × |phòng đủ chỗ|)`. Với dữ liệu hiện tại: `2 lớp × 3 buổi × 7 thứ × 3 ca × 3 phòng = 378` biến Boolean.

### 2.3 "Domain" (tập giá trị của biến) — ở đây được **nén sẵn khi tạo biến**
Không tạo biến cho phòng không đủ chỗ:
```csharp
if (phong.SucChua < lop.SiSo) continue;   // XepLichService.cs:105
```
⇒ Domain của biến = {(d,k,r) : SucChua(r) ≥ SiSo(lớp)}. Đây là cách "lọc domain" của riêng dự án, tương đương Forward Checking ở mức sinh biến.

### 2.4 Ràng buộc cứng (hard constraints) — tất cả đều là "tổng các biến ≤ 1" hoặc "= 1"

| # | Ràng buộc | Code | Biểu diễn |
|---|---|---|---|
| H1 | Mỗi buổi của lớp phải được xếp **đúng 1** chỗ | `:134-135` | `LinearExpr.Sum(theoBuoi[(c,s)]) == 1` |
| H2 | Mỗi phòng, mỗi (thứ, ca) chỉ 1 lớp | `:138-139` | `Sum(theoPhong[(d,k,r)]) <= 1` |
| H3 | Mỗi giáo viên, mỗi (thứ, ca) chỉ dạy 1 lớp | `:140-141` | `Sum(theoGiaoVien[(gv,d,k)]) <= 1` |
| H4 | 1 lớp không học 2 buổi trong cùng 1 ngày | `:144-145` | `Sum(theoNgayCuaLop[(c,d)]) <= 1` |
| H5 | 1 học viên không ở 2 lớp cùng (thứ, ca) | `:148-167` | `Sum(theoLopNgayCa của các lớp HV đó) <= 1` |

⇒ Nhờ tính chất "mỗi phần tử thuộc đúng 1 nhóm", các ràng buộc đều là **cardinality constraint** ⇒ ánh xạ trực tiếp sang `LinearExpr.Sum(...) <= 1` của CP-SAT (không cần `AllDifferent`).

### 2.5 Ràng buộc mềm (soft constraints) — hàm mục tiêu tối thiểu
Code dựng biến phạt và tối thiểu tổng trọng số:
```csharp
private const int PhatLechThu = 4;      // lệch ngày so với mẫu thời gian
private const int PhatLechCa  = 3;      // lệch ca so với ca ưu tiên
private const int PhatLechPhong = 1;    // lệch phòng so với phòng ưu tiên
private const int PhatCaMuon   = 1;     // dùng ca càng muộn càng bị phạt
private const int PhatNgayLienKe = 10;  // 2 buổi liền ngày (mềm)
...
model.Minimize(LinearExpr.WeightedSum(chiPhi, trongSo));   // :186
```
Ràng buộc "2 buổi liền ngày" được mã hoá bằng biến phụ `lienke_{c}_{d}` (`:178-182`) với ràng buộc `viPham >= Sum(homNay) + Sum(homSau) - 1` ⇒ nếu 2 ngày liền cùng có buổi thì bị phạt 10.

**Ý nghĩa**: "xếp được" là bắt buộc; "xếp đẹp" là điểm số cần tối thiểu hoá. Đây là điểm khác biệt quan trọng so với backtracking thuần: backtracking chỉ tìm "một" lời giải hợp lệ, CP-SAT tìm lời giải **tốt nhất theo trọng số** trong thời gian cho phép.

### 2.6 Giải và ghi kết quả
```csharp
solver.StringParameters = "max_time_in_seconds:20.0 num_search_workers:4 random_seed:42";  // :189
CpSolverStatus trangThai = solver.Solve(model);
if (trangThai != Optimal && trangThai != Feasible)  → trả lỗi có gợi ý cách sửa   // :192-195
```
Sau đó **mở rộng khung lịch theo ngày thật**: với mỗi lớp, duyệt `NgayBD..NgayKT`, giữ những ngày có `ChiSoThu(ngay) == d` (`:207-219`) ⇒ sinh dòng `BuoiHoc(MaLop, MaGV, Ngay, MaCa, MaPhong)`.
Nếu khung lịch có lời giải nhưng không sinh được buổi nào (khoảng ngày quá ngắn) ⇒ báo rõ (`:222-224`).
Ghi: `ThoiKhoaBieuRepository.LuuLich` — **1 transaction**: `DELETE FROM BuoiHoc` rồi INSERT từng dòng (`:108-129`).

---

## 3. Thuật toán: giải thích theo cách dễ hiểu

### 3.1 CSP là gì?
Bài toán: chọn giá trị cho các biến sao cho mọi ràng buộc được thoả. Ở đây biến = "buổi học thứ d, ca nào, phòng nào".

### 3.2 Backtracking thuần là gì và vì sao dự án KHÔNG tự viết
Backtracking: thử từng giá trị của biến; nếu vi phạm ràng buộc thì lùi lại. Tệ nhất là **O(D^N)** (D = số giá trị, N = số biến) — ở đây D ≈ 63 (7 thứ × 3 ca × 3 phòng), N = 378 ⇒ không khả thi nếu làm mù. Backtracking cũng dễ rơi vào "xếp được lời giải tệ" và rất khó bảo trì (cắt nhánh, heuristics).

### 3.3 Vì sao chọn CP-SAT thay vì tự viết
| Tiêu chí | Tự viết backtracking | Google OR-Tools CP-SAT (đang dùng) |
|---|---|---|
| Chất lượng lời giải | chỉ "hợp lệ", tùy thứ tự thử | tối ưu hàm mục tiêu trong thời gian cho phép |
| Hiệu năng | tệ nhất O(D^N) | CDCL/SAT + LP relaxation + clause learning, 4 worker song song |
| Ràng buộc mềm | tự viết phạt + backtrack | có sẵn trong hàm mục tiêu |
| Rủi ro cài đặt | cao (dễ sai logic, khó debug) | thư viện được Google bảo trì |
| Chi phí học/đọc code | cao | code của em chỉ ~120 dòng mô hình + ràng buộc |

### 3.4 "MRV / Forward Checking" có tương đương không?
- **Forward checking**: khi gán 1 biến, loại bỏ các giá trị của biến khác xung đột. Trong CP-SAT, cơ chế tương đương là **constraint propagation**: khi solver gán `x_0_0_2_1 = 1`, các ràng buộc tổng ≤ 1 tức thì loại bỏ các biến cùng phòng/giáo viên/ngày khác. ⇒ Có, hiệu ứng giống forward checking nhưng do solver xử lý, không phải code của em.
- **MRV (Minimum Remaining Values)**: chiến lược chọn biến có ít giá trị nhất còn lại để giảm nhánh. CP-SAT có chiến lược tương đương (chọn biến theo activity/impact), và mạnh hơn vì dùng **clause learning**: khi một phép gán dẫn tới xung đột, solver học một điều kiện mới để loại bỏ cả nhánh lỗi đó, thay vì thử lại từ đầu.
- ⇒ **Cách trả lời đúng**: "Em không tự viết MRV; em dùng solver CP-SAT có sẵn các chiến lược tương đương (propagation + clause learning). Nếu giảng viên hỏi về MRV, em giải thích ánh xạ này thay vì nói em đã làm MRV."

---

## 4. Các trường hợp khó — code xử lý thế nào (và cái nào **chưa** xử lý)

| Tình huống | Code làm gì | Bằng chứng | Mức độ |
|---|---|---|---|
| Không xếp được lịch | `trangThai` khác Optimal/Feasible ⇒ trả thông báo kèm gợi ý: giảm số buổi/tuần, thêm phòng, giảm số lớp mở cùng khung giờ | `:192-195` | ✅ xử lý |
| Không còn phòng đủ chỗ | Bỏ biến của phòng thiếu sức chứa; nếu **không phòng nào** đủ chỗ cho lớp ⇒ báo lỗi trước khi giải: "sĩ số N nhưng không phòng nào đủ chỗ" | `:53-54`, `:105` | ✅ xử lý |
| Giáo viên trùng lịch | Ràng buộc H3 (≤1 lớp/GV/thứ/ca) + `UX_BuoiHoc_GiaoVien` unique index ở DB | `:140-141`, `sys.indexes` | ✅ xử lý 2 tầng |
| Phòng trùng lịch | H2 + `UX_BuoiHoc_Phong` unique index | `:138-139` | ✅ 2 tầng |
| Lớp nhiều buổi/tuần | Tạo `SoBuoiTuan` biến con, ràng buộc "mỗi buổi xếp đúng 1 chỗ", và "1 lớp/ngày tối đa 1 buổi" ⇒ buổi phải rải trên các ngày khác nhau | `:96-131`, `:144-145` | ✅ xử lý |
| Lớp chưa có học viên | Bỏ qua (`SiSo > 0`), báo "Bỏ qua N lớp chưa có học viên đăng ký" | `:36-37`, `:236-237` | ✅ xử lý |
| Lớp chưa gán giáo viên | Báo lỗi trước khi giải | `:59-60` | ✅ xử lý |
| Ngày kết thúc trước ngày bắt đầu | Báo lỗi trước khi giải | `:56-57` | ✅ xử lý |
| Số buổi/tuần ngoài 1..7 | Báo lỗi trước khi giải | `:50-51` | ✅ xử lý |
| Kéo dài ngày kết thúc | Không ảnh hưởng mô hình (chỉ ảnh hưởng **bước mở rộng ngày thật**): lịch được sinh cho mọi ngày trong `[NgayBD, NgayKT]` có đúng thứ ⇒ kéo dài ⇒ **sinh thêm nhiều buổi học** | `:207-219` | ⚠️ cần nói rõ: kéo dài làm **tăng** số buổi, không chỉ "giãn" lịch |
| Khoảng ngày quá ngắn (không sinh được buổi) | Báo "khung lịch tìm được nhưng khoảng ngày quá ngắn" | `:222-224` | ✅ xử lý |
| **Preview trước khi lưu** | **KHÔNG có** — `btnTao_Click` gọi thẳng `XepVaLuu()` rồi lưu | `frmThoiKhoaBieu.cs:408` | ❌ **hạn chế, phải nói thẳng** |
| **Xác thực lại trước khi ghi** | **KHÔNG có** — dữ liệu đọc lúc đầu, giải xong ghi luôn | `ThoiKhoaBieuRepository.LuuLich` | ❌ hạn chế (khác Bulk Import) |
| **Ghi đè lịch cũ** | Có hỏi xác nhận "Thời khóa biểu hiện tại sẽ được thay thế" rồi `DELETE FROM BuoiHoc` + INSERT | `frmThoiKhoaBieu.cs:396-402`, `ThoiKhoaBieuRepository.cs:112` | ⚠️ cần nói rõ |
| Tối ưu chưa hoàn hảo | Solver giới hạn 20 giây, 4 worker ⇒ có thể trả `FEASIBLE` (tốt nhất tìm được trong thời gian), không phải `OPTIMAL` | `:189`, `:192` | ✅ xử lý (đã kiểm tra cả 2 trạng thái) |
| Trùng lịch do 2 người xếp cùng lúc | DB chặn bằng unique index; nhưng **không có bước thử lại** (retry) | `sys.indexes` | ⚠️ hạn chế |

---

## 5. Độ phức tạp và khả năng mở rộng

- **Số biến** tăng theo `Số lớp × Số buổi/tuần × 7 × Số ca × Số phòng`. Với 20 lớp × 3 buổi × 7 × 3 × 5 phòng ≈ 6.300 biến Boolean, vài nghìn ràng buộc ⇒ CP-SAT vẫn xử lý trong vài giây.
- Số ràng buộc H2 tăng theo `7 × |Ca| × |Phòng|`; H3 theo `|GV| × 7 × |Ca|`; H5 theo `số học viên học ≥ 2 lớp × 7 × |Ca|`.
- **Tổng số ràng buộc H5 là điểm nhạy cảm**: nếu 1.000 học viên đều học 2 lớp thì phải thêm `1000 × 7 × |Ca|` ràng buộc ⇒ cần tối ưu (ví dụ chỉ thêm ràng buộc cho các *cặp* lớp mà học viên đó cùng đăng ký, và bỏ qua cặp không liên quan). **Đây là hướng tối ưu em sẽ làm tiếp** — nói ra như hướng phát triển.
- CP-SAT là NP-hard nên không có thời gian xấp xỉ bảo đảm; dùng giới hạn 20 giây để bảo đảm UX (không treo form) — đây là trade-off hợp lý.

---

## 6. Ngân hàng câu hỏi riêng cho phần này

**Q1. Vì sao chọn thuật toán này?**
- *Mức 1 (15–30s)*: Vì bài toán là tối ưu ràng buộc với nhiều ràng buộc cứng và mềm; CP-SAT giải nhanh, trả lời tốt nhất, và còn bảo đảm không trùng lịch.
- *Mức 2 (45–90s)*: Bài toán có 3 loại xung đột chính: trùng giáo viên, trùng phòng, trùng học viên — đều là ràng buộc "tổng các biến ≤ 1" nên ánh xạ thẳng sang `LinearExpr.Sum(...) <= 1`. Ngoài ra còn ràng buộc mềm (ưu tiên thứ, ca, phòng, tránh 2 buổi liền ngày) nên cần **tối ưu** chứ không chỉ "tìm một lời giải hợp lệ". CP-SAT có sẵn hàm mục tiêu, propagation, clause learning, chạy nhiều worker song song; còn nếu tự viết backtracking thì vừa chậm vừa dễ sai, và tối ưu ràng buộc mềm sẽ rất khó.
- *Mức 3 (đào sâu)*: Về mặt hình thức, bài toán là CSP với biến Boolean, ràng buộc cardinality và một hàm mục tiêu tổng trọng số — tức là một bài toán **ILP 0-1** (0-1 Integer Linear Programming) với biến nhị phân, và CP-SAT giải bằng SAT (CDCL) kết hợp LP relaxation. Nó khác backtracking ở chỗ khi một phép gán dẫn tới xung đột, solver **học** một clause mới để cấm cả nhánh lỗi, thay vì quay lại thử lại từ đầu; đó là lý do thực tế nó nhanh hơn hẳn. Trong code em, "forward checking" tương đương được solver lo (propagation khi gán), còn "MRV" là chiến lược chọn biến mà CP-SAT tự quản lý theo activity/impact — em không tự cài MRV, và em không nói là đã làm.

**Q2. Nếu không tìm được lịch thì sao?**
- *Mức 1*: Trả thông báo kèm gợi ý cụ thể (giảm số buổi/tuần, thêm phòng, giảm số lớp cùng khung giờ); không ghi gì xuống DB.
- *Mức 2*: Solver trả `Status` khác `OPTIMAL`/`FEASIBLE` ⇒ `XepVaLuu` trả `KetQuaXepLich.Loi(...)` trước khi gọi `LuuLich` ⇒ DB không đổi. Ngoài ra em còn kiểm tra trước khi giải (lớp chưa có GV, không phòng nào đủ chỗ, ngày sai) để thông báo chính xác thay vì báo chung chung.
- *Mức 3*: Cần phân biệt 2 trường hợp: (1) **bản chất không tồn tại lời giải** (ví dụ 10 lớp cùng 1 giáo viên, 1 ca, 1 phòng) — solver báo `INFEASIBLE`; (2) **có lời giải nhưng không tìm ra trong 20 giây** — solver báo `UNKNOWN`. Code hiện gộp cả hai thành một thông báo "không tìm được thời khóa biểu thỏa tất cả ràng buộc" — đây là điểm em cần cải thiện: nên phân biệt `INFEASIBLE` với `UNKNOWN` và cho phép nới giới hạn 20 giây.

**Q3. Vì sao phải Preview trước khi ghi?**
- *Mức 1*: Vì xếp lịch ghi đè toàn bộ lịch cũ; người dùng cần xem lịch mới trước khi mất lịch cũ.
- *Mức 2*: Ở module xếp lịch, em **chưa làm** Preview — đây là hạn chế em thừa nhận (đối chiếu: module Bulk Import có Preview + Confirm đầy đủ). Cách em sẽ làm: `XepVaLuu` tách thành `XepThuoc()` (chỉ giải, trả `List<BuoiHoc>`) và `Luu(...)` (ghi), form hiển thị bảng lịch và nút "Lưu".
- *Mức 3*: Ngoài UX, còn là **tính đúng đắn**: dữ liệu đầu vào (đăng ký, phòng, GV) có thể đổi giữa lúc giải và lúc ghi. Lời giải CP-SAT có thể đã thành cũ. Ràng buộc unique index ở DB sẽ chặn lỗi khi ghi, nhưng người dùng sẽ thấy lỗi kỹ thuật thay vì thông báo rõ ràng. Nên trước khi ghi cần: (1) kiểm tra lại dữ liệu đầu vào còn như lúc giải, (2) ghi trong transaction, (3) bắt lỗi 2627 và trả thông báo "lịch đã thay đổi, hãy xếp lại".

**Q4. Độ phức tạp của Backtracking là gì?**
- *Mức 1*: Xấu nhất là số phương án nhân lại: O(D^N) — không khả thi khi N là số biến hàng trăm.
- *Mức 2*: Với D ≈ 63 giá trị/biến và N ≈ 378 biến thì không thể duyệt; dùng heuristic MRV + forward checking cắt được rất nhiều nhưng vẫn không có bảo đảm. CP-SAT giải bằng SAT+LP với clause learning nên thực tế xử lý được bài toán cỡ này trong vài giây.
- *Mức 3*: Bài toán xếp lịch là NP-hard; kỹ thuật chuẩn là **constraint programming**, **ILP**, **SAT**, và heuristic/metaheuristic (genetic, tabu, simulated annealing). OR-Tools hỗ trợ tất cả (CP-SAT, MIP, LinearProgramming, routing). Em chọn CP-SAT vì ràng buộc của em đều là cardinality trên biến nhị phân và cần hàm mục tiêu tuyến tính — đúng thế mạnh của CP-SAT.

**Q5. Khi số lượng lớp tăng lên thì thuật toán thay đổi gì?**
- *Mức 1*: Số biến và ràng buộc tăng tuyến tính; CP-SAT vẫn xử lý được ở quy mô vài chục lớp.
- *Mức 2*: Điểm nhạy cảm là ràng buộc "học viên không trùng lớp" (H5) vì nó nhân với số học viên. Em sẽ tối ưu bằng cách chỉ tạo ràng buộc cho cặp lớp mà học viên thực sự cùng đăng ký, và gom theo cặp lớp thay vì theo từng học viên.
- *Mức 3*: Ngoài ra cần phân vùng bài toán theo khung giờ/ngày (decompose), dùng **warm start** (gợi ý lời giải cũ), hoặc chuyển sang MIP/CP-SAT incremental. Với quy mô lớn hơn nữa thì bài toán xếp lịch trường học là bài toán **timetabling** chuẩn, có thể tham khảo các thuật toán heuristic đặc biệt cho trường học.

**Q6. Em có thực sự dùng thuật toán MRV hay chỉ đặt tên class?**
- *Mức 1*: Em **không** dùng MRV — dự án dùng CP-SAT của Google, không có class MRV nào trong code.
- *Mức 2*: Tên class `MRVSelector`, `ForwardChecker`, `BacktrackingSolver` không tồn tại trong dự án; em xác nhận bằng grep toàn bộ source. Cái có thật là: solver CP-SAT tự làm propagation (tương đương forward checking) và có chiến lược chọn biến theo activity (tương đương MRV về mặt mục đích).
- *Mức 3*: Nếu giảng viên hỏi "vậy em hiểu CSP thế nào", em trả lời bằng cách phân tích bài toán của dự án: biến là buổi học, domain là bộ (thứ, ca, phòng) lọc theo sức chứa, ràng buộc cứng là cardinality, hàm mục tiêu là tổng phạt; và em chọn công cụ giải quyết bài toán đó thay vì tự viết.
