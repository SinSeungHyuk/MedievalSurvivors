using Core;
using MS.Field;
using MS.Skill;
using NUnit.Framework;
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
            T skillObject = ObjectPoolManager.Instance.Get(_key).GetComponent<T>();

            if (skillObject != null)
            {
                Debug.Log("SO Spawn");
                skillObjectList.Add(skillObject);
                skillObject.InitSkillObject(_key, _owner , _targetLayer);
                skillObject.transform.position = _owner.Position;
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
    }
}