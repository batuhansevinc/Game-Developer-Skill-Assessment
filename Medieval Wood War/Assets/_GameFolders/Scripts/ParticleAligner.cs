using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAligner : MonoBehaviour
{

    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var main = ps.main;
        main.startRotation = -transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
    }
}