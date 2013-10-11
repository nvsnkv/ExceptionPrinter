using System;
using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;

namespace ExceptionPrinter.Tests
{
    [TestFixture]
    public class WhenDumpingException
    {
        private StringBuilder _output;
        private Mock<TextWriter> _wrtr;
        private Printer _printer;

        [Test]
        public void PrinterShouldPrintExceptionMessage()
        {
            _printer.Print(new Exception("message"));
            Assert.That(_output.ToString().Contains("Message: message"));
        }

        [Test]
        public void PrinterShouldPrintExceptionType()
        {
            _printer.Print(new OutOfMemoryException("ouch!"));
            Assert.That(_output.ToString().Contains("Type: OutOfMemoryException"));
        }

        [Test]
        public void PrinterShouldPrintExceptionSource()
        {
            _printer.Print(new Exception(){Source = "SOURCE"});
            Assert.That(_output.ToString().Contains("Source: SOURCE"));
        }

        [Test]
        public void PrinterShouldPrintInnerExceptions()
        {
            _printer.Print(new Exception("msg", new ArgumentException("inner")));
            Assert.That(_output.ToString().Contains("InnerException:"));
            Assert.That(_output.ToString().Contains("    Type: ArgumentException"));
            Assert.That(_output.ToString().Contains("    Message: inner"));
        }

        [Test]
        public void PrinterShouldNotPrintEmptyExceptionMessage()
        {
            _printer.Print(new Exception(string.Empty));
            Assert.That(!_output.ToString().Contains("Message: "));
        }

        [Test]
        public void PrinterShouldNotPrintEmptyExceptionSource()
        {
            _printer.Print(new Exception());
            Assert.That(!_output.ToString().Contains("Source: "));
        }

        [SetUp]
        public void TestSetup()
        {
            _output = new StringBuilder();
            _wrtr = Create.MockOf.TextWriter.WithOutputStoredTo(_output);

            _printer = Create.Printer.WithoutDefaultWriters().WithCustomWriter(_wrtr.Object);
        }
    }
}