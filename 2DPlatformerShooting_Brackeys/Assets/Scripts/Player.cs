using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // So that this class contents can be viewed in inspector also the object must also be public for this to work
    [System.Serializable]       
    public class PlayerStats
    {
        public int maxHealth=100;
        private int _curHealth;

        public int CurHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }


        public void Init()
        {
            CurHealth = maxHealth;
        }
    }
    public PlayerStats stats = new PlayerStats();

    public int fallBoundary =-20;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start()
    {
        stats.Init();

        if (statusIndicator == null)
            Debug.LogError("No status indicator referenced on player");
        else
        {
            statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
        }
    }


    private void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }
         
    }



    public void DamagePlayer(int damage)
    {
        stats.CurHealth -= damage;

        if (stats.CurHealth <= 0)
        {
            GameManager.KillPlayer(this);
        }
        statusIndicator.SetHealth(stats.CurHealth, stats.maxHealth);
    }


}
