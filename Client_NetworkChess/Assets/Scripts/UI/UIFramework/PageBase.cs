using System;
using System.Collections;
using UnityEngine.UI;

namespace UIFramework
{
    public class PageBase : AnimationUI
    {
        public bool isFullPage { get; set; }
        public bool saveToHistory { get; set; }
        public bool alwaysInMemory { get; set; }
        public bool isOpen { get; private set; }
        public bool isOpened { get; private set; }
        public object pageParam { get; private set; }

        RawImage mPageMask;

        public IEnumerator OpenPage(object pPageParam, bool pPlayAnimation, bool pIsBack)
        {
            isOpen = true;
            pageParam = pPageParam;

            yield return null;
        }

        public void PageAwake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnBeginOpen(bool pIsBack) { }

        protected virtual void OnOpened(bool pIsBack) { }

        protected virtual void OnBeginClose() { }

        protected virtual void OnClosed() { }

        protected virtual void OnReopen() { }

        protected virtual void SetMainUIButtons(string InParams)
        {
            if(string.IsNullOrEmpty(InParams))
                return;

            string[] splitString = InParams.Split('&');
            bool[] buttonsActive = new bool[6];
            for (int i = 0; i < splitString.Length && i < buttonsActive.Length; i ++)
                buttonsActive[i] = true;
        }
    }
}