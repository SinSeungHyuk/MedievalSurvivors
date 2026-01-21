using Cysharp.Threading.Tasks;
using MS.Manager;
using MS.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace MS.UI
{
    public class TitlePanel : BaseUI
    {
        private Button btnTitle;

        // todo test
        private TextMeshProUGUI txtTitle;
        private long downloadSize;


        private void Awake()
        {
            btnTitle = transform.FindChildComponentDeep<Button>("BtnTitle");
            txtTitle = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtTitle"); 
            btnTitle.onClick.AddListener(OnBtnTitleClicked);
        }

        private void Start()
        {
            PlayTitleBGMAsync().Forget();
            DownloadFirstRun().Forget();
        }

        private async UniTask DownloadFirstRun()
        {
            string key = "Remote";
            downloadSize = await Addressables.GetDownloadSizeAsync(key);

            if (downloadSize > 0)
            {
                float mbSize = downloadSize / (1024f * 1024f);
                txtTitle.text = $"화면을 터치하여 {mbSize:F2}MB 리소스를 다운로드 할 수 있습니다.";
            }
        }

        private void OnBtnTitleClicked()
        {
            if (downloadSize > 0)
            {
                DownloadFirstRunAsync().Forget();

            }

            else GameManager.Instance.StartGameAsync().Forget();
        }

        private async UniTask DownloadFirstRunAsync()
        {
            DownloadPopup downloadPanel = UIManager.Instance.ShowPopup<DownloadPopup>("DownloadPopup");
            downloadPanel.InitDownloadPopup(downloadSize);

            var downloadHandle = Addressables.DownloadDependenciesAsync("Remote", false);

            while (!downloadHandle.IsDone)
            {
                float percent = downloadHandle.PercentComplete;
                downloadPanel.UpdateProgress(percent);
                await UniTask.Yield();
            }

            if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(downloadHandle);
                downloadPanel.Close();
                GameManager.Instance.StartGameAsync().Forget();
            }
            else
            {
                Addressables.Release(downloadHandle);
                Debug.LogError("Download Failed");
            }
        }

        private async UniTask PlayTitleBGMAsync()
        {
            await SoundManager.Instance.InitSoundAsync();
            SoundManager.Instance.PlayBGM("BGM_Title");
        }
    }
}