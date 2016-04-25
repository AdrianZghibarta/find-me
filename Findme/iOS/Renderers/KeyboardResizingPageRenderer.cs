using System;
using Findme;
using Findme.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Foundation;

[assembly: ExportRenderer(typeof(KeyboardResizingPage), typeof(KeyboardResizingPageRenderer))]

namespace Findme.iOS
{
	public class KeyboardResizingPageRenderer: PageRenderer
	{
		public KeyboardResizingPageRenderer ()
		{
		}

		NSObject observerHideKeyboard;
		NSObject observerShowKeyboard;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification(notification);
			var bounds = Element.Bounds;
			var newBounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - frameBegin.Top + frameEnd.Top);
			Element.Layout(newBounds);
		}
	}
}

