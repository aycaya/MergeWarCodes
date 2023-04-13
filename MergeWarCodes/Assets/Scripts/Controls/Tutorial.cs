using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static bool isTutorialOn = true;
    int tutOn;
    public static int tutorialState = 0;
    [SerializeField] GameObject handCanvas;
    [SerializeField] GameObject handMerge;
    Animator animator;
    List<Button> charButtons = new List<Button>();

    private void Awake()
    {
        tutOn = PlayerPrefs.GetInt("Tutorial", 0);
        charButtons.Clear();
        Transform buttonsParent = GameObject.Find("EventSystem").transform.Find("--- UI Canvas --- (1)/Battle_Canvas/Heroes");
        charButtons.Add(buttonsParent.Find("ArcherBuy").GetComponent<Button>());
        charButtons.Add(buttonsParent.Find("HealerBuy").GetComponent<Button>());
        charButtons.Add(buttonsParent.Find("MageBuy").GetComponent<Button>());
    }
    // Start is called before the first frame update
    void Start()
    {
        if (tutOn == 0)
        {
            isTutorialOn = true;
        }
        else
        {
            isTutorialOn = false;
        }
        animator = handMerge.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameStarted || !isTutorialOn)
        {
            if (!charButtons[0].interactable)
            {
                foreach (Button element in charButtons)
                {
                    element.interactable = true;
                }
            }
            handCanvas.SetActive(false);
            handMerge.SetActive(false);
            return;
        }
        if (isTutorialOn)
        {
            if (charButtons[0].interactable)
            {
                foreach (Button element in charButtons)
                {
                    element.interactable = false;
                }
            }
        }
        if (tutorialState == 0)
        {
            handCanvas.SetActive(true);
        }
        else if (tutorialState == 4)
        {
            handCanvas.SetActive(false);
            handMerge.SetActive(true);

        }
        else if (tutorialState == 5)
        {
            animator.SetTrigger("Merge2");
        }
        else if (tutorialState == 6)
        {
            animator.SetTrigger("Merge3");
        }
        else if (tutorialState == 7)
        {
            animator.SetTrigger("Attack");
        }
    }

}
