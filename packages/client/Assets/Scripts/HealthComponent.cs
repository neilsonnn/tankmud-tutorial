using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud;
using mudworld;

public class HealthComponent : MUDComponent
{

    public int health;
    public bool dead;

    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public ParticleSystem m_hurtParticles, m_ExplosionParticles;


    protected override void PostInit() {
        m_ExplosionParticles.gameObject.SetActive(false);
    }
    
    protected override void UpdateComponent(MUDTable table, UpdateInfo updateInfo) {

        HealthTable update = table as HealthTable;
        health = (int)update.Value;
        dead = health <= 0;

        SetHealthUI();

        if(Loaded) {

            OnHit();

            if(dead) {
                //do some death animation
                OnDeath();
            }
        }
    }

    private void SetHealthUI() {
        // Adjust the value and colour of the slider.
        m_Slider.value = health;
        m_FillImage.color = health < 25 ? m_ZeroHealthColor : m_FullHealthColor;
    }

    void OnHit() {
        m_hurtParticles.Play();
    }

     void OnDeath() {
        m_ExplosionParticles.Play();
        Entity.Toggle(false);
    }


}
