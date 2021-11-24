using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        GettingHit
    }
    public SkeletonAnimation skeletonAnimation;
    CharacterState previousState, currentState;
    bool isGettingHit;

    public GameObject[] drops;
    public float speed;
    public float health;
    private Transform targetPos;
    //private Transform ShieldPos;
    public float increase;
    public ParticleSystem ps;
    public string targetTag;

    public bool IsDestroyed = false;
    //    GameObject enemyBar;
    // PlayerController Shield;
    //GameObject enemy;

    private Shake shake;
    [Range(0,100)]
    public int probabilityOfPickupDrop;
    public GameObject droppedPickup;

    public int startAtWave;

    void Awake() => targetPos = targetTag=="Player"? GameObject.FindGameObjectWithTag("Player").transform : GameObject.FindGameObjectWithTag("Companion").transform;

  
     void Start()
    {
        //enemyBar = GameObject.FindGameObjectWithTag("Enemy");
        // transform.Find("Enemy").gameObject.GetComponent<BarSystem>();
        //enemy = GameObject.FindGameObjectWithTag("Enemy");

        shake = GameObject.FindGameObjectWithTag("SHAKEScreen").GetComponent<Shake>();
        //Player????
        // Shield = GetComponent<PlayerController>();
        if (skeletonAnimation != null) skeletonAnimation.AnimationName = "Idle";
    }
    void HandleStateChanged()
    {
        // When the state changes, notify the animation handle of the new state.
        string stateName = null;
        switch (currentState)
        {
            case CharacterState.Idle:
                stateName = "Idle";
                break;
            case CharacterState.GettingHit:
                stateName = "get hitting";
                break;
            default:
                break;
        }

        if (skeletonAnimation != null) skeletonAnimation.AnimationName = stateName;

    }
    private void DetermineCharacterState()
    {
        if (isGettingHit)
        {
            currentState = CharacterState.GettingHit;
        }
        else
        {
            currentState = CharacterState.Idle;
        }

        bool stateChanged = previousState != currentState;
        previousState = currentState;

        // Animation
        // Do not modify character parameters or state in this phase. Just read them.
        // Detect changes in state, and communicate with animation handle if it changes.
        if (stateChanged)
            HandleStateChanged();

    }
    void Update()
    {
        Move_Towards();



        /* if (Vector2.Distance(transform.position, PlayerPos.position) > 0.85f)

       transform.position = Vector2.MoveTowards(transform.position, PlayerPos.position, speed * Time.deltaTime);
         */
        DetermineCharacterState();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            shake.CamShake();
            Debug.Log("billet");
/*            enemy.GetComponent<BarSystem>().Damage(10f);
*/
            Vector2 dif = transform.position - other.transform.position;
            transform.position = new Vector3(transform.position.x + dif.x, transform.position.y + dif.y, 1);
            StartCoroutine(FlashRed());
            ChangeHealth();

        }
       


    }
    public void ChangeHealth()
    {
        health--;
        SoundManger.HitEnemy();


        /* enemyBar.transform.Find("Enemy").gameObject.GetComponent<BarSystem>();
              enemyBar.GetComponent<BarSystem>().Damage(10);*/
        transform.Find("Bar").Find("BarHealth").gameObject.GetComponent<BarSystem>().Damage(1);

        if (health <= 0)
        { 
                  
            GameObject.FindObjectOfType<PlayerController>().RechargePoint(increase);
            if (GetComponent<EnemyShoots>() == null)
            {
                SoundManger.MonsterDeath();
            }
            else
            {
                SoundManger.ShootingMonsterDeath();
            }
            Ps();
            int generatedNum = Random.Range(0, 101);
            if (generatedNum <= probabilityOfPickupDrop)
            {
                int randomPickup = Random.Range(0, drops.Length);
                GameObject newPickup = Instantiate(drops[randomPickup], transform.position, Quaternion.identity);
                GameObject.FindObjectOfType<Companion>().AddNewPickup(newPickup);
            }
            Destroy(gameObject);
            

        }

    }


    public IEnumerator FlashRed()
    {
        isGettingHit = true;
        yield return new WaitForSeconds(0.2f);
        isGettingHit = false;
    }



    public void Ps()
    {
        ps.Play();
        ps = Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, 1);

    }



    void Move_Towards()
    {
        if (targetPos != null)
        {
            SoundManger.MovingEnemy();
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
            skeletonAnimation.Skeleton.ScaleX = targetPos.position.x - transform.position.x < 0 ? -1f : 1f;

        }


    }
}