using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BTL_LTTQ.Common.Reports
{
    /// <summary>Tạo RDLC local theo danh sách cột của báo cáo động.</summary>
    internal static class RdlcRuntimeBuilder
    {
        private static readonly XNamespace Rdl = "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition";
        private static readonly XNamespace Rd = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";

        public static byte[] Tao(string tieuDe, IReadOnlyList<string> tieuDeCot)
        {
            if (tieuDeCot == null || tieuDeCot.Count == 0)
                throw new ArgumentException("RDLC cần ít nhất một cột dữ liệu.", nameof(tieuDeCot));

            double tongRong = Math.Max(7.0, Math.Min(10.5, tieuDeCot.Count * 1.25));
            double rongCot = tongRong / tieuDeCot.Count;
            var fields = tieuDeCot.Select((_, i) => new XElement(Rdl + "Field",
                new XAttribute("Name", $"F{i}"),
                new XElement(Rdl + "DataField", $"F{i}"),
                new XElement(Rd + "TypeName", "System.String")));

            var colWidths = tieuDeCot.Select(_ => new XElement(Rdl + "TablixColumn",
                new XElement(Rdl + "Width", Inch(rongCot))));
            var headerCells = tieuDeCot.Select((label, i) => Cell($"Header{i}", label, true));
            var detailCells = tieuDeCot.Select((_, i) => Cell($"Detail{i}", $"=Fields!F{i}.Value", false));
            var colMembers = tieuDeCot.Select(_ => new XElement(Rdl + "TablixMember"));

            var report = new XElement(Rdl + "Report",
                new XAttribute(XNamespace.Xmlns + "rd", Rd),
                new XElement(Rdl + "AutoRefresh", 0),
                new XElement(Rdl + "DataSources",
                    new XElement(Rdl + "DataSource", new XAttribute("Name", "LocalData"),
                        new XElement(Rdl + "ConnectionProperties",
                            new XElement(Rdl + "DataProvider", "System.Data.DataSet"),
                            new XElement(Rdl + "ConnectString", "")),
                        new XElement(Rd + "DataSourceID", Guid.NewGuid().ToString()))),
                new XElement(Rdl + "DataSets",
                    new XElement(Rdl + "DataSet", new XAttribute("Name", "ReportData"),
                        new XElement(Rdl + "Query",
                            new XElement(Rdl + "DataSourceName", "LocalData"),
                            new XElement(Rdl + "CommandText", "")),
                        new XElement(Rdl + "Fields", fields))),
                new XElement(Rdl + "ReportSections",
                    new XElement(Rdl + "ReportSection",
                        new XElement(Rdl + "Body",
                            new XElement(Rdl + "ReportItems",
                                Textbox("ReportTitle", tieuDe, 0, 0, tongRong, 0.42, true),
                                Textbox("ReportDate", "Ngày lập: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), 0, 0.45, tongRong, 0.25, false),
                                new XElement(Rdl + "Tablix", new XAttribute("Name", "ReportTable"),
                                    new XElement(Rdl + "TablixBody",
                                        new XElement(Rdl + "TablixColumns", colWidths),
                                        new XElement(Rdl + "TablixRows",
                                            TablixRow(0.34, headerCells),
                                            TablixRow(0.28, detailCells))),
                                    new XElement(Rdl + "TablixColumnHierarchy",
                                        new XElement(Rdl + "TablixMembers", colMembers)),
                                    new XElement(Rdl + "TablixRowHierarchy",
                                        new XElement(Rdl + "TablixMembers",
                                            new XElement(Rdl + "TablixMember",
                                                new XElement(Rdl + "KeepWithGroup", "After"),
                                                new XElement(Rdl + "RepeatOnNewPage", true),
                                                new XElement(Rdl + "KeepTogether", true)),
                                            new XElement(Rdl + "TablixMember",
                                                new XElement(Rdl + "Group", new XAttribute("Name", "Details"))))),
                                    new XElement(Rdl + "DataSetName", "ReportData"),
                                    new XElement(Rdl + "Top", "0.78in"),
                                    new XElement(Rdl + "Left", "0in"),
                                    new XElement(Rdl + "Height", Inch(0.62)),
                                    new XElement(Rdl + "Width", Inch(tongRong)),
                                    new XElement(Rdl + "Style"))),
                            new XElement(Rdl + "Height", "1.55in"),
                            new XElement(Rdl + "Style")),
                        new XElement(Rdl + "Width", Inch(tongRong)),
                        new XElement(Rdl + "Page",
                            new XElement(Rdl + "PageHeight", "11.7in"),
                            new XElement(Rdl + "PageWidth", tongRong > 8.0 ? "11.7in" : "8.3in"),
                            new XElement(Rdl + "LeftMargin", "0.3in"),
                            new XElement(Rdl + "RightMargin", "0.3in"),
                            new XElement(Rdl + "TopMargin", "0.4in"),
                            new XElement(Rdl + "BottomMargin", "0.4in"),
                            new XElement(Rdl + "Style")))),
                new XElement(Rdl + "ConsumeContainerWhitespace", true),
                new XElement(Rd + "ReportID", Guid.NewGuid().ToString()));

            return Encoding.UTF8.GetBytes(new XDocument(new XDeclaration("1.0", "utf-8", null), report)
                .ToString(SaveOptions.DisableFormatting));
        }

        private static XElement TablixRow(double height, IEnumerable<XElement> cells) =>
            new(Rdl + "TablixRow",
                new XElement(Rdl + "Height", Inch(height)),
                new XElement(Rdl + "TablixCells", cells));

        private static XElement Cell(string name, string value, bool header) =>
            new(Rdl + "TablixCell",
                new XElement(Rdl + "CellContents",
                    Textbox(name, value, 0, 0, 1, header ? 0.34 : 0.28, header)));

        private static XElement Textbox(string name, string value, double left, double top,
            double width, double height, bool bold)
        {
            bool laTextBoxTrang = name is "ReportTitle" or "ReportDate";
            var noiDung = new List<object>
            {
                new XElement(Rdl + "CanGrow", true),
                new XElement(Rdl + "KeepTogether", true),
                new XElement(Rdl + "Paragraphs",
                    new XElement(Rdl + "Paragraph",
                        new XElement(Rdl + "TextRuns",
                            new XElement(Rdl + "TextRun",
                                new XElement(Rdl + "Value", value),
                                new XElement(Rdl + "Style",
                                    bold ? new XElement(Rdl + "FontWeight", "Bold") : null,
                                    new XElement(Rdl + "FontSize", "9pt")))),
                        new XElement(Rdl + "Style"))),
                new XElement(Rdl + "Style",
                    new XElement(Rdl + "Border",
                        new XElement(Rdl + "Style", "Solid")),
                    bold ? new XElement(Rdl + "BackgroundColor", "#DCE8F7") : null,
                    new XElement(Rdl + "PaddingLeft", "4pt"),
                    new XElement(Rdl + "PaddingRight", "4pt"),
                    new XElement(Rdl + "PaddingTop", "2pt"),
                    new XElement(Rdl + "PaddingBottom", "2pt"))
            };
            if (laTextBoxTrang)
            {
                noiDung.Add(new XElement(Rdl + "Top", Inch(top)));
                noiDung.Add(new XElement(Rdl + "Left", Inch(left)));
                noiDung.Add(new XElement(Rdl + "Height", Inch(height)));
                noiDung.Add(new XElement(Rdl + "Width", Inch(width)));
            }
            return new XElement(Rdl + "Textbox", new XAttribute("Name", name), noiDung);
        }

        private static string Inch(double value) => value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "in";
    }
}
