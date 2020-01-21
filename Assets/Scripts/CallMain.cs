using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallMain : MonoBehaviour
{
    //以下初動で使用
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
    }
}
