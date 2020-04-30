using Assets.Scripts.Managers;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Скрипт отображает текущее выбранное оружие
    /// </summary>
    public class CurrentWeaponTextScript : MonoBehaviour
    {
        [Header("Ссылки на объекты событий")]
        [Tooltip("Ссылка на события смены оружия")]
        public GameEventScriptableObject WeaponChangedEvent;

        [Header("Ссылки на компоненты")]
        [Tooltip("Ссылка на TextMeshProUGUI отображающее текущее оружие")]
        [SerializeField]
        private TextMeshProUGUI CurrentWeaponTextMesh;

        private void Awake()
        {
            WeaponChangedEvent.OnGameEvent += WeaponChangedHandler;
        }

        private void Start()
        {
            WeaponChangedHandler();
        }

        private void WeaponChangedHandler()
        {
            CurrentWeaponTextMesh.text = ManagerContainer.Get<WeaponManager>().CurrentWeapon.WeaponName;
        }

        private void OnDisable()
        {
            WeaponChangedEvent.OnGameEvent -= WeaponChangedHandler;
        }
    }
}

