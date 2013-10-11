using System;
using System.IO;
using System.Text;
using Moq;

namespace ExceptionPrinter.Tests
{
    public static class Create
    {
        public static PrinterCreator Printer
        {
            get
            {
                return new PrinterCreator();
            }
        }

        public class MockOf
        {
            public static TextWriterMock TextWriter
            {
                get
                {
                    return new TextWriterMock();
                }
            }
        }
    }

    public class TextWriterMock
    {
        private StringBuilder _output;

        public static implicit operator Mock<TextWriter>(TextWriterMock self)
        {
            var mock = new Mock<TextWriter>();
            
            if (self._output != null)
            {
                mock.Setup(w => w.Write(It.IsAny<string>()))
                    .Callback((string s) => self._output.Append(s));
                mock.Setup(w => w.WriteLine(It.IsAny<string>()))
                    .Callback((string s) => self._output.AppendLine(s));
            }

            return mock;
        }

        public TextWriterMock WithOutputStoredTo(StringBuilder ouptut)
        {
            _output = ouptut;

            return this;
        }
    }
}