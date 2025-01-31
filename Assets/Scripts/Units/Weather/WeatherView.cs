using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherView : ABasePanelView
{
    [SerializeField] private TextMeshProUGUI _temperature;
    [SerializeField] private RawImage _targetImage;

    public TextMeshProUGUI TemperatureText => _temperature;
    public RawImage TargetImage => _targetImage;

}
