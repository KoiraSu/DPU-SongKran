//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;
//using System.Collections;

//public class Door : MonoBehaviour
//{
//    public string nextSceneName;
//    public GameObject warningUI;
//    public TMP_Text warning;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        if (GameManager.instance.CanExit())
//        {
//            SceneManager.LoadScene(nextSceneName);
//        }
//        else
//        {
//            warningUI.SetActive(true);
//            StartCoroutine(TypeText("No... you still didn't finish all the job outside!"));
//        }
//    }

//    IEnumerator TypeText(string text)
//    {
//        warning.text = "";

//        foreach (char c in text)
//        {
//            warning.text += c;
//            yield return new WaitForSeconds(0.03f);
//        }

//        yield return new WaitForSeconds(1.5f);
//        HideWarning();
//    }

//    void HideWarning()
//    {
//        warningUI.SetActive(false);
//    }
//}