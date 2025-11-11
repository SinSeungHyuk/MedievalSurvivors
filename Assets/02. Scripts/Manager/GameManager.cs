using UnityEngine;
using Core;
using Cysharp.Threading.Tasks;
using System;

namespace MS.Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        // TODO :: TEST
        public TPlayerController player;


        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;

        }

        public void Start()
        {
            StartGameAsync();
        }

        public async void StartGameAsync()
        {
            try
            {
                await DataManager.Instance.LoadAllGameSettingDataAsync();
                // ... 그 외 기타 비동기 로드 후 로딩 종료

                player.Test();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
