using System.Collections.Generic;
using System.Diagnostics;

namespace IniFileParser.Models
{
    [DebuggerDisplay("Name: {Name}")]
    public class Section
    {
        public string Name { get; set; }
        public List<SectionValue> Values { get; set; } = new List<SectionValue>();

        public Section() { }
        public Section(string name)
            => Name = name;
    }
}
