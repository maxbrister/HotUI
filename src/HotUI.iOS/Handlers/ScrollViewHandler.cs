﻿using System;
using HotUI.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS.Handlers
{
    public class ScrollViewHandler : AbstractHandler<ScrollView, UIScrollView>
    {
        private UIView _content;

        public override bool AutoSafeArea => false;
        protected override UIScrollView CreateView()
        {
            var scrollView = new UIScrollView
            {
                ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always,
            };

            return scrollView;
        }

        public override void SetView(View view)
        {
            base.SetView(view);

            _content = VirtualView?.View?.ToView();
            if (_content != null)
            {
                if (VirtualView?.View != null)
                    VirtualView.View.NeedsLayout += HandleViewNeedsLayout;

                _content.SizeToFit();
                TypedNativeView.Add(_content);

                var measuredSize = VirtualView.View.Measure(new SizeF(float.PositiveInfinity, float.PositiveInfinity));   
                TypedNativeView.ContentSize = measuredSize.ToCGSize();
            }

            if (VirtualView.Orientation == Orientation.Horizontal)
            {
                TypedNativeView.ShowsVerticalScrollIndicator = false;
                TypedNativeView.ShowsHorizontalScrollIndicator = true;
            }
            else
            {
                TypedNativeView.ShowsVerticalScrollIndicator = true;
                TypedNativeView.ShowsHorizontalScrollIndicator = false;
            }
        }

        public override void Remove(View view)
        {
            if (VirtualView?.View != null)
                VirtualView.View.NeedsLayout -= HandleViewNeedsLayout;
            
            _content?.RemoveFromSuperview();
            _content = null;
            
            base.Remove(view);
        }

        private void HandleViewNeedsLayout(object sender, EventArgs e)
        {
            _content.SetNeedsLayout();
        }
    }
}