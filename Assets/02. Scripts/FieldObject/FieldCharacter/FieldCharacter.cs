using MS.Skill;
using UnityEngine;


namespace MS.Field
{
    public class FieldCharacter : FieldObject
    {
        private Animator animator;

        public SkillSystemComponent SSC { get; private set; }
        public Animator Animator => animator;


        private void Awake()
        {
            SSC = gameObject.AddComponent<SkillSystemComponent>();
            animator = GetComponent<Animator>();
        }
    }
}