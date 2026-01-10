using System;
using System.IO;
using Manager;
using Player;
using UnityEngine;
using Utils;

namespace Gameplay.GameData
{
    public class DataSaveSystem : SingletonMonoBehaviour<DataSaveSystem>
    {
        public static Action OnMonsterDeath;
        public static Action OnMazeComplete;
        public static Action<bool, float, float, float> OnSaveSettings;
        public static Action<bool, float, float, float> OnLoadSettings;
        
        public int Kill{get; private set;}
        //------------ Best Run Data ---------------------
        public int BRDKill { get; private set; }
        public float BRDTime{get; private set; }
        //------------------------------------------------
        
        private ClockGame _clockGame;   //on veut récup le temp actuel (float) et le niveau d'évolution des monstres (int)
        private Caretaker _caretaker = new Caretaker();

        private void Awake()
        {
            _caretaker.LoadSnap();
            _clockGame = ClockGame.Instance;
        }
        private void OnEnable()
        {
            OnMonsterDeath += IncrementKill;
            OnMazeComplete += MazeFished;
            OnSaveSettings += SaveSettings;
            OnLoadSettings += LoadSettings;
        }
        private void OnDisable()
        {
            OnMonsterDeath -= IncrementKill;
            OnMazeComplete -= MazeFished;
            OnSaveSettings -= SaveSettings;
            OnLoadSettings -= LoadSettings;
        }
        private void Start()
        {
            Time.timeScale = 1;
        }
        private void IncrementKill()
        {
            Kill++;
        }
        private void MazeFished()
        {
            _clockGame.ClockStop();
            SaveBestData();
        }
        private void SaveBestData()
        {
            Snapshot data = new Snapshot();
            data.Kill = Kill;
            data.Time = _clockGame.TimerGame;
            _caretaker.UpdateCurrentSave(data);
            BRDKill = _caretaker.BestSave.Kill;
            BRDTime = _caretaker.BestSave.Time;
        }

        public void SaveSettings(bool isLeftJoystick, float musicSlider, float sfxSlider, float luminositySlider)
        {
            SettingsSnapshot settings = new SettingsSnapshot();
            settings.IsLeftJoystick = isLeftJoystick;
            settings.MusicSlider = musicSlider;
            settings.SfxSlider = sfxSlider;
            settings.LuminositySlider = luminositySlider;
            _caretaker.UpdateSavedSettings(settings);
        }

        public void LoadSettings(bool isLeftJoystick, float musicSlider, float sfxSlider, float luminositySlider)
        {
            string filePath = Application.persistentDataPath + "/Settings.json" ;
            if (!File.Exists(filePath))
            {
                SaveSettings(isLeftJoystick,  musicSlider,  sfxSlider,  luminositySlider);
            }
            _caretaker.LoadSavedSettings();
            isLeftJoystick = _caretaker.SavedSettings.IsLeftJoystick;
            musicSlider = _caretaker.SavedSettings.MusicSlider;
            sfxSlider = _caretaker.SavedSettings.SfxSlider;
            luminositySlider = _caretaker.SavedSettings.LuminositySlider;
        }
    }
}
