using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelBTN : MonoBehaviour
{
    private Button btn;
    private GameObject imagemDeFundo;
    private GameObject button;
    private GameObject relpzinho;
    private Animator relpzinhoAnim;
    // Start is called before the first frame update
    void Start()
    {
        //imagemDeFundo = GameObject.Find("Start Screen");
        //button = GameObject.Find("Next Stage Button");
        //relpzinho = GameObject.Find("Relp");
        

        //if (relpzinho == null)
        //{
        //    Debug.LogError("Relp não encontrado!");
        //    return;
        //}

        //relpzinhoAnim = relpzinho.GetComponentInChildren<Animator>();
        //// Certifique-se de que o Animator está habilitado
        //if (!relpzinhoAnim.enabled)
        //{
        //    relpzinhoAnim.enabled = true;
        //    Debug.Log("Animator do relp ativo");
        //}

        //StartCoroutine(animationes());

        //relpzinhoAnim.Rebind();
        //relpzinhoAnim.Play("CongratulationsRelp");
        
        btn  = gameObject.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(NextLevel);
        }
    }

    IEnumerator animationes()
    {
        yield return new WaitForEndOfFrame();

        relpzinhoAnim.Rebind();
        relpzinhoAnim.Play("CongratulationsRelp");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("Level_03");

        //imagemDeFundo.SetActive(false);
        //button.SetActive(false);
        //relpzinho.SetActive(false);
    }
}
