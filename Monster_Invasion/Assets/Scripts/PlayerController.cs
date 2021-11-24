using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Run,
        Aim,
        Death,
        GettingHit
    }
    public SkeletonAnimation skeletonAnimation;

    public int health;
    int maxHealth;
    public ParticleSystem ps;

    [SerializeField]
    private GameObject shield;
    public GameObject canvas;
   
    [Header("Movements")]
    public float movementSpeed;
    public float acceleration;
    public float decelerationStep;
    public bool shielded;
    public bool canActivatetShield;
    public float fireRate = 0.2f;
    //public SpriteRenderer sprite;
    bool isMoving, isShooting, isDying, isGettingHit;
    bool canPlayFootStepSound, canPlayDeathSound, canPlayGameOverSound;

    float activeSpeed;
    Rigidbody2D myRigidbody;
    bool isStopping;
    int xDirection, yDirection;
    Vector2 input;
    Vector2 moveVelocity;
    bool movingRight;
    bool movingLeft;
    bool movingUp;
    bool movingDown;

    //Color spriteColor;

    CharacterState previousState, currentState;

    GameObject PlayerHP;
    GameObject Recharge;
    // GameObject enemy;

    public BarSystem barsystem;

    [Header("Shooting")]
    public GameObject bullet;
    public GameObject doubleBullet;
    private float nextFire = 0.0F;
    GameObject currentBullet;
    public float doubleBulletDuration;

    public void ChangeCurrentBullet(int numOfBullet)
    {
        if (numOfBullet == 1)
        {
            currentBullet = bullet;
        }
        if (numOfBullet == 2)
        {
            currentBullet = doubleBullet;
            StartCoroutine(DelayBulletChange());
            Debug.Log("Double shots!");
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        canPlayFootStepSound = true;
        canPlayDeathSound = true;
        canPlayGameOverSound = true;
        currentBullet = bullet;
        maxHealth = health;
        PlayerHP = GameObject.FindGameObjectWithTag("PlayerHP");
        Recharge = GameObject.FindGameObjectWithTag("RechargeBar");
      //  barsystem = GameButtons.FindObjectsOfType("barsystem");
        moveVelocity = new Vector2(0, 0);
        myRigidbody = GetComponent<Rigidbody2D>();
        activeSpeed = 0;
      //  canvas = GameObject.Find("GameOver");
        canvas.SetActive(false);

        //spriteColor = GetComponent<SpriteRenderer>().color;
        if (skeletonAnimation != null) skeletonAnimation.AnimationName = "idle";
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
                stateName = "Run";
                break;
            case CharacterState.Aim:
                stateName = "aim";
                break;
            case CharacterState.Death:
                stateName = "Death";
                break;
            case CharacterState.GettingHit:
                stateName = "getting hit";
                break;
            default:
                break;
        }

        if(skeletonAnimation!=null) skeletonAnimation.AnimationName = stateName;

    }
    private void MoveAround()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if (input.x != 0 || input.y != 0)
        {
            if (canPlayFootStepSound)
            {
                SoundManger.FootStep();
                StartCoroutine(PlayFootStepSounds());
            }
            isMoving = true;
            Accelerate();
            float angle = Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg - 90;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (skeletonAnimation != null)
            {
                skeletonAnimation.Skeleton.ScaleX = input.x > 0 ? -1f : 1f;
            }

        }
        else
        {
            isMoving = false;
        }
        if (input.x == 0 || input.y == 0)
        {
            Decelerate();
        }
        /*float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 moveVelocity = new Vector2(moveX, moveY);

        if (activeSpeed < 0)
            activeSpeed = 0;

        if (Input.GetAxisRaw("Horizontal")==0 && Input.GetAxisRaw("Vertical") == 0)
        {
            isStopping = true;
        }
        else
        {
            isStopping = false;
            xDirection = (int)moveX;
            yDirection = (int)moveY;
            float angle = Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        Accelerate(moveVelocity);*/
    }
    private void Accelerate()
    {
        if (input.x > 0 && moveVelocity.x < movementSpeed)
        {
            movingRight = true;
            movingLeft = false;
            moveVelocity += new Vector2(acceleration, 0);
        }
        else if (input.x < 0 && moveVelocity.x > -movementSpeed)
        {
            movingRight = false;
            movingLeft = true;
            moveVelocity += new Vector2(-acceleration, 0);
        }
        if (input.y > 0 && moveVelocity.y < movementSpeed)
        {
            movingUp = true;
            movingDown = false;
            moveVelocity += new Vector2(0, acceleration);
        }
        else if (input.y < 0 && moveVelocity.y > -movementSpeed)
        {
            movingUp = false;
            movingDown = true;
            moveVelocity += new Vector2(0, -acceleration);
        }
        myRigidbody.velocity = moveVelocity;
    }
    private void Decelerate()
    {
        if (input.x == 0)
        {
            if (movingRight)
            {
                if (moveVelocity.x > 0)
                    moveVelocity -= new Vector2(decelerationStep, 0);
                else
                {
                    moveVelocity.x = 0;
                    movingRight = false;
                }
            }
            if (movingLeft)
            {
                if (moveVelocity.x < 0)
                    moveVelocity -= new Vector2(-decelerationStep, 0);
                else
                {
                    moveVelocity.x = 0;
                    movingLeft = false;
                }
            }
        }

        if (input.y == 0)
        {
            if (movingUp)
            {
                if (moveVelocity.y > 0)
                    moveVelocity -= new Vector2(0, decelerationStep);
                else
                {
                    moveVelocity.y = 0;
                    movingUp = false;
                }
            }
            if (movingDown)
            {
                if (moveVelocity.y < 0)
                    moveVelocity -= new Vector2(0, -decelerationStep);
                else
                {
                    moveVelocity.y = 0;
                    movingDown = false;
                }
            }
        }
        myRigidbody.velocity = moveVelocity;
    }



            private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
            if (!shielded)
            {
                ChangeHealth();
                SoundManger.HitPlayer();
            }



}
    private void DetermineCharacterState()
    {
        if(!isMoving && !isDying && !isShooting && !isGettingHit)
        {
            currentState = CharacterState.Idle;
        }
        if(isMoving && !isDying)
        {
            currentState = CharacterState.Run;
        }
        if (isGettingHit)
        {
            currentState = CharacterState.GettingHit;
        }
        if (isDying)
        {
            currentState = CharacterState.Death;
        }
        if (isShooting)
        {
            currentState = CharacterState.Aim;
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
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health <= (int)health / 4)
        {
            SoundManger.PlayerHealthWarning(); //SHOULD BE STARTCOROUTINE TO AVOID OVERLAPPING!!
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.3f, 8.3f),Mathf.Clamp(transform.position.y,-4.5f, 4.5f), transform.position.z);
        MoveAround();
        Rotation();
        checkShield();

        //shoot fun

        if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
            StartCoroutine(DelayShootingStateChange());
            SoundManger.PlayshootingSound();

                nextFire = Time.time + fireRate;
            GameObject clone = Instantiate(currentBullet, transform.position, Quaternion.identity);

        }
        if(Input.GetKey(KeyCode.X) && canActivatetShield)
        {
            SoundManger.ShieldActivated();
            shield.SetActive(true);
            shielded = true;
            //code for turining off the shield
            barsystem.health = 0;
            Invoke("NoShoield", 9f);
        }

        DetermineCharacterState();
    }


    public void checkShield()
    {
        if (barsystem.health==200 && !shielded)
        {
            print(shield);
            canActivatetShield = true;
            SoundManger.FullyRechargedShield();
        }

    }

    public void RechargePoint(float increase)
    {
        SoundManger.RechargeShield();
        Recharge.GetComponent<BarSystem>().Heal(increase+10);
        checkShield();

    }
    void NoShoield()
    {
        SoundManger.ShieldDeactive();

        shield.SetActive(false);
        shielded = false;
        canActivatetShield = false;
    }

    public void ChangeHealth()
    {
        health--;
      PlayerHP.GetComponent<BarSystem>().Damage(1);

        StartCoroutine(FlashRed());


        if (health <= 0)
        {
            Ps();
            Die();
        }

    }




    private void Die()
    {
        isDying = true;
        StartCoroutine(DelayPlayerDestruction());
    }


    void Rotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.Find("FirePoint").transform.position.z - Camera.main.transform.position.z;
        Vector2 dir = Camera.main.ScreenToWorldPoint(mousePos) - transform.Find("FirePoint").transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Find("FirePoint").transform.rotation = rotation;

    }



    public void Ps()
    {
        ps.Play();
        ps = Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, 1);

    }
    public IEnumerator DelayShootingStateChange()
    {
        isShooting = true;
        yield return new WaitForSeconds(0.2f);
        isShooting = false;
    }
    public IEnumerator DelayPlayerDestruction()
    {
        if (canPlayDeathSound)
        {
            SoundManger.PlayerDeath();
            StartCoroutine(PlayDeathSound());
        }
        yield return new WaitForSeconds(1.5f);
        canvas.SetActive(true);
        if (canPlayGameOverSound)
        {
            SoundManger.GameOver();
            StartCoroutine(PlayGameOverSound());
        }
        Destroy(gameObject);
    }
    public IEnumerator DelayBulletChange()
    {
        yield return new WaitForSeconds(doubleBulletDuration);
        currentBullet = bullet;
    }
    public IEnumerator FlashRed()
    {
        //sprite.color = Color.red;
        isGettingHit = true;
        yield return new WaitForSeconds(0.1f);
        //sprite.color = spriteColor;
        isGettingHit = false;
    }
    IEnumerator PlayFootStepSounds()
    {
        canPlayFootStepSound = false;
        yield return new WaitForSeconds(0.25f);
        canPlayFootStepSound = true;
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

    public void RestorHealth()
    {
        health = maxHealth;
        PlayerHP.GetComponent<BarSystem>().fullHeal();
    }
}
