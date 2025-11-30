using Core;
using Unity.Cinemachine;
using UnityEngine;


namespace MS.Manager
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CinemachineCamera mainCamera;

        public CinemachineCamera MainCamera => mainCamera;


        public void InitMainCamera(Transform _target)
        {
            mainCamera.Follow = _target;
        }
    }
}