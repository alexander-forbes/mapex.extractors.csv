using System.Data;
using System.Linq;
using NUnit.Framework;

namespace Mapex.Extractors.Csv.Tests
{
	[TestFixture]
	public class When_calling_build_on_object_builder
	{
		[Test]
		public void It_should_build_expando_objects_from_the_data_table()
		{
			var table = CreateDataTable();
			var builder = new ObjectBuilder();

			var objects = builder.Build(table);

			Assert.IsTrue(Match(objects.ElementAt(0), "John", "Doe", "0123456789"));
			Assert.IsTrue(Match(objects.ElementAt(1), "Joe", "Soap", "9876543210"));
		}

		private static bool Match(dynamic obj, string name, string surname, string telNo)
		{
			Assert.AreEqual(obj.Name, name);
			Assert.AreEqual(obj.Surname, surname);
			Assert.AreEqual(obj.TelNo, telNo);
			return true;
		}

		private static DataTable CreateDataTable()
		{
			var table = new DataTable();
			table.Columns.Add("Name");
			table.Columns.Add("Surname");
			table.Columns.Add("Tel No");

			var row = table.NewRow();
			row["Name"] = "John";
			row["Surname"] = "Doe";
			row["Tel No"] = "0123456789";
			table.Rows.Add(row);

			row = table.NewRow();
			row["Name"] = "Joe";
			row["Surname"] = "Soap";
			row["Tel No"] = "9876543210";
			table.Rows.Add(row);

			return table;
		}
	}
}
