using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Companion : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Run,
        Walk,
        Fear,
        Death,
        GettingHit,
        PickUp,
        WalkPickUp
    }
    public SkeletonAnimation skeletonAnimation;
    CharacterState previousState, currentState;

    [SerializeField]
    public int health = 5;
    int maxHealth;

    [Header("Movements")]
    public float movementSpeed;
    public float acceleration;
    public float decelerationStep;
    float activeSpeed;
    Rigidbody2D myRigidbody;
    public float timeToChangeDirection;
    public float newPointDistanceCriteria;
    Vector2 randomPoint;
    float timeLeftToChangeDirection;
    Vector2 moveVelocity;
    float xDirection;
    float yDirection;
    bool shieldOn;
    public float PlayerDetectionRadius = 1f;
    bool isMoving, isDying, isGettingHit;


    [Header("Under Shield Movement")]
    public float followSpeed;
    public float followAcceleration;
    float followActiveSpeed;
    PlayerController Shield;
    bool startFollowingFlag;


    [Header("Fleeing Movement")]
    public float fleeSpeed;
    public float enemyDetectionRadius;
    public float enemyLeftDetectionRadius;
    public bool isFleeing;
    public bool isShaking;
    GameObject closestEnemy;
    public float shakingDuration;
    GameObject previousClosestEnemy;
    public float evadeDuration;
    bool canPlayDeathSound, canPlayFearVoice, canPlayGameOverSound;

    [Header("Constraints")]
    public float maxRight;
    public float maxLeft;
    public float maxUp;
    public float maxDown;
    public GameObject canvas;
    //public SpriteRenderer sprite;

    [Header("Collecting Pickup Movement")]
    public float collectSpeed;
    public float pickupDetectionRadius;
    List<GameObject> pickups;
    GameObject targetPickup;
    bool isCarryingPickup;
    GameObject CompBar;
    GameObject playerObject;
    List<GameObject> currentEnemies;
    Color spriteColor;

    [Header("Power-ups")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shootingDuration;
    public float radius;
    public int numOfBullets;
    public int shootingNumOfTimes;
    public float nextShootingWaitingTime;
    bool isShooting;
    public void SetShieldOn(bool isShieldOn)
    {
        shieldOn = isShieldOn;
    }
    public bool IsShielded()
    {
        return shieldOn;
    }

    // Start is called before the first frame update
    void Start()
    {
        canPlayDeathSound = true;
        canPlayFearVoice = true;
        canPlayGameOverSound = true;
        CompBar = GameObject.FindGameObjectWithTag("CompBar");

        maxHealth = health;
        //sprite = GetComponent<SpriteRenderer>();
        //spriteColor = sprite.color;
        myRigidbody = GetComponent<Rigidbody2D>();
        activeSpeed = 0;
        followActiveSpeed = 0;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        timeLeftToChangeDirection = timeToChangeDirection;
        do
        {
            xDirection = Random.Range(-1f, 1.1f);
        } while (xDirection == 0);
        do
        {
            yDirection = Random.Range(-1f, 1.1f);
        } while (yDirection == 0);

        moveVelocity = new Vector2(xDirection * movementSpeed, yDirection * movementSpeed);

        currentEnemies = FindObjectOfType<Enemy_Spawner>().GetCurrentEnemies();

        pickups = new List<GameObject>();
        FindPickups();

        Shield = GetComponent<PlayerController>();
        /*canvas = GameObject.Find("GameOver");

        canvas.SetActive(false);*/

        while (Vector2.Distance(randomPoint, transform.position) < newPointDistanceCriteria)
        {
            randomPoint = new Vector2(Random.Range(maxLeft, maxRight), Random.Range(maxUp, maxDown));
        }
    }
    void HandleStateChanged()
    {
        // When the state changes, notify the animation handle of the new state.
        string stateName = null;
        switch (currentState)
        {
            case CharacterState.Idle:
                stateName = "idle";
                break;
            case CharacterState.Run:
                stateName = "runing";
                break;
            case CharacterState.Walk:
                stateName = "walk";
                break;
            case CharacterState.Death:
                stateName = "Death";
                break;
            case CharacterState.Fear:
                stateName = "fear";
                break;
            case CharacterState.GettingHit:
                stateName = "getting hit";
                break;
            case CharacterState.PickUp:
                stateName = "pick up";
                break;
            case CharacterState.WalkPickUp:
                stateName = "walk-pick up";
                break;
            default:
                break;
        }

        if (skeletonAnimation != null) skeletonAnimation.AnimationName = stateName;

    }

    public void FindPickups()
    {
        GameObject[] currentPickups = GameObject.FindGameObjectsWithTag("Pickup");
        for(int i=0; i < currentPickups.Length; i++)
        {
            pickups.Add(currentPickups[i]);
        }
    }
    public void AddNewPickup(GameObject newPickup)
    {
        pickups.Add(newPickup);
    }
    public void UpdateCurrentEnemies()
    {
        currentEnemies = FindObjectOfType<Enemy_Spawner>().GetCurrentEnemies();
    }
    public void FollowPlayer()
    {
        if (startFollowingFlag)
        {
            SoundManger.CompanionMovement();
        }
        startFollowingFlag = false;
        Vector2 followDirection = playerObject.transform.position - transform.position;

        if (playerObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            followActiveSpeed = 0;
            isMoving = false;
            Debug.Log("Companion stopped moving");
        }
        else
        {
            isMoving = true;
        }
                
        Accelerate(followDirection);
        
    }
    private void Accelerate(Vector2 followDirection)
    {
        if (!shieldOn)
        {
            if (activeSpeed < movementSpeed)
                activeSpeed += acceleration;
            myRigidbody.velocity = followDirection * activeSpeed;
        }
        else
        {
            if (followActiveSpeed < followSpeed)
                followActiveSpeed += followAcceleration;
            myRigidbody.velocity = followDirection * followActiveSpeed;
        }
    }
    private void Accelerate_PositionBased(Vector2 targetPosition)
    {
        isMoving = true;
        if (activeSpeed < movementSpeed)
            activeSpeed += acceleration;
        transform.position= Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * activeSpeed);
    }
    private void Decelerate()
    {
        if (xDirection == 0)
        {
            if (moveVelocity.x > 0)
                moveVelocity -= new Vector2(decelerationStep, 0);
            else
            {
                moveVelocity.x = 0;
            }
            if (moveVelocity.x < 0)
                moveVelocity -= new Vector2(-decelerationStep, 0);
            else
            {
                moveVelocity.x = 0;
            }
        }

        if (yDirection == 0)
        {
            if (moveVelocity.y > 0)
                moveVelocity -= new Vector2(0, decelerationStep);
            else
            {
                moveVelocity.y = 0;
            }
            if (moveVelocity.y < 0)
                moveVelocity -= new Vector2(0, -decelerationStep);
            else
            {
                moveVelocity.y = 0;
            }
        }

        myRigidbody.velocity = moveVelocity;
    }
    private void Decelerate_PositionBased()
    {
        if (activeSpeed > 0)
        {
            activeSpeed -= decelerationStep;
        }
        transform.position = Vector2.MoveTowards(transform.position, randomPoint, Time.deltaTime * activeSpeed);

        if (activeSpeed <= 0)
        {
            while (Vector2.Distance(randomPoint, transform.position) < newPointDistanceCriteria)
            {
                randomPoint = new Vector2(Random.Range(maxLeft, maxRight), Random.Range(maxUp, maxDown));
            }
            activeSpeed = 0;
            isMoving = false;
        }
        
    }
    private Vector2 ChangeDirection()
    {
        if (transform.position.x >= maxRight)
        {
            xDirection = -1;
        }
        else if (transform.position.x <= maxLeft)
        {
            xDirection = 1;
        }
        else
            xDirection = Random.Range(-1, 2);

        if (transform.position.y >= maxUp)
        {
            yDirection = -1;
        }
        else if (transform.position.y <= maxDown)
        {
            yDirection = 1;
        }
        else
            yDirection = Random.Range(-1, 2);

        if (moveVelocity.x == 0 && moveVelocity.y == 0)
        {
            xDirection = 0;
            yDirection = 0;
            Decelerate();
            if (transform.position.x > 0)
                xDirection = -1;
            else
                xDirection = 1;

            if (transform.position.y > 0)
                yDirection = -1;
            else
                yDirection = 1;
        }

        moveVelocity = new Vector2(xDirection * movementSpeed, yDirection * movementSpeed);

        timeLeftToChangeDirection = timeToChangeDirection; //reset timer

        return moveVelocity;
    }
    private void MoveAround()
    {
        timeLeftToChangeDirection -= Time.deltaTime;
        if (timeLeftToChangeDirection <= 0)
        {
            moveVelocity = ChangeDirection();
        }

        if (transform.position.x >= maxRight && moveVelocity.x > 0)
        {
            moveVelocity.x = -1 * movementSpeed;
        }
        if (transform.position.x <= maxLeft && moveVelocity.x < 0)
        {
            moveVelocity.x = 1 * movementSpeed;
        }
        if (transform.position.y >= maxUp && moveVelocity.y > 0)
        {
            moveVelocity.y = -1 * movementSpeed;
        }
        if (transform.position.y <= maxDown && moveVelocity.y < 0)
        {
            moveVelocity.y = 1 * movementSpeed;
        }

        Accelerate(moveVelocity);
    }
    private void BounceAround()
    {
        if (transform.position.x >= maxRight && moveVelocity.x > 0)
        {
            moveVelocity.x = -1 * movementSpeed;
        }
        if (transform.position.x <= maxLeft && moveVelocity.x < 0)
        {
            moveVelocity.x = 1 * movementSpeed;
        }
        if (transform.position.y >= maxUp && moveVelocity.y > 0)
        {
            moveVelocity.y = -1 * movementSpeed;
        }
        if (transform.position.y <= maxDown && moveVelocity.y < 0)
        {
            moveVelocity.y = 1 * movementSpeed;
        }

        Accelerate(moveVelocity);
    }
    private float GetRandomRange()
    {
        return (float)Random.Range(-10, 11);
    }
    private void MoveToEndsOfRange()
    {
        if (isShaking)
        {
            myRigidbody.velocity = new Vector2(0, 0);
            return;
        }
        if (isFleeing)
        {
            FleeFromEnemy();
            return;
        }

        //First, change directions based on current position:
        if (transform.position.x >= maxRight && moveVelocity.x > 0)
        {
            xDirection = -1;
            do
            {
                yDirection = GetRandomRange();
            } while (yDirection == 0);
            Debug.Log("From max right, y direction = " + yDirection.ToString());
            //PREVIOUS SOLUTION:-------------
            /*if (moveVelocity.y != 0)
                xDirection = Random.Range(-1, 0);
            else
                xDirection = -1;
            yDirection = Random.Range(-1, 1);*/
            //------------------------------
            activeSpeed = 0;
        }
        if (transform.position.x <= maxLeft && moveVelocity.x < 0)
        {
            xDirection = 1;
            do
            {
                yDirection = GetRandomRange();
            } while (yDirection == 0);
            Debug.Log("From max left, y direction = " + yDirection.ToString());
            //PREVIOUS SOLUTION:-------------
            /*if (moveVelocity.y != 0)
                xDirection = Random.Range(0, 1);
            else
                xDirection = 1;
            yDirection = Random.Range(-1, 1);*/
            //------------------------------
            activeSpeed = 0;
        }
        if (transform.position.y >= maxUp && moveVelocity.y > 0)
        {
            yDirection = -1;
            do
            {
                xDirection = GetRandomRange();
            } while (xDirection == 0);
            Debug.Log("From max up, x direction = " + xDirection.ToString());
            //PREVIOUS SOLUTION:-------------
            /*if (moveVelocity.x != 0)
                yDirection = Random.Range(-1, 0);
            else
                yDirection = -1;
            xDirection = Random.Range(-1, 1);*/
            //-----------------------------
            activeSpeed = 0;
        }
        if (transform.position.y <= maxDown && moveVelocity.y < 0)
        {
            yDirection = 1;
            do
            {
                xDirection = GetRandomRange();
            } while (xDirection == 0);
            Debug.Log("From max down, x direction = " + xDirection.ToString());
            //PREVIOUS SOLUTION:-------------
            /*if (moveVelocity.x != 0)
                yDirection = Random.Range(0, 1);
            else
                yDirection = 1;
            xDirection = Random.Range(-1, 1);*/
            //-------------------------------
            activeSpeed = 0;
        }
        if(transform.position.x>=maxRight && transform.position.y >= maxUp)
        {
            xDirection = -1;
            yDirection = -1;
        }
        if(transform.position.x<=maxLeft && transform.position.y <= maxDown)
        {
            xDirection = 1;
            yDirection = 1;
        }
        if(transform.position.x>=maxRight && transform.position.y <= maxDown)
        {
            xDirection = -1;
            yDirection = 1;
        }
        if (transform.position.x <= maxLeft && transform.position.y >= maxUp)
        {
            xDirection = 1;
            yDirection = -1;
        }

        /*//Second, avoid (zero, zero) velocity:
        if (moveVelocity.x == 0 && moveVelocity.y == 0)
        {
            if (transform.position.x != maxRight && transform.position.x != maxLeft)
            {
                xDirection = transform.position.x > 0 ? -1 : 1;
            }
            if (transform.position.y != maxUp && transform.position.y != maxDown)
            {
                yDirection = transform.position.y > 0 ? -1 : 1;
            }
        }*/
        //Lastly, apply directions to the movement:
        Vector2 direction = new Vector2(xDirection, yDirection).normalized;
        moveVelocity = new Vector2(direction.x * movementSpeed / 2, direction.y * movementSpeed / 2);
        Accelerate(moveVelocity);
    }
    private void MoveToRandomPosition()
    {
        myRigidbody.velocity = new Vector2(0, 0);
        if (Vector2.Distance(transform.position, randomPoint) < 2f)
        {
            Decelerate_PositionBased();
        }

        else
        {
            Accelerate_PositionBased(randomPoint);
        }
    }
    private void FleeFromEnemy()
    {
        moveVelocity = new Vector2(fleeSpeed, fleeSpeed);
        Vector2 moveDirection = transform.position - closestEnemy.transform.position;
        myRigidbody.velocity = moveVelocity * moveDirection.normalized;
        //Keep companion fleeing within the specified x & y range
        if (transform.position.x >= maxRight && moveVelocity.x > 0)
        {
            transform.position = new Vector2(maxRight, transform.position.y);
        }
        if (transform.position.x <= maxLeft && moveVelocity.x < 0)
        {
            transform.position = new Vector2(maxLeft, transform.position.y);
        }
        if (transform.position.y >= maxUp && moveVelocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, maxUp);
        }
        if (transform.position.y <= maxDown && moveVelocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, maxDown);
        }
    }
    private bool IsEnemyOnTheOtherSide(GameObject currentEnemy, GameObject previousEnemy)
    {
        if (previousEnemy != null && previousEnemy!=closestEnemy)
        {
            return ((transform.position.x >= currentEnemy.transform.position.x && transform.position.x <= previousEnemy.transform.position.x)
                || (transform.position.x >= previousEnemy.transform.position.x && transform.position.x <= currentEnemy.transform.position.x))
                && ((transform.position.y >= currentEnemy.transform.position.y && transform.position.y <= previousEnemy.transform.position.y)
                || (transform.position.y >= previousEnemy.transform.position.y && transform.position.y <= currentEnemy.transform.position.y));
        }
        else return true;
    }
    private void CalculateEnemyDistances()
    {
        float minDistance = enemyDetectionRadius + 10;
        if (currentEnemies != null)
        {
            foreach (GameObject enemy in currentEnemies)
            {
                if (enemy != null)
                {
                    if (Vector2.Distance(transform.position, enemy.transform.position) < minDistance)
                    {
                        minDistance = Vector2.Distance(transform.position, enemy.transform.position);
                        closestEnemy = enemy;
                    }
                }
            }
        }
        if (closestEnemy != null && closestEnemy != previousClosestEnemy &&
            Vector2.Distance(transform.position, closestEnemy.transform.position) <= enemyDetectionRadius &&
            IsEnemyOnTheOtherSide(closestEnemy, previousClosestEnemy))
        {
            StartCoroutine(ShakeBeforeFleeing());
            previousClosestEnemy = closestEnemy;
        }

        if (closestEnemy != null && Vector2.Distance(transform.position, closestEnemy.transform.position) > enemyLeftDetectionRadius)
        {
            isFleeing = false;
            closestEnemy = null;
        }

    }
    private GameObject SearchAvailablePickup()
    {
        foreach(GameObject pickup in pickups)
        {
            if (pickup != null && Vector2.Distance(transform.position, pickup.transform.position) <= pickupDetectionRadius
                && pickup.transform.position.x <= maxRight && pickup.transform.position.x >= maxLeft
                && pickup.transform.position.y <= maxUp && pickup.transform.position.y >= maxDown)
            {
                return pickup;
            }
        }
        return null;
    }
    private void GetPickup()
    {
        isMoving = true;
        transform.position = Vector2.MoveTowards(transform.position, targetPickup.transform.position, Time.deltaTime * collectSpeed);
        if (skeletonAnimation != null)
        {
            skeletonAnimation.Skeleton.ScaleX = targetPickup.transform.position.x - transform.position.x > 0 ? -1f : 1f;
        }
        if (Vector2.Distance(transform.position, targetPickup.transform.position) < 0.1f) //when companion is close enough to pickup but not colliding
        {
            targetPickup.gameObject.GetComponent<Pickup>().FollowCompanion(true);
            myRigidbody.velocity = Vector2.zero;
            isCarryingPickup = true;
        }
    }
    private void SpawnBullets()
    {
        float angleStep = 360f / numOfBullets;
        float angle = 0f;

        for (int i = 0; i < numOfBullets; i++)
        {
            float bulletDirXposition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 100) * radius;
            float bulletDirYposition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 100) * radius;

            Vector3 bulletVector = new Vector2(bulletDirXposition, bulletDirYposition);
            Vector2 bulletMoveDirection = (bulletVector - transform.position).normalized * bulletSpeed;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y);

            angle += angleStep;
        }
    }
    private void DetermineCharacterState()
    {
        if (!isMoving && !isDying && !isShooting && !isShaking)
        {
            currentState = CharacterState.Idle;
        }
        if (isMoving && isFleeing && !isDying)
        {
            currentState = CharacterState.Run;
        }
        if (isMoving && !isFleeing && !isDying)
        {
            currentState = CharacterState.Walk;
        }
        if (isDying)
        {
            currentState = CharacterState.Death;
        }
        if(isCarryingPickup && !isMoving)
        {
            currentState = CharacterState.PickUp;
        }
        if (isCarryingPickup && isMoving)
        {
            currentState = CharacterState.WalkPickUp;
        }
        if (isShaking && !isMoving && !isDying)
        {
            currentState = CharacterState.Fear;
        }
        if (isGettingHit)
        {
            currentState = CharacterState.GettingHit;
        }

        bool stateChanged = previousState != currentState;
        previousState = currentState;

        // Animation
        // Do not modify character parameters or state in this phase. Just read them.
        // Detect changes in state, and communicate with animation handle if it changes.
        if (stateChanged)
            HandleStateChanged();

    }
    // Update is called once per frame
    void Update()
    {
        if (health <= (int)health / 4)
        {
            SoundManger.CompanionHealthWarning(); //SHOULD BE STARTCOROUTINE TO AVOID OVERLAPPING!!
        }

        DetermineCharacterState();

        if (skeletonAnimation != null)
        {
            if(!shieldOn)
                skeletonAnimation.Skeleton.ScaleX = randomPoint.x - transform.position.x > 0 ? -1f : 1f;
            else
                skeletonAnimation.Skeleton.ScaleX = playerObject.transform.position.x - transform.position.x > 0 ? -1f : 1f;
        }

        //CalculateEnemyDistances();

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (playerObject!=null && Vector2.Distance(transform.position,playerObject.transform.position)<PlayerDetectionRadius) {
            if (playerObject.GetComponent<PlayerController>().shielded)
            {
                shieldOn = true;
            }
            else
            {
                shieldOn = false;
                startFollowingFlag = true;
            }
            /*  playerObject = GameObject.FindGameObjectWithTag("Player");
              Shield.checkShield()*/;
        }

        targetPickup = SearchAvailablePickup();
        if (targetPickup!=null && !targetPickup.GetComponent<Pickup>().IsFollowingCompanion() && !shieldOn)
        {
            GetPickup();
            return;
        }

        if (shieldOn)
        {
            if (playerObject != null)
            {
                FollowPlayer();
            }
        }
        else if (!isFleeing && !isCarryingPickup && targetPickup == null)
            MoveToRandomPosition();//MoveToEndsOfRange();

        if (targetPickup != null && targetPickup.GetComponent<Pickup>().IsFollowingCompanion())
        {
            isCarryingPickup = true;
            /*if (Vector2.Distance(playerObject.transform.position, transform.position) < 0.1f) //When companion is close enough but not colliding
            {
                targetPickup.GetComponent<Pickup>().Heal();
                Destroy(targetPickup);
                isCarryingPickup = false;
                randomPoint = new Vector2(Random.Range(maxLeft, maxRight), Random.Range(maxUp, maxDown));
            }*/
            transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, Time.deltaTime * collectSpeed);
        }

        if (transform.position.x > maxRight || transform.position.x < maxLeft
            || transform.position.y > maxUp || transform.position.y < maxDown)
        {
            MoveToRandomPosition();//MoveToEndsOfRange();
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.3f, 8.3f), Mathf.Clamp(transform.position.y, -4.5f, 4.5f), transform.position.z);


        //MoveAround();
        //BounceAround();
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Enemy")) 
        { 
            ChangeHealth(other.gameObject);
            StartCoroutine(FlashRed());
            if (isCarryingPickup && targetPickup!=null)
            {
                isCarryingPickup = false;
                targetPickup.GetComponent<Pickup>().FollowCompanion(false);
                targetPickup.transform.position += new Vector3(1, 1, 0);
                targetPickup = null;
            }
         }
        if(other.gameObject.tag=="Player")
        {
            if (isCarryingPickup)
            {
                targetPickup.GetComponent<Pickup>().StartEffect();
                //Destroy(targetPickup);
                isCarryingPickup = false;
                randomPoint = new Vector2(Random.Range(maxLeft, maxRight), Random.Range(maxUp, maxDown));
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCarryingPickup && collision.gameObject == targetPickup && collision.gameObject.tag == "Pickup"
            && collision.gameObject.GetComponent<Pickup>().pickUpType != Pickup.PickupTypes.ShootingCompanion)
        {
            collision.gameObject.GetComponent<Pickup>().FollowCompanion(true);
            myRigidbody.velocity = Vector2.zero;
            isCarryingPickup = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(Evade(collision.gameObject));
        }
    }




    public void ChangeHealth(GameObject hittedBy)
    {
        health--;
        StartCoroutine(FlashRed());
        CompBar.GetComponent<BarSystem>().Damage(1);

        if (hittedBy.tag == "Bullet")
        {
            SoundManger.HitCompanionByPlayer();
        }
        else SoundManger.HitCompanion();

        if (health <= 0)
        {
            if (canPlayDeathSound)
            {
                SoundManger.CompanionDeath();
                StartCoroutine(PlayDeathSound());
            }
            StartCoroutine(DelayCompanionDestruction());
            isDying = true;
        }

    }
    public void RestorHealth()
    {
        health = maxHealth;
        CompBar.GetComponent<BarSystem>().fullHeal();
    }

    IEnumerator ShakeBeforeFleeing()
    {
        Debug.Log("IS SHAKING!");
        isShaking = true;
        isFleeing = false;
        yield return new WaitForSeconds(shakingDuration);
        isShaking = false;
        isFleeing = true;
    }
    IEnumerator Evade(GameObject enemy)
    {
        if (canPlayFearVoice)
        {
            SoundManger.FearVoice();
            StartCoroutine(PlayFearVoice());
        }
        isFleeing = true;
        if (transform.position.x <= maxRight && transform.position.x >= maxLeft
            && transform.position.y <= maxUp && transform.position.y >= maxDown)
            myRigidbody.AddForce(transform.position - enemy.transform.position, ForceMode2D.Impulse);

        yield return new WaitForSeconds(evadeDuration);
        isFleeing = false;
        while (Vector2.Distance(randomPoint, transform.position) < newPointDistanceCriteria)
        {
            randomPoint = new Vector2(Random.Range(maxLeft, maxRight), Random.Range(maxUp, maxDown));
        }
    }
    public void StartShooting()
    {
        isShooting = true;
        int shootingCounter = 1;
        SpawnBullets();
        while (shootingCounter < shootingNumOfTimes)
        {
            StartCoroutine(NextBulletSpawn());
            shootingCounter++;
            //Debug.Log(shootingCounter);
        }
        isShooting = false;
    }
    IEnumerator NextBulletSpawn()
    {
        yield return new WaitForSeconds(nextShootingWaitingTime);
        SpawnBullets();
    }
    public IEnumerator DelayCompanionDestruction()
    {
        yield return new WaitForSeconds(0.4f);
        canvas.SetActive(true);
        if (canPlayGameOverSound)
        {
            SoundManger.GameOver();
            StartCoroutine(PlayGameOverSound());
        }
        Destroy(gameObject);
    }
    IEnumerator PlayFearVoice()
    {
        canPlayFearVoice = false;
        yield return new WaitForSeconds(0.25f);
        canPlayFearVoice = true;
    }
    IEnumerator PlayDeathSound()
    {
        canPlayDeathSound = false;
        yield return new WaitForSeconds(2);
        canPlayDeathSound = true;
    }
    IEnumerator PlayGameOverSound()
    {
        canPlayGameOverSound = false;
        yield return new WaitForSeconds(2);
        canPlayGameOverSound = true;
    }
    public IEnumerator FlashRed()
    {
        isGettingHit = true;
        yield return new WaitForSeconds(0.1f);
        isGettingHit = false;
    }

}
