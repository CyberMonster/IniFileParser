using IniFileParser.Models;
using System.Collections.Generic;
using System.IO;

namespace IniFileParser
{
    public interface IIniParser
    {
        List<Section> Read(Stream stream);
        Section Read(Stream stream, string section);
        SectionValue Read(Stream stream, string section, string key);
    }
}
