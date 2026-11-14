# BỘ TÀI LIỆU CHUẨN BỊ BẢO VỆ ĐỒ ÁN

**Đồ án**: Quản lý lớp học ngắn hạn (Tin học & Ngoại ngữ) — C# WinForms + SQL Server.

> Toàn bộ nội dung trong thư mục này được viết từ **source code và database thật** của dự án
> (`D:\BTL_LTTQ`), không dựa trên giả định. Tài liệu **ghi rõ cả phần chưa làm** và các giới hạn.

## Đọc theo thứ tự nào?

| # | File | Dùng để làm gì | Đọc khi nào |
|---|---|---|---|
| 0 | [`00-TONG-QUAN-VA-TRANG-THAI.md`](00-TONG-QUAN-VA-TRANG-THAI.md) | Tổng quan nghiệp vụ, bảng trạng thái tính năng (Completed/Partially/Designed/Not Implemented), sơ đồ kiến trúc, 3 luồng dữ liệu thật | **Đọc đầu tiên** — đây là "bản đồ" để bạn tự kiểm tra mình thuộc phần nào |
| 1 | [`01-KIEN-TRUC-VA-ADO-NET.md`](01-KIEN-TRUC-VA-ADO-NET.md) | Phân tầng, ADO.NET dùng ở đâu, transaction, exception handling, bảo mật | Ôn vòng 2–3 |
| 2 | [`02-DATABASE.md`](02-DATABASE.md) | Schema thật, PK/FK/unique/CHECK/index, quan hệ 1-N / N-N, cảnh báo về `HocVien.MaLop` | Ôn vòng 3 |
| 3 | [`03-NGHIEP-VU.md`](03-NGHIEP-VU.md) | Từng module: lớp học, môn học, đăng ký, tra cứu, phân quyền, xuất Excel + những phần **chưa có UI** | Ôn vòng 4 |
| 4 | [`04-SMART-TIMETABLE.md`](04-SMART-TIMETABLE.md) | Xếp lịch CP-SAT: biến, domain, ràng buộc cứng/mềm, hàm mục tiêu, trường hợp khó, giới hạn | Ôn vòng 5 |
| 5 | [`05-AUTOMATION-RULESENGINE.md`](05-AUTOMATION-RULESENGINE.md) | Rule/Condition/Action, phần nào của RulesEngine vs code tự viết, dedup/cooldown, execution history, an toàn | Ôn vòng 6 |
| 6 | [`06-BULK-IMPORT.md`](06-BULK-IMPORT.md) | Pipeline nhập hàng loạt, all-or-nothing, so sánh INSERT/SqlBulkCopy/staging, quan hệ với Automation, phạm vi "không có Import History" | Ôn vòng 7 |
| 7 | [`07-CAU-HOI-PHAN-BIEN.md`](07-CAU-HOI-PHAN-BIEN.md) | 38 câu hỏi nhóm A–G, mỗi câu có **3 mức trả lời** (15–30s / 45–90s / đào sâu) | Ôn sâu trước buổi thi |
| 8 | [`08-KICH-BAN-DEMO.md`](08-KICH-BAN-DEMO.md) | Kịch bản demo 15 phút, tài khoản demo, dữ liệu chuẩn bị, thao tác cần tránh, phương án dự phòng | Chuẩn bị ngay trước buổi bảo vệ |
| 9 | [`09-MOCK-DEFENSE.md`](09-MOCK-DEFENSE.md) | Quy trình luyện 8 vòng, rubric chấm điểm, ngân hàng câu hỏi dự phòng | Khi luyện với tôi |
| 10 | [`10-HUONG-DAN-TEST-AUTOMATION.md`](10-HUONG-DAN-TEST-AUTOMATION.md) | Hướng dẫn test module Automation từng bước (test điều kiện, test end-to-end có action thật, SQL kiểm chứng, lỗi thường gặp, dọn dẹp) | Trước khi demo hoặc khi giảng viên hỏi "chứng minh nó chạy" |

## 5 điều phải nói trước khi giảng viên nói (tự nhận thẳng)

1. **Chưa có UI cho Học viên / Giáo viên / Danh mục khác / Báo cáo** — nút menu có, nhưng không có form.
2. **Chỉ `CLASS_ENROLLMENT_CHANGED` có nơi phát sự kiện**; `CLASS_CREATED` và `CLASS_STATUS_CHANGED` chưa được phát nên rule của 2 trigger đó chưa bao giờ chạy.
3. **`CLASS_CREATED` và `CLASS_STATUS_CHANGED` chưa có nơi phát sự kiện** ⇒ rule của 2 trigger này chưa bao giờ chạy.
4. **`BULK_IMPORT_COMPLETED` không tạo được Rule** vì CHECK constraint DB chỉ cho 3 trigger lớp.
5. **Xếp lịch chưa có Preview** và **ghi đè toàn bộ** `BuoiHoc` khi bấm "Tạo thời khóa biểu".

## Bắt đầu luyện

Nhắn: **`bắt đầu mock defense`** (hoặc `mock vòng N`) — tôi sẽ đóng vai giảng viên, hỏi **mỗi lượt một câu**, chờ bạn trả lời, rồi chấm điểm /10 và hỏi tiếp câu xoáy sâu.
