using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private AudioClip _moveClip, _loseClip;

    [SerializeField] private GameplayManager _gm;
    [SerializeField] private GameObject _explosionPrefab;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("Move");
            _rotateSpeed *= -1f;
        }    
    }


    private void FixedUpdate()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(_explosionPrefab, transform.GetChild(0).position, Quaternion.identity);
            AudioManager.instance.Play("Lose");
            _gm.GameEnded();
            Destroy(gameObject);
        }
    }
}
