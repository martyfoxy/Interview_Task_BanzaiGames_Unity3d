using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс отвечающий за поведение пуль
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class BulletCore : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Какие слои уничтожают пулю")]
        private LayerMask DestroyingLayers;

        /// <summary>
        /// Описание оружия из которого была выстрелена пуля
        /// </summary>
        public WeaponScriptableObject WeaponDescription
        {
            get
            {
                return _weaponDescription;
            }
            set
            {
                _weaponDescription = value;
            }
        }

        private Rigidbody _rigidBody;
        private WeaponScriptableObject _weaponDescription;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rigidBody.AddForce(transform.forward * 1000f, ForceMode.Acceleration);
        }

        private void OnDisable()
        {
            //При скрытии пули отменяем движущую силу
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((DestroyingLayers & 1 << otherLayer) == 1 << otherLayer)
                gameObject.SetActive(false);
        }
    }
}
