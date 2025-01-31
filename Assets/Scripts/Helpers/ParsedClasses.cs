using Newtonsoft.Json;
using System.Collections.Generic;
using System;

#region Weather
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

    [JsonProperty("windSpeed")]
    public string WindSpeed { get; set; }

    [JsonProperty("windDirection")]
    public string WindDirection { get; set; }

    [JsonProperty("icon")]
    public string Icon { get; set; }

    [JsonProperty("shortForecast")]
    public string ShortForecast { get; set; }

    [JsonProperty("detailedForecast")]
    public string DetailedForecast { get; set; }
}

#endregion

#region Facts Preview
public partial class FactsListData
{
    [JsonProperty("data")]
    public List<FactPreviewData> Data { get; set; }

}

public partial class FactPreviewData
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("attributes")]
    public Attributes Attributes { get; set; }
}

#endregion

#region One Fact

public partial class OneFact
{
    [JsonProperty("data")]
    public DogDataData Data { get; set; }

}

public partial class DogDataData
{
    [JsonProperty("attributes")]
    public Attributes Attributes { get; set; }

}

public partial class Attributes
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

}

#endregion