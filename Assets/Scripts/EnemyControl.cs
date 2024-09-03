using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour, IDamageable
{
    public Slider sliderHP;
    public ParticleSystem hitEffect;
    //public Button btnHit;
    //public Button btnDie;

    AudioSource screamSound;
    Animator animator;
    int enemyHP = 100;
    // Start is called before the first frame update
    void Start()
    {
        screamSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        //btnDie.onClick.AddListener(OnButtonDieClick);
        //btnHit.onClick.AddListener(OnButtonHitClick);
        sliderHP.value = enemyHP;
    }

    //void OnButtonHitClick()
    //{
    //    screamSound.Play();
    //    animator.SetTrigger("Hit");
    //}

    //void OnButtonDieClick()
    //{
    //    screamSound.Play();
    //    animator.SetTrigger("Die");
    //}

    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        
        screamSound.Play();

        enemyHP = enemyHP - (int)damage;
        if (enemyHP <= 0)
        {
            enemyHP = 0;
            animator.SetTrigger("Die");
        }
        else
        {
            animator.SetTrigger("Hit");
        }
        sliderHP.value = enemyHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
