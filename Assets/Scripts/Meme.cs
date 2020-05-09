using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meme : MonoBehaviour{
    public string level;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(level);
        }
    }
}
