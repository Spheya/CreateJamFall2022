using System;

public interface IFlipAnimation
{
    public delegate void OnFrameUpdateCallback();

    public uint AnimatedOn { get; }
    public uint AnimationLayer { get; }

    public void ProgressFrame();
}