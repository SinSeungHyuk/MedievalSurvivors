using MS.Data;
using MS.Manager;
using MS.Skill;
using MS.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace MS.UI
{
    public class SkillSlot : MonoBehaviour
    {
        public event Action<string> OnSkillSlotClicked;

        private Button btnSkillSlot;
        private Image imgSkillIcon;
        private Image overlayCooltime;
        private Image imgCooltime;

        private BaseSkill targetSkill;
        private string skillKey;


        public void InitSkillSlot(string _key, BaseSkill _skill)
        {
            imgSkillIcon = transform.FindChildComponentDeep<Image>("ImgSkillIcon");
            overlayCooltime = transform.FindChildComponentDeep<Image>("OverlayCooltime");
            imgCooltime = transform.FindChildComponentDeep<Image>("ImgCooltime");
            btnSkillSlot = GetComponent<Button>();

            btnSkillSlot.onClick.AddListener(OnSkillSlotClickedCallback);

            targetSkill = _skill;
            skillKey = _key;
            Sprite skillIcon = AddressableManager.Instance.LoadResource<Sprite>(_skill.SkillData.IconKey);
            imgSkillIcon.sprite = skillIcon;
        }

        public void OnUpdate(float _dt)
        {
            if (targetSkill == null) return;

            if (targetSkill.IsCooltime == false)
            {
                overlayCooltime.gameObject.SetActive(false);
            }
            else
            {
                overlayCooltime.gameObject.SetActive(true);
                imgCooltime.fillAmount = targetSkill.CooltimeRatio;
            }
        }

        public void UpdateExpBar(float _ratio)
        {
            imgCooltime.fillAmount = _ratio;
        }

        private void OnSkillSlotClickedCallback()
        {
            OnSkillSlotClicked?.Invoke(skillKey);
        }
    }
}