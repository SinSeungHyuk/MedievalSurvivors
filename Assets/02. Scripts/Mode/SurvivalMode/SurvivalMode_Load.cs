using Cysharp.Threading.Tasks;
using MS.Field;
using MS.Manager;
using UnityEngine;

namespace MS.Mode
{
    public partial class SurvivalMode
    {
        private void OnLoadEnter(int _prev, object[] _params)
        {
            LoadSurvivalModeAsync().Forget();
        }

        private void OnLoadUpdate(float _dt)
        {

        }

        private void OnLoadExit(int _next)
        {

        }

        private async UniTaskVoid LoadSurvivalModeAsync()
        {
            GameObject map = await AddressableManager.Instance.LoadResourceAsync<GameObject>(stageSettingData.MapKey);
            FieldMap mapInstance = GameObject.Instantiate(
                map,
                Vector3.zero,
                Quaternion.identity
            ).GetComponent<FieldMap>();

            await PlayerManager.Instance.SpawnPlayerCharacter("TestCharacter");
        }
    }
}