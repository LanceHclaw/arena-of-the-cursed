using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject Enemy;

    GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager.GetComponent<AssignPlayers>().AddPlayer(gameObject);
    }
}
