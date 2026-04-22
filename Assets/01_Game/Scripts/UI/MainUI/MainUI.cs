using _01_Game.Scripts.Tool;
using _01_Game.Scripts.UI.Element;
using MainGame.Services.Utils;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace _01_Game.Scripts.UI.MainUI
{
    public class MainUI : ScreenUI
    {
        [Header("UI")] 
        [SerializeField] private TMP_Text goldTxt;
        [Header("Element")] 
        [SerializeField] private ProgressUnbox progressUnbox;
        [SerializeField] private Transform progressRoot;
        private void Start()
        {
            Observer.Instance.AddObserver(ObserverKey.OpenBox,OpenBox);
        }

        private void OnDestroy()
        {
            Observer.Instance.RemoveObserver(ObserverKey.OpenBox,OpenBox);
        }
        public void OpenBox(object data)
        {
            var timeProg = 10f;
            var trans = data as Transform;
            var prog = progressUnbox.Spawn();
            prog.transform.SetParent(progressRoot);
            prog.RunProgress(timeProg);
            
            var rect =  prog.GetComponent<RectTransform>();
            var canvas = transform.GetComponent<Canvas>();
            Vector2 screenPos = Camera.main.WorldToScreenPoint(trans.position + Vector3.up*2);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPos
            );
            rect.anchoredPosition = localPos;
            
            Tween.Delay(timeProg, () =>
            {
                prog.Recycle();
            });
        }
    }
}