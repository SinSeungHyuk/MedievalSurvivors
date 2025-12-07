using Core;
using Cysharp.Threading.Tasks;
using MS.Core;
using MS.Field;
using MS.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Manager
{
    public class EffectManager : Singleton<EffectManager>
    {
        private List<MSEffect> effectList = new List<MSEffect>();


        public MSEffect PlayEffect(string effectKey, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(effectKey))
            {
                Debug.LogError("[EffectManager] Effect Key가 비어있습니다.");
                return null;
            }

            GameObject instance = ObjectPoolManager.Instance.Get(effectKey, position, rotation);
            if (instance == null)
            {
                Debug.LogError($"[EffectManager] 풀에서 '{effectKey}'를 가져오는데 실패했습니다.");
                return null;
            }

            MSEffect effectComponent = instance.GetComponent<MSEffect>();
            if (effectComponent == null)
            {
                Debug.LogError($"[EffectManager] Prefab '{effectKey}'에 MSEffect 컴포넌트가 없습니다.");
                ObjectPoolManager.Instance.Return(effectKey, instance);
                return null;
            }

            effectComponent.InitEffect(effectKey);
            effectList.Add(effectComponent);

            return effectComponent;
        }

        public void ClearEffect()
        {
            effectList.Clear();
        }

        public async UniTask LoadAllEffectAsync()
        {
            try
            {
                var tasks = new List<UniTask>
                {
                    ObjectPoolManager.Instance.CreatePoolAsync("Eff_StoneSlash", 10),
                    ObjectPoolManager.Instance.CreatePoolAsync("Eff_Teleport", 3),
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