using TMPro;
using UniRx;
using UnityEngine;

public class WeatherView : ABasePanelView
{
    [SerializeField] private TextMeshProUGUI _temperature;

    public TextMeshProUGUI TemperatureText => _temperature;

}
