using UnityEngine;
using TMPro;

namespace UI.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TextToParticles: MonoBehaviour
    {
        [SerializeField] private TMP_Text _textEmitter;

        private ParticleSystem _ps;
        private ParticleSystemRenderer _psRenderer;

        private Animator _animator;
        private static readonly int Spawn = Animator.StringToHash("Spawn");
        private static readonly int Dissolve = Animator.StringToHash("Dissolve");

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ps && _textEmitter)
                InitializePS();
        }
#endif

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _ps = GetComponent<ParticleSystem>();

            if (!_ps)
                _ps = gameObject.AddComponent<ParticleSystem>();

            _psRenderer = _ps.GetComponent<ParticleSystemRenderer>();
        }

        [ContextMenu("[EDITOR] Initialize PS")]
        private void InitializePS()
        {
            _textEmitter.mesh.UploadMeshData(false);

            ParticleSystem.ShapeModule psShape = _ps.shape;

            psShape.shapeType = ParticleSystemShapeType.MeshRenderer;
            psShape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
            psShape.meshSpawnMode = ParticleSystemShapeMultiModeValue.Random;
            psShape.mesh = _textEmitter.mesh;
            psShape.meshRenderer = _textEmitter.GetComponent<MeshRenderer>();
            _psRenderer.mesh = _textEmitter.mesh;
        }

        public void SpawnText()
        {
            _animator.SetTrigger(Spawn);
        }

        public void DissolveText()
        {
            _animator.SetTrigger(Dissolve);
        }

        public void TriggersParticles()
        {
            _ps.Play();
        }
    }
}