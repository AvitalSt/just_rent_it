using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Enums;
using JustRentItAPI.Services.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace JustRentItAPI.Services.Classes
{
    public class InterestExportService : IInterestExportService
    {
        private string TranslateStatus(RentStatus status)
        {
            return status switch
            {
                RentStatus.Pending => "בהמתנה",
                RentStatus.Confirmed => "אושרה",
                RentStatus.Cancelled => "בוטל",
                RentStatus.Paid => "שולם",
                _ => status.ToString()
            };
        }

        public byte[] GenerateExcel(List<InterestExportDTO> list)
        {
            //יוצרים אובייקט בזיכרון שמייצג קובץ אקסל
            using var package = new ExcelPackage();
            //package.Workbook מחזיק את כל האובייקט של כל הגיליון
            var ws = package.Workbook.Worksheets.Add("Interests");

            ws.View.RightToLeft = true;
            ws.Cells.Style.ReadingOrder = ExcelReadingOrder.RightToLeft;

            ws.Cells[1, 1].Value = "תאריך";
            ws.Cells[1, 2].Value = "שם מתעניינת";
            ws.Cells[1, 3].Value = "מייל מתעניינת";
            ws.Cells[1, 4].Value = "שם בעלת שמלה";
            ws.Cells[1, 5].Value = "מייל בעלת שמלה";
            ws.Cells[1, 6].Value = "שם שמלה";
            ws.Cells[1, 7].Value = "סטטוס";
            ws.Cells[1, 8].Value = "מחיר";
            ws.Cells[1, 9].Value = "הערות בכללי";
            ws.Cells[1, 10].Value = "עדכון מבעלת השמלה";
            ws.Cells[1, 11].Value = "עדכון מהמתעניינת";

            //עיצוב
            using (var range = ws.Cells["A1:K1"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                range.Style.ReadingOrder = ExcelReadingOrder.RightToLeft;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var row = i + 2;
                var item = list[i];

                ws.Cells[row, 1].Value = item.CreatedDate.ToString("yyyy-MM-dd");
                ws.Cells[row, 2].Value = item.UserName;
                ws.Cells[row, 3].Value = item.UserEmail;
                ws.Cells[row, 4].Value = item.OwnerName;
                ws.Cells[row, 5].Value = item.OwnerEmail;
                ws.Cells[row, 6].Value = item.DressName;
                ws.Cells[row, 7].Value = TranslateStatus(item.Status);
                ws.Cells[row, 8].Value = item.Price;
                ws.Cells[row, 9].Value = item.Notes;
                ws.Cells[row, 10].Value = item.OwnerComment;
                ws.Cells[row, 11].Value = item.UserComment;
            }

            // התאמת רוחב העמודות
            ws.Cells.AutoFitColumns();
            //byte[] הוא הדרך הסטנדרטית לייצג כל קובץ כך שהשרת יוכל להעביר אותו ללקוח דרך HTTP.
            return package.GetAsByteArray();
        }
    }
}
