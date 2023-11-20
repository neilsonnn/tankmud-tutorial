using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud;
using mudworld;

public class HealthComponent : MUDComponent
{

    public int health;
    
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;
    public GameObject shell;
    private ParticleSystem m_ExplosionParticles;
    private bool m_Dead;

    public void OnAttacked() {
        var initialShellPosition = transform.position;
        initialShellPosition.y += 10;
        Instantiate(shell, initialShellPosition, Quaternion.LookRotation(Vector3.down));
        SetHealthUI();
    }

    private void SetHealthUI() {
        // Adjust the value and colour of the slider.
        m_Slider.value = health;
        m_FillImage.color = health < 25 ? m_ZeroHealthColor : m_FullHealthColor;
    }

    public void OnDeath()
    {
        m_Dead = true;
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        gameObject.SetActive(false);
    }

    protected override void PostInit() {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    protected override void UpdateComponent(MUDTable table, UpdateInfo updateInfo) {

        HealthTable update = table as HealthTable;
        health = (int)update.Value;

        SetHealthUI();

        if(Loaded) {

            OnAttacked();

            if(health <= 0) {
                //do some death animation
                OnDeath();
            }
        }
    }

}
