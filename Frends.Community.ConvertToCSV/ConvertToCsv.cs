using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using FRENDS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Frends.Community.ConvertToCsv
{
    public enum FileType { Json, Xml }

    public class Input
    {
        /// <summary>
        /// Data to be converted into csv
        /// </summary>
        [DisplayName("Input data")]
        public string InputData { get; set; }

        /// <summary>
        /// File type. Either Xml or Json.
        /// </summary>
        [DefaultValue(FileType.Xml)]
        public FileType FileType { get; set; }

        /// <summary>
        /// Separator for the output columns.
        /// </summary>
        [DefaultValue("\";\"")]
        public string CsvSeparator { get; set; }

        /// <summary>
        /// True if the column headers should be included into the output.
        /// </summary>
        [DefaultValue(true)]
        public bool IncludeHeaders { get; set; }
    }
    /// <summary>
    /// Return object
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Result csv
        /// </summary>
        public string Result { get; set; }
    }

    public class ConvertToCsv
    {
        /// <summary>
        /// Convert xml or json data into the csv formated data. Errors are always thrown by an exception.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Object {string Result }</returns>
        public static Output ConvertToCsvTask(Input input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DataSet dataset;

            switch (input.FileType)
            {
                case FileType.Json:
                    // Validate indata against the JSON schema
                    var schema = JsonSchema.Parse(ValidSchemas.ValidJsonSchema);
                    var jsonInput = JObject.Parse(input.InputData);
                    if (!jsonInput.IsValid(schema))
                    {
                        throw new InvalidDataException("The input data does't comply with the schema");
                    }

                    dataset = JsonConvert.DeserializeObject<DataSet>(input.InputData);
                    break;

                case FileType.Xml:
                    dataset = new DataSet();
                    dataset.ReadXml(XmlReader.Create(new StringReader(input.InputData)));
                    break;

                default:
                    throw new InvalidDataException("The input data type was not recognized. Supported data types are XML and JSON");
            }

            return new Output { Result = ConvertDataTableToCsv(dataset.Tables[0], input.CsvSeparator, input.IncludeHeaders, cancellationToken)};
        }

        private static string ConvertDataTableToCsv(DataTable datatable, string separator, bool includeHeaders, CancellationToken cancellationToken)
        {
            var stringBuilder = new StringBuilder();

            if (includeHeaders)
            {
                var columnNames = datatable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                stringBuilder.AppendLine(string.Join(separator, columnNames));
            }

            foreach (DataRow row in datatable.Rows)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var fields = row.ItemArray.Select(field => field.ToString());
                fields = fields.Select(x => (x.Contains(separator) || x.Contains("\n") || x.Contains("\"")) ? "\"" + x.Replace("\"", "\"\"") + "\"" : x); // Fixes cases where input field contains special characters
                stringBuilder.AppendLine(string.Join(separator, fields));
            }

            return stringBuilder.ToString();
        }
    }
}
