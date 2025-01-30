using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class WebCommand
{
    private string GetApiKey;
    private CancellationTokenSource _cancellationToken;
    private Subject<ExecuteResult> _executeResult;

    private Subject<WebCommand> _onRelease = new Subject<WebCommand>();
    public Subject<ExecuteResult> ExecuteResult => _executeResult;
    public Subject<WebCommand> OnRelease => _onRelease;
    public CancellationToken DisposeToken => _cancellationToken.Token; 
    public string ApiKey => GetApiKey;

    public class Pool : MemoryPool<WebCommand>
    {
    
    
    }


    public void Prepare(string get)
    {
        GetApiKey = get;
        _cancellationToken = new CancellationTokenSource();
        _executeResult = new Subject<ExecuteResult>();
    }

    public async UniTask Execute()
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetApiKey);
        {
            var responseResult = await request.SendWebRequest()
                .WithCancellation(_cancellationToken.Token)
                .SuppressCancellationThrow()
                .TimeoutWithoutException(TimeSpan.FromSeconds(5));
            if (responseResult.IsTimeout)
            {
                _executeResult.OnNext(new ExecuteResult("TimeOut", true, this));
            }
            else if (responseResult.Result.IsCanceled)
            {
                _executeResult.OnNext(new ExecuteResult("Cancelled", true, this));
            }
            else
            {
                if (responseResult.Result.Result.result == UnityWebRequest.Result.Success)
                    _executeResult.OnNext(new ExecuteResult(responseResult.Result.Result.downloadHandler.text, this));
                else
                    _executeResult.OnNext(new ExecuteResult(responseResult.Result.Result.error, false, this));
            }
        }
    }

    public void Cancell()
    {
        _cancellationToken.Cancel();
        OnRelease.OnNext(this);
    }
}
