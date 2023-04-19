using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public static int level;

    public static ScoreScript instance;

    public TMP_Text text;
    public TMP_Text winText;
    
    

    public int robotsInGame ;

    public int fixedRobots = -1;
    public AudioSource musicSource;
public AudioClip musicClipOne;
    
   
    private void Awake()
    {
        instance = this;
        CountRobots();
        ChangeCount();

        winText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    

   
     
    // Update is called once per frame
    void Update()
    {
        
        
         if (Input.GetKey(KeyCode.R))
        {
         if(winText.gameObject == true)
         {  
              SceneManager.LoadScene("Main Scene");     
               
         }
        }
    }

    public void ChangeCount()
    {
        fixedRobots++;
        CountRobots();
        instance.text.text = $"Robot Count: {fixedRobots} / {robotsInGame}";

        // win
        if (SceneManager.GetActiveScene().name == "Level Two") 
        {
            if (fixedRobots == robotsInGame)
        {
           musicSource.clip = musicClipOne;
            musicSource.Play();
            winText.gameObject.SetActive(true);

            
           
            GameObject objectToDestroy = GameObject.FindWithTag("BackgroundMusic");
if(objectToDestroy != null) {
    Destroy(objectToDestroy);
}  
        }
        }
        


    }

    private void CountRobots()
    {
        robotsInGame = GameObject.FindGameObjectsWithTag("Robot").Count();
    }

}
