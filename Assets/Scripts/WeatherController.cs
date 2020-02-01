using UnityEngine;
using System.Collections;
using System;

public class WeatherController : MonoBehaviour
{
    // OpenWeatherMap.
    private readonly string openWeatherMapUrl = "http://api.openweathermap.org/data/2.5/weather?q=";
    private readonly string openWeatherMapAppId = "e3a642cec13d52496490dfa8e9ba11d3";
    private readonly string weatherKey = "main";
    private readonly string weatherDescriptionKey = "description";
    private readonly string windSpeedKey = "speed";
    private readonly string windDegreeKey = "deg";

    // geoPlugin.
    private readonly string geopluginUrl = "http://www.geoplugin.net/json.gp?ip=";
    private readonly string countryNameKey = "geoplugin_countryName";
    private readonly string cityNameKey = "geoplugin_city";
    private readonly string timeZoneKey = "geoplugin_timezone";

    // Fallback values.
    private readonly string fallbackWeather = "Clouds";
    private readonly string fallbackWeatherDescription = "Broken clouds";
    private readonly string fallbackWindSpeed = "3.2";
    private readonly string fallbackWindDegree = "128";
    private readonly string fallbackCountryName = "Finland";
    private readonly string fallbackCityName = "Helsinki";

    // Cached values.
    private string currentWeather;
    private string currentWeatherDescription;
    private string currentWindSpeed;
    private string currentWindDegree;
    private string currentCountryName;
    private string currentCityName;

    private void Start()
    {
        StartCoroutine(GetLocation());
    }

    private IEnumerator GetLocation()
    {
        string ip = IPManager.GetIP(IpVersion.IPv6);

        WWW locationRequest = new WWW(geopluginUrl + ip);

        yield return locationRequest;

        if (locationRequest.error == null)
        {
            string locationJson = locationRequest.text;

            if (!string.IsNullOrEmpty(locationJson))
            {
                string[] lines = locationJson.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(countryNameKey))
                    {
                        currentCountryName = GetValueWithKey(lines[i], countryNameKey);
                    }
                    else if (lines[i].Contains(cityNameKey))
                    {
                        if (string.IsNullOrEmpty(currentCityName))
                        {
                            currentCityName = GetValueWithKey(lines[i], cityNameKey);
                        }
                    }
                    else if (lines[i].Contains(timeZoneKey))
                    {
                        if (string.IsNullOrEmpty(currentCityName))
                        {
                            currentCityName = GetValueWithKey(lines[i], timeZoneKey);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log(locationRequest.error);
        }

        if (IsNullOrEmpty(currentCountryName))
        {
            currentCountryName = fallbackCountryName;
        }

        if (IsNullOrEmpty(currentCityName))
        {
            currentCityName = fallbackCityName;
        }

        Debug.Log("Country: " + currentCountryName + " | City: " + currentCityName);

        yield return StartCoroutine(GetWeather(currentCountryName, currentCityName));
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
                        if (string.IsNullOrEmpty(currentWeather))
                        {
                            currentWeather = GetValueWithKey(lines[i], weatherKey);
                        }
                    }
                    else if (lines[i].Contains(weatherDescriptionKey))
                    {
                        currentWeatherDescription = GetValueWithKey(lines[i], weatherDescriptionKey);
                    }
                    else if (lines[i].Contains(windSpeedKey))
                    {
                        currentWindSpeed = GetValueWithKey(lines[i], windSpeedKey);
                    }
                    else if (lines[i].Contains(windDegreeKey))
                    {
                        currentWindDegree = GetValueWithKey(lines[i], windDegreeKey);
                    }
                }
            }
        }
        else
        {
            Debug.Log(weatherRequest.error);
        }

        if (IsNullOrEmpty(currentWeather))
        {
            currentWeather = fallbackWeather;
        }

        if (IsNullOrEmpty(currentWeatherDescription))
        {
            currentWeatherDescription = fallbackWeatherDescription;
        }

        if (IsNullOrEmpty(currentWindSpeed))
        {
            currentWindSpeed = fallbackWindSpeed;
        }

        if (IsNullOrEmpty(currentWindDegree))
        {
            currentWindDegree = fallbackWindDegree;
        }

        Debug.Log("Weather: " + currentWeather + " | Description: " + currentWeatherDescription + " | Wind speed: " + currentWindSpeed + " | Wind degree: " + currentWindDegree);
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
        return string.IsNullOrWhiteSpace(str) || str.StartsWith("null");
    }
}