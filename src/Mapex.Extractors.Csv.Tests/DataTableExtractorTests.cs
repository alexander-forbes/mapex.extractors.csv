using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;

namespace Mapex.Extractors.Csv.Tests
{
	[TestFixture]
	public class When_calling_extract_on_data_table_extractor
	{
		private DataTable _Result;

		[SetUp]
		public void Setup()
		{
			var bytes = ResourceReader.ReadAsByteArray("Mapex.Extractors.Csv.Tests.Resources.PriceDistribution.csv");
			var options = new ExtractOptions { RowHeader = true, Delimiter = '\t', AutoMap = true };

			var extractor = new DataTableExtractor();
			_Result = extractor.Extract(bytes, options);
		}

		[Test]
		public void It_should_extract_a_data_table_from_the_byte_array_with_the_correct_number_of_rows()
		{
			var expectedColumns = new[]
			{
				"Message Type Code", "Message Type Name", "Type Code", "Type Name", "ManCo Code", "ManCo Name", "Fund Code",
					"Fund Name", "Value Date", "Price", "Date Submitted", "Status"
			};

			AssertThatDataTableColumnsMatch(_Result.Columns, expectedColumns);
		}

		[Test]
		public void It_should_extract_a_data_table_from_the_byte_array_with_the_correct_number_of_columns()
		{
			var expectedRows = new[]
			{
				new [] { "PD", "Price Distribution", "TY", "Today's accrual in cpu", "ASHUT", "Ashburton Management Company", "ASMMB1", "Ashburton Money Market Fund B1", "25/03/2018", "0.020632", "23/03/2018 13:48:26", "ORIGINAL" },
				new [] { "PD", "Price Distribution", "AY", "Average annualised effective yield", "ASHUT", "Ashburton Management Company", "ASMMB1", "Ashburton Money Market Fund B1", "25/03/2018", "7.799844", "23/03/2018 13:48:26", "ORIGINAL" }
			};

			AssertThatDataTableRowsMatch(_Result.Rows, expectedRows);
		}

		private static void AssertThatDataTableColumnsMatch(IEnumerable columnCollection, IEnumerable<string> expectedColumns)
		{
			CollectionAssert.AreEquivalent(columnCollection.Cast<object>().Select(c => c.ToString()), expectedColumns);
		}

		private static void AssertThatDataTableRowsMatch(DataRowCollection rowCollection, IReadOnlyCollection<string[]> expectedRowValues)
		{
			Assert.AreEqual(rowCollection.Count, expectedRowValues.Count);

			for (var rowIndex = 0; rowIndex < expectedRowValues.Count; rowIndex++)
			{
				var row = expectedRowValues.ElementAt(rowIndex);
				for (var colIndex = 0; colIndex < row.Length; colIndex++)
					Assert.AreEqual(rowCollection[rowIndex][colIndex], row.ElementAt(colIndex));
			}
		}
	}

	[TestFixture]
	public class When_calling_extract_on_data_table_extractor_with_no_rows
	{
		[Test]
		public void It_should_throw_an_exception()
		{
			var bytes = new byte[] { };
			var options = new ExtractOptions { RowHeader = true, Delimiter = '\t', AutoMap = true };

			var extractor = new DataTableExtractor();
			var exception = Assert.Throws<Exception>(() => extractor.Extract(bytes, options));

			Assert.AreEqual("No rows were found.", exception.Message);
		}
	}
}
