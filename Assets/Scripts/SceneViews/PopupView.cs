
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PopupView : ABaseScene
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;

    public IObservable<Unit> OnClose => _closeBtn.OnClickAsObservable();
    public TextMeshProUGUI Title => _title;
    public TextMeshProUGUI Description => _description;
}
