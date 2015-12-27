using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Media.Plugin.Abstractions;
using System.Threading.Tasks;
using Media.Plugin;

namespace BobMessenger
{
	[Activity (Label = "BobMessenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		/// <summary>
		/// Gets if a camera is available on the device
		/// </summary>
		bool IsCameraAvailable { get; }

		/// <summary>
		/// Gets if Photos are supported on the device
		/// </summary>
		bool PhotosSupported { get; }

		/// <summary>
		/// Gets if Videos are supported on the device
		/// </summary>
		bool VideosSupported { get; }

		/// <summary>
		/// Picks a photo from the default gallery
		/// </summary>
		/// <returns>Media file or null if canceled</returns>
		Task<MediaFile> PickPhotoAsync();

		/// <summary>
		/// Take a photo async with specified options
		/// </summary>
		/// <param name="options">Camera Media Options</param>
		/// <returns>Media file of photo or null if canceled</returns>
		Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options);


		/// <summary>
		/// Picks a video from the default gallery
		/// </summary>
		/// <returns>Media file of video or null if canceled</returns>
		Task<MediaFile> PickVideoAsync();

		/// <summary>
		/// Take a video with specified options
		/// </summary>
		/// <param name="options">Video Media Options</param>
		/// <returns>Media file of new video or null if canceled</returns>
		Task<MediaFile> TakeVideoAsync(StoreVideoOptions options);


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += async (sender, args) =>
			{

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.PhotosSupported)
				{
					DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
					{

						Directory = "Sample",
						Name = "test.jpg"
					});

				if (file == null)
					return;

				DisplayAlert("File Location", file.Path, "OK");

				image.Source = ImageSource.FromStream(() =>
					{
						var stream = file.GetStream();
						file.Dispose();
						return stream;
					}); 
			};


		}


	}
}


