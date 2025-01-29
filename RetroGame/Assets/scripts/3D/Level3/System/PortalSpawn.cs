using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

namespace Level3
{
    public class PortalSpawn : MonoBehaviour
    {
        public GameObject portalEffect;
        public GameObject tvBlock;
        public float moveAmount = 1f;
        public bool bossIsDead = false;
        private bool activated = false;
        private PlayerControl playerScript;
        private Boo booLife;
        public CinemachineVirtualCamera[] virtualCameras;
        public AudioSource[] portalSounds;
        private BossFightZone bossZone;
        public Volume volume;
        private Vignette vignette;
        public GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            bossZone = GameObject.FindObjectOfType<BossFightZone>();
            playerScript = GameObject.FindObjectOfType<PlayerControl>();
            booLife = GameObject.FindObjectOfType<Boo>();
            tvBlock.SetActive(false);
            portalEffect.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!booLife.bossLive && !activated)
            {
                activated = true;
                StartCoroutine(SpawnPortalEffect());
            }
        }
        IEnumerator SpawnPortalEffect()
        {
            playerScript.cameraInCutScene = true;
            virtualCameras[0].Priority = 0;
            virtualCameras[1].Priority = 1;
            yield return new WaitForSeconds(0.5f);
            tvBlock.SetActive(true);
            portalEffect.SetActive(true);
            portalSounds[0].Play();
            gameObject.GetComponent<BoxCollider>().enabled= true;
            yield return new WaitForSeconds(1f);
            portalSounds[1].Play();
            yield return new WaitForSeconds(1.5f);
            virtualCameras[0].Priority = 1;
            virtualCameras[1].Priority = 0;
            playerScript.cameraInCutScene = false;
            bossZone.gameTheme.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
                PlayerPrefs.SetInt("Score3", (int)player.playerScore); // Armazena a pontuação com a chave "Score"
                PlayerPrefs.Save(); // Salva as alterações
                //Time.timeScale = 0;
                player.gameObject.SetActive(false);
                // Get the Vignette layer
                if (volume.profile.TryGet(out vignette))
                {
                    // Start the coroutine to change the intensity
                    StartCoroutine(ChangeIntensity());
                }
                Debug.Log("TELA FINAL");
            }
        }
        IEnumerator ChangeIntensity()
        {
            float targetIntensity = 1f;
            float step = 0.1f;

            // Gradually increase the intensity
            for (float i = 0; i <= targetIntensity; i += step)
            {
                vignette.intensity.value = i;
                yield return new WaitForSeconds(0.1f);
            }
            SceneManager.LoadScene("EndUINivel3-3D");
        }

    } 
}
