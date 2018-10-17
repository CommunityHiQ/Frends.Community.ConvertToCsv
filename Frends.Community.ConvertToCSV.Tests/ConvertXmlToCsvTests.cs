using System.Xml;
using System.Threading;
using Frends.Community.ConvertXmlToCsv.Tests.CsvValidation;
using NUnit.Framework;

namespace Frends.Community.ConvertXmlToCsv.Tests
{
    [TestFixture]
    public class ConvertXmlToCsvTests
    {
        [Test]
        public void TestConvertXmlToCsv()
        {
            var indata = new Input
            {
                InputXmlString = ConvertXmlToCsvTestData.TestXml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertXmlToCsv.ConvertXmlToCsvTask(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual(ConvertXmlToCsvTestData.ExpectedCsvResult, result.Result);
        }

        [Test]

        public void TestConvertXmlToCsvWithSpecialCharacters()
        {
            var indata = new Input
            {
                InputXmlString = "<root><v1>foo1</v1><v2>bar2;bar2</v2><v3>baz3\r\nbaz3</v3><v4>\"fo\"o4\"</v4></root>",
                CsvSeparator = ";",
                IncludeHeaders = true
            };

            var result = ConvertXmlToCsv.ConvertXmlToCsvTask(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual("v1;v2;v3;v4\r\nfoo1;\"bar2;bar2\";\"baz3\nbaz3\";\"\"\"fo\"\"o4\"\"\"\r\n", result.Result);
        }

        [Test]
        public void TestConvertToCsv_Xml_WithWrongFileType_ShouldThrowAnException()
        {
            var indata = new Input
            {
                InputXmlString = ConvertXmlToCsvTestData.TestJson,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            Assert.Throws<XmlException>(() => ConvertXmlToCsv.ConvertXmlToCsvTask(indata, new CancellationToken()));
        }

    }
}
