using System.Threading;
using Frends.Community.ConvertToCSV.Tests.CsvValidation;
using FRENDS;
using NUnit.Framework;

namespace Frends.Community.ConvertToCSV.Tests
{
    [TestFixture]
    public class ConvertToCSVTests
    {
        [Test]
        public void TestConvertXmlToCsv()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.TestXml,
                FileType = ConvertToCsv.FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual(ConvertToCsvTestData.ExpectedCsvResult, result.Result);
        }

        public void TestConvertXmlToCsvWithSpecialCharacters()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = "<root><v1>foo1</v1><v2>bar2;bar2</v2><v3>baz3\r\nbaz3</v3><v4>\"fo\"o4\"</v4></root>",
                FileType = ConvertToCsv.FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
            System.Console.WriteLine(result.Result);
            Assert.AreEqual("v1;v2;v3;v4\r\nfoo1;\"bar2;bar2\";\"baz3\nbaz3\";\"\"\"fo\"\"o4\"\"\"\r\n", result.Result);
        }

    [Test]
        [ExpectedException("System.Xml.XmlException")]
        public void TestConvertToCSVXML_WithWrongFileType_ShouldThrowAnException()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = ConvertToCsv.FileType.Xml,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
        }

        [Test]
        [ExpectedException("System.IO.InvalidDataException")]
        public void TestConvertToCSVJSON_ThrowsError_WhenInvalidJSONSchemaIsUsed()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.InvalidJson,
                FileType = ConvertToCsv.FileType.Json,
                CsvSeparator = ";",
                IncludeHeaders = true
            };

            ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
        }

        [Test]
        public void TestConvertJsonToCsv()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = ConvertToCsv.FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
            Assert.AreEqual(ConvertToCsvTestData.ExpectedCsvResult, result.Result);
        }

        [Test]
        public void TestConvertToCSVJSON_WorksWithoutHeaders()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = ConvertToCsv.FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = false
            };

            var result = ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
            Assert.IsFalse(result.Result.StartsWith("Title"));
        }

        [Test]
        public void TestConvertToCSVJSON_WorksWithHeaders()
        {
            var indata = new ConvertToCsv.Input
            {
                InputData = ConvertToCsvTestData.TestJson,
                FileType = ConvertToCsv.FileType.Json,
                CsvSeparator = ",",
                IncludeHeaders = true
            };

            var result = ConvertToCsv.ExecuteConvertToCsv(indata, new CancellationToken());
            Assert.IsTrue(result.Result.StartsWith("Title"));
        }
    }
}
