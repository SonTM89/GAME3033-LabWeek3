using System.Collections;
using System.Collections.Generic;
using Systems.Health;
using UnityEngine;
using TMPro;

namespace UI.Player_UI
{
    public class HealthInfoUI : MonoBehaviour
    {
        [SerializeField] TMP_Text HealthText;

        private HealthComponent PlayerHealthComponent;

        private void OnEnable()
        {
            PlayerEvents.OnHealthInitialized += OnHealthInitialized;
        }

        private void OnDisable()
        {
            PlayerEvents.OnHealthInitialized -= OnHealthInitialized;
        }

        private void OnHealthInitialized(HealthComponent healthComponent)
        {
            PlayerHealthComponent = healthComponent;
        }

        // Update is called once per frame
        void Update()
        {
            HealthText.text = PlayerHealthComponent.Health.ToString();
        }
    }
}