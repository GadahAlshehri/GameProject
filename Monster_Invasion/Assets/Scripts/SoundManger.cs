using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    public static AudioClip ShootSound;
    public static AudioClip ShieldActive;
    public static AudioClip companionEnteringShield;
   // public static AudioClip monsterVoice;
    public static AudioClip monsterDeath;
    public static AudioClip shieldDeactive;
    public static AudioClip playerGettingHit;
    public static AudioClip playerDeath;
    public static AudioClip shootingEnemy;
    public static AudioClip enemyGettingHit;
    public static AudioClip enemyMoving;
    public static AudioClip playerLowHealthWarning;
    public static AudioClip companionLowHealthWarning;
    public static AudioClip shieldRecharge;
    public static AudioClip shieldFullyRecharged;
    public static AudioClip companionGettingHit;
    public static AudioClip companionGettingHitByPlayer;
    public static AudioClip companionDeath;
    public static AudioClip fear;
    public static AudioClip companionPickUp;
    public static AudioClip shootingMonsterDeath;
    public static AudioClip footStep;
    public static AudioClip getCoins;
    public static AudioClip heal;
    public static AudioClip buyItem;
    public static AudioClip click;
    public static AudioClip gameBegin;
    public static AudioClip gameOver;
    public static AudioClip levelUp;
    public static AudioClip quit;
    public static AudioClip nextWave;


    static AudioSource audioSourc;


    void Start()
    {
        ShootSound = Resources.Load<AudioClip>("PlayerSound/PlayerShooting");
        ShieldActive = Resources.Load<AudioClip>("ShieldSound/ShieldActive");
        companionEnteringShield = Resources.Load<AudioClip>("CompanionSound/CompanionEnterTheShield");
        playerGettingHit = Resources.Load<AudioClip>("PlayerSound/PlayerGettingHit_VoiceActor_");
        monsterDeath = Resources.Load<AudioClip>("MonsterSound/GhostMonsterDeath");
        shieldDeactive = Resources.Load<AudioClip>("ShieldSound/ShieldDeactive");
        playerDeath = Resources.Load<AudioClip>("PlayerSound/PlayerDeath");
        shootingEnemy = Resources.Load<AudioClip>("MonsterSound/MonsterBullet");
        enemyGettingHit = Resources.Load<AudioClip>("MonsterSound/MonstarGetHitVoiceActor");
        //enemyMoving = Resources.Load<AudioClip>("");
        playerLowHealthWarning = Resources.Load<AudioClip>("UISound/lower-health-alarm");
        companionLowHealthWarning = Resources.Load<AudioClip>("UISound/lower-health-alarm");
        shieldRecharge = Resources.Load<AudioClip>("UISound/ChargingBar");
        shieldFullyRecharged = Resources.Load<AudioClip>("UISound/ChargingComplete");
        companionGettingHit = Resources.Load<AudioClip>("CompanionSound/CompanionGettingHit_voiceActor_");
        companionGettingHitByPlayer = Resources.Load<AudioClip>("CompanionSound/GettingHitByPlayer-voice");
        companionDeath = Resources.Load<AudioClip>("CompanionSound/CompanionDeath");
        fear = Resources.Load<AudioClip>("CompanionSound/FearVoiceActor");
        companionPickUp= Resources.Load<AudioClip>("CompanionSound/PickUp");
        shootingMonsterDeath = Resources.Load<AudioClip>("MonsterSound/MonsterWithouthandDeath");
        footStep = Resources.Load<AudioClip>("PlayerSound/footstepGrass");
        getCoins = Resources.Load<AudioClip>("PlayerSound/GetCoins");
        heal = Resources.Load<AudioClip>("PlayerSound/Healing");
        buyItem = Resources.Load<AudioClip>("UISound/BuyIteam");
        click = Resources.Load<AudioClip>("UISound/ClickSound");
        gameBegin = Resources.Load<AudioClip>("UISound/GameBegin");
        gameOver = Resources.Load<AudioClip>("UISound/GameOverSound");
        levelUp = Resources.Load<AudioClip>("UISound/Level_up");
        quit = Resources.Load<AudioClip>("UISound/QuitSound");
        nextWave = Resources.Load<AudioClip>("UISound/WaveSound");

        audioSourc = GetComponent<AudioSource>();
    }

    public static void PlayshootingSound()
    {

        audioSourc.PlayOneShot(ShootSound);
    }


    public static void ShieldActivated()
    {
       
        audioSourc.PlayOneShot(ShieldActive);
    }
    public static void CompanionMovement()
    {

        audioSourc.PlayOneShot(companionEnteringShield);
    }

    public static void HitPlayer()
    {

        audioSourc.PlayOneShot(playerGettingHit);
    }


    public static void MonsterDeath()
    {

        audioSourc.PlayOneShot(monsterDeath);
    }


    public static void ShieldDeactive()
    {

        audioSourc.PlayOneShot(shieldDeactive);
    }
    public static void PlayerDeath()
    {

        audioSourc.PlayOneShot(playerDeath);
    }
    public static void ShootingEnemy()
    {
        audioSourc.PlayOneShot(shootingEnemy);
    }
    public static void HitEnemy()
    {
        audioSourc.PlayOneShot(enemyGettingHit);
    }
    public static void MovingEnemy()
    {
        audioSourc.PlayOneShot(enemyMoving);
    }
    public static void PlayerHealthWarning()
    {
        audioSourc.PlayOneShot(playerLowHealthWarning);
    }
    public static void CompanionHealthWarning()
    {
        audioSourc.PlayOneShot(companionLowHealthWarning);
    }
    public static void RechargeShield()
    {
        audioSourc.PlayOneShot(shieldRecharge);
    }
    public static void FullyRechargedShield()
    {
        audioSourc.PlayOneShot(shieldFullyRecharged);
    }
    public static void HitCompanion()
    {
        audioSourc.PlayOneShot(companionGettingHit);
    }
    public static void HitCompanionByPlayer()
    {
        audioSourc.PlayOneShot(companionGettingHitByPlayer);
    }
    public static void CompanionDeath()
    {
        audioSourc.PlayOneShot(companionDeath);
    }
    public static void FearVoice()
    {
        audioSourc.PlayOneShot(fear);
    }
    public static void PickUp()
    {
        audioSourc.PlayOneShot(companionPickUp);
    }
    public static void ShootingMonsterDeath()
    {
        audioSourc.PlayOneShot(shootingMonsterDeath);
    }
    public static void FootStep()
    {
        audioSourc.PlayOneShot(footStep);
    }
    public static void CollectCoins()
    {
        audioSourc.PlayOneShot(getCoins);
    }
    public static void Heal()
    {
        audioSourc.PlayOneShot(heal);
    }
    public static void BuyItem()
    {
        audioSourc.PlayOneShot(buyItem);
    }
    public static void Click()
    {
        audioSourc.PlayOneShot(click);
    }
    public static void GameBegin()
    {
        audioSourc.PlayOneShot(gameBegin);
    }
    public static void GameOver()
    {
        audioSourc.PlayOneShot(gameOver);
    }
    public static void LevelUp()
    {
        audioSourc.PlayOneShot(levelUp);
    }
    public static void Quit()
    {
        audioSourc.PlayOneShot(quit);
    }
    public static void WaveSound()
    {
        audioSourc.PlayOneShot(nextWave);
    }
}

