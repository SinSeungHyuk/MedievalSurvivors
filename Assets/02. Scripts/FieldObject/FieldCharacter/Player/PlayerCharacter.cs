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
        public PlayerLevelSystem LevelSystem { get; private set; }
        public bool IsMovementLocked { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PlayerController = GetComponent<PlayerController>();
            LevelSystem = GetComponent<PlayerLevelSystem>();
        }

        public void InitPlayer(string _characKey)
        {
            ObjectType = FieldObjectType.Player;
            ObjectLifeState = FieldObjectLifeState.Live;

            if (!DataManager.Instance.CharacterSettingData.CharacterSettingDataDict.TryGetValue(_characKey, out CharacterSettingData _characterData))
            {
                Debug.LogError($"InitPlayer Key Missing : {_characKey}");
                return;
            }

            LevelSystem.InitLevelSystem(); // 레벨 세팅

            PlayerAttributeSet playerAttributeSet = new PlayerAttributeSet();
            playerAttributeSet.InitAttributeSet(_characterData.AttributeSetSettingData);

            SSC.InitSSC(this, playerAttributeSet);
            SSC.GiveSkill(_characterData.DefaultSkillKey);
            SSC.GiveSkill("Teleport");

            // TODO TEST
            SSC.GiveSkill("FOBS");
            SSC.GiveSkill("Meteor");
            SSC.GiveSkill("Blizzard");
            SSC.GiveSkill("FastCrystal");
            SSC.GiveSkill("CrystalFront");
            SSC.GiveSkill("BigCrystal"); 
            SSC.GiveSkill("SlashBlue"); 
            SSC.GiveSkill("SlashGreen");
        }

        public void SetMovementLock(bool isLocked)
        {
            IsMovementLocked = isLocked;
        }

        public override void ApplyKnockback(Vector3 _dir, float _force)
        {

        }

        // TODO :: INPUT TEST
        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                SSC.UseSkill("FOBS").Forget();
                //SSC.UseSkill("Teleport").Forget();
            }
        }
    }
}