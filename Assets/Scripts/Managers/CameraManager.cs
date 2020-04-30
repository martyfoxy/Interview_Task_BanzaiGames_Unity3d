using Assets.Scripts.Interface;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера камеры
    /// </summary>
    [CreateAssetMenu(fileName = "Camera Manager", menuName = "Game Manager/Camera Manager")]
    public class CameraManager : BaseManager, IAwake, IFixedUpdate
    {
        [Header("Параметры")]
        [Tooltip("Скорость перемещения камеры")]
        public float CameraSpeed;
        [Tooltip("Отступ камеры от танка")]
        public Vector3 CameraOffset;

        [Header("Ссылки на объекты событий")]
        [Tooltip("Ссылки на объект с событием создания игрока")]
        public GameEventScriptableObject PlayerSpawnedEvent;

        private Camera _camera;
        private Transform _target;

        public void OnAwake()
        {
            UpdateManager.Register(this);

            _camera = Camera.main;

            PlayerSpawnedEvent.OnTransformEvent += PlayerTransformHandler;
        }

        public void OnFixedUpdate()
        {
            if (_target != null)
            {
                Vector3 desiredPos = Quaternion.AngleAxis(_target.rotation.eulerAngles.y, Vector3.up) * CameraOffset;

                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _target.position + desiredPos, Time.fixedDeltaTime * CameraSpeed);
                _camera.transform.LookAt(_target.position);
            }
        }

        private void OnDisable()
        {
            PlayerSpawnedEvent.OnTransformEvent -= PlayerTransformHandler;
        }

        /// <summary>
        /// Обработчик события создания игрока, в качестве аргумента передает Transform
        /// </summary>
        private void PlayerTransformHandler(TransformArgs args)
        {
            _target = args.TransformArg;
        }
    }
}
