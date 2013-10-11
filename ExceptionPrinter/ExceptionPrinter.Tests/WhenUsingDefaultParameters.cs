using System;
using System.Linq;
using NUnit.Framework;

namespace ExceptionPrinter.Tests
{
    [TestFixture]
    public class WhenUsingDefaultParameters
    {
        [Test]
        public void ExceptionPrinterShouldUseOnlyStdErr()
        {
            Printer printer = Create.Printer.WithDefaultParameters();
           
            Assert.That(printer.Writers.Count(), Is.EqualTo(1));
            Assert.That(printer.Writers.First(), Is.EqualTo(Console.Error));
        }
    }
}