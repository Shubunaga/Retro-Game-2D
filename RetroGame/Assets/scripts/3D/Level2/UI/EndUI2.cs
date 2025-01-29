using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level2
{
    public class EndUI2 : MonoBehaviour
    {
        public GameObject[] gameObjectsOnScreen;
        public TMP_Text KillsText;
        public AudioSource gunSfx;
        public AudioSource music;

        public float delayTime = 0.7f;
        // Start is called before the first frame update
        void Start()
        {
            music.Play();
            //PlayerData.points = 5;
            int points = PlayerData.points;
            KillsText.text = points.ToString();
            StartCoroutine(StartScreen());
        }

        IEnumerator StartScreen()
        {
            yield return new WaitForSeconds(0.2f);
            for(int i = 0;i < gameObjectsOnScreen.Length;i++)
            {
                gunSfx.Play();
                gameObjectsOnScreen[i].SetActive(true);
                yield return new WaitForSeconds(delayTime);
            }
            
            yield return null;
        }
        

        public void ContinueToLevel3()
        {
            SceneManager.LoadScene("Nivel 3 - 3D");
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene("Nivel 2 - 3D");
        }
    }

}