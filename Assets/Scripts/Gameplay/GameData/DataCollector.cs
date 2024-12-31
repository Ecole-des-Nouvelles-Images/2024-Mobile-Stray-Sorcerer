using System;
using Manager;
using Player;
using UnityEngine;
using Utils;

namespace Gameplay.GameData
{
    public class DataCollector : SingletonMonoBehaviour<DataCollector>
    {
        public static Action OnMonsterDeath;
        public static Action OnMazeComplete;
        public static Action OnPlayerSpawned;
        public int Kill{get; private set;}
        public int MazeComplete{get; private set;}
        public int PlayerLevel{get; private set;}
        public int PlayerMaxHp{get; private set;}
        public int PlayerHp{get; private set;}
        public int PlayerConstitution{get; private set;}
        public int PlayerSwiftness{get; private set;}
        public int PlayerPower{get; private set;}
        //------------ Best Run Data ---------------------
        public int BRDkill{get; private set;}
        public int BRDmazeComplete{get; private set;}
        public float BRDtime{get; private set;}
        public int BRDplayerLevel{get; private set;}
        public int BRDplayerMaxHp{get; private set;}
        public int BRDplayerHp{get; private set;}
        public int BRDplayerConstitution{get; private set;}
        public int BRDplayerSwiftness{get; private set;}
        public int BRDplayerPower{get; private set;}
        //------------------------------------------------
        
        private int _playerXp;
        private int _playerCurrentSpellIndex;
        private ClockGame _clockGame;   //on veut récup le temp actuel (float) et le niveau d'évolution des monstres (int)
        private Caretaker _caretaker = new Caretaker();
        private bool _restoreData;

        private void Awake()
        {
            _caretaker.LoadSnap();
            _clockGame = ClockGame.Instance;
            RestoreBestRunData();
            if (_caretaker.CurrentSave != null)
            {
                _restoreData = true;
                RestoreData(_caretaker.CurrentSave);
            }
        }
        private void OnEnable()
        {
            OnMonsterDeath += IncrementKill;
            OnMazeComplete += MazeFished;
            OnPlayerSpawned += UpdatePlayer;
        }
        private void OnDisable()
        {
            OnMonsterDeath -= IncrementKill;
            OnMazeComplete -= MazeFished;
        }
        private void Start()
        {
            Time.timeScale = 1;
        }
        private void RestoreData(Snapshot saveToRestore)
        {
            _clockGame.TimerGame = saveToRestore.Time;
            _clockGame.GrowingLevel = saveToRestore.MonsterLevel;
            Kill = saveToRestore.Kill;
            MazeComplete = saveToRestore.MazeComplete;
            PlayerLevel = saveToRestore.PlayerLevel;
            _playerXp = saveToRestore.PlayerXp;
            PlayerMaxHp = saveToRestore.PlayerMaxHp;
            PlayerHp = saveToRestore.PlayerHp;
            PlayerConstitution = saveToRestore.PlayerConstitution;
            PlayerSwiftness = saveToRestore.PlayerSwiftness;
            PlayerPower = saveToRestore.PlayerPower;
            _playerCurrentSpellIndex = saveToRestore.PlayerCurrentSpellIndex;
            UpdatePlayer();
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
        private Snapshot CreateSnapshot()
        {
            Snapshot data = new Snapshot();
            data.MazeComplete = MazeComplete;
            data.Kill = Kill;
            data.MonsterLevel = _clockGame.GrowingLevel;
            data.Time = _clockGame.TimerGame;
            data.PlayerLevel = PlayerLevel;
            data.PlayerXp = _playerXp; 
            data.PlayerMaxHp = PlayerMaxHp;
            data.PlayerHp = PlayerHp;
            data.PlayerConstitution = PlayerConstitution;
            data.PlayerSwiftness = PlayerSwiftness;
            data.PlayerPower = PlayerPower;
            data.PlayerCurrentSpellIndex = _playerCurrentSpellIndex;
            return data;
        }
        private void RestoreBestRunData()
        {
            if (_caretaker.BestSave != null)
            {
                BRDkill = _caretaker.BestSave.Kill;
                BRDmazeComplete = _caretaker.BestSave.MazeComplete;
                BRDtime = _caretaker.BestSave.Time;
                BRDplayerLevel = _caretaker.BestSave.PlayerLevel;
                BRDplayerMaxHp = _caretaker.BestSave.PlayerMaxHp;
                BRDplayerHp = _caretaker.BestSave.PlayerHp;
                BRDplayerConstitution = _caretaker.BestSave.PlayerConstitution;
                BRDplayerSwiftness = _caretaker.BestSave.PlayerSwiftness;
                BRDplayerPower = _caretaker.BestSave.PlayerPower;
            }
        }
        
        public void UpdateDataCollector()
        {
            if(Character.Instance)
            {
                PlayerLevel = Character.Instance.Level;
                PlayerConstitution=Character.Instance.Constitution;
                PlayerPower=Character.Instance.Power;
                PlayerSwiftness=Character.Instance.Swiftness;
                PlayerHp =Character.Instance.HP;
                PlayerMaxHp=Character.Instance.MaxHP;
                _playerXp=Character.Instance.EXP;
                _playerCurrentSpellIndex=Character.Instance.SpellUnlock;
            }
            Debug.Log("Update Data");
        }
        public void SaveDataAndContinue()
        {
            UpdateDataCollector();
            if(!_clockGame)
                _clockGame = ClockGame.Instance;
            _caretaker.UpdateCurrentSave(CreateSnapshot());
            Destroy(Character.Instance.gameObject);
            SceneLoader.Instance.ReloadGameScene();
        }
        public void SaveDataAndLeave()
        {
            UpdateDataCollector();
            if(!_clockGame)
                _clockGame = ClockGame.Instance;
            _caretaker.UpdateCurrentSave(CreateSnapshot());
            Application.Quit();
        }
        public void ResetSave()
        {
            _caretaker.CleanCurrentSave(CreateSnapshot());
        }
        public void UpdatePlayer()
        {
            if(Character.Instance&& _restoreData)
            {
                Debug.Log("attribution stat to player");
                Character.Instance.Level = PlayerLevel;
                Character.Instance.Constitution = PlayerConstitution;
                Character.Instance.Power = PlayerPower;
                Character.Instance.Swiftness = PlayerSwiftness;
                Character.Instance.HP = PlayerHp;
                Character.Instance.MaxHP = PlayerMaxHp;
                Character.Instance.EXP = _playerXp;
                Character.Instance.SpellUnlock = _playerCurrentSpellIndex;
                Character.Instance.UpdateSpell();
            }
            else
            {
                UpdateDataCollector();
            }
        }
        
    }
}
