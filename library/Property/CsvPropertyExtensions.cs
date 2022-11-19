﻿namespace FluentCsvMachine.Property
{
    /// <summary>
    /// Extensions which make the fluent definition of columns possible
    /// </summary>
    public static class CsvPropertyExtensions
    {
        public static CsvPropertyBase ColumnName(this CsvPropertyBase column, string columnName)
        {
            column.ColumnName = columnName;
            return column;
        }

        public static CsvPropertyBase InputFormat(this CsvPropertyBase column, string inputFormat)
        {
            column.InputFormat = inputFormat;
            return column;
        }
    }
}