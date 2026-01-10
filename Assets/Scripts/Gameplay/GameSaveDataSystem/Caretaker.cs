using System.IO;
using UnityEngine;

namespace Gameplay.GameData
{
    public class Caretaker
    {
        public Snapshot CurrentSave;
        public Snapshot BestSave;
        public SettingsSnapshot SavedSettings;
        
        private void BestSaveFilter()
        {
            if (BestSave.Kill == 0 && BestSave.Time == 0)
            {
                BestSave = CurrentSave;
                SaveBestSnap();
            }
            else {
                if (BestSave.Kill < CurrentSave.Kill) BestSave.Kill = CurrentSave.Kill;
                if(BestSave.Time > CurrentSave.Time) BestSave.Time = CurrentSave.Time;
                SaveBestSnap();
            }
        }
        private void SaveBestSnap()
        {
            string filePath = Application.persistentDataPath + "/BestSave.json" ;
            string data = JsonUtility.ToJson(BestSave, true);
            File.WriteAllText(filePath, data);
            //Debug.Log(Application.persistentDataPath);
        }
        private void LoadBestSnap()
        {
            string filePath = Application.persistentDataPath + "/BestSave.json" ;
            if (File.Exists(filePath))
            {
                string data = System.IO.File.ReadAllText(filePath);
                BestSave = JsonUtility.FromJson<Snapshot>(data);
            }
        }
        private void SaveSettings()
        {
            string filePath = Application.persistentDataPath + "/Settings.json" ;
            string data = JsonUtility.ToJson(SavedSettings, true);
            File.WriteAllText(filePath, data);
            Debug.Log(Application.persistentDataPath);
        }
        
        public void UpdateCurrentSave(Snapshot newSave)
        {
            CurrentSave = newSave;
            BestSaveFilter();
        }

        public void LoadSnap()
        {
            LoadBestSnap();
        }
        public void UpdateSavedSettings(SettingsSnapshot newSavedSettings)
        {
            SavedSettings = newSavedSettings;
            SaveSettings();
        }
        public void LoadSavedSettings()
        {
            string filePath = Application.persistentDataPath + "/Settings.json" ;
            if (File.Exists(filePath))
            {
                string data = System.IO.File.ReadAllText(filePath);
                SavedSettings = JsonUtility.FromJson<SettingsSnapshot>(data);
            }
        }
    }
}
