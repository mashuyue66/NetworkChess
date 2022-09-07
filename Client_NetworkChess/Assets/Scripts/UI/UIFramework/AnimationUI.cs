using System.Collections;
using UnityEngine;
using ShineEngine;

namespace UIFramework
{
    public class AnimationUI : MonoBehaviour
    {
        Animator mAnimator;
        float mEnterTime;
        float mExitTime;
        bool mShow;

        public IEnumerator Show(bool pPlayAnimation)
        {
            if (mShow)
                pPlayAnimation = false;

            mShow = true;
            gameObject.SetActive(true);

            if (pPlayAnimation)
                yield return PlayEnterAnimation();
            else if (HaveAnimationClipper())
            {
                mAnimator.SetBool("On", true);
                mAnimator.Play("On", -1, 1);
            }

            OnShowed();
        }

        public IEnumerator Hide(bool pPlayAnimation)
        {
            if (!mShow)
                pPlayAnimation = false;

            mShow=false;

            OnBeginHide(pPlayAnimation);
            if (pPlayAnimation)
                yield return PlayExitAnimation();
            OnHided();
        }

        protected virtual void OnBeginShow(bool pPlayAnimation) { }

        protected virtual void OnShowed() { }

        protected virtual void OnBeginHide(bool pPlayAnimation) { }

        protected virtual void OnHided() { }

        protected virtual IEnumerator PlayEnterAnimation()
        {
            if(!HaveAnimationClipper())
                yield break;

            mAnimator.SetBool("On", true);
            yield return new WaitForSeconds(mEnterTime);
        }

        protected virtual IEnumerator PlayExitAnimation()
        {
            if (!HaveAnimationClipper())
                yield break;

            mAnimator.SetBool("On", false);
            yield return new WaitForSeconds(mExitTime);
        }

        private bool HaveAnimationClipper()
        {
            if(mAnimator == null)
            {
                Transform tPanel = transform.Find("Panel");
                if(tPanel == null)
                    return false;

                mAnimator = tPanel.GetComponent<Animator>();
                if(mAnimator == null)
                    return false;
            }

            AnimationClip[] tAnimationClips = AnimTool.GetAnimationClip(mAnimator);
            if(tAnimationClips == null || tAnimationClips.Length == 0)
            {
                Log.errorLog(name + " animator controller doesn't contain animation clips.");
                return false;
            }

            for(int i = 0; i < tAnimationClips.Length; i++)
            {
                if (tAnimationClips[i].name.Equals("On"))
                    mEnterTime = tAnimationClips[i].length;
                else if (tAnimationClips[i].name.Equals("Off"))
                    mExitTime = tAnimationClips[i].length;
            }

            return mAnimator != null;
        }

        public float GetAnimationTime(bool pEnter)
        {
            if (!HaveAnimationClipper())
            {
                return 0f;
            }

            return pEnter ? mEnterTime : mExitTime;
        }
    }

    public static class AnimTool
    {
        public static AnimationClip[] GetAnimationClip(Animator InAnim)
        {
            if (null == InAnim) return null;

            RuntimeAnimatorController runtimeAnimController = InAnim.runtimeAnimatorController;
            if (null == runtimeAnimController) return null;
            AnimationClip[] clips = runtimeAnimController.animationClips;
            return clips;
        }
    }
}
