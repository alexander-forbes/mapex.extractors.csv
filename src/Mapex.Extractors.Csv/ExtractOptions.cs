using YamlDotNet.Serialization;

namespace Mapex.Extractors.Csv
{
	public class ExtractOptions
    {
	    [YamlMember(Alias = "automap", ApplyNamingConventions = false)]
		public bool AutoMap { get; set; }

	    [YamlMember(Alias = "delimiter", ApplyNamingConventions = false)]
	    public char Delimiter { get; set; }

	    [YamlMember(Alias = "rowheader", ApplyNamingConventions = false)]
	    public bool RowHeader { get; set; }
	}
}
