
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.UI
{
    public class ScreenUI : MonoBehaviour
    {
        [HideInInspector]
        public MasterUI Controller;
        public bool IsModal;
        public int priority;
        [Header("Popup Animation")]
        [SerializeField] private Animator popupAnimator;
        [SerializeField] private CanvasGroup bg;
        public virtual void Init()
        {

        }

        public virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && IsModal)
            {
                HideScreen();
            }
        }

        public virtual void OnShow()
        {
            if (popupAnimator != null)
            {
                popupAnimator.Play("Open");
            }
            if (bg != null)
            {
                Tween.Alpha(bg, 0f, 1f, 0.5f);
            }
        }

        public virtual void OnHide()
        {
            
        }

        public virtual void HideScreen()
        {
            gameObject.SetActive(false);
            OnHide();
            Controller.UpdateScreenStatus();
        }

        public virtual void ShowScreenByName(string scr)
        {
            MasterUI.Global.Show(scr);
        }
    }
}