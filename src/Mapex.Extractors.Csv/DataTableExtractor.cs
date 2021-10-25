using System;
using System.Data;
using System.IO;
using ExcelDataReader;
using Mapex.Extractors.Csv.Logging;

namespace Mapex.Extractors.Csv
{
	public interface IDataTableExtractor
	{
		DataTable Extract(byte[] data, ExtractOptions options);
	}

	public class DataTableExtractor : IDataTableExtractor
	{
		private static readonly ILog Log = LogProvider.For<DataTableExtractor>();

		public DataTable Extract(byte[] data, ExtractOptions options)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (!options.AutoMap)
				return null; //TODO: Implement manual mappings

			Log.Debug("Extracting data into data table...");

			var dataset = ReadContentAsDataSet(data, options);
			var values = GetDataSetValues(dataset);

			Log.Debug("Completed extracting data from dataset into data table.");

			return values;
		}

		private static DataSet ReadContentAsDataSet(byte[] content, ExtractOptions options)
		{
			var dataSetConfiguration = CreateExcelDataSetConfiguration(options);
			var readerConfiguration = CreateExcelReaderConfiguration(options);

			using var reader = ExcelReaderFactory.CreateCsvReader(new MemoryStream(content), readerConfiguration);

			Log.Debug($"Reading {content.Length} bytes into a dataset...");
			var result = reader.AsDataSet(dataSetConfiguration);
			Log.Debug("Completed reading data into a dataset...");
			return result;
		}

		private static ExcelReaderConfiguration CreateExcelReaderConfiguration(ExtractOptions options)
		{
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

			return new ExcelReaderConfiguration
			{
				AutodetectSeparators = new[] { options.Delimiter }
			};
		}

		private static ExcelDataSetConfiguration CreateExcelDataSetConfiguration(ExtractOptions options)
		{
			return new ExcelDataSetConfiguration
			{
				ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
				{
					UseHeaderRow = options.RowHeader
				}
			};
		}

		private static DataTable GetDataSetValues(DataSet dataset)
		{
			ValidateTableExists(dataset);
			ValidateRowsExist(dataset);

			return dataset.Tables[0];
		}

		private static void ValidateTableExists(DataSet ds)
		{
			if (ds.Tables.Count == 0)
				throw new Exception("No table data exists.");
		}

		private static void ValidateRowsExist(DataSet ds)
		{
			if (ds.Tables[0].Rows.Count == 0)
				throw new Exception("No rows were found.");
		}
	}
}
