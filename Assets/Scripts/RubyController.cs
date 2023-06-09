using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class RubyController : MonoBehaviour
{
   
    public float speed = 3.0f;
    
    public int maxHealth = 5;

    public GameObject projectilePrefab;

     public GameObject hitParticlePrefab;

     public TextMeshProUGUI cogText;

public GameObject speedslimeParticlePrefab; 

     public GameObject loseTextObject;

     

     public int score; 

     
     public int cog; 

    



     
     
     
    public AudioClip collectSound;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip gameoverSound;
    public AudioClip cogcollectSound;
    public AudioClip npcSound;
    public AudioClip speedslimeSound;
    public AudioClip slowSound;

    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
        cog = 4;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        SetCogText ();

       


       

        loseTextObject.SetActive(false);
       

        
    }

   
     

    void SetCogText()
	{
		cogText.text = "Cog Ammo: " + cog.ToString();
	}

    
    // Update is called once per frame
    void Update()
    {

        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }


        
        if( currentHealth == 0)
        {
            loseTextObject.SetActive(true);
            speed = 0;
           
           PlaySound(gameoverSound);
           

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

    // Disable the box collider
    boxCollider2D.enabled = false;
    




            
            GameObject objectToDestroy = GameObject.FindWithTag("BackgroundMusic");
if(objectToDestroy != null) {
    Destroy(objectToDestroy);
}
        }

        
             

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {   
        
          if( cog >= 1)
          {
            Launch();
            cog = cog - 1;
            SetCogText ();

          }
    
            
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    PlaySound(npcSound);
                }
            }
        }

        

        

            if (Input.GetKey(KeyCode.R))
            {
            if (loseTextObject == true)
        {

              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
        }
    }
    }

    void OnTriggerEnter2D(Collider2D other) 
	{
	
		if (other.gameObject.CompareTag ("CogAmmo"))
		{
			other.gameObject.SetActive (false);
			
            cog = cog + 4;

			SetCogText ();

            PlaySound(cogcollectSound);
		}

        if (other.gameObject.CompareTag ("SpeedSlime"))
        {
            other.gameObject.SetActive (false);

            GameObject speedslimeParticleOject = Instantiate(speedslimeParticlePrefab, transform.position, Quaternion.identity);

            PlaySound(speedslimeSound);

            speed=speed + 2;
        }

        if (other.gameObject.CompareTag ("Slow"))
        {
        speed = speed / 2;

        PlaySound(slowSound);

        

        
        }
	}
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);


    }

    public void ChangeScore()
    {
        ScoreScript.instance.ChangeCount();
    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            PlaySound(hitSound);

            GameObject hitParticleOject = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            
        
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
       
    }


    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch"); 
        
        PlaySound(throwSound);

        
        
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        
    }
}