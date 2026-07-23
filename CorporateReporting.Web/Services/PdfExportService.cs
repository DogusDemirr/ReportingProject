using CorporateReporting.Web.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CorporateReporting.Web.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] CreateReportFile(
       ReportResultModel report,
       string reportTitle,
       string createdBy)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(24);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(column =>
                    {
                        column.Item().Text(reportTitle)
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        column.Item().PaddingTop(4).Text(text =>
                        {
                            text.Span("Oluşturan: ").SemiBold();
                            text.Span(createdBy);
                            text.Span("    |    ");
                            text.Span("Tarih: ").SemiBold();
                            text.Span(DateTime.Now.ToString(
                                "dd.MM.yyyy HH:mm"));
                        });

                        column.Item()
                            .PaddingTop(10)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten2);
                    });

                    page.Content().PaddingVertical(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in report.Columns)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        table.Header(header =>
                        {
                            foreach (var column in report.Columns)
                            {
                                header.Cell()
                                    .Background(Colors.Blue.Darken2)
                                    .Padding(6)
                                    .Text(column.DisplayName)
                                    .FontColor(Colors.White)
                                    .Bold();
                            }
                        });

                        foreach (var row in report.Rows)
                        {
                            foreach (var column in report.Columns)
                            {
                                row.TryGetValue(
                                    column.Name,
                                    out var value);

                                table.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten3)
                                    .Padding(6)
                                    .Text(FormatValue(value));
                            }
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Corporate Reporting · Sayfa ");

                            text.CurrentPageNumber();

                            text.Span(" / ");

                            text.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }

        private static string FormatValue(object? value)
        {
            return value switch
            {
                null => string.Empty,

                DateTime date => date.ToString("dd.MM.yyyy"),

                decimal decimalValue =>
                    decimalValue.ToString("N2"),

                double doubleValue =>
                    doubleValue.ToString("N2"),

                _ => value.ToString() ?? string.Empty
            };
        }
    }
}
