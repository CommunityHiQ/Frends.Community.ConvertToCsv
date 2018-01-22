using System.Xml;
using System.Threading;
using Frends.Community.ConvertToCsv.Tests.CsvValidation;
using NUnit.Framework;

namespace Frends.Community.ConvertToCsv.Tests
{
    [TestFixture]
    public class ConvertToCsvTests
    {
        [Test]
        public void TestConvertXmlToCsv()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.TestXml,
                FileType = FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual(ConvertToCsvTestData.ExpectedCsvResult, result.Result);
        }

        public void TestConvertXmlToCsvWithSpecialCharacters()
        {
            var indata = new Input
            {
                InputData = "<root><v1>foo1</v1><v2>bar2;bar2</v2><v3>baz3\r\nbaz3</v3><v4>\"fo\"o4\"</v4></root>",
                FileType = FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual("v1;v2;v3;v4\r\nfoo1;\"bar2;bar2\";\"baz3\nbaz3\";\"\"\"fo\"\"o4\"\"\"\r\n", result.Result);
        }

        [Test]
        public void TestConvertToCsv_Xml_WithWrongFileType_ShouldThrowAnException()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            Assert.Throws<XmlException>(() => ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken()));
        }

        [Test]
        public void TestConvertToCsv_Json_ThrowsError_WhenInvalidJSONSchemaIsUsed()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.InvalidJson,
                FileType = FileType.Json,
                CsvSeparator = ";",
                IncludeHeaders = true
            };

            Assert.Throws<System.IO.InvalidDataException>(() => ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken()));
        }

        [Test]
        public void TestConvertJsonToCsv()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken());
            Assert.AreEqual(ConvertToCsvTestData.ExpectedCsvResult, result.Result);
        }

        [Test]
        public void TestConvertToCsv_Json_WorksWithoutHeaders()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = false
            };

            var result = ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken());
            Assert.IsFalse(result.Result.StartsWith("Title"));
        }

        [Test]
        public void TestConvertToCsvJson_WorksWithHeaders()
        {
            var indata = new Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ConvertToCsvTask(indata, new CancellationToken());
            Assert.IsTrue(result.Result.StartsWith("Title"));
        }
    }
}
