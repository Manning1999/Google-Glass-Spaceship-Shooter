using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSaucer : MonoBehaviour, IShootable
{

    int health = 1;

    [SerializeField]
    float moveSpeed = 0.3f;

    Vector3 targetDestination;
    private float distanceThreshhold = 0.4f;

    [SerializeField]
    private int pointsWhenKilled = 1;


    float baseTimeBetweenMissiles = 8;



    // Start is called before the first frame update
    void OnEnable()
    {
        DecideNewDestination();
        StartCoroutine(MissileTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance._isPaused == false) MoveToDestination();            
    }



    private void MoveToDestination()
    {

        transform.position = Vector3.Lerp(transform.position, targetDestination, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, targetDestination) <= distanceThreshhold)
        {
            DecideNewDestination();
        }
    }

    private void DecideNewDestination()
    {
        targetDestination = GameController.Instance._playArea.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(4, 7)));
    }


  


    [ContextMenu("Kill")]
    public void Die()
    {
        GameController.Instance.DeactivateSaucer(this);

        GameController.Instance._explosion.GetComponent<DisappearTimer>().SetLocation(transform.position);

        GameController.Instance._explosion.gameObject.SetActive(true);

        GameController.Instance.AddPoints(pointsWhenKilled);
    }

    public void TakeDamage(int damageTaken = 1)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            Die();
        }
    }


    private IEnumerator MissileTimer()
    {
        yield return new WaitForSeconds(4);

        GameController.Instance.ReactivateMissile().Reactivate(transform.position);
        StartCoroutine(MissileTimer());
    }
}
