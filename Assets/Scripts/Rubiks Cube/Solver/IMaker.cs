using System.Collections;

public interface IMaker
{
    public IEnumerator Work();
    public bool HasFinished();
}