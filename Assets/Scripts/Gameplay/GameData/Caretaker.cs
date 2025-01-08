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
            if (BestSave == null)
            {
                BestSave = CurrentSave;
                SaveBestSnap();
            }
            else if (BestSave.MazeComplete < CurrentSave.MazeComplete)
            {
                BestSave = CurrentSave;
                SaveBestSnap();
            }
            else if (BestSave.MazeComplete == CurrentSave.MazeComplete &&
                     BestSave.Time < CurrentSave.Time)
            {
                BestSave = CurrentSave;
                SaveBestSnap();
            }
        }

        private void SaveCurrentSnap()
        {
            string filePath = Application.persistentDataPath + "/CurrentSave.json" ;
            string data = JsonUtility.ToJson(CurrentSave,true);
            File.WriteAllText(filePath,  data);
            Debug.Log(Application.persistentDataPath);
        }
        private void SaveBestSnap()
        {
            string filePath = Application.persistentDataPath + "/BestSave.json" ;
            string data = JsonUtility.ToJson(BestSave, true);
            File.WriteAllText(filePath, data);
            Debug.Log(Application.persistentDataPath);
        }
        private void LoadCurrentSnap()
        {
            string filePath = Application.persistentDataPath + "/CurrentSave.json" ;
            if (File.Exists(filePath))
            {
                string data = System.IO.File.ReadAllText(filePath);
                CurrentSave = JsonUtility.FromJson<Snapshot>(data);
            }
            Debug.Log(Application.persistentDataPath);
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
            SaveCurrentSnap();
        }
        public void CleanCurrentSave(Snapshot newSave)
        {
            UpdateCurrentSave(newSave);
            CurrentSave = null;
            string filePath = Application.persistentDataPath + "/CurrentSave.json" ;
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public void LoadSnap()
        {
            LoadCurrentSnap();
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
