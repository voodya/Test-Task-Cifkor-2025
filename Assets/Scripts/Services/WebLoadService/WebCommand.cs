using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine.Networking;
using Zenject;

public class WebCommand
{
    private string _targetApiUrl;
    private CancellationTokenSource _cancellationToken;
    private Subject<ExecuteResult> _executeResult;
    private Subject<WebCommand> _onRelease = new Subject<WebCommand>();

    public Subject<ExecuteResult> ExecuteResult => _executeResult;
    public Subject<WebCommand> OnRelease => _onRelease;
    public CancellationToken DisposeToken => _cancellationToken.Token;
    public string ApiKey => _targetApiUrl;
    public class Pool : MemoryPool<WebCommand> { }

    /// <summary>
    /// prepare command to webrequest
    /// </summary>
    /// <param name="url"></param>
    public void Prepare(string url)
    {
        _targetApiUrl = url;
        _cancellationToken = new CancellationTokenSource();
        _executeResult = new Subject<ExecuteResult>();
    }

    /// <summary>
    /// Start webrequest process
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute()
    {
        using UnityWebRequest request = UnityWebRequest.Get(_targetApiUrl);
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
                    _executeResult.OnNext(new ExecuteResult(responseResult.Result.Result.downloadHandler.text, responseResult.Result.Result.downloadHandler.data, this));
                else
                    _executeResult.OnNext(new ExecuteResult(responseResult.Result.Result.error, false, this));
            }
            Cancell();
        }
    }

    /// <summary>
    /// Cancell operation 
    /// </summary>
    public void Cancell()
    {
        OnRelease.OnNext(this);
        _cancellationToken.Cancel();
    }
}
