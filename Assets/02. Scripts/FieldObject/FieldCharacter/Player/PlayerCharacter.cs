using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Skill;
using MS.UI;
using MS.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace MS.Field
{
    public class PlayerCharacter : FieldCharacter
    {
        private HPBar hpBar;

        public PlayerController PlayerController {  get; private set; }
        public PlayerLevelSystem LevelSystem { get; private set; }
        public bool IsMovementLocked { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PlayerController = GetComponent<PlayerController>();
            LevelSystem = GetComponent<PlayerLevelSystem>();
            hpBar = GetComponentInChildren<HPBar>();
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

            PlayerAttributeSet playerAttributeSet = new PlayerAttributeSet();
            playerAttributeSet.InitAttributeSet(_characterData.AttributeSetSettingData);

            SSC.InitSSC(this, playerAttributeSet);
            SSC.GiveSkill(_characterData.DefaultSkillKey);
            SSC.GiveSkill("Teleport");

            LevelSystem.InitLevelSystem(); // 레벨 세팅
            hpBar.InitHPBar(this);

            // TODO TEST
            SSC.GiveSkill("FOBS");
            SSC.GiveSkill("Meteor");
            SSC.GiveSkill("Blizzard");
            SSC.GiveSkill("FastCrystal");
            SSC.GiveSkill("BigCrystal"); 
            SSC.GiveSkill("SlashBlue"); 
           
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
                //SSC.UseSkill("Blizzard").Forget();
                SSC.GiveSkill("SlashGreen");
            }
        }
    }
}