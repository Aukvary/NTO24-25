using UnityEngine;

public class TestServerHandler : MonoBehaviour
{
    private async void Awake()
    {
        var users = await User.GetUsers();
        print(users[0].Resources["wood"]);
    }
}