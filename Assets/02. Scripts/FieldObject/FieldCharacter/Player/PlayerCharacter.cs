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
        public PlayerController PlayerController {  get; private set; }
        public bool IsMovementLocked { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PlayerController = GetComponent<PlayerController>();
        }

        public void InitPlayer(string _characKey)
        {
            ObjectType = FieldObjectType.Player;
            ObjectLifeState = FieldObjectLifeState.Live;

            if (!DataManager.Instance.CharacterSettingDataDict.TryGetValue(_characKey, out CharacterSettingData _characterData))
            {
                Debug.LogError($"InitPlayer Key Missing : {_characKey}");
                return;
            }

            PlayerAttributeSet playerAttributeSet = new PlayerAttributeSet();
            playerAttributeSet.InitAttributeSet(_characterData.AttributeSetSettingData);

            SSC.InitSkillActorInfo(this, playerAttributeSet);
            SSC.GiveSkill(_characterData.DefaultSkillKey);
            SSC.GiveSkill("Teleport");
        }

        public void SetMovementLock(bool isLocked)
        {
            IsMovementLocked = isLocked;
        }

        #region TEST 
        // TODO :: INPUT TEST
        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                //SSC.UseSkill("StoneSlash").Forget();
                SSC.UseSkill("Teleport").Forget();
            }
        }
        #endregion
    }
}