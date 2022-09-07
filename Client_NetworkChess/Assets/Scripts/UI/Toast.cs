using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class Toast : MonoSingleton<Toast>
    {
        const float SCREEN_BACKGROUND_XSPACE = 100f;
        const float BACKGROUND_TEXT_XSPACE = 200f;
        const float BACKGROUND_TEXT_YSPACE = 30f;
        const float HIDE_ANIMATION_DURATION = 1f;

        public Text messageText;
        public RectTransform backgroundTransform;
        public Animator animator;

        float mScreenWidth;
        ToastInfo mToastInfo;
        Queue<ToastInfo> mQueue = new Queue<ToastInfo>();

        protected override void Awake()
        {
            base.Awake();
            if(transform is RectTransform)
            {
                mScreenWidth = (transform as RectTransform).rect.width;
            }
            else
            {
                mScreenWidth = 1000f;
            }
            HideToast();
        }

        public void ShowMessage(string pMessage, float pDuration = 2.5f)
        {
            if (mToastInfo != null && mToastInfo.message.Equals(pMessage))
                return;
            if (mQueue.Count > 0 && mQueue.Peek().message.Equals(pMessage))
                return;
            ToastInfo tToastInfo = new ToastInfo(pMessage, pDuration);
            mQueue.Enqueue(tToastInfo);
            ShowToast();
        }

        void ShowToast()
        {
            if (mQueue.Count == 0 || mToastInfo != null)
                return;

            StartCoroutine(ShowToastTask());
        }

        IEnumerator ShowToastTask()
        {
            mToastInfo = mQueue.Dequeue();

            backgroundTransform.gameObject.SetActive(true);
            messageText.text = mToastInfo.message;
            SetToastSize();

            animator.SetBool("show", true);
            yield return new WaitForSeconds(mToastInfo.duration);
            animator.SetBool("show", false);
            yield return new WaitForSeconds(HIDE_ANIMATION_DURATION);

            mToastInfo = null;
            HideToast();
            ShowToast();
        }
        
        void SetToastSize()
        {
            float tTextWidth = messageText.preferredWidth;
            if(tTextWidth + BACKGROUND_TEXT_XSPACE + SCREEN_BACKGROUND_XSPACE > mScreenWidth)
                tTextWidth = mScreenWidth - SCREEN_BACKGROUND_XSPACE - BACKGROUND_TEXT_XSPACE;

            // Text.preferredHeight受Text.Width影响，因此先设置正确的Text.Width再取Text.preferredHeight的值才正确
            backgroundTransform.sizeDelta = new Vector2(tTextWidth + BACKGROUND_TEXT_XSPACE, BACKGROUND_TEXT_YSPACE);
            float tTextHeight = messageText.preferredHeight;

            backgroundTransform.sizeDelta = new Vector2(tTextWidth + BACKGROUND_TEXT_XSPACE, tTextHeight + BACKGROUND_TEXT_YSPACE);
        }

        void HideToast()
        {
            messageText.text = "";
            backgroundTransform.gameObject.SetActive(false);
        }
    }

    class ToastInfo 
    {
        public string message;
        public float duration;

        public ToastInfo(string pMessage, float pDuration)
        {
            message = pMessage;
            duration = pDuration;
        }
    }
}
