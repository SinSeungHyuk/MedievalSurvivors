using Core;
using Cysharp.Threading.Tasks;
using MS.Core;
using MS.UI;
using MS.Utils;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


namespace MS.Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, GameObject> uiPrefabDict = new Dictionary<string, GameObject>(); // 어드레서블을 통해 로드한 UI 원본
        private Dictionary<string, BaseUI> cachedUIDict = new Dictionary<string, BaseUI>(); // 실제 씬에 배치되어 있는 캐시

        private Transform viewCanvas;
        private Transform popupCanvas;
        private Transform systemCanvas;

        private BaseUI curViewUI;


        protected override void Awake()
        {
            base.Awake();

            viewCanvas = transform.FindChildDeep("ViewCanvas");
            popupCanvas = transform.FindChildDeep("PopupCanvas");
            systemCanvas = transform.FindChildDeep("SystemCanvas");

            BaseUI titlePanel = transform.FindChildComponentDeep<BaseUI>("TitlePanel");
            BaseUI loadingPanel = transform.FindChildComponentDeep<BaseUI>("LoadingPanel");
            cachedUIDict.Add("TitlePanel", titlePanel);
            cachedUIDict.Add("LoadingPanel", loadingPanel);
            curViewUI = titlePanel;
        }


        #region Show UI
        public T ShowView<T>(string _key) where T : BaseUI
        {
            curViewUI?.Close();

            if (cachedUIDict.TryGetValue(_key, out BaseUI _viewUI))
            {
                _viewUI.Show();
                curViewUI = _viewUI;
                return curViewUI.GetComponent<T>();
            }
            if (uiPrefabDict.TryGetValue(_key, out GameObject _loadUI))
            {
                BaseUI viewInstance = Instantiate(_loadUI, viewCanvas).GetComponent<BaseUI>();
                viewInstance.name = _key;
                viewInstance.Show();

                cachedUIDict.Add(_key, viewInstance);
                curViewUI = viewInstance;
                return curViewUI.GetComponent<T>();
            }

            Debug.LogError($"[UIManager] ShowView :: Key '{_key}' not found.");
            return null;
        }

        //public BasePopup ShowPopup(string _key)
        //{

        //}

        public BaseUI ShowSystemUI(string _key)
        {
            if (cachedUIDict.TryGetValue(_key, out BaseUI _systemUI))
            {
                _systemUI.Show();
                return _systemUI;
            }
            if (uiPrefabDict.TryGetValue(_key, out GameObject _loadUI))
            {
                BaseUI viewInstance = Instantiate(_loadUI, viewCanvas).GetComponent<BaseUI>();
                viewInstance.name = _key;
                viewInstance.Show();

                cachedUIDict.Add(_key, viewInstance);
                return viewInstance;
            }

            Debug.LogError($"[UIManager] ShowView :: Key '{_key}' not found.");
            return null;
        }

        public void CloseUI(string _key)
        {
            if (cachedUIDict.TryGetValue(_key, out BaseUI ui))
                ui.Close();
            else
                Debug.LogWarning($"[UIManager] CloseUI : {_key}");
        }
        #endregion

        #region Damage Text
        public void ShowDamageText(Vector3 _pos, int _damage, bool _isCritic)
        {
            GameObject textObj = ObjectPoolManager.Instance.Get("DamageText", _pos + Vector3.up * 2.0f, Quaternion.identity);
            var damageText = textObj.GetComponent<DamageText>();
            damageText.InitDamageText(_damage, _isCritic);
        }
        public void ShowEvasionText(Vector3 _pos)
        {
            GameObject textObj = ObjectPoolManager.Instance.Get("DamageText", _pos + Vector3.up * 2.0f, Quaternion.identity);
            var damageText = textObj.GetComponent<DamageText>();
            damageText.InitEvasionText();
        }
        #endregion


        public async UniTask LoadAllUIPrefabAsync()
        {
            try
            {
                IList<GameObject> loadedUIs = await AddressableManager.Instance.LoadResourcesLabelAsync<GameObject>("UI");

                if (loadedUIs == null)
                {
                    Debug.LogError("[UIManager] BaseUI 로드에 실패했습니다.");
                    return;
                }

                foreach (GameObject baseUI in loadedUIs)
                {
                    if (baseUI != null && !uiPrefabDict.ContainsKey(baseUI.name))
                    {
                        uiPrefabDict.Add(baseUI.name, baseUI);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[UIManager] 로드 실패: {e.Message}");
            }
        }
    }
}