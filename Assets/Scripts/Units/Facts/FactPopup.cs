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


