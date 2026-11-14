# 09 — CHƯƠNG TRÌNH MOCK DEFENSE (LUYỆN BẢO VỆ)

> Tài liệu này đặt quy tắc cho buổi luyện. Khi bạn nhắn **"bắt đầu mock defense"** (hoặc "mock vòng N"),
> tôi sẽ đóng vai giảng viên và dẫn dắt đúng theo quy tắc bên dưới.

---

## 1. QUY TẮC (giữ nguyên trong suốt buổi luyện)

1. **Mỗi lượt chỉ MỘT câu hỏi.** Không hỏi kèm, không gợi ý trước.
2. **Chờ bạn tự trả lời** — không tiết lộ đáp án trước.
3. Sau khi bạn trả lời, tôi đánh giá **thang 10 điểm** theo rubric ở §3.
4. Chỉ ra: **phần đúng** / **phần thiếu hoặc sai** / **kiến thức bạn chưa nắm**.
5. Đưa **một câu trả lời mẫu tốt hơn** (theo 3 mức: ngắn / đầy đủ / đào sâu).
6. Nếu câu trả lời **chưa vững** ⇒ hỏi tiếp **1 câu xoáy sâu** liên quan (cùng chủ đề, khó hơn).
7. **Tăng dần độ khó** theo năng lực thể hiện được.

**Quy tắc bất di bất dịch cho bạn:** nếu bạn không biết, hãy nói "Em chưa làm phần này / em không chắc" — đây là câu trả lời **đúng** trong phòng bảo vệ. Không bịa. Điểm ở mọi câu trong `07-CAU-HOI-PHAN-BIEN.md` đều dựa trên source code thật.

---

## 2. KẾ HOẠCH 8 VÒNG

| Vòng | Chủ đề | Mục tiêu | Số câu (ước tính) | Tài liệu điểm |
|---|---|---|---|---|
| **1** | Tổng quan dự án | Nói trọn bài 60 giây; nêu đúng phạm vi đã làm / chưa làm | 4 | `00`, `07` nhóm A |
| **2** | Kiến trúc & OOP | Giải thích phân tầng bằng đúng tên class/method; phân biệt Service/Repository/DTO | 5 | `01`, `07` nhóm B |
| **3** | Database & ADO.NET | PK/FK/unique/CHECK; transaction bắt đầu–kết thúc ở đâu; parameterized query | 5 | `02`, `01`, `07` nhóm C |
| **4** | Nghiệp vụ | Đăng ký lớp, chặn vượt sĩ số, phạm vi dữ liệu, phân quyền, transaction ở 6 nơi | 5 | `03`, `07` nhóm D/E |
| **5** | Thuật toán (Smart Timetable) | Mô hình hóa bài toán; giải thích CP-SAT và vì sao không tự viết backtracking; trường hợp khó | 5 | `04` |
| **6** | Workflow Automation | Trigger/Condition/Action; RulesEngine chịu trách nhiệm gì; dedup/cooldown; lịch sử thực thi; an toàn | 5 | `05` |
| **7** | Bulk Import | Pipeline; all-or-nothing; rollback; tránh trùng/vượt sức chứa; quan hệ với Automation; phạm vi không có Import History | 5 | `06` |
| **8** | Phản biện & tình huống thực tế | Câu hỏi xoáy sâu + tình huống "nếu… thì sao" + phòng vấn hành vi khi có sự cố | 5 | `07` nhóm F/G, `08` |

**Sau mỗi vòng**, tôi tổng kết:
- Điểm trung bình vòng + điểm mạnh.
- **3 điểm yếu cần ôn** (kèm file tài liệu để đọc lại).
- Gợi ý câu hỏi cần chuẩn bị cho vòng sau.

---

## 3. RUBRIC CHẤM ĐIỂM (10 điểm)

| Tiêu chí | Điểm | Mô tả |
|---|---|---|
| **1. Đúng với code** | 0–3 | Có nói đúng tên class/method/bảng/ràng buộc? Hay nói chung chung hoặc sai? |
| **2. Bằng chứng cụ thể** | 0–2 | Có dẫn được "file/class/method/SQL/constraint" không? |
| **3. Hiểu bản chất** | 0–2 | Giải thích **vì sao** quyết định đó, không chỉ "em làm vậy"? |
| **4. Thành thật về phạm vi** | 0–1 | Có nói rõ phần chưa làm / hạn chế không, hay nhận công quá mức? |
| **5. Trả lời câu hỏi đúng ý** | 0–1 | Có đi thẳng vào trả lời, không né? |
| **6. Truyền đạt** | 0–1 | Rõ ràng, ngắn gọn, tự nhiên khi nói? |

**Ngưỡng xếp loại (gợi ý):**
- **9–10**: đạt — sẵn sàng bảo vệ.
- **7–8**: khá — chỉ cần luyện thêm 1–2 chủ đề.
- **5–6**: trung bình — còn "nói theo văn bản", cần nắm bản chất.
- **< 5**: yếu — cần đọc lại tài liệu module tương ứng trước khi luyện tiếp.

**Điểm "đạt" bắt buộc**: câu nào bạn tự bịa chức năng không có trong code (ví dụ nói "có lịch sử nhập hàng loạt", "có màn hình quản lý học viên", "có unit test") ⇒ trừ điểm mục 4 và mục 1.

---

## 4. MẪU ĐÁNH GIÁ SAU MỖI CÂU

```
━━ Câu hỏi: <viết lại câu đã hỏi>

ĐIỂM: x/10

✔ Phần đúng
  • ...
  • ...

✘ Phần thiếu / sai
  • ...
  • (kèm: file/class/method đúng là ...)

💡 Kiến thức bạn còn thiếu
  • ...

📌 Câu trả lời mẫu
  Mức 1 (15–30s): ...
  Mức 2 (45–90s): ...
  Mức 3 (đào sâu): ...

➜ Câu hỏi xoáy sâu tiếp theo (nếu điểm < 8): <1 câu>
```

---

## 5. NGÂN HÀNG CÂU HỎI DỰ PHÒNG THEO VÒNG

> Tôi sẽ hỏi linh hoạt trong các câu này, ưu tiên những câu **chưa** bạn đã luyện.

### Vòng 1 — Tổng quan
1. Em tóm tắt đồ án trong 60 giây.
2. Ba vai trò người dùng khác nhau thế nào? Cho ví dụ cụ thể từ dữ liệu trong DB.
3. Trong dự án em, những chức năng nào **đã xong**, cái nào **mới một phần**, cái nào **chưa làm**?
4. Ba điểm mạnh nhất và hai hạn chế lớn nhất của hệ thống?

### Vòng 2 — Kiến trúc
5. Vì sao chia tầng? Chỉ ra một thao tác đi qua những class nào (từ Form tới SQL Server).
6. Service khác Repository thế nào? Cho ví dụ một lỗi được bắt ở tầng nào.
7. Vì sao Form không được viết SQL? Rủi ro gì nếu viết?
8. "Model và DTO" khác nhau thế nào trong dự án em? Ví dụ DTO nào đáng kể nhất.
9. Nếu đổi SQL Server sang MySQL thì phải sửa những file nào?

### Vòng 3 — Database & ADO.NET
10. Transaction của em bắt đầu ở đâu, kết thúc ở đâu, ai gọi `BeginTransaction`?
11. Em kiểm tra trùng dữ liệu bằng cách nào? Có mấy tầng?
12. FK, CHECK, unique index khác nhau thế nào? Cho ví dụ trong DB của em.
13. `DangKy` là bảng trung gian — vì sao không đặt `MaLop` vào bảng `HocVien`?
14. Xoá một lớp đang có học viên thì chuyện gì xảy ra, code xử lý ra sao?
15. Hai người cùng đăng ký lớp ở hai máy — làm sao không vượt sĩ số?

### Vòng 4 — Nghiệp vụ
16. Nghiệp vụ đăng ký lớp chạy qua những class/method nào?
17. Sức chứa lớp được kiểm tra ở đâu, và vì sao phải khóa dòng?
18. "Chỉ của mình" được hiện thực thế nào cho giáo viên và học viên?
19. Phân quyền có mấy lớp kiểm soát? Lớp nào là lớp bảo vệ cuối?
20. Có 6 nơi dùng transaction — liệt kê và giải thích vì sao mỗi nơi cần/không cần.

### Vòng 5 — Thuật toán
21. Mô hình hóa bài toán xếp lịch của em: biến, domain, ràng buộc cứng, ràng buộc mềm.
22. Vì sao dùng CP-SAT mà không tự viết backtracking? So sánh ưu/nhược.
23. Em có dùng MRV/Forward Checking không? Trả lời thẳng.
24. Nếu solver không tìm được lịch thì code làm gì, DB có bị ảnh hưởng không?
25. Tại sao xếp lịch chưa có Preview mà Bulk Import thì có? Em sẽ làm thế nào?

### Vòng 6 — Workflow Automation
26. Trigger, Condition, Action khác nhau thế nào? Cho ví dụ rule trong DB của em.
27. Microsoft RulesEngine chịu trách nhiệm phần nào, phần nào em tự viết?
28. "Test Rule" hoạt động ra sao mà không chạy action thật?
29. Làm sao chống chạy trùng? Có mấy lớp?
30. Vì sao không cho Admin nhập C#/SQL tùy ý? Nêu 2 lý do cụ thể trong code.
31. Hạn chế nào của module Automation mà em phải nói trước?

### Vòng 7 — Bulk Import
32. Đi cho em nghe pipeline từ file Excel tới dòng trong DB.
33. All-or-nothing được đảm bảo bằng cơ chế nào? Em đã kiểm chứng thế nào?
34. Nếu tệp có 3 dòng đăng ký vào lớp chỉ còn 2 chỗ thì hệ thống xử lý ra sao?
35. Vì sao không dùng `SqlBulkCopy`/staging table? Khi nào nên đổi?
36. Vì sao không có lịch sử nhập hàng loạt? Nếu cần thì thiết kế ra sao?
37. Khi nào phát `BULK_IMPORT_COMPLETED`? Nếu rollback thì sao?

### Vòng 8 — Phản biện & tình huống
38. Em nói "kiểm tra trùng" — kiểm tra bằng cách nào, có chắc tuyệt đối không?
39. Nếu Service gọi Repository mà SQL lỗi thì tầng nào bắt lỗi? Vì sao?
40. Em nói hệ thống tự động hoá — phần nào **thực sự** tự động, phần nào chưa?
41. Nếu RulesEngine lỗi thì nghiệp vụ chính có bị ảnh hưởng không?
42. Làm sao chứng minh chức năng thực sự hoạt động? (chứng minh bằng gì?)
43. **Tình huống**: ngày demo, thầy bảo "đăng ký 50 học viên vào lớp còn 30 chỗ bằng cách gõ tay" — em làm gì?
44. **Tình huống**: giáo viên phản ánh "tôi bị xếp trùng lịch 2 lớp" — em kiểm tra bằng cách nào, và nếu đúng thì nguyên nhân ở đâu?
45. **Tình huống**: DB chết giữa lúc đang import 3.000 dòng — dữ liệu còn nhất quán không, và em chứng minh điều đó bằng gì?

---

## 6. GHI CHÚ BẢO VỆ (DÙNG TRONG LÚC ĐỐI THOẠI)

- **Mỗi câu trả lời nên có cấu trúc 3 câu**: (1) **kết luận** trong 1 câu, (2) **bằng chứng** (class/method/bảng/ràng buộc), (3) **giải thích vì sao** + **hạn chế/đánh đổi**.
- Khi bị hỏi phần chưa làm: **nói thẳng trong 10 giây đầu**, rồi nói "em đã làm được phần nào" — đừng vòng vo.
- Khi bị hỏi "vì sao không làm X": nêu **lý do có căn cứ** (phạm vi đồ án, thời gian, rủi ro dữ liệu) + **hướng làm** — tránh trả lời kiểu "chưa kịp".
- Nếu không nhớ chi tiết: nói rõ phần nào mình **chắc** và phần nào cần tra lại — rồi hứa tra và trả lời sau; đừng bịa.
- Nhớ 3 câu mở đầu an toàn: *"Em chia hệ thống thành 4 tầng…"*, *"Bài toán xếp lịch em mô hình hoá thành 0-1 ILP và giải bằng CP-SAT…"*, *"Phần nhập hàng loạt em áp dụng nguyên tắc all-or-nothing: toàn bộ tệp là một transaction."*

---

## 7. BẢNG ĐIỂM TỔNG (điền sau mỗi vòng)

| Vòng | Chủ đề | Điểm | Điểm mạnh | 3 điểm yếu cần ôn |
|---|---|---|---|---|
| 1 | Tổng quan | /10 | | |
| 2 | Kiến trúc | /10 | | |
| 3 | Database & ADO.NET | /10 | | |
| 4 | Nghiệp vụ | /10 | | |
| 5 | Thuật toán | /10 | | |
| 6 | Automation | /10 | | |
| 7 | Bulk Import | /10 | | |
| 8 | Phản biện | /10 | | |
| **Tổng** | | **/80** | | |
