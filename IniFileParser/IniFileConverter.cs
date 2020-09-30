using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace IniFileParser
{
    public static class IniFileConverter
    {
        public static string GetJsonFromIni(string iniFilePath)
        {
            var iniFileValues = new FileIniParser(new StreamIniParser()).Read(iniFilePath);
            var result = JsonConvert.SerializeObject(iniFileValues);
            return result;
        }

        public static string GetJsonFromIniBySectionName(string iniFilePath, string sectionName)
        {
            var iniFileValues = new FileIniParser(new StreamIniParser()).Read(iniFilePath, sectionName);
            var result = JsonConvert.SerializeObject(iniFileValues);
            return result;
        }

        public static string GetRubRurSectionMissingCurrencies(string iniFilePath)
        {
            var parser = new FileIniParser(new StreamIniParser());
            var rubSection = parser.Read(iniFilePath, "BimCurr_643");
            var rurSection = parser.Read(iniFilePath, "BimCurr_810");

            var currencies = new List<string>
            {
                "BimNom_1000",
                "BimNom_5000",
                "BimNom_10000",
                "BimNom_20000",
                "BimNom_50000",
                "BimNom_100000",
                "BimNom_200000",
                "BimNom_500000"
            };

            var result = new List<ResultClass>();

            if (rubSection != null)
            {
                var rubKeys = rubSection.Values.Select(x => x.Key).ToList();
                var rubSectionMissingCurrencies = currencies.Where(x => !rubKeys.Any(y => y == x)).ToList();

                result.Add(new ResultClass
                {
                    SectionName = rubSection.Name,
                    MissingCurrencies = rubSectionMissingCurrencies
                });
            }

            if (rurSection != null)
            {
                var rurKeys = rurSection.Values.Select(x => x.Key).ToList();
                var rurSectionMissingCurrencies = currencies.Where(x => !rurKeys.Any(y => y == x)).ToList();

                result.Add(new ResultClass
                {
                    SectionName = rurSection.Name,
                    MissingCurrencies = rurSectionMissingCurrencies
                });
            }

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        internal class ResultClass
        {
            public string SectionName { get; set; }
            public List<string> MissingCurrencies { get; set; }
        }
    }
}
