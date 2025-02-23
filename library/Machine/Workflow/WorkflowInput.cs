﻿using FluentCsvMachine.Helpers;
using FluentCsvMachine.Property;

namespace FluentCsvMachine.Machine.Workflow
{
    internal class WorkflowInput<T>
    {
        /// <summary>
        /// Object in order to create a workflow
        /// Assign nullable objects outside of the constructor
        /// </summary>
        /// <param name="stream">Stream of the CSV file</param>
        /// <param name="properties">List of defined properties</param>
        /// <param name="searchForHeaders">True: Header needs to be found in CSV, False: Columns are predefined via CsvNoHeaderAttribute</param>
        /// <param name="config"><see cref="CsvConfiguration"/> - if null we are using the defaults</param>
        internal WorkflowInput(Stream stream, List<CsvPropertyBase> properties, bool searchForHeaders, CsvConfiguration? config)
        {
            Guard.IsNotNull(stream);
            Guard.IsNotNull(properties);

            Stream = stream;
            Properties = properties;
            SearchForHeaders = searchForHeaders;
            Config = config ?? new CsvConfiguration();


            if (Config.EntityQueueSize < 20)
            {
                ThrowHelper.ThrowCsvConfigurationException("Please choose a larger queue size. Values larger 20 are valid");
            }

            SetParsers();
        }

        internal Stream Stream { get; }

        internal List<CsvPropertyBase> Properties { get; }

        public bool SearchForHeaders { get; }


        internal CsvConfiguration Config { get; }

        internal List<Action<T, IReadOnlyList<object?>>>? LineActions { get; set; }

        /// <summary>
        /// Sets ValueParser of the Properties
        /// </summary>
        private void SetParsers()
        {
            var parserProvider = new ValueParserProvider();

            foreach (var p in Properties)
            {
                if (p.PropertyType != typeof(DateTime) && p.PropertyType != typeof(DateTime?))
                {
                    // DateTime requires InputFormat
                    // Therefore it is set by InputFormat.Set
                    p.ValueParser = parserProvider.GetParser(p.PropertyType!);
                }
                else
                {
                    p.ValueParser = parserProvider.GetParser(p.PropertyType, p.InputFormat);
                }
            }
        }
    }
}
