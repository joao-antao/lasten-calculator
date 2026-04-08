using System.Collections.Frozen;
using ClosedXML.Excel;
using Lasten.Domain.Gemeentelijkebelastingen;
using Lasten.Infrastructure.Extensions;

namespace Lasten.Infrastructure;

/// <summary>
/// Loads municipality tax data from the COELO Excel workbook into <see cref="Gemeente"/> instances.
/// </summary>
public static class GemeentenLoader
{
    // Column indices (0-based) in the COELO spreadsheet after skipping 4 header rows
    private const int ColCode = 1;     // Unique code for the municipality
    private const int ColNaam = 2;     // Name of the municipality
    private const int ColOzb = 3;      // OZB tariff tax as percentage column
    private const int ColAfval1p = 13; // Single-person household waste tax column
    private const int ColAfvalMp = 15; // Multi-person household waste tax column
    private const int ColRiool1p = 18; // Single-person household sewerage tax column
    private const int ColRioolMp = 20; // Multi-person household sewerage tax column

    private const string WorksheetName = "Gegevens per gemeente"; // Worksheet name containing relevant properties

    public static FrozenDictionary<string, Gemeente> Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Coelo/Gemeentelijke_belastingen_2025.xlsx");
        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(WorksheetName);

        var result = new Dictionary<string, Gemeente>();

        // Data starts at row 5 (4 header rows + 1-based index)
        const int firstDataRow = 5;
        int lastRow = sheet.LastRowUsed()?.RowNumber() ?? firstDataRow;

        for (int row = firstDataRow; row <= lastRow; row++)
        {
            var name = sheet.Cell(row, ColNaam + 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var code = sheet.Cell(row, ColCode + 1).GetString().Trim().PadLeft(4, '0');
            var ozb = sheet.Cell(row, ColOzb + 1).GetDecimalOrDefault();
            var afval1p = sheet.Cell(row, ColAfval1p + 1).GetDecimalOrDefault();
            var afvalmp = sheet.Cell(row, ColAfvalMp + 1).GetDecimalOrDefault();
            var riool1p = sheet.Cell(row, ColRiool1p + 1).GetDecimalOrDefault();
            var rioolmp = sheet.Cell(row, ColRioolMp + 1).GetDecimalOrDefault();

            if (ozb == 0 && afval1p == 0)
                continue;

            result[name] = new Gemeente(code, name, ozb, afval1p, afvalmp, riool1p, rioolmp);
        }

        return result.ToFrozenDictionary();
    }

}