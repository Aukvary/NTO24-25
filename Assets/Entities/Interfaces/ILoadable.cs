using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

#pragma warning disable CS4014
public interface ILoadable : IEntity
{
    public string Name { get; }

    public bool IsInitialized { get; set; }

    public User User { get; }

    public UnityEvent OnUserInitializeEvent { get; }

    public async void InitilizeUserInfo()
    {
        IEnumerable<string> parametors = GetStringParametors();

        await User.InitializeUser(parametors);

        await Initialize();

        IsInitialized = true;
        OnUserInitializeEvent.Invoke();
    }

    public IEnumerable<string> GetStringParametors();

    public Task Initialize();

    public void UpdateUserInfo(string parametor, int value)
        => User.UpdateUser(parametor, value);
}