using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
public class UpdaterManager : MonoBehaviour
{
    public event UnityAction OnUpdateComplete;
    [SerializeField] private UnityEngine.UI.Slider progressSlider; // Slider to show progress
    [SerializeField] private UnityEngine.UI.Text progressText;     // Text to display progress percentage
    [SerializeField] private UnityEngine.UI.Text statusText;       // Text to display status (e.g., "Checking", "Downloading")
    private bool _updateFailed = false;
    [SerializeField] private FloatGameEvent OnLoadProgressEvent;


    private void Start()
    {
        // Start the catalog update process
        StartCoroutine(UpdateCatalogCoroutine());
    }

    private IEnumerator UpdateCatalogCoroutine()
    {
        //statusText.text = "Checking for updates...";
        Debug.Log("Checking for updates...");

        // Step 1: Check for catalog updates
        AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        if (checkHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to check for catalog updates.");
            //statusText.text = "Failed to check for updates.";
            yield break;
        }

        List<string> catalogsToUpdate = checkHandle.Result;

        if (catalogsToUpdate.Count == 0)
        {
            Debug.Log("No catalog updates available.");
            OnUpdateComplete.Invoke();
            //statusText.text = "Game is up-to-date.";
            //progressSlider.gameObject.SetActive(false);
            yield break;
        }

        Debug.Log("Catalog updates found. Updating...");
        //statusText.text = "Updating catalogs...";

        // Step 2: Update catalogs
        AsyncOperationHandle updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);

        // Monitor the download progress
        while (!updateHandle.IsDone)
        {
            OnLoadProgressEvent.RaiseEvent(updateHandle.PercentComplete);
            Debug.Log($"{(updateHandle.PercentComplete * 100):0}%");
            //progressSlider.value = updateHandle.PercentComplete;
            //progressText.text = $"{(updateHandle.PercentComplete * 100):0}%";
            yield return null;
        }

        // Finalize update
        if (updateHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Catalogs updated successfully.");
            //statusText.text = "Update complete.";
            _updateFailed = false;
        }
        else
        {
            Debug.LogError("Failed to update catalogs.");
            _updateFailed = true;
            //statusText.text = "Update failed.";
        }

        // Clean up
        Addressables.Release(checkHandle);
        Addressables.Release(updateHandle);

        if (!_updateFailed)
        {
            OnUpdateComplete.Invoke();
        }

    }
}
