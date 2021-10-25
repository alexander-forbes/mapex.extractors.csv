using NUnit.Framework;

namespace Mapex.Extractors.Csv.Tests
{
	[TestFixture]
	public class When_constructing_extractor_options
	{
		[Test]
		public void It_should_set_the_automap_property_to_false()
		{
			Assert.IsFalse(new ExtractOptions().AutoMap);
		}

		[Test]
		public void It_should_set_the_rowheader_property_to_false()
		{
			Assert.IsFalse(new ExtractOptions().RowHeader);
		}
	}
}
