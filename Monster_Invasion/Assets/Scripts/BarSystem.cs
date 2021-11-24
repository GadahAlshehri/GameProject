
using UnityEngine;
using UnityEngine.UI;
public class BarSystem : MonoBehaviour
{




    public Image healthBar;
    public Image[] healthPoints;

    public float health, maxHealth ;
    float lerpSpeed;
    GameObject target;
    private void Start()
    {
        health = 0;
        if (gameObject.tag.Equals("PlayerHP"))
            health = GameObject.FindObjectOfType<PlayerController>().health;
        if (gameObject.tag.Equals("Enemy"))
            health = transform.parent.parent.GetComponent<Enemy>().health;
        if (gameObject.tag.Equals("CompBar"))
        health = GameObject.FindObjectOfType<Companion>().health;
        if (!gameObject.tag.Equals("RechargeBar"))
        {
            maxHealth = health;
        }
    }

    private void Update()
    {

      


        if (health > maxHealth) health = maxHealth;
        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);
        

        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(health, i);
        }
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
   
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    public void Damage(float damagePoints)
    {
        //Debug.Log("change recharge");
        if (health > 0)
            health -= damagePoints;
    }
    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }
    public void fullHeal()
    {
        health = maxHealth;

    }
}
