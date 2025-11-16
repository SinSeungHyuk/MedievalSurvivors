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
        public Dictionary<string, SkillSettingData> DictSkillSettingData { get; private set; }
        public Dictionary<string, CharacterSettingData> DictCharacterSettingData { get; private set; }


        public async UniTask LoadAllGameSettingDataAsync()
        {
            try
            {
                TextAsset skillJson = await AddressableManager.Instance.LoadResourceAsync<TextAsset>("SkillSettingData");
                DictSkillSettingData = JsonConvert.DeserializeObject<Dictionary<string, SkillSettingData>>(skillJson.text);

                TextAsset characterJson = await AddressableManager.Instance.LoadResourceAsync<TextAsset>("CharacterSettingData");
                DictCharacterSettingData = JsonConvert.DeserializeObject<Dictionary<string, CharacterSettingData>>(characterJson.text);

                
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataManager] 데이터 로드 실패: {e.Message}");
            }
        }
    }
}