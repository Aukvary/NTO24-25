using UnityEngine;

public class TestServerHandler : MonoBehaviour
{
    private async void Awake()
    {
        await RequestSender.CreateUser(
            "{\r\n  \"name\": \"us\",\r\n  \"resources\": {\r\n     \"apples\": 1\r\n  }\r\n}");

        print(RequestSender.GetUser("", "us").Result);
    }
}