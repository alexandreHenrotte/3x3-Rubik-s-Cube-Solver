using System.Collections;

interface IMaker
{
    public IEnumerator Work();
    public bool HasFinished();
}