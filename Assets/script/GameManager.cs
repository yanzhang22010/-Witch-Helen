using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 添加这一行
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject endingCanvas; // 游戏结束时显示的Canvas
    public Image[] endingImages; // 最后的剧情图片
    private int currentEndingImageIndex = 0; // 当前显示的剧情图片的索引

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame()
    {
        StartCoroutine(PlayEndingImages());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator PlayEndingImages()
    {
        Debug.Log("Ending Canvas: " + endingCanvas);
        // 显示游戏结束时的Canvas
        endingCanvas.SetActive(true);

        // 依次显示每张图片，等待玩家点击鼠标左键后再显示下一张
        for (currentEndingImageIndex = 0; currentEndingImageIndex < endingImages.Length; currentEndingImageIndex++)
        {
            endingImages[currentEndingImageIndex].enabled = true;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            endingImages[currentEndingImageIndex].enabled = false;
        }

        // 显示完所有的图片后，重启游戏
        RestartGame();
    }
}
