# BỘ TÀI LIỆU CHUẨN BỨC BẢO VỤ ĐỒ ÁN

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
| 6 | [`06-BULK-IMPORT.md`](06-BULK-IMPORT.md) | Pipeline nhập hàng loạt, all-or-nothing, so sánh INSERT/SqlBulkCopy/staging, tự sinh lịch học khi đăng ký | Ôn vòng 7 |
| 7 | [`07-CAU-HOI-PHAN-BIEN.md`](07-CAU-HOI-PHAN-BIEN.md) | 38 câu hỏi nhóm A–G, mỗi câu có **3 mức trả lời** (15–30s / 45–90s / đào sâu) | Ôn sâu trước buổi thi |
| 8 | [`08-KICH-BAN-DEMO.md`](08-KICH-BAN-DEMO.md) | Kịch bản demo 15 phút, tài khoản demo, dữ liệu chuẩn bị, thao tác cần tránh, phương án dự phòng | Chuẩn bị ngay trước buổi bảo vệ |
| 9 | [`09-MOCK-DEFENSE.md`](09-MOCK-DEFENSE.md) | Quy trình luyện 8 vòng, rubric chấm điểm, ngân hàng câu hỏi dự phòng | Khi luyện với tôi |

## 5 điều phải nói trước khi giảng viên nói (tự nhận thẳng)

1. **Chưa có UI cho Học viên / Giáo viên / Danh mục khác / Báo cáo** — nút menu có, nhưng không có form.
2. **Module Workflow Automation và bộ xếp lịch CP-SAT đã gỡ** khỏi ứng dụng; không còn code, bảng, nút phát sự kiện.
3. **Lịch học (`BuoiHoc`) được tạo tự động** khi có đăng ký lớp (thủ công hoặc nhập hàng loạt), trong cùng transaction với đăng ký; `LopHoc` không còn cần bước "xếp lịch" thủ công.
4. **Module Bulk Import** đã có đủ: Preview → Confirm → all-or-nothing → xuất báo cáo lỗi → drag-drop.

## Bắt đầu luyện

Nhắn: **`bắt đầu mock defense`** (hoặc `mock vòng N`) — tôi sẽ đóng vai giảng viên, hỏi **mỗi lượt một câu**, chờ bạn trả lời, rồi chấm điểm /10 và hỏi tiếp câu xoáy sâu.
