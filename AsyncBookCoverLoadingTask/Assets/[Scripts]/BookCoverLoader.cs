using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Loading controller for a book
/// </summary>
public class BookCoverLoader : MonoBehaviour
{

    #region Fields

    // cover file url
    public string coverURL = null;

    // A fail safe image in case of error
    // and unable to display when can't show cover
    public Texture2D loading, notFound;

    public bool isLoading = false;

    #endregion

    #region Main Methods

    /// <summary>
    /// Detect mouse clicks for Tap Load Mode
    /// </summary>
    private void Update()
    {
        if (!this.isLoading && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == this.name)
                {
                    print("Book Clicked: " + this.name);
                    StartCoroutine(DownloadCover_Cor(true));
                }
            }
        }
    }


    /// <summary>
    /// The async "Unity style" cover download and display
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownloadCover_Cor(bool freeMode = false)
    {
        // Prevent multiple callsed with manual mode
        if (isLoading)
            yield return null;
        else
        {
            isLoading = true;

            var renderer = GetComponent<MeshRenderer>();
            renderer.material.mainTexture = loading;

            Texture2D results = null;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(coverURL);
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
                results = ((DownloadHandlerTexture)www.downloadHandler).texture;

            if (results != null)
                renderer.material.mainTexture = results;
            else
                renderer.material.mainTexture = notFound;

            var manager = GameObject.FindGameObjectWithTag("ResourceManager")?.GetComponent<ResourceManager>();

            if (freeMode && manager)
                manager.SubmitProgressStep();

        }
    }

    #endregion

}