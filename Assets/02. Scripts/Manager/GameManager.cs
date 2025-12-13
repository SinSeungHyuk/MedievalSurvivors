using Core;
using Cysharp.Threading.Tasks;
using MS.Field;
using MS.Mode;
using System;
using UnityEngine;

namespace MS.Manager
{
    public partial class GameManager : MonoSingleton<GameManager>
    {
        // TODO :: TEST
        public PlayerCharacter player;
        public MonsterCharacter monster;

        private GameModeBase curGameMode;

        public GameModeBase CurGameMode => curGameMode;


        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            StartGameAsync();
        }

        private void Update()
        {
            if (curGameMode != null)
                curGameMode.OnUpdate(Time.deltaTime);
        }

        public async void StartGameAsync()
        {
            try
            {
                await DataManager.Instance.LoadAllGameSettingDataAsync();
                await GameplayCueManager.Instance.LoadAllGameplayCueAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void ChangeMode(GameModeBase _mode)
        {
            curGameMode = _mode;
            curGameMode.StartMode();
        }
    }
}
