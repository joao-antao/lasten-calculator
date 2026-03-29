using ClosedXML.Excel;
using Lasten.Domain;

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

    public static IReadOnlyList<Gemeente> Load(string path)
    {
        using var workbook = new XLWorkbook(path);
        var sheet = workbook.Worksheet(WorksheetName);

        var result = new List<Gemeente>();

        // Data starts at row 5 (4 header rows + 1-based index)
        const int firstDataRow = 5;
        int lastRow = sheet.LastRowUsed()?.RowNumber() ?? firstDataRow;

        for (int row = firstDataRow; row <= lastRow; row++)
        {
            var name = sheet.Cell(row, ColNaam + 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var code = sheet.Cell(row, ColCode + 1).GetString().Trim().PadLeft(4, '0');
            var ozb = GetDecimal(sheet, row, ColOzb + 1);
            var afval1p = GetDecimal(sheet, row, ColAfval1p + 1);
            var afvalmp = GetDecimal(sheet, row, ColAfvalMp + 1);
            var riool1p = GetDecimal(sheet, row, ColRiool1p + 1);
            var rioolmp = GetDecimal(sheet, row, ColRioolMp + 1);

            if (ozb == 0 && afval1p == 0)
                continue;

            result.Add(new Gemeente(code, name, ozb, afval1p, afvalmp, riool1p, rioolmp));
        }

        return result;
    }

    private static decimal GetDecimal(IXLWorksheet sheet, int row, int col)
    {
        var cell = sheet.Cell(row, col);
        return cell.TryGetValue<decimal>(out var value) ? value : 0m;
    }
}