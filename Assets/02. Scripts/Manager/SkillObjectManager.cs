using Core;
using Cysharp.Threading.Tasks;
using MS.Field;
using MS.Skill;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MS.Manager
{
    public class SkillObjectManager : Singleton<SkillObjectManager>
    {
        private List<SkillObject> skillObjectList = new List<SkillObject>();
        private List<SkillObject> releaseSkillObjectList = new List<SkillObject>();


        public T SpawnSkillObject<T>(string _key, FieldCharacter _owner, LayerMask _targetLayer) where T : SkillObject
        {
            T skillObject = ObjectPoolManager.Instance.Get(_key, _owner.transform).GetComponent<T>();

            if (skillObject != null)
            {
                skillObjectList.Add(skillObject);
                skillObject.InitSkillObject(_key, _owner , _targetLayer);
            }
            return skillObject;
        }

        public void OnUpdate(float _deltaTime)
        {
            foreach (SkillObject skillObject in skillObjectList)
            {
                skillObject.OnUpdate(_deltaTime);
                if (skillObject.ObjectLifeState == FieldObject.FieldObjectLifeState.Death)
                    releaseSkillObjectList.Add(skillObject);
            }

            foreach (SkillObject releaseObject in releaseSkillObjectList)
            {
                ObjectPoolManager.Instance.Return(releaseObject.SkillObjectKey, releaseObject.gameObject);
                skillObjectList.Remove(releaseObject);
            }
            releaseSkillObjectList.Clear();
        }

        public void OnFixedUpdate(float _fixedDeltaTime)
        {
            foreach (SkillObject skillObject in skillObjectList)
            {
                skillObject.OnFixedUpdate(_fixedDeltaTime);
            }
        }

        public void ClearSkillObject()
        {
            skillObjectList.Clear();
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