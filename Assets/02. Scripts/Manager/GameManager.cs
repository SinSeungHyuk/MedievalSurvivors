using UnityEngine;
using Core;
using Cysharp.Threading.Tasks;
using System;
using MS.Field;

namespace MS.Manager
{
    public partial class GameManager : MonoSingleton<GameManager>
    {
        // TODO :: TEST
        public PlayerCharacter player;
        public MonsterCharacter monster;


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
            SkillObjectManager.Instance.OnUpdate(Time.deltaTime);
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
    }
}
