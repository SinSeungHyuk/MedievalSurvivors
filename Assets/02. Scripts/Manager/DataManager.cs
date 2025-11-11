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

                    // 상세 로그 출력
                    Debug.Log($"--- 스킬 키: {skillKey} ---");
                    Debug.Log($"이름: {data.skillName}");
                    Debug.Log($"설명: {data.description}");
                    Debug.Log($"계수: {data.coefficient}");
                    Debug.Log($"마나 소모: {data.manaCost}");
                    Debug.Log($"--------------------------");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataManager] 데이터 로드 실패: {e.Message}");
            }
        }
    }
}