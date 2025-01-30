using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;

public class WeatherModel
{
    private IReactiveProperty<int> _temperature;
    private string _apiUrl;

    public IReactiveProperty<int> Temperature => _temperature;
    public string ApiUrl => _apiUrl;

    public WeatherModel(string api)
    {
        _temperature = new ReactiveProperty<int>(0);
        _apiUrl = api;
    }

    public void SetTemperature(int value)
    {
        _temperature.Value = value;
    }
}

public partial class WeatherData
{

    [JsonProperty("properties")]
    public Properties Properties { get; set; }
}


public partial class Properties
{
    [JsonProperty("periods")]
    public List<Period> Periods { get; set; }
}

public partial class Period
{
    [JsonProperty("number")]
    public long Number { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("startTime")]
    public DateTimeOffset StartTime { get; set; }

    [JsonProperty("endTime")]
    public DateTimeOffset EndTime { get; set; }

    [JsonProperty("isDaytime")]
    public bool IsDaytime { get; set; }

    [JsonProperty("temperature")]
    public int Temperature { get; set; }

    [JsonProperty("temperatureUnit")]
    public TemperatureUnit TemperatureUnit { get; set; }



    [JsonProperty("windSpeed")]
    public string WindSpeed { get; set; }

    [JsonProperty("windDirection")]
    public string WindDirection { get; set; }

    [JsonProperty("icon")]
    public Uri Icon { get; set; }

    [JsonProperty("shortForecast")]
    public string ShortForecast { get; set; }

    [JsonProperty("detailedForecast")]
    public string DetailedForecast { get; set; }
}
public enum TemperatureUnit { F };
internal class TemperatureUnitConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(TemperatureUnit) || t == typeof(TemperatureUnit?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        if (value == "F")
        {
            return TemperatureUnit.F;
        }
        throw new Exception("Cannot unmarshal type TemperatureUnit");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (TemperatureUnit)untypedValue;
        if (value == TemperatureUnit.F)
        {
            serializer.Serialize(writer, "F");
            return;
        }
        throw new Exception("Cannot marshal type TemperatureUnit");
    }

    public static readonly TemperatureUnitConverter Singleton = new TemperatureUnitConverter();
}

