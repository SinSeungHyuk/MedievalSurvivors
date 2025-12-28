using Cysharp.Threading.Tasks;
using DG.Tweening;
using MS.Manager;
using MS.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


namespace MS.Field
{
    public class FieldMap : MonoBehaviour
    {
        private List<Transform> floorList = new List<Transform>();
        private List<Transform> navBlockerList = new List<Transform>();

        private int maxSpawnAttempts = 50;


        private void Awake()
        {
            for (int i =1; i<= Settings.MaxWaveCount; i++)
            {
                floorList.Add(this.gameObject.transform.FindChildDeep("Floor_" + i));
            }

            for (int i = 1; i <= Settings.MaxWaveCount; i++)
            {
                navBlockerList.Add(this.gameObject.transform.FindChildDeep("NavBlocker_" + i));
            }
        }

        public Vector3 GetRandomSpawnPoint(Vector3 _origin, int _waveCount)
        {
            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                float randomDistance = Random.Range(Settings.DefaultMinSpawnDistance, Settings.DefaultMaxSpawnDistance + (_waveCount * 5.0f));
                Vector3 spawnOffset = new Vector3(randomDir.x, 0f, randomDir.y) * randomDistance;

                Vector3 targetPosition = _origin + spawnOffset;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return _origin;
        }

        public async UniTask ActivateNextFloor(int _floorIdx)
        {
            CameraManager.Instance.ShakeCamera(4f, 5f);

            await floorList[_floorIdx].transform
                .DOMoveY(6f, 5f)
                .SetEase(Ease.OutQuad)
                .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

            navBlockerList[_floorIdx].gameObject.SetActive(false);
        }
    }
}