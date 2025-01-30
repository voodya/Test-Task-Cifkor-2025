
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;

public class FactsModel
{
    private IReactiveProperty<List<Datum>> _facts;
    private string _apiUrl;

    public IReactiveProperty<List<Datum>> Facts => _facts;
    public string ApiUrl => _apiUrl;

    public FactsModel(string api, int factsMaxCount)
    {
        _facts = new ReactiveProperty<List<Datum>>();
        _apiUrl = api;
    }

    public void SetFacts(List<Datum> value)
    {
        _facts.Value = value;
    }
}

public partial class FactsListData
{
    [JsonProperty("data")]
    public List<Datum> Data { get; set; }

    [JsonProperty("links")]
    public Links Links { get; set; }
}

public partial class Datum
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("attributes")]
    public Attributes Attributes { get; set; }
}

public partial class Links
{
    [JsonProperty("self")]
    public Uri Self { get; set; }

    [JsonProperty("current")]
    public Uri Current { get; set; }

    [JsonProperty("next")]
    public Uri Next { get; set; }

    [JsonProperty("last")]
    public Uri Last { get; set; }
}