using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера камеры
    /// </summary>
    [CreateAssetMenu(fileName = "Camera Manager", menuName = "Game Manager/Camera Manager", order = 3)]
    public class CameraManager : BaseManager, IAwake, IUpdate
    {
        private Camera _camera;
        private Transform _target;

        public void OnAwake()
        {
            UpdateManager.Register(this);

            _camera = Camera.main;
        }

        public void OnUpdate()
        {
            var inputManager = ManagerContainer.Get<InputManager>();

            var horInput = inputManager.HorizontalInput.Value;
            var vertInput = inputManager.VerticalInput.Value;

            _camera.transform.Translate(new Vector3(horInput, 0, vertInput));
        }

        public void FollowCamera(Transform target)
        {
            _target = target;
        }

        public void UnFollowCamera()
        {
            _target = null;
        }
    }
}
