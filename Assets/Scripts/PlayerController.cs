using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public int speed;
    public Text countText;
    private int count;
    public GameObject coin;
    public AudioSource efxSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CreatCoin();
        count = 0;
        countText.text = "Scores: " + count.ToString();
    }

    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            efxSource.Play();
            other.gameObject.SetActive(false);
            count = count + 1;
            countText.text = "Scores: " + count.ToString();

            CreatCoin();
        }

    }

    private void CreatCoin()
    {
        int x = UnityEngine.Random.Range(-9, 9);
        int z = UnityEngine.Random.Range(-9, 9);
        Vector3 position = new Vector3(x, 0.8f, z);
        Quaternion rotation = coin.transform.rotation;

        Instantiate(coin, position, rotation);
    }

}
