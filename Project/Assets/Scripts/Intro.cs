using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {


    private GameObject npcObject;
    private Animator anim;
    public GameObject npcPrefab;
    public GameObject playerObject;
    private float moveSpeed = 1f;
    private float letterTime = 0.03f;
    private Vector3 startPosition;
    public RectTransform bottomLine;
    public RectTransform topLine;
    public Text textObject;
    public GameObject skipText;
    public MenuController menuController;
    private float holdTime;
    private bool held;
    private bool active;

    private void Start()
    {
        held = false;
        active = false;
    }

    private void Update()
    {
        if (active)
        {
            if (held)
            {
                holdTime += Time.deltaTime;
                if (holdTime > 3f)
                {
                    holdTime = 0;
                    SkipIntro();
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    held = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                skipText.SetActive(true);
                held = true;
            }

        }
    }

    public void StartIntro()
    {
        active = true;
        menuController.DisableInput();
        npcObject = Instantiate(npcPrefab) as GameObject;
        anim = npcObject.GetComponent<Animator>();
        playerObject.GetComponent<PlayerMovement>().canMove = false;
        startPosition = npcObject.transform.position;
        StartCoroutine(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(1f);

        //появляются полосы
        while(FadeInLines())
        {
            yield return null;
        }


        //подходит нпс
        anim.SetFloat("input_x", 1);
        anim.SetFloat("input_y", 1);
        anim.SetBool("isWalking", true);
        Vector3 positionToMove = new Vector2(playerObject.transform.position.x - 0.3f, playerObject.transform.position.y - 0.3f);
        while (MoveTowardsPosition(positionToMove))
        {
            yield return null;
        }
        anim.SetBool("isWalking", false);

        //диалог
        string fullText = textObject.text;
        textObject.text = string.Empty;
        textObject.gameObject.SetActive(true);
        foreach (char letter in fullText.ToCharArray())
        {
            textObject.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterTime);
        }

        yield return new WaitForSeconds(3f);
        textObject.gameObject.SetActive(false);

        //уходит нпс
        anim.SetFloat("input_x", -1);
        anim.SetFloat("input_y", -1);
        anim.SetBool("isWalking", true);
        while (MoveTowardsPosition(startPosition))
        {
            yield return null;
        }
        anim.SetBool("isWalking", false);

        //исчезает нпс
        Destroy(npcObject);

        //полосы уходят
        while (FadeOutLines())
        {
            yield return null;
        }

        playerObject.GetComponent<PlayerMovement>().canMove = true;

        if (!GameManager.instance.questManager.quests[0].gameObject.activeSelf)
        {
            GameManager.instance.questManager.quests[0].startQuest();
        }
        active = false;
        held = false;
        menuController.EnableInput();
    }

    private bool MoveTowardsPosition(Vector3 target)
    {
        return target != (npcObject.transform.position = Vector3.MoveTowards(npcObject.transform.position, target, moveSpeed * Time.deltaTime));
    }

    private bool FadeInLines()
    {
        bottomLine.localScale = new Vector2(bottomLine.localScale.x, bottomLine.localScale.y + 1f * Time.deltaTime);
        topLine.localScale = new Vector2(topLine.localScale.x, topLine.localScale.y + 1f * Time.deltaTime);
        return bottomLine.localScale.y < 1;
    }

    private bool FadeOutLines()
    {
        bottomLine.localScale = new Vector2(bottomLine.localScale.x, bottomLine.localScale.y - 1f * Time.deltaTime);
        topLine.localScale = new Vector2(topLine.localScale.x, topLine.localScale.y - 1f * Time.deltaTime);

        return bottomLine.localScale.y > 0;
    }

    private void SkipIntro()
    {
        active = false;
        held = false;
        textObject.gameObject.SetActive(false);
        skipText.SetActive(false);
        Destroy(npcObject);
        bottomLine.localScale = new Vector2(bottomLine.localScale.x, 0);
        topLine.localScale = new Vector2(topLine.localScale.x, 0);
        playerObject.GetComponent<PlayerMovement>().canMove = true;
        if (!GameManager.instance.questManager.quests[0].gameObject.activeSelf)
        {
            GameManager.instance.questManager.quests[0].startQuest();
        }
        menuController.EnableInput();
        Destroy(gameObject);

    }
}
