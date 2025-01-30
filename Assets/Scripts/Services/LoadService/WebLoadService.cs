using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebLoadService
{
    UniTask<bool> CheckConnection();
    void Initialize();
    SendCommandResult SendCommand(string ApiKey, out WebCommand command);
}



public class WebLoadService : IWebLoadService
{
    private WebCommand _currentCommand;
    private Queue<WebCommand> _commands;
    private readonly WebCommand.Pool _commandPool;

    public WebLoadService(WebCommand.Pool pool)
    {
        _commandPool = pool;
    }

    public async UniTask<bool> CheckConnection()
    {
        using UnityWebRequest request = UnityWebRequest.Get("https://www.google.com");
        {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
                return true;
            else
                return false;
        }
    }

    public void Initialize()
    {
        _commands = new Queue<WebCommand>();
    }

    public SendCommandResult SendCommand(string ApiKey, out WebCommand command)
    {
        if(_currentCommand != null)
        {
            if (_currentCommand.ApiKey == ApiKey || _commands.Any(x => x.ApiKey == ApiKey))
            {
                command = null;
                return new SendCommandResult()
                {
                    SendSuccess = false,
                    Message = "Already exist"
                };
            }
        }
            
        if (_commandPool.NumInactive > 0)
        {
            command = _commandPool.Spawn();
            command.Prepare(ApiKey);
            command.OnRelease
                .Subscribe(command => _commandPool.Despawn(command))
                .AddTo(command.DisposeToken);
            AddCommand(command);
            return new SendCommandResult()
            {
                SendSuccess = true,
                Message = ApiKey
            };
        }
        else
        {
            command = null;
            return new SendCommandResult()
            {
                SendSuccess = false,
                Message = "PoolIsEmpty"
            };
        }
    }

    private void AddCommand(WebCommand webCommand)
    {
        _commands.Enqueue(webCommand);
        TryGetData();
    }

    private async void TryGetData()
    {
        _currentCommand = _commands.Dequeue();
        await _currentCommand.Execute();
        _currentCommand = null;

        if (_commands.Count > 0)
            TryGetData();
    }
}

public struct SendCommandResult
{
    public bool SendSuccess;
    public string Message;
}




public struct ExecuteResult
{
    public string ResultDescription;
    public string ResultData;
    public byte[] RawData;
    public bool Success;
    public WebCommand Command;

    public ExecuteResult(string data, byte[] rawData, WebCommand command)
    {
        Success = true;
        ResultData = data;
        ResultDescription = "No problem";
        Command = command;
        RawData = rawData;
    }

    public ExecuteResult(string error, bool isCancelled, WebCommand command)
    {
        Success = false;
        ResultData = null;
        ResultDescription = error;
        Command = command;
        RawData = default;
    }
}


