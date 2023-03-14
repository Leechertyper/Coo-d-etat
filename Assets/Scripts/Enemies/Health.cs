using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{


    public int maxHealth = 100;
    public int health;
    public Color hurtColor;
    public GameObject[] deathEffects;

    public GameObject theDeathItems;
    private void Start()
    {
        health = maxHealth;

        
    }

    /// <summary>
    /// If health avalable takes damage
    /// </summary>
    /// <param name="damage">Damage Taken</param>
    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            StartCoroutine(DamageFlash());
            if (health - damage > 0)
            {
                health -= damage;
            }
            else 
            { 
                health = 0; 
            }
        }
        if(health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    /// <summary>
    /// Set health to a desired value
    /// </summary>
    /// <param name="newHealth">New health value</param>
    public void SetHealth(int newHealth)
    {
        if(newHealth > 0)
        {
            if(newHealth < maxHealth)
            {
                health = newHealth;
            }
            else
            {
                health = maxHealth;
            }
        }  
    }
    
    /// <summary>
    /// Flashes red
    /// </summary>
    /// <returns>IEnum</returns>
    private IEnumerator DamageFlash()
    {
        float timeElapsed = 0;
        while (timeElapsed < 0.1f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, hurtColor, timeElapsed / 0.1f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = hurtColor;
        timeElapsed = 0;
        while (timeElapsed < 0.1f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(hurtColor, Color.white, timeElapsed / 0.1f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        yield break;
    }

    /// <summary>
    /// Death Animation + DESTROYS GAMEOBJECT
    /// </summary>
    /// <returns>IEnum</returns>
    private IEnumerator Death()
    {
        GetComponent<Enemy>().Die();
        float timeElapsed = 0;
        while (timeElapsed < 0.5f)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, timeElapsed / 0.1f);
            timeElapsed += Time.deltaTime;
            yield return null;
            transform.localScale += new Vector3(0.0002f, 0.0002f, 0.0002f);
        }
        GetComponent<SpriteRenderer>().color = Color.red;
        var deathExplosion = Instantiate(deathEffects[0], transform.position, transform.rotation);
        Destroy(deathExplosion,1.5f);
        Destroy(gameObject);
        theDeathItems.GetComponent<deathItems>().SpawnItem(transform.position);

    }
}
