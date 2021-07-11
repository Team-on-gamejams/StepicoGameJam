using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	[Header("Audio"), Space]
	[SerializeField] AudioClip onLaunchClip;
	[SerializeField] AudioClip onHitEnemyClip;
	[SerializeField] AudioClip onHitPlayerClip;
	[SerializeField] AudioClip onHitEnvironmentClip;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] SpriteRenderer sr;
	[SerializeField] new Collider2D collider;
	[SerializeField] ParticleSystem hitps;

	bool isPlayerWeapon = false;
	float damage = 25;
	float flySpeed = 10;
	float maxFlyDist = 30.0f;
	bool isPiercing = false;
	float flyDist = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();	
		if (!sr)
			sr = GetComponent<SpriteRenderer>();
		if (!collider)
			collider = GetComponent<Collider2D>();
		if (!hitps)
			hitps = GetComponentInChildren<ParticleSystem>();
	}
#endif

	private void Start() {
		collider.enabled = false;
		rb.isKinematic = true;
	}

	private void FixedUpdate() {
		flyDist += rb.velocity.magnitude * Time.deltaTime;

		if(flyDist >= maxFlyDist) {
			OnHit();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(
			(isPlayerWeapon && collision.gameObject.layer == UnityConstants.Layers.Player) ||
			(!isPlayerWeapon && collision.gameObject.layer == UnityConstants.Layers.Enemy)
		) {
			return;
		}

		Health health = collision.GetComponent<Health>();

		if (health) {
			health.ChangeHp(-damage);
		}

		switch (collision.gameObject.layer) {
			case UnityConstants.Layers.Hitbox:
				switch (collision.transform.parent.gameObject.layer) {
					case UnityConstants.Layers.Enemy:
						AudioManager.Instance.Play(onHitEnemyClip);
						break;
					case UnityConstants.Layers.Player:
						AudioManager.Instance.Play(onHitPlayerClip);
						break;
				}
				break;

			case UnityConstants.Layers.Environment:
				AudioManager.Instance.Play(onHitEnvironmentClip);
				break;
		}

		if (collision.gameObject.layer == UnityConstants.Layers.Environment || !isPiercing) 
			OnHit();
	}

	public void Init(bool isPlayerWeapon, float damage, float flySpeed, float maxFlyDist, bool isPiercing) {
		this.isPlayerWeapon = isPlayerWeapon;
		this.damage = damage;
		this.flySpeed = flySpeed;
		this.maxFlyDist = maxFlyDist;
		this.isPiercing = isPiercing;
	}

	public void Launch() {
		collider.enabled = true;
		rb.isKinematic = false;

		rb.velocity = transform.right.normalized * flySpeed;

		AudioManager.Instance.Play(onLaunchClip);
	}

	void OnHit() {
		collider.enabled = false;
		sr.enabled = false;
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;

		hitps.Play();
		Destroy(gameObject, hitps.main.startLifetime.constantMax * hitps.main.startLifetimeMultiplier + hitps.main.duration + Random.Range(1.0f, 2.0f));
	}
}
