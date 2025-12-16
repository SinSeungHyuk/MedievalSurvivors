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
                floorList.Add(TransformExtensions.FindChildDeep(this.gameObject.transform, "Floor_" + i));
            }

            for (int i = 1; i <= Settings.MaxWaveCount; i++)
            {
                navBlockerList.Add(TransformExtensions.FindChildDeep(this.gameObject.transform, "NavBlocker_" + i));
            }
        }

        public Vector3 GetRandomSpawnPoint(Vector3 playerPos)
        {
            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                float randomDistance = Random.Range(Settings.DefaultMinSpawnDistance, Settings.DefaultMaxSpawnDistance);
                Vector3 spawnOffset = new Vector3(randomDir.x, 0f, randomDir.y) * randomDistance;

                Vector3 targetPosition = playerPos + spawnOffset;

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return playerPos;
        }

        public void ActivateNextFloor(int _floorIdx)
        {
            CameraManager.Instance.ShakeCamera(4f, 5f);
            floorList[_floorIdx].transform.DOMoveY(6f, 5f).OnComplete(()
                => navBlockerList[_floorIdx].gameObject.SetActive(false));
        }
    }
}