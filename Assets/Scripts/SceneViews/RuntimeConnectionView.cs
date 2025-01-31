using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeConnectionView : ABaseScene
{
    [SerializeField] private WeatherView _weatherView;
    [SerializeField] private FactsView _factsView;
    [SerializeField] private Button _weatherButton;
    [SerializeField] private Button _factsButton;

    public IObservable<Unit> OnWeather => _weatherButton.OnClickAsObservable();
    public IObservable<Unit> OnFacts => _factsButton.OnClickAsObservable();

    public bool WeatherInteractable { get => _weatherButton.interactable; set { _weatherButton.interactable = value; } }
    public bool FactsInteractable { get => _factsButton.interactable; set { _factsButton.interactable = value; } }
    public WeatherView WeatherView => _weatherView;
    public FactsView FactsView => _factsView;
}
