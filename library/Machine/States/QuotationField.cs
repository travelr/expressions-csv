﻿using FluentCsvMachine.Exceptions;

namespace FluentCsvMachine.Machine.States
{
    /// <summary>
    /// Machine reading quoted CSV fields according RFC 4180
    /// https://en.wikipedia.org/wiki/Comma-separated_values
    /// </summary>
    internal class QuotationField<T> : BaseElement where T : new()
    {
        internal enum States
        {
            Initial, //Initial -> Closed and Delimiter or line-break
            Running, // Quote Open
            Escape, // Process next char regardless if it is a quote
            Closed, // Second Quote
            FastForward
        }

        internal States State { get; private set; }


        private readonly Line<T> line;


        public QuotationField(Line<T> lineMachine, CsvConfiguration config) : base(config)
        {
            line = lineMachine;
        }

        /// <summary>
        /// Process the current char in the stream
        /// </summary>
        /// <param name="c">current char</param>
        public void Process(char c)
        {
            switch (c, State)
            {
                case { State: States.Initial } t when t.c == Quote:
                    // First quote
                    State = line.Parser != null ? States.Running : States.FastForward;
                    break;

                case { State: States.Running } t when t.c != Quote && t.c != QuoteEscape:
                    // Quote content
                    line.Parser!.Process(c);
                    break;

                case { State: States.Running } t when t.c == Quote:
                    // Second quote
                    State = States.Closed;
                    break;

                case { State: States.Running } t when t.c == QuoteEscape:
                    // Escape next quote: needs to be after "when t.c == Quote:"
                    State = States.Escape;
                    break;

                case { State: States.Escape }:
                    // Process char and return back to Running
                    line.Parser!.Process(c);
                    State = States.Running;
                    break;

                case { State: States.Closed } t when t.c == Delimiter || t.c == NewLine:
                    // Second quote followed by a delimiter or line break
                    CloseField();
                    break;

                case { State: States.Closed } t when t.c == Quote:
                    // Quote inside a quoted field ""hi"" -> "hi"
                    // Escape char is a quote therefore Escape state is not used - achieved by the order of statements
                    line.Parser!.Process(Quote);
                    State = States.Running;
                    break;

                case { State: States.FastForward } t:
                    if (t.c == Delimiter || t.c == NewLine)
                    {
                        CloseField();
                    }

                    break;

                default:
                    throw new CsvMachineException($"Unknown Quotation state == {State} && c == '{c}'");
            }
        }

        private void CloseField()
        {
            line.Value();
            State = States.Initial;
        }
    }
}