using Core;
using Cysharp.Threading.Tasks;
using MS.Data;
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

        public async UniTask LoadAllMonsterAsync(StageSettingData _stageSettingData)
        {
            try
            {
                // 몬스터키 중복 방지를 위한 해시셋
                HashSet<string> uniqueMonsterKeys = new HashSet<string>();

                foreach (var wave in _stageSettingData.WaveSpawnInfoList)
                {
                    if (!string.IsNullOrEmpty(wave.BossMonsterKey))
                    {
                        uniqueMonsterKeys.Add(wave.BossMonsterKey);
                    }

                    if (wave.MonsterSpawnInfoList != null)
                    {
                        foreach (var monsterInfo in wave.MonsterSpawnInfoList)
                        {
                            if (!string.IsNullOrEmpty(monsterInfo.MonsterKey))
                            {
                                uniqueMonsterKeys.Add(monsterInfo.MonsterKey);
                            }
                        }
                    }
                }

                // 해시셋에 들어간 모든 키값으로 태스크 생성
                var tasks = new List<UniTask>();
                foreach (var key in uniqueMonsterKeys)
                {
                    tasks.Add(ObjectPoolManager.Instance.CreatePoolAsync(key, 50));
                }

                await UniTask.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}