using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ABaseWebPanelPresenter<T> : IDisposable where T : ABasePanelView
{
    protected CompositeDisposable _compositeDisposable;
    protected List<WebCommand> _currentCommands = new();
    protected IWebLoadService _webLoadService;
    protected T _view;

    public ABaseWebPanelPresenter(IWebLoadService webLoadService)
    {
        _webLoadService = webLoadService;
    }

    #region View

    public void SetView(T view)
    {
        _view = view;
        _view.OnDestroyed.Subscribe(_ => Dispose()).AddTo(_view);
    }

    public virtual void Show()
    {
        _view.LoadIndicator.SetActive(true);
        _view.RectTransform.DOScale(Vector3.one, 0.5f);
        _view.CanvasGroup.DOFade(1f, 0.5f);
        _view.CanvasGroup.interactable = true;
        _view.CanvasGroup.blocksRaycasts = true;
        _compositeDisposable = new CompositeDisposable();
    }

    public virtual void Hide()
    {
        for (int i = 0; i < _currentCommands.Count; i++)
        {
            _currentCommands[i]?.Cancell();
        }
        _compositeDisposable?.Dispose();
        _view.RectTransform.DOScale(Vector3.zero, 0.5f);
        _view.CanvasGroup.DOFade(0f, 0.5f);
        _view.CanvasGroup.interactable = true;
        _view.CanvasGroup.blocksRaycasts = true;
    }
    #endregion

    #region Web

    protected void SendRequest(string url, Action<ExecuteResult> callback, Action<WebCommand> successCallback = null)
    {
        var result = _webLoadService.SendCommand(url, out WebCommand command);

        if (result.SendSuccess)
        {
            successCallback?.Invoke(command);
            _currentCommands.Add(command);

            command.OnRelease
                .Subscribe(com => _currentCommands.Remove(com))
                .AddTo(command.DisposeToken);

            command.ExecuteResult
                .Subscribe(ExecuteResult => ExecuteResult.Validate(callback))
                .AddTo(command.DisposeToken);
        }
        else
            Debug.LogError(result.Message);
    }

    #endregion

    public virtual void Dispose()
    {
        _compositeDisposable?.Dispose();
    }

}
