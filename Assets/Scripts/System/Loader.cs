using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        GameScene,
        MainMenuScene,
        LoadingScene
    }
    public static Scene targetScene;
    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    public static void LoadCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}