using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace ExceptionPrinter
{
    public class Printer
    {
        #region Options
        public class Options
        {
            private static Options _default;

            public static Options Default
            {
                get
                {
                    return _default ?? (_default = CreateDefaultOptions());
                }
            }

            public bool UseStdErr { get; set; }

            public bool UseStdOut { get; set; }
            public string IndentIncrease { get; set; }
            public uint Depth { get; set; }

            private static Options CreateDefaultOptions()
            {
                return new Options
                {
                    UseStdErr = true,
                    UseStdOut = false,

                    IndentIncrease = "    ",
                    Depth = 3
                };
            }
        } 
        #endregion

        private readonly List<TextWriter> _writers = new List<TextWriter>();
        private string _indent;
        private readonly string _indentIncrease;
        private uint _depth;
        private uint _depthMax;

        public IEnumerable<TextWriter> Writers
        {
            get { return _writers; }
        }

        #region ctors
        public Printer()
            : this(Options.Default)
        {

        }

        public Printer(Options options)
        {
            if (options.UseStdErr)
                _writers.Add(Console.Error);

            if (options.UseStdOut)
                _writers.Add(Console.Out);

            if (options.IndentIncrease != null)
                _indentIncrease = options.IndentIncrease;

            _depthMax = options.Depth;
        } 
        #endregion

        public void AddWriter(TextWriter writer)
        {
            if (_writers.IndexOf(writer) == -1)
            {
                _writers.Add(writer);
            }
        }

        public void Print(Exception exception)
        {
            _depth++;
            if (_depth > _depthMax)
            {
                foreach (var writer in Writers)
                {
                    writer.WriteLine(_indent + "...");
                }
                _depth = 0;
                return;
            }

            if (_depth == 1)
                _indent = string.Empty;

            foreach (var writer in Writers)
            {
                writer.WriteLine(_indent + "Type: " + exception.GetType().Name);
                
                if (!string.IsNullOrEmpty(exception.Message))
                    writer.WriteLine(_indent + "Message: " + exception.Message);

                if (!string.IsNullOrEmpty(exception.Source))
                    writer.WriteLine(_indent + "Source: " + exception.Source);
            }

            if (exception.InnerException != null)
            {

                _indent += _indentIncrease;
                foreach (var writer in Writers)
                {
                    writer.WriteLine(_indent + "InnerException:");
                }
                Print(exception.InnerException);
            }
        }
    }
}
