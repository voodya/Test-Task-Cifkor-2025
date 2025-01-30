using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FactButtonView : MonoBehaviour
{
    [SerializeField] private Button _onOpen;
    [SerializeField] private GameObject _loadProcess;
    [SerializeField] private TextMeshProUGUI _name; 

    public TextMeshProUGUI Name => _name;
    public IObservable<Unit> OnOpen => _onOpen.OnClickAsObservable(); 

    public GameObject LoadProcess => _loadProcess;
}

public class FactButtonModel
{
    public string Name;
    public string ID;

    public FactButtonModel(string name, string iD)
    {
        Name = name;
        ID = iD;
    }
}

