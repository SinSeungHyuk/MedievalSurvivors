using Cysharp.Threading.Tasks;
using MS.Core;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.UI;
using MS.Utils;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode
    {
        private void OnLastWaveEnter(int _prev, object[] _params)
        {
            Notification notification = UIManager.Instance.ShowSystemUI<Notification>("Notification");
            if (notification)
            {
                notification.InitNotification("Warning", "LastWave");
            }
        }

        private void OnLastWaveUpdate(float _dt)
        {
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

                    Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, CurWaveCount.Value);
                    MonsterCharacter monster = MonsterManager.Instance.SpawnMonster(monsterKey, spawnPos, Quaternion.identity);
                    monster.SSC.OnDeadCallback += OnMonsterDead;
                }
            }

            if (!isBossLive)
            {
                CurWaveTimer.Value -= _dt;
                if (CurWaveTimer.Value <= 0)
                {
                    EndLastWaveAsync().Forget();
                }
            }
        }

        private void OnLastWaveExit(int _next)
        {

        }

        private async UniTask EndLastWaveAsync()
        {
            isBossLive = true;
            Vector3 spawnPos = curFieldMap.GetRandomSpawnPoint(player.Position, CurWaveCount.Value);

            GameplayCueManager.Instance.PlayCue("GC_BossPortal", spawnPos);
            OnBossSpawned?.Invoke();

            await UniTask.WaitForSeconds(1.5f);

            MonsterCharacter boss = MonsterManager.Instance.SpawnMonster(curWaveSpawnInfo.BossMonsterKey, spawnPos, Quaternion.identity);
            boss.SetBossMonster();
            boss.SSC.OnDeadCallback += OnBossMonsterDead;

            Notification notification = UIManager.Instance.ShowSystemUI<Notification>("Notification");
            if (notification)
            {
                notification.InitNotification("Warning", "BossAppear");
            }
        }
    }
}