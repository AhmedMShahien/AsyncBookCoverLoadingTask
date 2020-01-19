using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main bool loading controller
/// </summary>
public class ResourceManager : MonoBehaviour
{

    #region Fields

    // A list of all books in the scene
    [SerializeField] private List<BookCoverLoader> BookLoaders;

    // Set the cover loading mode for this scene
    [SerializeField] private LoadingMode loadingMode = LoadingMode.Free;

    // Ref to loading icon indicator
    [SerializeField] private Image loadingImage;

    // Ref to loading text "show progress percentage"
    [SerializeField] private Text loadingText;

    // Progress counter vars
    private float total, counter;

    #endregion

    #region Startups 

    private void Start()
    {
        total = BookLoaders.Count;
        counter = 0;

        switch (loadingMode)
        {
            case LoadingMode.Sequence:
                this.SequenceLoad();
                break;
            case LoadingMode.Free:
                this.FreeLoad();
                break;
        }
    }

    #endregion

    #region Main Methods

    /// <summary>
    /// Call sequence loading "one by one"
    /// </summary>
    public void SequenceLoad() =>
        StartCoroutine(SequenceLoad_Cor());

    /// <summary>
    /// Call Free loading mode no sequence control
    /// </summary>
    public void FreeLoad()
    {
        foreach (var bl in BookLoaders)
            StartCoroutine(bl.DownloadCover_Cor(true));
    }

    /// <summary>
    /// Call all registered books to download and show covers one by one
    /// </summary>
    /// <returns></returns>
    private IEnumerator SequenceLoad_Cor()
    {
        foreach (var bl in BookLoaders)
        {
            // Call book cover and wait till it's finished
            yield return StartCoroutine(bl.DownloadCover_Cor());

            // Show loading progress
            SubmitProgressStep();
        }
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Submit a done load to increase loading progress in Free Load 
    /// </summary>
    public void SubmitProgressStep()
    {
        // Show loading progress
        var loadingPercentage = roundValue(++counter / total);

        // Display progress
        loadingImage.fillAmount = loadingPercentage / 100;
        loadingText.text = $"{loadingPercentage}%";

        // track loading progress in editor
        print($"Loaded {counter} of {total}: {loadingPercentage}%");
    }

    /// <summary>
    /// Helper for clean progress rounding
    /// </summary>
    /// <param name="value">The value to round</param>
    /// <returns>The result rounded value in percentage</returns>
    private float roundValue(float value) =>
        Mathf.Round(value * 100);

    #endregion
}
