using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public bool Firing => _firing;

    public float speed = 5.0f;
    public float distance = 20.0f;

    public float holdDistance = 0.5f;
    public Vector3 holdCenter;

    public ParticleSystem particleSystem1;
    public ParticleSystem particleSystem2;
    public CameraShake shaker;

    public AudioClip hit1sound;
    public AudioClip hit2sound;
    public AudioClip click; 

    private AudioSource source;

    private bool _firing = false;
    private Vector3 _target;

    private Vector3 _parentOrigin;
    private Vector3 _parentTarget;
    private float _parentTargetTime;

    private bool _pull;
    private float _t; 

    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(_firing)
        {
            float prevT = _t;
            _t += Time.deltaTime * speed / Vector2.Distance(_target, transform.parent.position);
            transform.position = Vector3.Lerp(transform.parent.position + Vector3.back * 5f, _target, _t);

            if(_t > 1.0f)
            {
                if (prevT <= 1.0f && _pull)
                {
                    particleSystem1.Play();
                    shaker.Shake(0.6f);
                    if (source.isPlaying)
                        source.Stop();
                    source.clip = hit1sound;
                    source.Play();
                }

                if(_pull)
                {
                    transform.position = _target;
                    transform.parent.position = Vector3.Lerp(_parentOrigin, _parentTarget, _t - 1.0f);
                }
                else
                {
                    transform.position = Vector3.Lerp(_target, transform.parent.position + Vector3.back * 5f, _t - 1.0f);
                }

                if (_t > 2.0f)
                {
                    if (_pull)
                    {
                        particleSystem2.Play();
                        shaker.Shake(Mathf.Max(0.4f * Vector2.Distance(_parentOrigin, _parentTarget), 1.5f));
                        if (source.isPlaying)
                            source.Stop();

                        source.clip = hit2sound;
                        source.Play();
                    }

                    _firing = false;
                    transform.position = transform.parent.position + Vector3.back * 5f;

                    if(_pull)
                        transform.parent.position = _parentTarget;
                }
            }
        }
        else
        {
            var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.parent.position.z));
            var direction = (mousePos - transform.parent.position);
            direction.z = 0.0f;
            direction = direction.normalized;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * 180.0f / Mathf.PI - 90.0f);

            transform.position = transform.parent.position + new Vector3(holdCenter.x * transform.parent.localScale.x, holdCenter.y, holdCenter.z) + direction * holdDistance;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, -5.0f);
    }

    public void Fire()
    {
        if (_firing)
            return;

        if (source.isPlaying)
            source.Stop();

        source.clip = click;
        source.Play();

        _firing = true;
        _t = 0.0f;
        _parentOrigin = transform.parent.position;

        var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.parent.position.z));
        var direction = (mousePos - transform.parent.position);
        direction.z = 0.0f;
        direction = direction.normalized;
        var hit = Physics2D.Raycast(transform.parent.position, direction, distance, ~0, -1.0f, 1.0f);
        var boxHit = Physics2D.BoxCast(transform.parent.position, Vector2.one, 0.0f, direction, distance, ~0, -1.0f, 1.0f);
        _pull = hit.collider != null;

        if (_pull)
        {
            _target = hit.point;

            _parentTarget = hit.point + hit.normal * Vector2.one * 0.5f;
            particleSystem1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(hit.normal.y, hit.normal.x) * 180.0f / Mathf.PI + 90.0f);
            particleSystem1.transform.position = _target;
            particleSystem2.transform.position = _target;
            particleSystem2.transform.rotation = particleSystem1.transform.rotation;
            if (Physics2D.OverlapBox(_parentTarget, new Vector2(0.79f, 0.89f), 0.0f) != null)
            {
                if (boxHit.distance > 0.0f)
                {
                    _parentTarget = transform.parent.position + direction * 0.2f + direction * (boxHit.distance - 0.01f);
                    particleSystem2.transform.position = boxHit.point;
                    particleSystem2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(boxHit.normal.y, boxHit.normal.x) * 180.0f / Mathf.PI + 90.0f);
                }
            }
        }
        else
        {
            _target = transform.parent.position + direction * distance;
        }
        _target.z = transform.position.z;
    }
}
