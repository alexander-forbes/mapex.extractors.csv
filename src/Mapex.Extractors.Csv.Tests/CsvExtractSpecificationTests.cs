using System.Linq;
using Moq;
using NUnit.Framework;

namespace Mapex.Extractors.Csv.Tests
{
	[TestFixture]
	public class When_constructing_a_csv_extract_specification
	{
		[Test]
		public void It_should_set_the_options_property_to_an_instance()
		{
			var specification = new CsvExtractSpecification();
			Assert.IsInstanceOf<ExtractOptions>(specification.Options);
		}
	}

	[TestFixture]
	public class When_calling_process_on_csv_extract_specification
	{
		[Test]
		public void It_should_return_a_list_of_expando_objects()
		{
			var bytes = ResourceReader.ReadAsByteArray("Mapex.Extractors.Csv.Tests.Resources.PriceDistribution.csv");
			var options = new ExtractOptions { RowHeader = true, Delimiter = '\t', AutoMap = true };

			var document = new Mock<IDocument>();
			document.Setup(d => d.Data).Returns(bytes);

			var specification = new CsvExtractSpecification { Options = options };

			var results = specification.Process(document.Object);

			Assert.IsTrue(Match(results.ElementAt(0), "PD", "Price Distribution", "TY", "Today's accrual in cpu", "ASHUT", "Ashburton Management Company", "ASMMB1", "Ashburton Money Market Fund B1", "25/03/2018", "0.020632", "23/03/2018 13:48:26", "ORIGINAL"));
			Assert.IsTrue(Match(results.ElementAt(1), "PD", "Price Distribution", "AY", "Average annualised effective yield", "ASHUT", "Ashburton Management Company", "ASMMB1", "Ashburton Money Market Fund B1", "25/03/2018", "7.799844", "23/03/2018 13:48:26", "ORIGINAL"));
		}

		private static bool Match(dynamic obj, string messageTypeCode, string messageTypeName, string typeCode, string typeName, string manCoCode, string manCoName,
			string fundCode, string fundName, string valueDate, string price, string dateSubmitted, string status)
		{
			Assert.AreEqual(obj.MessageTypeCode, messageTypeCode);
			Assert.AreEqual(obj.MessageTypeName, messageTypeName);
			Assert.AreEqual(obj.TypeCode, typeCode);
			Assert.AreEqual(obj.TypeName, typeName);
			Assert.AreEqual(obj.ManCoCode, manCoCode);
			Assert.AreEqual(obj.ManCoName, manCoName);
			Assert.AreEqual(obj.FundCode, fundCode);
			Assert.AreEqual(obj.FundName, fundName);
			Assert.AreEqual(obj.ValueDate, valueDate);
			Assert.AreEqual(obj.Price, price);
			Assert.AreEqual(obj.DateSubmitted, dateSubmitted);
			Assert.AreEqual(obj.Status, status);
			return true;
		}
	}
}
