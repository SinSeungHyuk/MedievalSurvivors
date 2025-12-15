using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Skill;
using MS.Utils;
using System.Threading;
using UnityEngine;


namespace MS.Skill
{
    public abstract class BaseSkill
    {
        protected SkillSystemComponent ownerSSC;
        protected FieldCharacter owner;
        protected SkillSettingData skillData;

        private float curCooltime;
        private int curSkillLevel;

        public bool IsCooltime => curCooltime > 0;
        public bool IsPostUseCooltime => skillData.IsPostUseCooltime;
        public int CurSkillLevel => curSkillLevel;


        public virtual void InitSkill(SkillSystemComponent _owner, SkillSettingData _skillData)
        {
            ownerSSC = _owner;
            owner = ownerSSC.Owner;
            skillData = _skillData;
            curCooltime = 0;
            curSkillLevel = 1;
        }

        public abstract UniTask ActivateSkill(CancellationToken token);

        public void SetCooltime() => curCooltime = skillData.Cooltime;


        #region Util
        // 스킬 캐스팅 세팅 (플레이어만 호출할 함수)
        public async UniTask SetSkillCasting(CancellationToken token)
        {
            owner.Animator.SetBool(Settings.AnimHashCasting, true);
            await UniTask.WaitForSeconds(skillData.SkillValueDict[ESkillValueType.Casting]);
            owner.Animator.SetBool(Settings.AnimHashCasting, false);
        }
        #endregion


        public void UpdateCooltime(float _deltaTime)
        {
            if (curCooltime > 0)
            {
                curCooltime -= _deltaTime;
            }
        }
    }
}