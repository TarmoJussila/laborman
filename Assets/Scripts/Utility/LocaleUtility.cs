using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocaleUtility
{
    private static readonly Dictionary<SystemLanguage, string> countryCodes = new Dictionary<SystemLanguage, string>()
    {
        { SystemLanguage.Afrikaans, "ZA" },
        { SystemLanguage.Arabic, "SA" },
        { SystemLanguage.Basque, "US" },
        { SystemLanguage.Belarusian, "BY" },
        { SystemLanguage.Bulgarian, "BJ" },
        { SystemLanguage.Catalan, "ES" },
        { SystemLanguage.Chinese, "CN" },
        { SystemLanguage.Czech, "HK" },
        { SystemLanguage.Danish, "DK" },
        { SystemLanguage.Dutch, "BE" },
        { SystemLanguage.English, "US" },
        { SystemLanguage.Estonian, "EE" },
        { SystemLanguage.Faroese, "FU" },
        { SystemLanguage.Finnish, "FI" },
        { SystemLanguage.French, "FR" },
        { SystemLanguage.German, "DE" },
        { SystemLanguage.Greek, "JR" },
        { SystemLanguage.Hebrew, "IL" },
        { SystemLanguage.Icelandic, "IS" },
        { SystemLanguage.Indonesian, "ID" },
        { SystemLanguage.Italian, "IT" },
        { SystemLanguage.Japanese, "JP" },
        { SystemLanguage.Korean, "KR" },
        { SystemLanguage.Latvian, "LV" },
        { SystemLanguage.Lithuanian, "LT" },
        { SystemLanguage.Norwegian, "NO" },
        { SystemLanguage.Polish, "PL" },
        { SystemLanguage.Portuguese, "PT" },
        { SystemLanguage.Romanian, "RO" },
        { SystemLanguage.Russian, "RU" },
        { SystemLanguage.SerboCroatian, "SP" },
        { SystemLanguage.Slovak, "SK" },
        { SystemLanguage.Slovenian, "SI" },
        { SystemLanguage.Spanish, "ES" },
        { SystemLanguage.Swedish, "SE" },
        { SystemLanguage.Thai, "TH" },
        { SystemLanguage.Turkish, "TR" },
        { SystemLanguage.Ukrainian, "UA" },
        { SystemLanguage.Vietnamese, "VN" },
        { SystemLanguage.ChineseSimplified, "CN" },
        { SystemLanguage.ChineseTraditional, "CN" },
        { SystemLanguage.Unknown, "US" },
        { SystemLanguage.Hungarian , "HU" },
    };

    public static string ToCountryCode(this SystemLanguage language)
    {
        string result;
        if (countryCodes.TryGetValue(language, out result))
        {
            return result;
        }
        else
        {
            return countryCodes[SystemLanguage.Unknown];
        }
    }
}