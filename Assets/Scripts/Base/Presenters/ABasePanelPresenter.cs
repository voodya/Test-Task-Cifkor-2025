using DG.Tweening;
using System;
using UniRx;
using UnityEngine;

public class ABasePanelPresenter<T> : IDisposable where T : ABasePanelView
{
    protected CompositeDisposable _compositeDisposable;

    protected T _view;

    public void SetView(T view)
    {
        _view = view;
        _view.OnDestroyed.Subscribe(_ => Dispose()).AddTo(_view);
    }

    public virtual void Show()
    {
        _view.RectTransform.DOScale(Vector3.one, 0.5f);
        _view.CanvasGroup.DOFade(1f, 0.5f);
        _view.CanvasGroup.interactable = true;
        _view.CanvasGroup.blocksRaycasts = true;
        _compositeDisposable = new CompositeDisposable();
    }

    public virtual void Hide()
    {
        _compositeDisposable?.Dispose();
        _view.RectTransform.DOScale(Vector3.zero, 0.5f);
        _view.CanvasGroup.DOFade(0f, 0.5f);
        _view.CanvasGroup.interactable = true;
        _view.CanvasGroup.blocksRaycasts = true;
    }

    public virtual void Dispose()
    {
        _compositeDisposable?.Dispose();
    }
}
