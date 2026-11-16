namespace BTL_LTTQ.Common.Reports
{
    /// <summary>
    /// Một lựa chọn cho ComboBox trong lưới chỉnh sửa: giữ giá trị gốc và nhãn hiển thị.
    /// Dùng chung để mọi cột chọn trong module hiển thị tiếng Việt mà vẫn lưu đúng enum/giá trị.
    /// </summary>
    public sealed class LuaChon<T>
    {
        public LuaChon(T giaTri, string nhan)
        {
            GiaTri = giaTri;
            Nhan = nhan;
        }

        public T GiaTri { get; }
        public string Nhan { get; }

        public override string ToString() => Nhan;
    }
}
