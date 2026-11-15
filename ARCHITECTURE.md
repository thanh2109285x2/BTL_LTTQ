# Kiến trúc dự án

Ứng dụng tuân theo hướng phụ thuộc một chiều sau:

`Forms -> BUS -> DAL`

| Tầng | Thư mục trong project | Trách nhiệm |
| --- | --- | --- |
| Forms | `BTL_LTTQ/Forms/` | Giao diện Windows Forms, tương tác với người dùng và hiển thị kết quả/lỗi do service trả về. |
| BUS | `BTL_LTTQ/BUS/` | Kiểm tra nghiệp vụ và điều phối giữa form với tầng truy cập dữ liệu. |
| DAL | `BTL_LTTQ/DAL/`, `BTL_LTTQ/DAL/Repositories/` | Truy cập cơ sở dữ liệu và thực thi SQL thông qua `DbHelper`. |
| Dùng chung | `BTL_LTTQ/Common/`, `BTL_LTTQ/Models/` | Các thành phần dùng chung (giao diện, bảo mật, hằng số) và các lớp mô hình dữ liệu được các tầng sử dụng. |