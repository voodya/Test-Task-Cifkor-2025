using System.Collections.Generic;
using UniRx;

public class FactsModel
{
    private IReactiveProperty<List<FactPreviewData>> _facts;
    private string _apiUrl;

    public IReactiveProperty<List<FactPreviewData>> Facts => _facts;
    public string ApiUrl => _apiUrl;

    public FactsModel(string api, int factsMaxCount)
    {
        _facts = new ReactiveProperty<List<FactPreviewData>>();
        _apiUrl = api;
    }

    public void SetFacts(List<FactPreviewData> value)
    {
        _facts.Value = value;
    }
}