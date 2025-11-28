using Core;
using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Utils;
using UnityEngine;


namespace MS.Manager
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private PlayerCharacter player;

        public PlayerCharacter Player => player;


        public async UniTask<PlayerCharacter> SpawnPlayerCharacter(string _key)
        {
            GameObject playerResource = await AddressableManager.Instance.LoadResourceAsync<GameObject>("PlayerCharacter");
            player = PlayerCharacter.Instantiate(
                playerResource,
                Vector3.zero,
                Quaternion.identity
            ).GetComponent< PlayerCharacter>();

            if (!DataManager.Instance.CharacterSettingDataDict.TryGetValue(_key, out CharacterSettingData _characterData))
            {
                return null;
            }

            Mesh weaponMesh = await AddressableManager.Instance.LoadResourceAsync<Mesh>(_characterData.DefaultWeaponKey);
            Transform weapon = TransformExtensions.FindChildDeep(player.gameObject.transform, "Weapon");
            if (weapon)
            {
                MeshFilter weaponMeshFilter = weapon.GetComponent<MeshFilter>();
                weaponMeshFilter.mesh = weaponMesh;
            }

            player.InitPlayer(_key);

            return player;
        }
    }
}