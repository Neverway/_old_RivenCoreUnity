//======== Neverway 2022 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Utility
// Purpose: Asynchronously unload the current scene, and load a targetScene.
//	Show a loading screen while the process is active.
// Applied to: The persistent system manager
//
//=============================================================================

using System;
using System.Collections;
//using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class System_SceneLoader : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float delayBeforeSceneChange = 0.25f;
    [SerializeField] private float minRequiredLoadTime = 1f;
    [SerializeField] private string loadingScreenSceneID = "Loading";
    public static event Action SceneLoaded;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string targetSceneID;
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Image loadingBar;
    //private NetworkManager networkManager;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator Load()
    {
	    yield return new WaitForSeconds(delayBeforeSceneChange);
	    SceneManager.LoadScene(loadingScreenSceneID);
	    
	    // The following should execute on the loading screen scene
	    var loadingBarObject = GameObject.FindWithTag("Sys_LoadingBar");
	    if (loadingBarObject) loadingBar = loadingBarObject.GetComponent<Image>();
	    
	    yield return new WaitForSeconds(minRequiredLoadTime);
	    StartCoroutine(LoadAsyncOperation());
    }
    
    private IEnumerator ForceLoad(float loadDelay)
    {
	    yield return new WaitForSeconds(loadDelay);
	    SceneManager.LoadScene(targetSceneID);
    }

    private IEnumerator LoadAsyncOperation()
    {
	    // Create an async operation (Will automatically switch to target scene once it's finished loading)
	    var targetLevel = SceneManager.LoadSceneAsync(targetSceneID);
	    
	    while (targetLevel.progress < 1)
	    {
		    // Set loading bar to reflect async progress
		    if (loadingBar) loadingBar.fillAmount = targetLevel.progress;
		    yield return new WaitForEndOfFrame();
	    }
	    // Scene has finished loading, trigger the SceneLoaded event
	    if (SceneLoaded != null)
	    {
		    SceneLoaded.Invoke();
	    }
    }
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
    public void LoadScene(string _targetSceneID)
    {
	    targetSceneID = _targetSceneID;
	    if (!DoesSceneExist(_targetSceneID))
	    {
		    Debug.LogWarning(_targetSceneID + " Is not a valid level! Loading fallback scene...");
		    targetSceneID = "Error";
	    }
	    StartCoroutine(Load());
    }
    
    public void ForceLoadScene(string _targetSceneID, float delay)
    {
	    targetSceneID = _targetSceneID;
	    if (!DoesSceneExist(_targetSceneID))
	    {
		    Debug.LogWarning(_targetSceneID + " Is not a valid level! Loading fallback scene...");
		    targetSceneID = "Error";
	    }
	    StartCoroutine(ForceLoad(delay));
    }
    /*
    [Tooltip("Load a scene using the network manager (Skips over the loading scene as well to avoid desync)")]
    public void NetworkLoadScene(string _targetSceneID)
    {
	    if (!networkManager) networkManager = FindObjectOfType<NetworkManager>();
	    networkManager.SceneManager.LoadScene("Tmp_Title", LoadSceneMode.Single);
    }*/
    
    // This code was expertly copied from @Yagero on github.com
    // https://gist.github.com/yagero/2cd50a12fcc928a6446539119741a343
    // (Seriously though, this function is a life saver, so thanks!)
    public static bool DoesSceneExist(string _targetSceneID)
    {
	    if (string.IsNullOrEmpty(_targetSceneID))
		    return false;

	    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
	    {
		    var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
		    var lastSlash = scenePath.LastIndexOf("/");
		    var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

		    if (string.Compare(_targetSceneID, sceneName, true) == 0)
			    return true;
	    }

	    return false;
    }
}

