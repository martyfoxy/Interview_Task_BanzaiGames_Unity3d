using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс для логики танка, которым управляет игрок
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankCore : MonoBehaviour
    {
        public TankScriptableObject TankDescription;

        //Ссылки на колеса танка
        [SerializeField]
        private GameObject[] _rightWheels;
        [SerializeField]
        private GameObject[] _leftWheels;

        private Rigidbody _rigidBody;
        private BoxCollider _boxCollider;

        private float _horInput;
        private float _vertInput;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            _horInput = Input.GetAxis("Horizontal");
            _vertInput = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            MoveTank();
            RotateTank();
        }

        private void MoveTank()
        {
            Vector3 movementVector = transform.forward * TankDescription.Speed *_vertInput * Time.fixedDeltaTime;

            _rigidBody.MovePosition(_rigidBody.position + movementVector);
        }

        private void RotateTank()
        {
            Quaternion rotationQuat = Quaternion.Euler(0f, _horInput * 45f * Time.fixedDeltaTime, 0f);

            _rigidBody.MoveRotation(_rigidBody.rotation * rotationQuat);
        }
    }
}
