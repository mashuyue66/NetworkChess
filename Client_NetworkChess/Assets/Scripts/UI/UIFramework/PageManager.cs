using UnityEngine;

public partial class PageManager : MonoSingleton<PageManager>
{
    const string DEFAULT_PAGE_NAME = "";
    static bool OpenTaskOpening = false;

    public Transform pageRoot;
    public Transform hiddenPageRoot;


}