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
            SkillObjectManager.Instance.OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            SkillObjectManager.Instance.OnFixedUpdate(Time.fixedDeltaTime);
        }

        public async void StartGameAsync()
        {
            try
            {
                await DataManager.Instance.LoadAllGameSettingDataAsync();

                // TODO :: 임시로 시작하자마자 로드
                await LoadAllEffectAsync();
                await LoadAllSkillObjectAsync();
                await GameplayCueManager.Instance.LoadGameplayCueAsync();

                await player.InitPlayer("TestCharacter");
                monster.InitMonster("TestMonster");
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
