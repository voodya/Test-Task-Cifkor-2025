using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public abstract class ABaseScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public virtual async UniTask Hide(bool force = false)
    {
        
        if (force)
        {
            _canvasGroup.alpha = 0;
            await UniTask.CompletedTask;
        }
        else
        {
            await _canvasGroup.DOFade(0f, 0.5f).AsyncWaitForCompletion();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public virtual async UniTask Show()
    {
        await _canvasGroup.DOFade(1f, 0.5f).AsyncWaitForCompletion();
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
}
