using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public EnemyDetection targetEnemy;
    public Text feedbackText;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (feedbackText != null)
            feedbackText.text = "";
    }

    void Update()
    {
        if (targetEnemy == null) return;

        // update UI feedback
        if (targetEnemy.CanBeBackstabbed())
        {
            feedbackText.text = "STAB NOW!";
            feedbackText.color = Color.yellow;
        }
        else if (feedbackText.text == "STAB NOW!") // only clear if it was the prompt
        {
            feedbackText.text = "";
        }

        // stab trigger
        if (Input.GetMouseButtonDown(0))
        {
            TryBackstab();
        }
    }

    void TryBackstab()
    {
        // stab animation
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");

        if (targetEnemy.CanBeBackstabbed())
        {
            ShowFeedback("Backstab Successful!", Color.green);
            targetEnemy.Die(); // enemy disappears
        }
        else
        {
            ShowFeedback("Attack Failed!", Color.red);
        }
    }

    void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.color = color;
    }
}
