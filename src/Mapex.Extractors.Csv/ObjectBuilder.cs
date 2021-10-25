using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using Celloc.DataTable;
using Mapex.Extractors.Csv.Logging;

namespace Mapex.Extractors.Csv
{
	public interface IObjectBuilder
	{
		IEnumerable<ExpandoObject> Build(DataTable table);
	}

	public class ObjectBuilder : IObjectBuilder
	{
		private static readonly ILog Log = LogProvider.For<ObjectBuilder>();

		public IEnumerable<ExpandoObject> Build(DataTable table)
		{
			Log.Debug("Building objects from data table...");

			var records = GetDataTableRecords(table);

			var values = BuildObjects(records);
			Log.Debug($"Successfully built objects from {values.Count()} records.");
			return values;
		}

		private static IEnumerable<IDictionary<string, object>> GetDataTableRecords(DataTable table)
		{
			if (table == null)
				throw new ArgumentNullException(nameof(table));

			Log.Debug("Extracting data from data table...");

			var columnNames = GetTrimmedColumnNames(table.Columns);

			var values = table.GetValuesByRow((
				(0, 0),
				(table.Columns.Count - 1, table.Rows.Count - 1)));

			var result = values.Select(row => row
				.Select((value, index) => new KeyValuePair<string, object>(columnNames.ElementAt(index), value))
				.ToDictionary(s => s.Key, s => s.Value)
			).ToArray();

			Log.Debug($"Completed extracting {result.Length} records from data table.");

			return result;
		}

		private static IEnumerable<string> GetTrimmedColumnNames(ICollection columns)
		{
			Log.Debug($"Trimming spaces from {columns.Count} column names...");
			return columns.Cast<object>().Select(col => col.ToString().Replace(" ", ""));
		}

		private static IEnumerable<ExpandoObject> BuildObjects(IEnumerable<IDictionary<string, object>> values)
		{
			Log.Debug($"About to build objects from {values.Count()} records...");

			foreach (var row in values)
			{
				var e = new ExpandoObject();

				foreach (var kvp in row)
				{
					((IDictionary<string, object>)e).Add(kvp.Key, kvp.Value);
				}

				yield return e;
			}

			Log.Debug($"Successfully built objects from {values.Count()} records.");
		}
	}
}
