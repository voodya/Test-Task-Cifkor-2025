using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactsView : ABasePanelView
{
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private List<FactButtonView> _btns;
    [SerializeField] private FactPopup _factPopup;

    public ScrollRect Scroll => _scroll;
    public List<FactButtonView> Bnts => _btns;

    public FactPopup FactPopup => _factPopup;
}
