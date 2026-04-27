using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _01_Game.Scripts.Gameplay;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.Tool;
using UnityEngine;

namespace _01_Game.Scripts.Manager
{
    public class SaveDataManager : MonoBehaviour
    {
        public static SaveDataManager Global;
        
        private const string SaveKey = "SaveData_v100";
        
        [SerializeField] private SaveData saveData;
        private void Awake()
        {
            Global = this;
            LoadData();
        }
        #region LoadSave
        public void SaveData()
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }
        public void LoadData()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                // Initialize default data if no save exists
                saveData = new SaveData
                {
                    level = 1,
                    playerData = new PlayerData
                    {
                        gold = 100,
                    },
                    settingData = new SettingData(),
                    gardenData = new GardenData
                    {
                        treeData = new List<TreeData>
                        {
                            new TreeData{slot = 1},
                            new TreeData{slot = 2},
                            new TreeData{slot = 3},
                            new TreeData{slot = 4},
                        }
                    },
                    allUpgrade = new List<string>()
                };
            }
        }
        public SaveData GetSaveData()
        {
            return saveData;
        }
        public SettingData GetSetting()
        {
            return saveData.settingData;
        }
        #endregion

        #region Currency
        // --- Gold Management ---
        public BigInteger GetGold()
        {
            return saveData.playerData.gold;
        }

        public void AddGold(BigInteger amount)
        {
            saveData.playerData.gold += amount;
            Observer.Instance.Notify(ObserverKey.GoldKey);
        }

        public bool SubtractGold(BigInteger amount)
        {
            if (saveData.playerData.gold >= amount)
            {
                saveData.playerData.gold -= amount;
                Observer.Instance.Notify(ObserverKey.GoldKey);
                return true;
            }
            return false; // Not enough gold
            
        }
        #endregion
        
        #region Setting

        public void SetCameraZoom(float value)
        {
            saveData.settingData.zoomCamera = value;
            Observer.Instance.Notify(ObserverKey.ZoomCameraKey,value);
        }

        public float GetCameraZoom()
        {
            return saveData.settingData.zoomCamera;
        }
        
        public void SetMusic(bool value)
        {
            saveData.settingData.musicEnabled = value;
        }

        public bool GetMusic()
        {
            return saveData.settingData.musicEnabled;
        }
        
        public void SetSfx(bool value)
        {
            saveData.settingData.sfxEnabled = value;
        }

        public bool GetSfx()
        {
            return saveData.settingData.sfxEnabled;
        }
        
        public void SetVibration(bool value)
        {
            saveData.settingData.vibrationEnabled = value;
        }

        public bool GetVibration()
        {
            return saveData.settingData.vibrationEnabled;
        }
        #endregion

        #region Garden

        public List<string> GetAllUpgrade()
        {
            return saveData.allUpgrade;
        }
        public void AddUpgrade(string id)
        {
            saveData.allUpgrade.Add(id);
        }
        public void AddCustomer(int amount)
        {
            saveData.gardenData.amountCustomer += amount;
            Observer.Instance.Notify(ObserverKey.AddCustomer, amount); 
        }

        public int GetTotalCustomer()
        {
            return saveData.gardenData.amountCustomer;
        }
        public void UpgradeTree(int slot, int level)
        {
            var a = saveData.gardenData.treeData.FirstOrDefault(x => x.slot == slot);
            if (a != null)
            {

                a.level = level; 
            }

        }
        public void OpenTree(Transform tree)
        {
            if (tree.TryGetComponent(out TreeSlot data))
            {
                var a = saveData.gardenData.treeData.FirstOrDefault(x => x.slot == data.IndexSlot);
                if (a != null)
                {
                    a.status = TreeStatus.Available;
                }
            }
        }
        public TreeData GetTreeData(int slot)
        {
            return saveData.gardenData.treeData.FirstOrDefault(x => x.slot == slot);
        }

        #endregion
        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveData();
        }
    }
    [Serializable]
    public class SaveData
    {
        public int level = 1;
        public PlayerData playerData;
        public SettingData settingData;
        public GardenData gardenData;
        public List<string> allUpgrade;
    }
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private string goldStr = "0";
        public BigInteger gold
        {
            get => BigInteger.Parse(goldStr);
            set => goldStr = value.ToString();
        }
    }
    [Serializable]
    public class SettingData
    {
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool vibrationEnabled = true;
        public float zoomCamera = 1;

        public SettingData()
        {
            musicEnabled = true;
            sfxEnabled = true;
            vibrationEnabled = true;
            zoomCamera = 1;
        }
    }
}