using System;

public interface ILoadable
{
    bool Loaded { get; set; }

    void Initialize();
}