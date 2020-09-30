using IniFileParser.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace IniFileParser
{
    public class FileIniParser
    {
        private readonly IIniParser _parser;

        public FileIniParser(IIniParser iniParser)
            => _parser = iniParser;

        public List<Section> Read(string filePath)
            => Read(filePath, (stream) => _parser.Read(stream));

        public Section Read(string filePath, string section)
            => Read(filePath, (stream) => _parser.Read(stream, section));

        public SectionValue Read(string filePath, string section, string key)
            => Read(filePath, (stream) => _parser.Read(stream, section, key));

        private T Read<T>(string filePath, Func<Stream, T> action)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found");

            using var fileStream = File.OpenRead(filePath);
            return action(fileStream);
        }
    }
}
