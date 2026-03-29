using ClosedXML.Excel;

namespace Lasten.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="IXLCell"/>.
/// </summary>
internal static class XlCellExtensions
{
    /// <summary>
    /// Returns the cell value as a <see cref="decimal"/>, or <c>0</c> when the value cannot be parsed.
    /// </summary>
    internal static decimal GetDecimalOrDefault(this IXLCell cell)
        => cell.TryGetValue<decimal>(out var value) ? value : 0m;
}

