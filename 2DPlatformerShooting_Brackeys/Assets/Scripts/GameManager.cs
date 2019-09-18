using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public Transform playerPrefab;
    public Transform spawnPoint;
  // public Transform spawnPrefab;
    public int spawnDelay = 2;

    public CameraShake camShake;

    private void Awake()
    {
        if (gm == null)
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        if (camShake == null)
            Debug.LogError("No camera shake referenced in game manager");
    }

    public IEnumerator _RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm._RespawnPlayer());
    }


    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }


    public void _KillEnemy(Enemy _enemy)
    {
        camShake.Shake(_enemy.shakeAmt, _enemy.shakeLen);
        Destroy(_enemy.gameObject);
    }
}
