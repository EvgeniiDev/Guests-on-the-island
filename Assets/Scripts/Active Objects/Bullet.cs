using Assets.Scripts.DamagedObject;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _gravity = 9;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _damage = 20;

    private Vector3 _lastPos;
    private Vector3 _velocity;

    private void Start()
    {
        _lastPos = transform.position;
        _velocity = transform.forward * _speed;
    }

    private void Update()
    {
        if(transform.position.y < 0)
            Destroy(gameObject);

        _velocity += _gravity * Time.deltaTime * Vector3.down;
        transform.position += _velocity * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Linecast(_lastPos, transform.position, out hit))
        {
            var dist = Vector3.Distance(_lastPos, hit.point);
            //Debug.Log(dist);
            if (hit.collider != null)
            {
                var player = hit.collider.GetComponent<DamagedObject>();
                if (player != null)
                {
                    player.ApplyDamage(_damage);
                    Destroy(gameObject);
                }
            }
        }
        _lastPos = transform.position;
    }
}