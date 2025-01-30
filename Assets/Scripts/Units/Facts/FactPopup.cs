using Newtonsoft.Json;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FactPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _onClose;

    public IObservable<Unit> OnClose => _onClose.OnClickAsObservable();
    public TextMeshProUGUI Title => _title;
    public TextMeshProUGUI Description => _description;
}

public partial class DogData
{
    [JsonProperty("data")]
    public DogDataData Data { get; set; }

    [JsonProperty("links")]
    public Links Links { get; set; }
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
