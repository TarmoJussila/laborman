using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Television : MonoBehaviour
{
    [SerializeField] private Text text;

    private void Start()
    {
        text.text = string.Empty;

        StartCoroutine(FetchTelevisionText());
    }

    private IEnumerator FetchTelevisionText()
    {
        while (string.IsNullOrWhiteSpace(text.text))
        {
            yield return new WaitForSeconds(0.5f);

            var cityName = LocaleController.Instance.CurrentCityName;
            var countryName = LocaleController.Instance.CurrentCountryName;
            var weather = LocaleController.Instance.CurrentWeather;
            var weatherDescription = LocaleController.Instance.CurrentWeatherDescription;
            var windDegree = LocaleController.Instance.CurrentWindDegree;
            var windSpeed = LocaleController.Instance.CurrentWindSpeed;

            if (!string.IsNullOrWhiteSpace(cityName) && !string.IsNullOrWhiteSpace(countryName))
            {
                string weatherText = "WEATHER:" + "\n" +
                cityName + ", " + countryName + "\n" + "\n" +
                weather + ", " + weatherDescription + "\n" + "\n" +
                "WIND: " + windDegree + "\n" +
                "SPEED: " + windSpeed;

                text.text = weatherText;
            }
        }
    }
}
