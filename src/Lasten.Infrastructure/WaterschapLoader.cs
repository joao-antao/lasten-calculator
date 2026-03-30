using ClosedXML.Excel;
using Lasten.Domain.Waterschapsbelastingen;
using Lasten.Infrastructure.Extensions;

namespace Lasten.Infrastructure;

public static class WaterschapLoader
{
    // Column indices (0-based) after skipping 5 header rows
    private const int ColCode            = 0;
    private const int ColNaam            = 1;
    private const int ColZuivering1P     = 2;
    private const int ColZuiveringMP     = 3;
    private const int ColIngezetenen     = 6;
    private const int ColGebouwd         = 8;   // % of WOZ
    private const int ColWegenInget      = 15;  // wegenheffing ingezetenen (€), 0 if not applicable

    private const string WorksheetName = "Gegevens per waterschap";
    
    public static IReadOnlyDictionary<string, Waterschap> Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Coelo/Waterschapsbelastingen_2025.xlsx");
        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(WorksheetName);

        var result = new Dictionary<string, Waterschap>();

        // Data starts at row 6 (5 header rows + 1-based index)
        const int firstDataRow = 6;
        int lastRow = sheet.LastRowUsed()?.RowNumber() ?? firstDataRow;

        for (int row = firstDataRow; row <= lastRow; row++)
        {
            var name = sheet.Cell(row, ColNaam + 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(name)) 
                continue;

            var code = sheet.Cell(row, ColCode + 1).GetString().Trim().PadLeft(4, '0');

            var waterschap = new Waterschap(
                Code:                    code,
                Name:                    name,
                ZuiveringsheffingEen:    sheet.Cell(row, ColZuivering1P + 1).GetDecimalOrDefault(),
                ZuiveringsheffingMeer:   sheet.Cell(row, ColZuiveringMP + 1).GetDecimalOrDefault(),
                WatersysteemIngezetenen: sheet.Cell(row, ColIngezetenen + 1).GetDecimalOrDefault(),
                WatersysteemGebouwd:     sheet.Cell(row, ColGebouwd + 1).GetDecimalOrDefault(),
                WegenIngezetenen:        sheet.Cell(row, ColWegenInget + 1).GetDecimalOrDefault()
            );

            result[code] = waterschap;
        }

        return result;
    }
}
