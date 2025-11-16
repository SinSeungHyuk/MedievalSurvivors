using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Skill;
using MS.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace MS.Field
{
    public class PlayerCharacter : FieldCharacter
    {
        public bool IsMovementLocked { get; private set; }


        public async UniTask InitPlayer(string _characKey)
        {
            ObjectType = FieldObjectType.Player;
            ObjectLifeState = FieldObjectLifeState.Live;

            if (!DataManager.Instance.DictCharacterSettingData.TryGetValue(_characKey, out CharacterSettingData _characterData))
            {
                Debug.LogError($"InitPlayer Key Missing : {_characKey}");
                return;
            }

            Mesh weaponMesh = await AddressableManager.Instance.LoadResourceAsync<Mesh>(_characterData.DefaultWeaponKey);
            Transform weapon = TransformExtensions.FindChildDeep(gameObject.transform, "Weapon");
            if (weapon)
            {
                MeshFilter weaponMeshFilter = weapon.GetComponent<MeshFilter>();
                weaponMeshFilter.mesh = weaponMesh;
            }

            PlayerAttributeSet playerAttributeSet = new PlayerAttributeSet();
            playerAttributeSet.InitAttributeSet(_characterData.AttributeSetSettingData);

            SSC.InitSkillActorInfo(this, playerAttributeSet);
            SSC.GiveSkill(_characterData.DefaultSkillKey);
        }

        public void SetMovementLock(bool isLocked)
        {
            IsMovementLocked = isLocked;
            Debug.Log("무브 락 " + isLocked);
        }

        #region TEST 
        // TODO :: INPUT TEST
        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                Debug.Log("E키 눌림! 상호작용!");
                //animator.SetTrigger("Attack01");
                SSC.UseSkill("StoneSlash").Forget();
            }
        }
        #endregion
    }
}