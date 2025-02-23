﻿// ReSharper disable InconsistentNaming

namespace FluentCsvMachine.Test.Models
{
    /// <summary>
    /// https://github.com/snowindy/csv-test-data-generator
    /// email,first,last,age,street,city,state,zip,digid,date(2012/11/25),latitude,longitude,pick(RED|BLUE|YELLOW|GREEN|WHITE),string
    /// </summary>
    public class BigDataSet
    {
        public string? Email { get; set; }
        public string? First { get; set; }
        public string? Last { get; set; }
        public int Age { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int Zip { get; set; }
        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public BigDataSetPick Pick { get; set; }
        public string? String { get; set; }
    }

    public enum BigDataSetPick
    {
        RED,
        BLUE,
        YELLOW,
        GREEN,
        WHITE
    }
}