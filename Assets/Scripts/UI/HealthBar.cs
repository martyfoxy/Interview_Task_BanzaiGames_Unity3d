using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ScriptableObjects.Variables;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Класс управляющий health bar'ом
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("Ссылки на объекты переменных")]
        [SerializeField]
        [Tooltip("Ссылка на переменную хранящую текущее здоровье")]
        private FloatReference HPVariable;

        [Header("Ссылки на компоненты")]
        [SerializeField]
        [Tooltip("Ссылка на изображение заполняющее шкалу")]
        private Image FillImage;

        [Tooltip("Ссылка на TextMeshProUGUI отображающее здоровье")]
        [SerializeField]
        private TextMeshProUGUI HealthTextMesh;

        //TODO: Избавиться от update!
        private void Update()
        {
            FillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, 100, HPVariable));
            HealthTextMesh.text = HPVariable.Value.ToString();
        }
    }
}
