using IniFileParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IniFileParser
{
    public class StreamIniParser : IIniParser
    {
        private readonly ParserSettings _settings;

        public StreamIniParser(ParserSettings settings = null)
            => _settings = settings ?? new ParserSettings();

        public List<Section> Read(Stream stream)
        {
            var result = new List<Section>();

            var isSection = false;
            SectionValue value = null;

            List<Section> Handler(string line)
            {
                if (line == null)
                    return result;

                value = ParseLine(line, out isSection);

                if (isSection)
                    result.Add(new Section(value.Key));
                else if (value != null && result.Count > 0)
                    result.Last().Values.Add(value);

                return null;
            }

            return Reader(stream, Handler);
        }

        public Section Read(Stream stream, string section)
        {
            Section result = null;

            var isSection = false;
            SectionValue value = null;

            Section Handler(string line)
            {
                if (line == null)
                    return result;

                value = ParseLine(line, out isSection);

                if (isSection)
                {
                    if (result != null)
                        return result;
                    else if (value.Key == section)
                        result = new Section(value.Key);
                }
                else if (result != null && value != null)
                    result.Values.Add(value);

                return null;
            }

            return Reader(stream, Handler);
        }

        public SectionValue Read(Stream stream, string section, string key)
        {
            SectionValue result = null;

            var isSection = false;
            var isNeededSection = false;
            SectionValue value = null;

            SectionValue Handler(string line)
            {
                if (line == null)
                    return result;

                value = ParseLine(line, out isSection);

                if (isSection)
                {
                    if (isNeededSection)
                        return null;
                    else if (value.Key == section)
                        isNeededSection = true;
                }
                else if (isNeededSection
                    && value != null
                    && value.Key == key)
                    return value;

                return null;
            }

            return Reader(stream, Handler);
        }

        private T Reader<T>(Stream stream, Func<string, T> loopHandler) where T : class
        {
            using var reader = new StreamReader(stream);

            var line = string.Empty;
            for (; ; line = reader.ReadLine())
            {
                var result = loopHandler(line);
                if (result != null)
                    return result;

                if (line == null)
                    return null;
            }
        }

        public SectionValue ParseLine(string line, out bool isSection)
        {
            isSection = false;

            line = line.Trim();
            var commentIndex = line.IndexOf(_settings.CommentChar);
            if (string.IsNullOrWhiteSpace(line)
                || commentIndex == 0)
                return null;

            if (commentIndex != -1)
                line = line.Substring(0, commentIndex);

            if (line.IndexOf('[') < line.IndexOf(']'))
            {
                isSection = true;
                return new SectionValue(line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - 1), null);
            }

            if (line.IndexOf('=') == -1)
                return null;

            var lineParts = line.Split('=');
            if (_settings.TrimValues)
            {
                lineParts[0] = lineParts[0].Trim();
                lineParts[1] = lineParts[1].Trim();
            }

            return new SectionValue(lineParts[0], lineParts[1]);
        }
    }
}
