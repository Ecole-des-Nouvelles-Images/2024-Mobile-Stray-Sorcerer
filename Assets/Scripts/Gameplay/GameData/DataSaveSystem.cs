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
        /*
         * On récupère les stats sauvegardé du joueur directement par arguments
         * donc 8 entiers:
         * _Level
         * _MaxHp
         * _Hp
         * _Constitution
         * _Swiftness
         * _Power
         * _Xp
         * _CurrentSpellIndex
         */
        public static Action<int, int, int, int, int, int, int, int> OnSaveGameData;
        public static Action<int, int, int, int, int, int, int, int> OnLoadGameData;
        public static Action OnResetGameData;
        public static Action<bool, float, float, float> OnSaveSettings;
        public static Action<bool, float, float, float> OnLoadSettings;
        
        public int Kill{get; private set;}
        public int MazeComplete{get; private set;}
        //------------ Best Run Data ---------------------
        public int BRDkill{get; private set;}
        public int BRDmazeComplete{get; private set;}
        public float BRDtime{get; private set;}
        public int BRDplayerLevel{get; private set;}
        public int BRDplayerConstitution{get; private set;}
        public int BRDplayerSwiftness{get; private set;}
        public int BRDplayerPower{get; private set;}
        //------------------------------------------------
        
        private ClockGame _clockGame;   //on veut récup le temp actuel (float) et le niveau d'évolution des monstres (int)
        private Caretaker _caretaker = new Caretaker();

        private void Awake()
        {
            _caretaker.LoadSnap();
            _clockGame = ClockGame.Instance;
            RestoreBestRunData();
            ResetSave();
        }
        private void OnEnable()
        {
            OnMonsterDeath += IncrementKill;
            OnMazeComplete += MazeFished;
            OnSaveGameData += SaveGameData;
            OnLoadGameData += RestoreGameData;
            OnSaveSettings += SaveSettings;
            OnLoadSettings += LoadSettings;
        }
        private void OnDisable()
        {
            OnMonsterDeath -= IncrementKill;
            OnMazeComplete -= MazeFished;
            OnSaveGameData -= SaveGameData;
            OnLoadGameData -= RestoreGameData;
            OnSaveSettings -= SaveSettings;
            OnLoadSettings -= LoadSettings;
        }
        private void Start()
        {
            Time.timeScale = 1;
        }
        private void RestoreGameData( int level, int xp, int maxHp, int hp, int constitution, int Swiftness, int power, int spellIndex)
        {
            if (_caretaker.CurrentSave != null)
            {
                _clockGame.TimerGame = _caretaker.CurrentSave.Time;
                _clockGame.GrowingLevel = _caretaker.CurrentSave.MonsterLevel;
                Kill = _caretaker.CurrentSave.Kill;
                MazeComplete = _caretaker.CurrentSave.MazeComplete;
                level = _caretaker.CurrentSave.PlayerLevel;
                xp = _caretaker.CurrentSave.PlayerXp;
                maxHp = _caretaker.CurrentSave.PlayerMaxHp;
                hp = _caretaker.CurrentSave.PlayerHp;
                constitution = _caretaker.CurrentSave.PlayerConstitution;
                Swiftness = _caretaker.CurrentSave.PlayerSwiftness;
                power = _caretaker.CurrentSave.PlayerPower;
                spellIndex = _caretaker.CurrentSave.PlayerCurrentSpellIndex;
            }
        }
        private void IncrementKill()
        {
            Kill++;
        }
        private void MazeFished()
        {
            _clockGame.ClockStop();
            MazeComplete++;
        }
        private void SaveGameData(int level, int xp, int maxHp, int hp, int constitution, int swiftness, int power, int spellIndex)
        {
            Snapshot data = new Snapshot();
            data.MazeComplete = MazeComplete;
            data.Kill = Kill;
            data.MonsterLevel = _clockGame.GrowingLevel;
            data.Time = _clockGame.TimerGame;
            data.PlayerLevel = level;
            data.PlayerXp = xp; 
            data.PlayerMaxHp = maxHp;
            data.PlayerHp = hp;
            data.PlayerConstitution = constitution;
            data.PlayerSwiftness = swiftness;
            data.PlayerPower = power;
            data.PlayerCurrentSpellIndex = spellIndex;
            _caretaker.UpdateCurrentSave(data);
        }
        
        public void RestoreBestRunData()
        {
            if (_caretaker.BestSave != null)
            {
                BRDkill = _caretaker.BestSave.Kill;
                BRDmazeComplete = _caretaker.BestSave.MazeComplete;
                BRDtime = _caretaker.BestSave.Time;
                BRDplayerLevel = _caretaker.BestSave.PlayerLevel;
                BRDplayerConstitution = _caretaker.BestSave.PlayerConstitution;
                BRDplayerSwiftness = _caretaker.BestSave.PlayerSwiftness;
                BRDplayerPower = _caretaker.BestSave.PlayerPower;
            }
        }
        public void ResetSave()
        {
            _caretaker.CleanCurrentSave();
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

        public int GetPlayerLevelData()
        {
            return _caretaker.CurrentSave.PlayerLevel;
        }
        public int GetPlayerConstitutionData()
        {
            return _caretaker.CurrentSave.PlayerConstitution;
        }
        public int GetPlayerSwiftnessData()
        {
            return _caretaker.CurrentSave.PlayerSwiftness;
        }
        public int GetPlayerPowerData()
        {
            return _caretaker.CurrentSave.PlayerPower;
        }
    }
}
