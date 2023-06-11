using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollPickupController : MonoBehaviour
{
    private bool playerNearby = false;
    public GameObject canvas;
    public Image[] images;
    private int currentImageIndex = 0;
    public int C = 0;
    public static int scrollsPickedUp = 0;
    public Text booksText;
   // public Image[] endingImages; // 最后的剧情图片
    private int currentEndingImageIndex = 0; // 当前显示的剧情图片的索引
   // public GameObject endingCanvas; // 游戏结束时显示的Canvas

    private void Start()
    {
        booksText.text = "0";
        scrollsPickedUp = 0;
        //开始时，隐藏Canvas
        canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>().EnablePickUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>().DisablePickUp();
        }
    }

    private void Update()
    {
        C = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C;

        if (playerNearby && (Input.GetKeyDown(KeyCode.Space) || C == 1))
        {
            //隐藏卷轴对象
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            // 增加已经拾取的卷轴数量
            scrollsPickedUp++;
            // 显示Canvas并播放第一张图片
            canvas.SetActive(true);
            ShowImage(0);
        }

        // 如果按下鼠标左键，并且Canvas是激活的
        if (Input.GetMouseButtonDown(0) && canvas.activeSelf)
        {
            currentImageIndex++;
            // 如果还有更多的图片
            if (currentImageIndex < images.Length)
            {
                // 显示下一张图片
                ShowImage(currentImageIndex);
            }
            else
            {
                // 没有更多的图片，隐藏Canvas
                canvas.SetActive(false);
                // 重置currentImageIndex，便于下次观看
                currentImageIndex = 0;
            }
        }

        // 检查是否已经拾取了所有的卷轴
        if (scrollsPickedUp >= 7)
        {
            // 如果已经拾取了所有的卷轴，结束游戏
            GameManager.instance.EndGame();
        }

        booksText.text = scrollsPickedUp.ToString();  //更新BooksText的显示
    }

    void ShowImage(int index)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = (i == index);
        }
    }
}
