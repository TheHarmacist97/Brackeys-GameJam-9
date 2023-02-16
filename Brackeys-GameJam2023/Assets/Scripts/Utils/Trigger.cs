using System;
/// <summary>
/// Class which fires a function once only until it has been reset
/// </summary>
public class Trigger
{
    public bool fire { get; private set; }

    private Action func;
    private Action resetFunc;

    /// <summary>
    /// Set the function
    /// </summary>
    /// <param name="func">List a function to trigger as an Action</param>
    public Trigger(Action func, Action resetFunc = null)
    {
        this.func = func;
        this.resetFunc = resetFunc ?? Dummy;
        fire = false;
    }

    /// <summary>
    /// Fire the Trigger
    /// </summary>
    public void Fire()
    {
        if (fire) return;
        fire = true;
        func();
    }

    /// <summary>
    /// Reset the Trigger
    /// </summary>
    public void Reset()
    {
        if (!fire) return;
        fire = false;
        resetFunc();
    }

    /// <summary>
    /// Clears all invokes
    /// </summary>
    public void Clear()
    {
        func = Dummy;
        resetFunc = Dummy;
    }

    //Simple dummy function which returns.
    private void Dummy()
    {
        return;
    }
}