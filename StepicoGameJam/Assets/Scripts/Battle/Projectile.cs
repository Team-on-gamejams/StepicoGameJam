using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] SpriteRenderer sr;
	[SerializeField] new Collider2D collider;
	[SerializeField] ParticleSystem hitps;

	bool isPlayerWeapon = false;
	float damage = 25;
	float flySpeed = 10;

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

	private void OnTriggerEnter2D(Collider2D collision) {
		if(
			(isPlayerWeapon && collider.gameObject.layer == UnityConstants.Layers.Player) ||
			(!isPlayerWeapon && collider.gameObject.layer == UnityConstants.Layers.Enemy)
		) {
			return;
		}

		Health health = collision.GetComponent<Health>();

		if (health) {
			health.ChangeHp(-damage);
		}

		collider.enabled = false;
		sr.enabled = false;
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;

		hitps.Play();

		Destroy(gameObject, hitps.main.startLifetime.constantMax * hitps.main.startLifetimeMultiplier + hitps.main.duration + Random.Range(1.0f, 2.0f));
	}

	public void Init(bool isPlayerWeapon, float damage, float flySpeed) {
		this.isPlayerWeapon = isPlayerWeapon;
		this.damage = damage;
		this.flySpeed = flySpeed;
	}

	public void Launch() {
		collider.enabled = true;
		rb.isKinematic = false;

		rb.velocity = transform.right.normalized * flySpeed;
	}
}
