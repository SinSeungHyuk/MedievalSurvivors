using MS.Manager;
using UnityEngine;


namespace MS.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class MSEffect : MonoBehaviour
    {
        private ParticleSystem _particle;

        public string PoolKey { get; set; }


        void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
            if (_particle != null)
            {
                var main = _particle.main;
                main.stopAction = ParticleSystemStopAction.Callback;
            }
        }

        void OnEnable()
        {
            if (_particle != null)
            {
                _particle.Play();
            }
        }

        void OnParticleSystemStopped()
        {
            ObjectPoolManager.Instance.Return(PoolKey, this.gameObject);
        }

        public void InitEffect(string _poolKey)
        {
            PoolKey = _poolKey;
        }
    }
}