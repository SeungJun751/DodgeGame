using UnityEngine;
using Cinemachine;

public class Cinemacine : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (cam.Follow == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                cam.Follow = player.transform;
                cam.LookAt = player.transform;
            }   
        }
    }
}
