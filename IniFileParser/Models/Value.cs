using System.Diagnostics;

namespace IniFileParser.Models
{
    [DebuggerDisplay("Key: {Key} Value = {Value}")]
    public class SectionValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public SectionValue() { }
        public SectionValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public void Deconstruct(out string key, out string value)
        {
            key = Key;
            value = Value;
        }
    }
}
