using UnityEngine;

public class CloseTutorial : MonoBehaviour
{
    public GameObject txt;

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Enter");
    //     }
    // }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            txt.SetActive(false);
        }
    }
}
