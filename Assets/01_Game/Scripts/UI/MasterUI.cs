using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Game.Scripts.UI
{
    public class MasterUI : MonoBehaviour
    {
        public static MasterUI Global { get; private set; }
        
        [SerializeField] protected List<ScreenUI> _screens;
        [SerializeField] protected Transform screenParent;
        [SerializeField] protected CanvasGroup transition;
        
        protected Dictionary<Type, ScreenUI> _screenLookup;
        protected ScreenUI _activeScreen;

        public bool IsShowDialog { get; private set; }
        protected virtual void Awake() {
            if (Global != null && Global != this)
            {
                Destroy(gameObject); // Prevent duplicates
                return;
            }
            Global = this;
            DontDestroyOnLoad(gameObject);
            _screenLookup = new Dictionary<Type, ScreenUI>();

            var allScreens = Resources.LoadAll<ScreenUI>("UI");
            foreach (var screen in allScreens)
            {
                if (!_screens.Contains(screen))
                {
                    _screens.Add(screen);
                }
            }
            _screens = _screens
                .OrderBy(s => s.IsModal ? 1 : 0)  // non-modal (false) first
                .ThenBy(s => s.priority)          // then sort by priority ascending
                .ToList();
            foreach (var screen in _screens)
            {
                var scr = Instantiate(screen,screenParent);
                scr.gameObject.SetActive(false);
                scr.Controller = this;

                var t = scr.GetType();
                while (true) {
                    if (!_screenLookup.ContainsKey(t))
                        _screenLookup.Add(t, scr);
                    if (t.BaseType == null || typeof(ScreenUI).IsAssignableFrom(t) == false || t.BaseType == typeof(ScreenUI)) {
                        break;
                    }

                    t = t.BaseType;
                }
            }

            foreach (var screen in _screens) {
                screen.Init();
            }
        }
        protected virtual void Start() {
            //PreferenceService.Instance.SetLastTimeShowInters(ToolUtils.GetCurrentUnixTimestampSeconds());
            /*if (_screens != null && _screens.Count > 0)
            {
                var screen = _screenLookup.FirstOrDefault().Value;
                screen.gameObject.SetActive(true);
                _activeScreen = screen;
                screen.OnShow();
            }
            transition?.DOFade(0, 0.5f).From(1).OnComplete(() =>
            {
                transition.gameObject.SetActive(false);
            });*/
        }
        public virtual void Show<S>() where S : ScreenUI {
            if (_screenLookup.TryGetValue(typeof(S), out var result)) {
                if (result.IsModal == false && _activeScreen != result && _activeScreen) {
                    _activeScreen.gameObject.SetActive(false);
                    _activeScreen.OnHide();
                }
                if (_activeScreen != result) {
                    result.gameObject.SetActive(true);
                }
                if (result.IsModal == false) {
                    _activeScreen = result;
                }
                result.OnShow();
            } else {
                Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
            }
            IsShowDialog = CheckDialogOpen();
        }
        public virtual void Show(string screenName) {
            // Find the screen type from the name
            var screenType = _screenLookup.Keys.FirstOrDefault(t => t.Name == screenName);
            if (screenType != null && _screenLookup.TryGetValue(screenType, out var result)) {
                if (!result.IsModal && _activeScreen != result && _activeScreen != null) {
                    _activeScreen.gameObject.SetActive(false);
                    _activeScreen.OnHide();
                }
                if (_activeScreen != result) {
                    result.gameObject.SetActive(true);
                }
                if (!result.IsModal) {
                    _activeScreen = result;
                }
                result.OnShow();
            } else {
                Debug.LogError($"Show() - Screen name '{screenName}' not found");
            }
            IsShowDialog = CheckDialogOpen();
        }
        public virtual void ShowTransit<S>(Action onComplete = null) where S : ScreenUI
        {
            Transit(() =>
            {
                onComplete?.Invoke();
                Show<S>();
            });
        }
        public virtual S Get<S>() where S : ScreenUI {
            if (_screenLookup.TryGetValue(typeof(S), out var result)) {
                return result as S;
            } else {
                Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
                return null;
            }
    
        }

        public void HideAllDialog()
        {
            foreach (var screen in _screenLookup)
            {
                if (screen.Value.IsModal)
                {
                    screen.Value.HideScreen();
                }
            }
        }
        public bool CheckDialogOpen()
        {
            foreach (var screen in _screenLookup)
            {
                if (screen.Value.gameObject.activeInHierarchy && screen.Value.IsModal)
                {
                    Debug.Log($"Show() - Screen type '{screen.Value.gameObject.name}' is open");
                    return true;
                }
            }
            return false;
        }

        public void UpdateScreenStatus()
        {
            IsShowDialog = CheckDialogOpen();
        }
        public virtual void Transit(Action action)
        {
            transition.alpha = 0;
            transition.gameObject.SetActive(true);
            Sequence.Create()
                .Chain(Tween.Alpha(transition, 0f, 1f, 0.3f))
                .ChainCallback(() =>
                {
                    action?.Invoke();
                })
                .Chain(Tween.Alpha(transition, 0f, 0.3f))
                .ChainCallback(() =>
                {
                    transition.gameObject.SetActive(false);
                });
        }
    }
}
