using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Класс менеджера камеры
    /// </summary>
    public class CameraManager : SingletonBase<CameraManager>
    {
        private Camera _camera;
        private Transform _target;

        private void Awake()
        {
            _camera = Camera.main;
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
