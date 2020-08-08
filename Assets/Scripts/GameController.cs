using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : UnityGlassGestureDetector
{


    int points;
    int healthRemaining;
    int maxHealth;

    [SerializeField]
    int maximumEnemies = 10;

    [SerializeField]
    int currentEnemiesInPlay = 0;

    [SerializeField]
    int initialEnemiesToSpawn = 2;

    [SerializeField]
    GameObject alienSaucer = null;

    [SerializeField]
    GameObject missilePrefab = null;

    [SerializeField]
    Camera playerCam = null;

    [SerializeField]
    Camera playArea = null;
    public Camera _playArea { get { return playArea; } }

    [SerializeField]
    private GameObject damageIndicator = null;

   

    [SerializeField]
    List<AlienSaucer> activeAlienSaucers = new List<AlienSaucer>();

    [SerializeField]
    List<AlienSaucer> inactiveAlienSaucers = new List<AlienSaucer>();


    [SerializeField]
    List<MissileController> activeMissiles = new List<MissileController>();

    [SerializeField]
    List<MissileController> inactiveMissiles = new List<MissileController>();


    [SerializeField]
    private float timeBetweenSpawns = 2f;

    [SerializeField]
    private float spawnTimeDecreaseRate = 0.9f;

    bool isPaused = false;
    public bool _isPaused { get { return isPaused; } }



    #region----------GUI----------

    [SerializeField]
    private GameObject explosion = null;
    public GameObject _explosion {  get { return explosion; } }

    [SerializeField]
    private TextMeshProUGUI pointsText = null;

   


    #endregion



    //create singleton - This means that there can only be one of this in the scene at any given time
    public static GameController instance;
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialEnemiesToSpawn; i++)
        {
            CreateNewSaucer();
        }
        Debug.Log(timeBetweenSpawns * (currentEnemiesInPlay / spawnTimeDecreaseRate));

        StartCoroutine(AddNewEnemyTimer());

      //  GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      //  cube.transform.position = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0)) Tap();
    }



    public void ModifyHealth(int healthChange)
    {
        //If the health bonus will give the user more health than the maximum then reduce the health bonus so it doesn't go over the max
        if(healthChange + healthRemaining >= maxHealth)
        {
            healthChange -= maxHealth - healthChange;
        }

        if(healthChange < 0)
        {
            damageIndicator.SetActive(true);
        }

        
        healthRemaining += healthChange;

        if(healthRemaining <= 0)
        {
            EndGame();
        }
    }



    public void DeactivateSaucer(AlienSaucer saucer)
    {
        activeAlienSaucers.Remove(saucer);
        inactiveAlienSaucers.Add(saucer);
        saucer.gameObject.SetActive(false);
        currentEnemiesInPlay--;

    }


    private void AddNewSaucer()
    {
        Debug.Log("Adding new saucer now");
        if (inactiveAlienSaucers.Count >= 1)
        {
            ReactivateSaucer(inactiveAlienSaucers[0]); 
        }
        else
        {
            CreateNewSaucer();
        }
    }



    private void CreateNewSaucer()
    {
        int spawnSide = UnityEngine.Random.Range(1, 4);
        Vector3 spawnPos = new Vector3();

        if (spawnSide == 1) spawnPos = playArea.ViewportToWorldPoint(new Vector3(0, Random.Range(0, 1), 5));  //Left Side
        if (spawnSide == 2) spawnPos = playArea.ViewportToWorldPoint(new Vector3(1, Random.Range(0, 1), 5)); //Right Side
        if (spawnSide == 3) spawnPos = playArea.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), 1, 5)); //Top
        if (spawnSide == 4) spawnPos = playArea.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), 0, 5)); //Bottom
        currentEnemiesInPlay++;
        GameObject newSaucer = GameObject.Instantiate(alienSaucer, spawnPos, Quaternion.identity);
        activeAlienSaucers.Add(newSaucer.transform.GetComponent<AlienSaucer>());
    }



    private void ReactivateSaucer(AlienSaucer saucerToReactivate)
    {
        activeAlienSaucers.Add(saucerToReactivate);
        inactiveAlienSaucers.Remove(saucerToReactivate);


        int spawnSide = UnityEngine.Random.Range(1, 4);
        Vector3 spawnPos = new Vector3();

        if (spawnSide == 1) spawnPos = playArea.ViewportToWorldPoint(new Vector3(0, Random.Range(0, 1), 3));  //Left Side
        if (spawnSide == 2) spawnPos = playArea.ViewportToWorldPoint(new Vector3(1, Random.Range(0, 1), 3)); //Right Side
        if (spawnSide == 3) spawnPos = playArea.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), 1, 3)); //Top
        if (spawnSide == 4) spawnPos = playArea.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), 0, 3)); //Bottom
        currentEnemiesInPlay++;
        saucerToReactivate.transform.position = spawnPos;
        saucerToReactivate.gameObject.SetActive(true);
    }


    public void DeactivateMissile(MissileController deactivatedMissile)
    {
        activeMissiles.Remove(deactivatedMissile);
        inactiveMissiles.Add(deactivatedMissile);
        deactivatedMissile.gameObject.SetActive(false);
    }

    public MissileController ReactivateMissile()
    {
        MissileController newMissile = null;
        if (inactiveMissiles.Count >= 1)
        {
            newMissile = inactiveMissiles[0];
            inactiveMissiles.Remove(newMissile);
            
        }
        else
        {
            newMissile = Instantiate(missilePrefab, new Vector3(999, 999, 999), Quaternion.identity).transform.GetComponent<MissileController>();
        }

        activeMissiles.Add(newMissile);

        return newMissile;
    }

    public int GetActiveMissiles()
    {
        return activeMissiles.Capacity;
    }


    private void EndGame()
    {

    }


    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        pointsText.SetText(points.ToString());
    }


    

    
    public void SetPause(bool set = true)
    {
        isPaused = set;

        if(isPaused == true) OnSwipeDown.Invoke();
        


    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        SetPause(true);
    }


    private IEnumerator AddNewEnemyTimer()
    {
        yield return new WaitForSeconds((float)(timeBetweenSpawns * (currentEnemiesInPlay / spawnTimeDecreaseRate)));
        AddNewSaucer();
        StartCoroutine(AddNewEnemyTimer());
    }



    public void QuitApplication()
    {
        Application.Quit(0);
    }



    #region -----------Gestures-----------

    protected override void SwipeDown()
    {
        base.SwipeDown();
        Debug.Log("Swiped down");

        SetPause(true);
         
        

    }

    [ContextMenu("Tap")]
    protected override void Tap()
    {
        base.Tap();

        if(isPaused == false)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f)), playerCam.transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.GetComponent<IShootable>() != null)
                {
                    hit.transform.GetComponent<IShootable>().TakeDamage();
                    Debug.DrawRay(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f)), playerCam.transform.forward * hit.distance, Color.green);
                    Debug.Log("Did Hit");
                }
                else
                {
                    Debug.DrawRay(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f)), playerCam.transform.forward * 20, Color.red, 5);
                }
            }
            else
            {
                
                    Debug.DrawRay(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f)), playerCam.transform.forward * 20, Color.red, 5);
                
            }
           
        }
    }

    #endregion

}
