using UnityEngine;
using System.Collections;
using System;

public class LocaleController : MonoBehaviour
{
    public static LocaleController Instance { get; private set; }

    public string CurrentWeather { get; private set; }
    public string CurrentWeatherDescription { get; private set; }
    public string CurrentWindSpeed { get; private set; }
    public string CurrentWindDegree { get; private set; }
    public string CurrentCountryCode { get; private set; }
    public string CurrentCountryName { get; private set; }
    public string CurrentCityName { get; private set; }
    public Sprite CurrentFlagSprite { get; private set; }

    [Header("Debug Settings")]
    [SerializeField] private string overrideIP = "173.162.43.195";
    [SerializeField] private bool useOverrideIP = false;

    // OpenWeatherMap.
    private readonly string openWeatherMapUrl = "http://api.openweathermap.org/data/2.5/weather?q=";
    private readonly string openWeatherMapAppId = "e3a642cec13d52496490dfa8e9ba11d3";
    private readonly string weatherKey = "main";
    private readonly string weatherDescriptionKey = "description";
    private readonly string windSpeedKey = "speed";
    private readonly string windDegreeKey = "deg";

    // geoPlugin.
    private readonly string geoPluginUrl = "http://www.geoplugin.net/json.gp?ip=";
    private readonly string countryCodeKey = "geoplugin_countryCode";
    private readonly string countryNameKey = "geoplugin_countryName";
    private readonly string cityNameKey = "geoplugin_city";
    private readonly string timeZoneKey = "geoplugin_timezone";

    // CountryFlags.
    private readonly string countryFlagsUrl = "https://www.countryflags.io/";
    private readonly string countryFlagsSuffix = "/shiny/64.png";

    // Fallback values.
    private readonly string fallbackWeather = "Clouds";
    private readonly string fallbackWeatherDescription = "Broken clouds";
    private readonly string fallbackWindSpeed = "3.2";
    private readonly string fallbackWindDegree = "128";
    private readonly string fallbackCountryCode = "FI";
    private readonly string fallbackCountryName = "Finland";
    private readonly string fallbackCityName = "Helsinki";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(GetLocation());
    }

    private IEnumerator GetLocation()
    {
        string ip = useOverrideIP ? overrideIP : IPUtility.GetIP(IpVersion.IPv6);

        WWW locationRequest = new WWW(geoPluginUrl + ip);

        yield return locationRequest;

        if (locationRequest.error == null)
        {
            string locationJson = locationRequest.text;

            if (!string.IsNullOrEmpty(locationJson))
            {
                string[] lines = locationJson.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(countryCodeKey))
                    {
                        if (IsNullOrEmpty(CurrentCountryCode))
                        {
                            CurrentCountryCode = GetValueWithKey(lines[i], countryCodeKey);
                        }
                    }
                    else if (lines[i].Contains(countryNameKey))
                    {
                        if (IsNullOrEmpty(CurrentCountryName))
                        {
                            CurrentCountryName = GetValueWithKey(lines[i], countryNameKey);
                        }
                    }
                    else if (lines[i].Contains(cityNameKey))
                    {
                        if (IsNullOrEmpty(CurrentCityName))
                        {
                            CurrentCityName = GetValueWithKey(lines[i], cityNameKey);
                        }
                    }
                    else if (lines[i].Contains(timeZoneKey))
                    {
                        if (IsNullOrEmpty(CurrentCityName))
                        {
                            CurrentCityName = GetValueWithKey(lines[i], timeZoneKey);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log(locationRequest.error);
        }

        if (IsNullOrEmpty(CurrentCountryCode))
        {
            var systemCountryCode = Application.systemLanguage.ToCountryCode();

            if (!IsNullOrEmpty(systemCountryCode))
            {
                CurrentCountryCode = systemCountryCode;
            }
            else
            {
                CurrentCountryCode = fallbackCountryCode;
            }
        }

        if (IsNullOrEmpty(CurrentCountryName))
        {
            CurrentCountryName = fallbackCountryName;
        }

        if (IsNullOrEmpty(CurrentCityName))
        {
            CurrentCityName = fallbackCityName;
        }

        Debug.Log("Country code: " + CurrentCountryCode + " | Country: " + CurrentCountryName + " | City: " + CurrentCityName);

        yield return StartCoroutine(GetWeather(CurrentCountryName, CurrentCityName));
        yield return StartCoroutine(GetFlag(CurrentCountryCode));
    }

    private IEnumerator GetWeather(string countryName, string cityName)
    {
        WWW weatherRequest = new WWW(openWeatherMapUrl + cityName + "," + countryName + "&APPID=" + openWeatherMapAppId);

        yield return weatherRequest;

        if (weatherRequest.error == null)
        {
            var weatherJson = weatherRequest.text;

            if (!string.IsNullOrEmpty(weatherJson))
            {
                string[] lines = weatherJson.Split(new string[] { "," }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(weatherKey))
                    {
                        if (IsNullOrEmpty(CurrentWeather))
                        {
                            CurrentWeather = GetValueWithKey(lines[i], weatherKey);
                        }
                    }
                    else if (lines[i].Contains(weatherDescriptionKey))
                    {
                        CurrentWeatherDescription = GetValueWithKey(lines[i], weatherDescriptionKey);
                    }
                    else if (lines[i].Contains(windSpeedKey))
                    {
                        CurrentWindSpeed = GetValueWithKey(lines[i], windSpeedKey);
                    }
                    else if (lines[i].Contains(windDegreeKey))
                    {
                        CurrentWindDegree = GetValueWithKey(lines[i], windDegreeKey);
                    }
                }
            }
        }
        else
        {
            Debug.Log(weatherRequest.error);
        }

        if (IsNullOrEmpty(CurrentWeather))
        {
            CurrentWeather = fallbackWeather;
        }

        if (IsNullOrEmpty(CurrentWeatherDescription))
        {
            CurrentWeatherDescription = fallbackWeatherDescription;
        }

        if (IsNullOrEmpty(CurrentWindSpeed))
        {
            CurrentWindSpeed = fallbackWindSpeed;
        }

        if (IsNullOrEmpty(CurrentWindDegree))
        {
            CurrentWindDegree = fallbackWindDegree;
        }

        Debug.Log("Weather: " + CurrentWeather + " | Description: " + CurrentWeatherDescription + " | Wind speed: " + CurrentWindSpeed + " | Wind degree: " + CurrentWindDegree);
    }

    private IEnumerator GetFlag(string countryCode)
    {
        WWW flagRequest = new WWW(countryFlagsUrl + countryCode.ToLower() + countryFlagsSuffix);

        yield return flagRequest;

        if (flagRequest.error == null)
        {
            var texture = flagRequest.texture;

            if (texture != null)
            {
                var sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
                CurrentFlagSprite = sprite;
            }
        }
        else
        {
            Debug.Log(flagRequest.error);
        }
    }

    private string GetValueWithKey(string line, string key)
    {
        string value = string.Empty;
        if (line.Contains(key))
        {
            var parse = line.Split(new string[] { ":" }, StringSplitOptions.None);
            if (parse != null && parse.Length > 0)
            {
                var last = parse[parse.Length - 1];
                last = last.Trim(new char[] { '"', ',' });
                var check = last.Split(new string[] { "/" }, StringSplitOptions.None);
                value = check[check.Length - 1];
            }
        }
        return value;
    }

    private bool IsNullOrEmpty(string str)
    {
        return string.IsNullOrWhiteSpace(str) || str.StartsWith("null") || str.StartsWith("0");
    }
}