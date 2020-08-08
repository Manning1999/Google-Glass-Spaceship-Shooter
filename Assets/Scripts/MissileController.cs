using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour, IShootable
{

    [SerializeField]
    private Light light = null;

    [SerializeField]
    private float lightFluctuationSpeed = 1.5f;

    [SerializeField]
    private float lightMinRange = 0.7f;

    [SerializeField]
    private float lightMaxRange = 1.4f;

    bool lightRangegIsGoingUp = true;

    [SerializeField]
    bool isOnPlayerLevel = false;

    [SerializeField]
    private float moveSpeed = 0.7f;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LightFluctuation();

        if (Mathf.Abs(transform.position.y - GameObject.FindGameObjectWithTag("MainCamera").transform.position.y) < 0.15f)
        {
            isOnPlayerLevel = true;
        }
        else
        {
            isOnPlayerLevel = false;
        }
       

        if (isOnPlayerLevel == true)
        {
            MoveTowardsPlayer();    
        }
        else
        {
            GoToPlayerLevel();
        }

        
    }

    public void Reactivate(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void LightFluctuation()
    {
        if (lightRangegIsGoingUp == true)
        {
            light.range = Mathf.LerpUnclamped(light.range, lightMaxRange, lightFluctuationSpeed * Time.deltaTime);

        }
        else
        {
            light.range = Mathf.LerpUnclamped(light.range, lightMinRange, lightFluctuationSpeed * Time.deltaTime);
        }
        if ((Mathf.Abs(light.range - lightMaxRange) <= 0.01f && lightRangegIsGoingUp == true)|| (Mathf.Abs(light.range - lightMinRange) <= 0.01f && lightRangegIsGoingUp == false)) lightRangegIsGoingUp = !lightRangegIsGoingUp;

    }


    //Move to the y position of the player. This gives the player a little bit of extra time to shoot the missiles
    private void GoToPlayerLevel()
    {
        if(transform.position.y < GameObject.FindGameObjectWithTag("MainCamera").transform.position.y)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.LookAt(Vector3.up);
        }
        else
        {
            transform.position += Vector3.up * -moveSpeed * Time.deltaTime;
            transform.LookAt(Vector3.down);
        }

        
    }


    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position, moveSpeed * Time.deltaTime);
        transform.up = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
    }

    public void TakeDamage(int damageTaken = 1)
    {
        Die();
    }


    
    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject == GameObject.FindGameObjectWithTag("MainCamera"))
        {
            Die();
            GameController.Instance.ModifyHealth(-5);
        }
    }

    void Die()
    {
        GameController.Instance.DeactivateMissile(this);
        //GameController.Instance._explosion.GetComponent<DisappearTimer>().SetLocation(transform.position);

        //GameController.Instance._explosion.gameObject.SetActive(true);
    }
}
