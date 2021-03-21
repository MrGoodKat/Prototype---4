using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody playerRigidbody;
    private GameObject focalPoint;
    public GameObject powerupInducator;
    private float strengthPower = 15.0f;
    public bool isPowerUp = false;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRigidbody.AddForce(focalPoint.transform.forward * verticalInput * speed);
        powerupInducator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            isPowerUp = true;
            powerupInducator.SetActive(isPowerUp);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        // yield kelimesi ile update fonksiyonunun dışında oluşturduğumuz bir thread (iş parçacığı)'tir.
        yield return new WaitForSeconds(5.0f);
        isPowerUp = false;
        powerupInducator.SetActive(isPowerUp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 enemyThrowForce = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(enemyThrowForce * strengthPower, ForceMode.Impulse);

            //Debug.Log("Collided object name is : " + collision.gameObject.name + " and player is " + isPowerUp);
        }
    }
}
