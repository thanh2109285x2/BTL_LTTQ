using BTL_LTTQ.DAL;
using BTL_LTTQ.Models.Reports;
using System.Data;

namespace BTL_LTTQ.BUS.Reports
{
    /// <summary>
    /// Nguồn cấp dữ liệu cho báo cáo. Tách khỏi UI để có thể thay bằng
    /// dữ liệu thật (SQL) hoặc dữ liệu mẫu khi thiết kế.
    /// </summary>
    public interface IReportDataProvider
    {
        string Ten { get; }
        bool LaDuLieuMau { get; }
        DataTable LayDuLieu(ReportQueryBuilder.KetQua truyVan, DataSourceDefinition nguon);
    }

    /// <summary>Nguồn dữ liệu thật: chạy truy vấn tham số hóa qua <see cref="DbHelper"/>.</summary>
    public sealed class ReportSqlDataProvider : IReportDataProvider
    {
        public string Ten => "Cơ sở dữ liệu";
        public bool LaDuLieuMau => false;

        public DataTable LayDuLieu(ReportQueryBuilder.KetQua truyVan, DataSourceDefinition nguon)
            => DbHelper.ExecuteQuery(truyVan.Sql, truyVan.ThamSo);
    }
}
