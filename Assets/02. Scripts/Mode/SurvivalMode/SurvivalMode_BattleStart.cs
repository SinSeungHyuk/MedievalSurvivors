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


        private void OnBattleStartEnter(int _prev, object[] _params)
        {
            curWaveCount = 1;
            curWaveSpawnInfo = stageSettingData.WaveSpawnInfoList[0];
            elapsedWaveTime = 0f;
            elapsedSpawnTime = 0f;
        }

        private void OnBattleStartUpdate(float _dt)
        {
            elapsedWaveTime += _dt;

            if (elapsedSpawnTime < curWaveSpawnInfo.SpawnInterval)
            {
                elapsedSpawnTime += _dt;
                return;
            }
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

                Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position);
                MonsterManager.Instance.SpawnMonster(monsterKey, spawnPos, Quaternion.identity);
            }

            if (elapsedWaveTime > Settings.WaveTimer)
            {
                //elapsedWaveTime = 0f;
                //Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position);
                //MonsterCharacter boss = MonsterManager.Instance.SpawnMonster(curWaveSpawnInfo.BossMonsterKey, spawnPos, Quaternion.identity);
                //boss.SetBossMonster();

                //CurFieldMap.ActivateNextFloor(curWaveCount);
            }
        }

        private void OnBattleStartExit(int _next)
        {

        }
    }
}