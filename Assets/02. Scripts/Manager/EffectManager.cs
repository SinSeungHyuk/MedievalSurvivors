using Core;
using MS.Core;
using MS.Manager;
using UnityEngine;

namespace MS.Manager
{
    public class EffectManager : Singleton<EffectManager>
    {
        public MSEffect PlayEffect(string effectKey, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(effectKey))
            {
                Debug.LogError("[EffectManager] Effect Key가 비어있습니다.");
                return null;
            }

            GameObject instance = ObjectPoolManager.Instance.Get(effectKey);
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

            instance.transform.position = position;
            instance.transform.rotation = rotation;

            return effectComponent;
        }
    }
}