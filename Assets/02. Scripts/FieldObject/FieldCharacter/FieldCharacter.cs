using MS.Skill;
using UnityEngine;


namespace MS.Field
{
    public abstract class FieldCharacter : FieldObject
    {
        private Animator animator;

        public SkillSystemComponent SSC { get; private set; }
        public Animator Animator => animator;


        virtual protected void Awake()
        {
            SSC = gameObject.AddComponent<SkillSystemComponent>();
            animator = GetComponent<Animator>();
        }

        public abstract void ApplyKnockback(Vector3 _dir, float _force);
    }
}