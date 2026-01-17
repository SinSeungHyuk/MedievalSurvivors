using Cysharp.Threading.Tasks;
using MS.Manager;
using MS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace MS.UI
{
    public class TitlePanel : BaseUI
    {
        private Button btnTitle;


        private void Awake()
        {
            btnTitle = transform.FindChildComponentDeep<Button>("BtnTitle");
            btnTitle.onClick.AddListener(OnBtnTitleClicked);
        }

        private void Start()
        {
            PlayTitleBGMAsync().Forget();
        }

        private void OnBtnTitleClicked()
        {
            GameManager.Instance.StartGameAsync().Forget();
        }

        private async UniTask PlayTitleBGMAsync()
        {
            await SoundManager.Instance.InitSoundAsync();
            SoundManager.Instance.PlayBGM("BGM_Title");
        }
    }
}