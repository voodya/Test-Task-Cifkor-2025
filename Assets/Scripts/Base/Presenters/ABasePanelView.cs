using UniRx;
using UnityEngine;

public class ABasePanelView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _loadIndicator;
    private Subject<Unit> _onDestroyed = new();

    public RectTransform RectTransform => _rectTransform;
    public CanvasGroup CanvasGroup => _canvasGroup;
    public Subject<Unit> OnDestroyed => _onDestroyed;
    public GameObject LoadIndicator => _loadIndicator;

    private void OnDestroy()
    {
        OnDestroyed?.OnNext(Unit.Default);
    }
}
