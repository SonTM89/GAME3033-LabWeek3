using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptable_Objects;
using Character;

namespace Systems.Health
{
    public class HealthComponent : MonoBehaviour, iDamageable
    {
        public float Health => CurrentHealth;
        public float MaxHealth => TotalHealth;

        private float CurrentHealth;
        [SerializeField] private float TotalHealth;

        [SerializeField] private ConsumableScriptable potionItem;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            CurrentHealth = TotalHealth;

            CurrentHealth = 50;
        }

        public void HealPlayer(int effect)
        {
            if(CurrentHealth <= MaxHealth)
            {
                CurrentHealth = Mathf.Clamp( CurrentHealth + effect, 0, MaxHealth);
            }

            
        }

        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        public virtual void Destroy()
        {
            Debug.Log("You Died!");
            //Destroy(gameObject);
        }

        public void SetCurrentHealth(float currentHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
}