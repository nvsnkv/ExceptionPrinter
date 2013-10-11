using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace ExceptionPrinter.Tests
{
    public class PrinterCreator
    {
        private Printer.Options _options;
        private readonly List<TextWriter> _writers = new List<TextWriter>();

        public PrinterCreator()
        {
            _options = Printer.Options.Default; 
        }
        public PrinterCreator WithDefaultParameters()
        {
            _options = Printer.Options.Default;

            return this;
        }

        public static implicit operator Printer(PrinterCreator self)
        {
            var printer = new Printer(self._options);
            
            foreach (var writer in self._writers)
            {
                printer.AddWriter(writer);
            }

            return printer;
        }

        public PrinterCreator WithoutDefaultWriters()
        {
            _options.UseStdErr = false;
            _options.UseStdOut = false;

            return this;
        }

        public PrinterCreator WithCustomWriter(TextWriter writer)
        {
            _writers.Add(writer);
            return this;
        }
    }
}