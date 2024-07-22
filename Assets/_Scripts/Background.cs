using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform camera;
    public float relaiveMove = .3f;

    private void Update()
    {
        transform.position = new Vector2(camera.position.x * relaiveMove, camera.position.y * relaiveMove);
    }


}
