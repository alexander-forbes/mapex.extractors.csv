using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mapex.Extractors.Csv.Logging;
using Mapex.Specifications;
using Notus;

namespace Mapex.Extractors.Csv
{
	public class CsvExtractSpecification : IExtractSpecification
	{
		private static readonly ILog Log = LogProvider.For<CsvExtractSpecification>();
		private readonly IDataTableExtractor _DataTableExtractor;
		private readonly IObjectBuilder _ObjectBuilder;

		public ExtractOptions Options { get; set; } = new ExtractOptions();

		public CsvExtractSpecification()
			: this(new DataTableExtractor(), new ObjectBuilder())
		{
		}

		internal CsvExtractSpecification(IDataTableExtractor dataTableExtractor, IObjectBuilder objectBuilder)
		{
			_DataTableExtractor = dataTableExtractor ?? throw new ArgumentNullException(nameof(dataTableExtractor));
			_ObjectBuilder = objectBuilder ?? throw new ArgumentNullException(nameof(objectBuilder));
		}

		public IEnumerable<ExpandoObject> Process(IDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));

			var table = _DataTableExtractor.Extract(document.Data, Options);
			var objects = _ObjectBuilder.Build(table);

			Log.Debug($"Succesfully created {objects.Count()} objects from document {document.Id}");

			return objects;
		}

		public void Validate(Notification notification)
		{
			if (Options.Delimiter == '\0')
				notification.AddError("A delimiter was not found in the Options property.");
		}
	}
}
