using Assets.Scripts.DamagedObject;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Types;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float _shootDelay = 1;

    [SerializeField]
    private float _attackRadius;

    [SerializeField]
    private GameObject _head;
    private Transform _shootPivot;

    [SerializeField]
    private Bullet _bulletPrefab;

    private DamagedObject _player;

    [SerializeField]
    public Team target;

    private AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _shootPivot = _head.transform;
        StartCoroutine(LoopWave());
    }

    public IEnumerator LoopWave()
    {
        while (true)
        {
            _player = Resources.AllObjects.Where(x => x != null)
                                          .Where(x => x.team == target)
                                          .OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                                          .FirstOrDefault();
            if (_player != default)
            {
                var dist = Vector3.Distance(_shootPivot.position , _player.transform.position);

                if (dist <= _attackRadius)
                {
                    var vector = _player.transform.position;
                    var distenition = new Vector3();
                    if (dist > 125)
                        distenition = new Vector3(vector.x, -5, vector.z);
                    else
                        distenition = new Vector3(vector.x, 5, vector.z);

                    _shootPivot.LookAt(distenition);
                    TryToShoot();
                }
            }
            yield return new WaitForSeconds(_shootDelay);
        }
    }

    private void TryToShoot()
    {
            Instantiate(_bulletPrefab, _shootPivot.position, _shootPivot.rotation);
            _audio.Play();
    }
}