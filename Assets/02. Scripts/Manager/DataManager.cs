using Core;
using Cysharp.Threading.Tasks;
using MS.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Manager
{
    public class DataManager : Singleton<DataManager>
    {
        public Dictionary<string, SkillSettingData> SkillSettingData { get; private set; }


        public async UniTask LoadAllGameSettingDataAsync()
        {
            try
            {
                TextAsset skillJson = await AddressableManager.Instance.LoadResourceAsync<TextAsset>("SkillSettingData");
                SkillSettingData = JsonConvert.DeserializeObject<Dictionary<string, SkillSettingData>>(skillJson.text);

                foreach (var pair in SkillSettingData)
                {
                    string skillKey = pair.Key; // "Fireball"
                    SkillSettingData data = pair.Value; // SkillSettingData 객체
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataManager] 데이터 로드 실패: {e.Message}");
            }
        }
    }
}