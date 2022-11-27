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
        }


        internal Stream Stream { get; }

        internal List<CsvPropertyBase> Properties { get; }

        public bool SearchForHeaders { get; }


        internal CsvConfiguration Config { get; }

        internal List<Action<T, IReadOnlyList<object?>>>? LineActions { get; set; }
    }
}
