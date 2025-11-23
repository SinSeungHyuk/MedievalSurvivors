using Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace MS.Manager
{
    public partial class GameManager
    {

        public async UniTask LoadAllEffectAsync()
        {
            try
            {
                await ObjectPoolManager.Instance.CreatePoolAsync("Eff_StoneSlash", 10);
                // ...
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async UniTask LoadAllSkillObjectAsync()
        {
            try
            {
                await ObjectPoolManager.Instance.CreatePoolAsync("Projectile_StoneSlash", 10);
                await ObjectPoolManager.Instance.CreatePoolAsync("Area_StoneSlash", 10);
                // ...
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}