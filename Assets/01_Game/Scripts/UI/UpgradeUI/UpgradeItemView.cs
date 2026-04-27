using System.Numerics;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Game.Scripts.UI.UpgradeUI
{
    public class UpgradeItemView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text displayNameTxt;
        [SerializeField] private TMP_Text descriptionTxt;
        [SerializeField] private TMP_Text priceTxt;
        [SerializeField] private Image icon;
        
        private UpgradeItemSO _data;

        public void Setup(UpgradeItemSO data)
        {
            _data = data;
            displayNameTxt.text = _data.displayName;
            descriptionTxt.text = _data.description;
            priceTxt.text = NumberFormatter.FormatShort(BigInteger.Parse(_data.price));
            icon.sprite = _data.icon;
        }

        public void OnBuy()
        {
            _data.Apply();
            Destroy(gameObject);
        }
    }
}