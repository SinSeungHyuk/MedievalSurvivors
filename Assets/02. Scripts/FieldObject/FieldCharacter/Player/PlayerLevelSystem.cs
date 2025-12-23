using MS.Manager;
using System;
using UnityEngine;


namespace MS.Field
{
    public class PlayerLevelSystem : MonoBehaviour
    {
        public event Action<float, float> OnExpChanged;
        public event Action OnLevelUpCallback;

        private float curExp;
        private float maxExp;
        private int curLevel;

        public float CurExp
        {
            get => curExp;
            set
            {
                curExp = value;
                while (curExp >= maxExp)
                {
                    curExp -= maxExp;
                    LevelUp();
                }
                OnExpChanged?.Invoke(curExp, maxExp);
            }
        }

        public int CurLevel => curLevel;


        public void InitLevelSystem()
        {
            curLevel = 1;
            curExp = 0;
            maxExp = DataManager.Instance.CharacterSettingData.LevelSettingData.BaseExp;
        }

        private void LevelUp()
        {
            float increaseValue = DataManager.Instance.CharacterSettingData.LevelSettingData.IncreaseExpPerLevel;
            curLevel++;
            maxExp += (maxExp * (increaseValue * 0.01f));

            OnLevelUpCallback?.Invoke();
        }
    }
}