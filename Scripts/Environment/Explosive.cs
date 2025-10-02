using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    private float _time = 0.0f;

    private float _checkInterval = 1.0f;

    public GameObject ExplosionVfxPrefab;
    private GameObject _explosionVfxObj;

    public MeshGenerator DetectionRange;

    private bool _exploded = false;

    private void OnEnable()
    {
        DetectionRange.CreateArc(transform, 1f, 360f);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _checkInterval)
        {
            // cleanup the next time the timer hits the interval if already exploded
            if (_exploded)
            {
                Destroy(gameObject);
                Destroy(_explosionVfxObj);
                return;
            }

            _time = 0.0f;

            Collider[] hit = Physics.OverlapSphere(transform.position, 1f);

            foreach (var h in hit)
            {
                if (h.tag == "Enemy")
                {
                    Enemy e = h.gameObject.GetComponent<Enemy>();
                    e.DealDamage(100, HealthContext.ExplosiveTakedown);

                    if (_exploded)
                        break;

                    _exploded = true;
                    DetectionRange.ClearMesh();

                    // play vfx and sfx
                    PlayExplosionVfx();
                    AudioManager.Instance.PlayAudioAt(AudioEnum.Explosion, transform.position);

                    Director.Instance.ProcessMessage(Message.Explosion, transform.position);
                }
            }
        }
    }

    private void PlayExplosionVfx()
    {
        if (_explosionVfxObj != null)
            Destroy(_explosionVfxObj);

        _explosionVfxObj = Instantiate(ExplosionVfxPrefab, transform.position, transform.rotation);
    }
}
