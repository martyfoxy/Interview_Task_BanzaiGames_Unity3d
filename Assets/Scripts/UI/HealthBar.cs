using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ScriptableObjects.Variables;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Класс управляющий health bar'ом
    /// </summary>

    //TODO: Избавиться от update!
    public class HealthBar : MonoBehaviour
    {
        [Header("Ссылки на объекты переменных")]
        [Tooltip("Ссылка на переменную хранящую текущее здоровье")]
        public IntReference HPVariable;

        [Space(10)]

        [Header("Ссылки на компоненты")]
        [Tooltip("Ссылка на изображение заполняющее шкалу")]
        [SerializeField]
        private Image FillImage;

        [Tooltip("Ссылка на TextMeshProUGUI отображающее здоровье")]
        [SerializeField]
        private TextMeshProUGUI HealthTextMesh;

        private void Update()
        {
            FillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, 100, HPVariable));
            HealthTextMesh.text = HPVariable.Value.ToString();
        }
    }
}
