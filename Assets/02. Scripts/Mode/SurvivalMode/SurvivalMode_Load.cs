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
            await EffectManager.Instance.LoadAllEffectAsync();
            await SkillObjectManager.Instance.LoadAllSkillObjectAsync();
            await MonsterManager.Instance.LoadAllMonsterAsync(); // todo :: 스테이지에 나오는 몬스터만 로드하도록 수정

            GameObject map = await AddressableManager.Instance.LoadResourceAsync<GameObject>(stageSettingData.MapKey);
            FieldMap mapInstance = GameObject.Instantiate(map,Vector3.zero, Quaternion.identity).GetComponent<FieldMap>();
            
            await PlayerManager.Instance.SpawnPlayerCharacter("TestCharacter");


            // todo test
            MonsterManager.Instance.SpawnMonster("TestMonster", new Vector3(5, 8, 5), Quaternion.identity);
        }
    }
}