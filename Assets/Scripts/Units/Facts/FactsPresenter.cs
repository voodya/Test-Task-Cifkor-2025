using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FactsPresenter : ABasePanelPresenter<FactsView>
{
    private FactsModel _factsModel;
    private FactsView _factView;
    private IWebLoadService _webLoadService;
    private List<WebCommand> _currentCommands = new();
    private WebCommand _lastCommand;
    private FactButtonView _lastBtnView;

    public FactsPresenter(FactsModel weatherModel, FactsView weatherView, IWebLoadService webLoadService)
    {
        SetView(weatherView);
        Hide();

        _factsModel = weatherModel;
        _factView = weatherView;

        _factsModel.Facts.Subscribe(UpdateFactsListView);

        _webLoadService = webLoadService;
    }

    public override void Show()
    {
        base.Show();
        UpdateData();
    }

    private void UpdateData()
    {
        foreach (var item in _factView.Bnts)
        {
            item.gameObject.SetActive(false);
            item.LoadProcess.SetActive(false);
        }
        var result = _webLoadService.SendCommand(_factsModel.ApiUrl, out WebCommand command);
        if (result.SendSuccess)
        {
            _currentCommands.Add(command);
            command.OnRelease
                .Subscribe(com => _currentCommands.Remove(com))
                .AddTo(command.DisposeToken);
            command.ExecuteResult
                .Subscribe(ExecuteResult => RequestCallback(ExecuteResult))
                .AddTo(command.DisposeToken);
        }
        else
            Debug.LogError(result.Message);
    }

    private void RequestCallback(ExecuteResult executeResult)
    {
        if (!executeResult.Success)
        {
            Debug.LogError(executeResult.ResultDescription);
            executeResult.Command.Cancell();
            return;
        }
        FactsListData data = JsonConvert.DeserializeObject<FactsListData>(executeResult.ResultData);
        _factsModel.SetFacts(data.Data);
        executeResult.Command.Cancell();
    }

    private void UpdateFactsListView(List<Datum> facts)
    {
        if (facts == null) return;
        int size = _factView.Bnts.Count;
        int counter = 0;
        List<FactButtonModel> factButtons = new List<FactButtonModel>();

        foreach (var item in _factView.Bnts)
        {
            if (counter >= facts.Count) break;
            FactButtonModel model = new FactButtonModel(facts[counter].Attributes.Name, facts[counter].Id);
            factButtons.Add(model);
            item.OnOpen.Subscribe(_ => OpenPopup(model.ID, item));
            item.Name.text = facts[counter].Attributes.Name;
            item.gameObject.SetActive(true);
            counter++;
        }
    }

    private void OpenPopup(string id, FactButtonView factButtonView)
    {
        _lastBtnView?.LoadProcess?.SetActive(false);
        _lastCommand?.Cancell();
        factButtonView.LoadProcess.SetActive(true);
        CompositeDisposable tempDisposable = new CompositeDisposable();

        var result = _webLoadService.SendCommand(_factsModel.ApiUrl + "/" + id, out WebCommand command);
        if (result.SendSuccess)
        {
            _currentCommands.Add(command);
            command.OnRelease
                .Subscribe(com => _currentCommands.Remove(com))
                .AddTo(command.DisposeToken);
            command.ExecuteResult
                .Subscribe(ExecuteResult =>
                {
                    PopupCallback(ExecuteResult);
                    factButtonView.LoadProcess.SetActive(false);

                }).AddTo(command.DisposeToken);
            _factView.FactPopup.OnClose.Subscribe(_ =>
            {
                _factView.FactPopup.gameObject.SetActive(false);
                tempDisposable?.Dispose();
            }).AddTo(tempDisposable);
        }
        else
        {
            Debug.LogError(result.Message);
            factButtonView.LoadProcess.SetActive(false);
        }

        
    }

    private void PopupCallback(ExecuteResult executeResult)
    {
        if (!executeResult.Success)
        {
            Debug.LogError(executeResult.ResultDescription);
            executeResult.Command.Cancell();
            return;
        }
        DogData data = JsonConvert.DeserializeObject<DogData>(executeResult.ResultData);
        _factView.FactPopup.Description.text = data.Data.Attributes.Description;
        _factView.FactPopup.Title.text = data.Data.Attributes.Name;
        _factView.FactPopup.gameObject.SetActive(true);
        executeResult.Command.Cancell();
    }
}

