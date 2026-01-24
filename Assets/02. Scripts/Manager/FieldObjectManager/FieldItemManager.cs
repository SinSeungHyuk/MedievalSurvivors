using Core;
using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Manager
{
    public class FieldItemManager : Singleton<FieldItemManager>
    {
        private List<FieldItem> fieldItemList = new List<FieldItem>();


        public FieldItem SpawnFieldItem(string _key, Vector3 _spawnPos)
        {
            FieldItem fieldItem = ObjectPoolManager.Instance.Get(_key, _spawnPos, Quaternion.identity).GetComponent<FieldItem>();

            if (fieldItem != null)
            {
                if (!DataManager.Instance.ItemSettingDataDict.TryGetValue(_key, out ItemSettingData _itemData))
                {
                    Debug.LogError($"SpawnFieldItem Key Missing : {_key}");
                    return null;
                }
                
                fieldItem.InitFieldItem(_key, _itemData);
                fieldItemList.Add(fieldItem);
            }

            return fieldItem;
        }

        public void SpawnRandomFieldItem(Vector3 _spawnPos)
        {
            int minIndex = (int)EItemType.RedCrystal;
            int maxIndex = (int)EItemType.BlueCrystal;

            int randomIndex = UnityEngine.Random.Range(minIndex, maxIndex + 1);
            string randomKey = ((EItemType)randomIndex).ToString();

            SpawnFieldItem(randomKey, _spawnPos);
        }

        public void ClearFieldItem()
        {
            fieldItemList.Clear();
        }

        public async UniTask LoadAllFieldItemAsync()
        {
            try
            {
                var tasks = new List<UniTask>
                {
                    ObjectPoolManager.Instance.CreatePoolAsync("Coin", 100),
                    ObjectPoolManager.Instance.CreatePoolAsync("BlueCrystal", 50),
                    ObjectPoolManager.Instance.CreatePoolAsync("GreenCrystal", 50),
                    ObjectPoolManager.Instance.CreatePoolAsync("RedCrystal", 50),
                    ObjectPoolManager.Instance.CreatePoolAsync("BossChest", 5),
                    ObjectPoolManager.Instance.CreatePoolAsync("Artifact", 5),
                    // ...
                };

                await UniTask.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}