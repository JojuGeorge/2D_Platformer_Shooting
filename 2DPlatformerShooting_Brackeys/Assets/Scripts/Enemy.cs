using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damage=10;

        private int _curHealth;
        public int CurHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            CurHealth = maxHealth;
        }
    }
    public EnemyStats stats = new EnemyStats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    public float shakeAmt = .1f;
    public float shakeLen = .1f;

    private void Start()
    {
        stats.Init();

        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
        }
    }


    public void DamageEnemy(int damage)
    {
        stats.CurHealth -= damage;

        if (stats.CurHealth <= 0)
        {
            Debug.Log("Kill Enemy");
            GameManager.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
        }
    }


    private void OnCollisionEnter2D(Collision2D _other)
    {
        Player _player = _other.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(999999);
        }
    }

}
