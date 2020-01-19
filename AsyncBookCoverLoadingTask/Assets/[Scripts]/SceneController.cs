using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    #region Main Methods

    public void HomeScene() => this.loadScene("01_SwitchScene");

    public void SequenceLoadScene() => this.loadScene("02_SequenceLoadScene");

    public void FreeLoadScene() => this.loadScene("03_FreeLoadScene");

    public void TapLoadScene() => this.loadScene("04_TapLoadScene");

    /// <summary>
    /// Common method to load any given theme
    /// </summary>
    /// <param name="sceneName"></param>
    private void loadScene(string sceneName) =>
      SceneManager.LoadScene(sceneName);

    public void Exit() => Application.Quit();

    #endregion

}
