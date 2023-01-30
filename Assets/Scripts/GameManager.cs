using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridVisualsManager visualsManager;
    [SerializeField] Map map;
    PlayerInput player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Utils.PLAYER_TAG).GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        map.CreateMap();
    }

    private void Update()
    {
        player.Interact();
    }
}
