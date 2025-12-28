using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Utils;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode
    {
        private int curWaveCount;  // 현재 진행중인 웨이브 카운트
        private WaveSpawnInfo curWaveSpawnInfo; // 현재 진행중인 웨이브 스폰정보

        private float elapsedWaveTime;
        private float elapsedSpawnTime;

        private bool isActivateNextFloor = false; // 다음 웨이브 연출 중인지 여부
        private bool isBossLive = false;          // 보스가 살아있는지 여부


        private void OnBattleStartEnter(int _prev, object[] _params)
        {
            curWaveCount = 1;
            curWaveSpawnInfo = stageSettingData.WaveSpawnInfoList[0];
            elapsedWaveTime = 0f;
            elapsedSpawnTime = 0f;
        }

        private void OnBattleStartUpdate(float _dt)
        {
            if (isActivateNextFloor) return; // 다음 웨이브로 넘어가는 중에는 스폰 X

            elapsedSpawnTime += _dt;
            if (elapsedSpawnTime >= curWaveSpawnInfo.SpawnInterval)
            {
                elapsedSpawnTime = 0f; 
                for (int i = 0; i < curWaveSpawnInfo.CountPerSpawn; i++)
                {
                    string monsterKey = "";
                    int totalRatio = curWaveSpawnInfo.MonsterSpawnInfoList.Sum(x => x.MonsterSpawnRate);
                    int ratioSum = 0;
                    int randomRate = Random.Range(0, totalRatio);

                    foreach (var monsterInfo in curWaveSpawnInfo.MonsterSpawnInfoList)
                    {
                        ratioSum += monsterInfo.MonsterSpawnRate;
                        if (randomRate < ratioSum)
                        {
                            monsterKey = monsterInfo.MonsterKey;
                            break;
                        }
                    }

                    Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, curWaveCount);
                    MonsterCharacter monster = MonsterManager.Instance.SpawnMonster(monsterKey, spawnPos, Quaternion.identity);
                    monster.SSC.OnDeadCallback += OnMonsterDead;
                }
            }

            elapsedWaveTime += _dt;
            if (!isBossLive && elapsedWaveTime > Settings.WaveTimer)
            {
                SpawnBossAsync().Forget();
            }
        }

        private void OnBattleStartExit(int _next)
        {

        }


        private async UniTask SpawnBossAsync()
        {
            isBossLive = true;
            Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, curWaveCount);

            // TODO :: UI 알림
            GameplayCueManager.Instance.PlayCue("GC_BossPortal", spawnPos);
            await UniTask.WaitForSeconds(1f);

            MonsterCharacter boss = MonsterManager.Instance.SpawnMonster(curWaveSpawnInfo.BossMonsterKey, spawnPos, Quaternion.identity);
            boss.SetBossMonster();
            boss.SSC.OnDeadCallback += OnBossMonsterDead;
        }

        private async UniTask ActivateNextWaveAsync()
        {
            isActivateNextFloor = true;

            await CurFieldMap.ActivateNextFloor(curWaveCount);

            elapsedWaveTime = 0f;
            elapsedSpawnTime = 0f;
            isActivateNextFloor = false;
            isBossLive = false;
            curWaveCount++;
            curWaveSpawnInfo = stageSettingData.WaveSpawnInfoList[curWaveCount - 1];
        }
    }
}