using System.Collections;
using System.Collections.Generic;
using Maze;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace AI
{
    public class SquadDistributor : SingletonMonoBehaviour<SquadDistributor>
    {
        [SerializeField] private Transform _parentSquads;
        [SerializeField] private GameObject[] _squadsPrefabs;
        [SerializeField] private MazeBuilder _mazeBuilder;
        [SerializeField] private Vector2 _playerSpawnPosition = Vector2.zero;
        [SerializeField] private int _offsetPosition = 2;

        private int _scale;
        private int[,] _distribution;
        private int _squadsQtt;
        private string _debugDistribution;
        private List<Vector2> _emptyPosition;

        private void Awake()
        {
            _emptyPosition = new List<Vector2>();
            _scale = _mazeBuilder.Scale;
            _distribution = new int [_scale, _scale];
            if (_offsetPosition > _scale / 4)
                _offsetPosition = _scale / 4;
            if (_offsetPosition < 1)
                _offsetPosition = 1;
            if (_playerSpawnPosition.x < 0)
                _playerSpawnPosition.x = 0;
            if (_playerSpawnPosition.x > _scale)
                _playerSpawnPosition.x = _scale;
            if (_playerSpawnPosition.y < 0)
                _playerSpawnPosition.y = 0;
            if (_playerSpawnPosition.y > _scale)
                _playerSpawnPosition.y = _scale;
        }

        private void Start()
        {
            _squadsQtt = _scale * _scale / 3;
            //SquadsDistributionInLab();
        }

        private IEnumerator OffsetZoneForPlayer()
        {
            for (int i = (int)_playerSpawnPosition.x - _offsetPosition; i < (int)_playerSpawnPosition.x + _offsetPosition + 1; i++)
            for (int j = (int)_playerSpawnPosition.y - _offsetPosition; j < (int)_playerSpawnPosition.y + _offsetPosition + 1; j++)
            {
                if (i >= 0 && i <= _scale && j >= 0 && j <= _scale) _distribution[i, j] = -1;

                yield return null;
            }
        }

        private IEnumerator SearchEmpty()
        {
            for (int i = 0; i < _distribution.GetLength(0); i++)
            for (int j = 0; j < _distribution.GetLength(1); j++)
            {
                if (_distribution[i, j] == 0)
                {
                    Vector2 position = new(i, j);
                    _emptyPosition.Add(position);
                }

                yield return null;
            }
        }

        private IEnumerator SquadDistribution(int squadDistributed)
        {
            if (squadDistributed == _squadsQtt) yield break;

            int randomPos = Random.Range(0, _emptyPosition.Count - 1);
            _distribution[(int)_emptyPosition[randomPos].x, (int)_emptyPosition[randomPos].y] = 1;
            _emptyPosition.Remove(_emptyPosition[randomPos]);

            yield return StartCoroutine(SquadDistribution(squadDistributed + 1));
        }

        private void Display()
        {
            for (int i = 0; i < _distribution.GetLength(0); i++)
            for (int j = 0; j < _distribution.GetLength(1); j++)
            {
                if (i % _scale == 0 || j % _scale == 0)
                    _debugDistribution += "\n";
                _debugDistribution += _distribution[i, j];
            }

            Debug.Log(_debugDistribution);
        }

        private IEnumerator SquadsCreation()
        {
            for (int i = 0; i < _distribution.GetLength(0); i++)
            for (int j = 0; j < _distribution.GetLength(1); j++)
            {
                if (_squadsPrefabs.Length == 0)
                {
                    Debug.LogError("No prefab for squads in list");
                    yield break;
                }

                if (_distribution[i, j] == 1)
                    Instantiate(_squadsPrefabs[Random.Range(0, _squadsPrefabs.Length - 1)],
                        new Vector3(i * 20, 0, j * 20), quaternion.identity, _parentSquads);

                yield return null;
            }
        }

        public IEnumerator Reset()
        {
            for (int i = 0; i < _distribution.GetLength(0); i++)
            for (int j = 0; j < _distribution.GetLength(1); j++)
            {
                _distribution[i, j] = 0;
                yield return null;
            }

            _emptyPosition.Clear();
            _debugDistribution = new string("");
        }

        public IEnumerator SquadsDistributionInLab()
        {
            yield return Reset();
            yield return OffsetZoneForPlayer();
            yield return SearchEmpty();
            yield return SquadDistribution(0);
            yield return SquadsCreation();
        }
    }
}