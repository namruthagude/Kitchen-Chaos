using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene
    }
    public static Scene targetScene;

    public static void Load(Scene sceneName)
    {
        Loader.targetScene = sceneName;
        SceneManager.LoadScene(Loader.Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
