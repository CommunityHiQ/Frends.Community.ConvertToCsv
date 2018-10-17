using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

#pragma warning disable 1591

namespace Frends.Community.ConvertXmlToCsv
{
    public class Input
    {
        /// <summary>
        /// XML string to be converted into csv
        /// </summary>
        [DisplayName("Input XML as string")]
        public string InputXmlString { get; set; }

        /// <summary>
        /// Separator for the output columns.
        /// </summary>
        [DefaultValue("\",\"")]
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

    public class ConvertXmlToCsv
    {
        /// <summary>
        /// Convert xml or json data into the csv formated data. Errors are always thrown by an exception. See: https://github.com/CommunityHiQ/Frends.Community.ConvertToCsv
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Object {string Result }</returns>
        public static Output ConvertXmlToCsvTask(Input input, CancellationToken cancellationToken)
        {
            DataSet dataset;
            dataset = new DataSet();
            dataset.ReadXml(XmlReader.Create(new StringReader(input.InputXmlString)));

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
