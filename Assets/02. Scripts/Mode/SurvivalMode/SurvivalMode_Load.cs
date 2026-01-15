using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.UI;
using UnityEngine;
using UnityEngine.InputSystem;

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
            UIManager.Instance.ShowSystemUI<BaseUI>("LoadingPanel");

            await EffectManager.Instance.LoadAllEffectAsync();
            await SkillObjectManager.Instance.LoadAllSkillObjectAsync();
            await FieldItemManager.Instance.LoadAllFieldItemAsync();
            await MonsterManager.Instance.LoadAllMonsterAsync(stageSettingData);

            GameObject map = await AddressableManager.Instance.LoadResourceAsync<GameObject>(stageSettingData.MapKey);
            curFieldMap = GameObject.Instantiate(map,Vector3.zero, Quaternion.identity).GetComponent<FieldMap>();

            player = await PlayerManager.Instance.SpawnPlayerCharacter("TestCharacter");

            battlePanel = UIManager.Instance.ShowView<BattlePanel>("BattlePanel");
            BattlePanelViewModel data = new BattlePanelViewModel(this, player);
            battlePanel.InitBattlePanel(data);

            player.InitPlayer("TestCharacter"); // todo
            player.LevelSystem.CurLevel.Subscribe(OnPlayerLevelUpCallback);
            player.SSC.OnDeadCallback += OnPlayerDeadCallback;

            modeStateMachine.TransitState((int)SurvivalModeState.BattleStart);
            UIManager.Instance.CloseUI("LoadingPanel");
        }
    }
}