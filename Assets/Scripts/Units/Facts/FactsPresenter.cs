using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FactsPresenter : ABaseWebPanelPresenter<FactsView>
{
    private FactsModel _factsModel;
    private FactsView _factView;
    private FactButtonView _lastBtnView;
    private ISceneManager _sceneManager;

    public FactsPresenter(
        FactsModel weatherModel,
        FactsView weatherView,
        IWebLoadService webLoadService,
        ISceneManager sceneManager) : base(webLoadService)
    {
        SetView(weatherView);
        Hide();

        _factsModel = weatherModel;
        _factView = weatherView;
        _sceneManager = sceneManager;

        _factsModel.Facts.Subscribe(UpdateFactsListView).AddTo(_factView);

        _webLoadService = webLoadService;
    }

    public override void Show()
    {
        base.Show();
        foreach (var item in _factView.Bnts)
        {
            item.gameObject.SetActive(false);
            item.LoadProcess.SetActive(false);
        }
        SendRequest(_factsModel.ApiUrl, RequestCallback);
    }

    private void RequestCallback(ExecuteResult executeResult)
    {
        FactsListData data = JsonConvert.DeserializeObject<FactsListData>(executeResult.ResultData);
        _factsModel.SetFacts(data.Data);
    }

    private void UpdateFactsListView(List<FactPreviewData> facts)
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
            item.OnOpen.Subscribe(_ => OpenPopup(model.ID, item)).AddTo(_compositeDisposable);
            item.Name.text = facts[counter].Attributes.Name;
            item.gameObject.SetActive(true);
            counter++;
        }
        _view.LoadIndicator.SetActive(false);
    }

    private void OpenPopup(string id, FactButtonView factButtonView)
    {
        _lastBtnView?.LoadProcess?.SetActive(false);
        for (int i = 0; i < _currentCommands.Count; i++)
        {
            _currentCommands[i]?.Cancell();
        }
        _lastBtnView = factButtonView;
        _lastBtnView.LoadProcess.SetActive(true);
        SendRequest(_factsModel.ApiUrl + "/" + id, PopupCallback);
    }


    private async void PopupCallback(ExecuteResult executeResult)
    {
        _lastBtnView.LoadProcess.SetActive(false);
        OneFact data = JsonConvert.DeserializeObject<OneFact>(executeResult.ResultData);
        var popup = await _sceneManager.GetScene<PopupView>(false);
        popup.Description.text = data.Data.Attributes.Description;
        popup.Title.text = data.Data.Attributes.Name;
        await popup.Show();
        popup.OnClose.Subscribe(_ => _sceneManager.ReleaseScene<PopupView>()).AddTo(popup);
    }
}

