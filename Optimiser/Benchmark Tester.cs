using System;
using System.Diagnostics;
using UnityEngine;

public class Benchmark_Tester : MonoBehaviour
{
    [Range(1, 1000000), SerializeField] private int Iterations;
    private Benchmark_Tester tester;
    private Vector3 Direction = Vector3.zero;
    private void Awake()
    {
        tester = GetComponent<Benchmark_Tester>();
    }
    private void Start()
    {
        //RunTest();
    }
    public void RunTest()
    {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        for(int i = 1; i <= Iterations; i++)
        {
            tester.Test();
        }
        sw.Stop();
        print($"{sw.ElapsedMilliseconds} ms");
    }
    private void Test()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Direction = transform.right * x + transform.forward * z;
        Direction = 100 * 1 * Time.deltaTime * Direction.normalized;
    }
}
