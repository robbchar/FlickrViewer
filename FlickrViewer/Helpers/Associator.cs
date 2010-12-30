using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows;

namespace FlickrViewer
{
    public class Associator
    {
        private List<AnimationElements> UnusedAnimationElements { get; set; }

        private List<AnimationElements> AllAnimationElements { get; set; }

        static Associator instance = null;

        static readonly object padlock = new object();

        public Associator()
        {
            this.UnusedAnimationElements = new List<AnimationElements>();
            this.AllAnimationElements = new List<AnimationElements>();
        }

        public static Associator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Associator();
                    }
                    return instance;
                }
            }
        }

        public AnimationElements GetAnimationElements(string uniqueId)
        {
            if (this.UnusedAnimationElements.Count == 0)
            {
                AnimationElements animationElements = new AnimationElements();

                animationElements.Storyboard = new Storyboard();
                animationElements.Image = AnimationManager.GetImage();

                animationElements.Image.Name = uniqueId;

                DoubleAnimation doubleAnimation = AnimationManager.GetAnimation();

                animationElements.Storyboard.Duration = doubleAnimation.Duration;

                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
                Storyboard.SetTarget(doubleAnimation, animationElements.Image);
                animationElements.Storyboard.Children.Add(doubleAnimation);

                this.AllAnimationElements.Add(animationElements);

                return animationElements;
            }
            else
            {
                AnimationElements animationElements = this.UnusedAnimationElements[this.UnusedAnimationElements.Count - 1];
                this.UnusedAnimationElements.RemoveAt(this.UnusedAnimationElements.Count - 1);
                return animationElements;
            }
        }

        public void StoryboardCompleted(Storyboard storyboard)
        {
            foreach (AnimationElements animationElements in this.AllAnimationElements)
            {
                if (animationElements.Storyboard == storyboard)
                {
                    this.UnusedAnimationElements.Add(animationElements);
                }
            }
        }
    }
}