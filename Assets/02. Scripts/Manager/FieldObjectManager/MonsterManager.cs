using Core;
using Cysharp.Threading.Tasks;
using MS.Field;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MS.Manager
{
    public class MonsterManager : Singleton<MonsterManager>
    {
        private List<MonsterCharacter> monsterList = new List<MonsterCharacter>();


        public MonsterCharacter SpawnMonster(string _key, Vector3 _spawnPos, Quaternion _spawnRot)
        {
            MonsterCharacter monster = ObjectPoolManager.Instance.Get(_key, _spawnPos, _spawnRot).GetComponent<MonsterCharacter>();

            if (monster != null)
            {
                monsterList.Add(monster);
                monster.InitMonster(_key);
            }

            return monster;
        }

        public void OnUpdate(float _deltaTime)
        {
            foreach (MonsterCharacter monster in monsterList)
            {
                monster.OnUpdate(_deltaTime);
            }
        }


        public void ClearMonster()
        {
            monsterList.Clear();
        }

        public async UniTask LoadAllMonsterAsync()
        {
            try
            {
                await ObjectPoolManager.Instance.CreatePoolAsync("Skeleton_Tier1", 10);
                // ...
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}