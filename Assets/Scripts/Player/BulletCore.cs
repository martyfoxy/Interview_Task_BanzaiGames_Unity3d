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

        //Описание оружия из которого была выстрелена пуля
        private WeaponScriptableObject _weaponDescription;

        //Список компонентов
        private Rigidbody _rigidBody;

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

        /// <summary>
        /// Задать описание оружия
        /// </summary>
        /// <param name="weaponDesc">Описание оружия</param>
        public void SetDescription(WeaponScriptableObject weaponDesc)
        {
            _weaponDescription = weaponDesc;
        }

        /// <summary>
        /// Получить описание оружия
        /// </summary>
        /// <returns>Описание оружия</returns>
        public WeaponScriptableObject GetDescription()
        {
            return _weaponDescription;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var otherLayer = collision.collider.gameObject.layer;

            if ((DestroyingLayers & 1 << otherLayer) == 1 << otherLayer)
                gameObject.SetActive(false);
        }
    }
}
