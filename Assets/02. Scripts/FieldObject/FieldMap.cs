using MS.Utils;
using UnityEngine;


namespace MS.Field
{
    public class FieldMap : MonoBehaviour
    {
        private Transform playerSpawnPoint;


        public Transform PlayerSpawnPoint => playerSpawnPoint;


        private void Awake()
        {
            playerSpawnPoint = TransformExtensions.FindChildDeep(this.gameObject.transform, "PlayerSpawnPoint");
        }
    }
}