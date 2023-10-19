﻿using UnityEngine.Networking;
using UnityEngine.Video;

namespace AndroidGoodiesExamples
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Linq;
	using DeadMosquito.AndroidGoodies;
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;

	public class OtherGoodiesTest : MonoBehaviour
	{
		public Texture2D wallpaperTexture;
		public Image image;
		public InputField input;
		public VideoPlayer videoPlayer;

		Texture2D _lastTakenScreenshot;

		string _imageFilePath;

		AndroidGoodiesDemoController _demoController;
		AudioSource _audioSource;
		bool togglePauseText = false;

		void Start()
		{
			input.text = "com.twitter.android";
			_audioSource = GetComponent<AudioSource>();
			_demoController = GetComponent<AndroidGoodiesDemoController>();
		}

		#region toast

		[UsedImplicitly]
		public void OnShortToastClick()
		{
			AGUIMisc.ShowToast("hello short!");
		}

		[UsedImplicitly]
		public void OnLongToastClick()
		{
			AGUIMisc.ShowToast("hello long!", AGUIMisc.ToastLength.Long);
		}

		#endregion

		#region maps

		[UsedImplicitly]
		public void OnOpenMapClick()
		{
			AGMaps.OpenMapLocation(47.6f, -122.3f, 9);
		}

		[UsedImplicitly]
		public void OnOpenMapLabelClick()
		{
			AGMaps.OpenMapLocationWithLabel(47.6f, -122.3f, "My Label");
		}

		[UsedImplicitly]
		public void OnOpenMapAddress()
		{
			AGMaps.OpenMapLocation("1st & Pike, Seattle");
		}

		#endregion

		[UsedImplicitly]
		public void OnEnableImmersiveMode()
		{
			AGUIMisc.EnableImmersiveMode();
		}

		[UsedImplicitly]
		public void OnShowStatusBar()
		{
			Screen.fullScreen = false;

			AGUIMisc.ShowStatusBar(Color.red);
		}

		[UsedImplicitly]
		public void OnHideStatusBar()
		{
			Screen.fullScreen = true;

			AGUIMisc.HideStatusBar();
		}

		[UsedImplicitly]
		public void OnVibrate()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			AGVibrator.Cancel();
			AGVibrator.Vibrate(500);
		}

		[UsedImplicitly]
		public void OnVibratePattern()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			// Start without a delay
			// Each element then alternates between vibrate, sleep, vibrate, sleep...
			long[] pattern = { 0, 100, 1000, 300, 200, 100, 500, 200, 100 };

			AGVibrator.Cancel();
			AGVibrator.VibratePattern(pattern);
		}

		[UsedImplicitly]
		public void OnVibrateOneSHot()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			if (!AGVibrator.AreVibrationEffectsSupported)
			{
				Debug.LogWarning("This device does not support vibration effects API!");
				return;
			}

			AGVibrator.Cancel();
			//Create a one shot vibration for 1000 ms at default amplitude
			AGVibrator.Vibrate(VibrationEffect.CreateOneShot(1000, VibrationEffect.DEFAULT_AMPLITUDE));
		}

		[UsedImplicitly]
		public void OnVibrateWaveForm1()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			if (!AGVibrator.AreVibrationEffectsSupported)
			{
				Debug.LogWarning("This device does not support vibration effects API!");
				return;
			}

			// Start without a delay
			// Each element then alternates between vibrate, sleep, vibrate, sleep...
			long[] mVibratePattern = { 0, 400, 1000, 600, 1000, 800, 1000, 1000 };

			AGVibrator.Cancel();
			//Create a waveform vibration.
			AGVibrator.Vibrate(VibrationEffect.CreateWaveForm(mVibratePattern, -1));
		}

		[UsedImplicitly]
		public void OnVibrateWaveForm2()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			if (!AGVibrator.AreVibrationEffectsSupported)
			{
				Debug.LogWarning("This device does not support vibration effects API!");
				return;
			}

			long[] mVibratePattern = { 0, 400, 1000, 600, 1000, 800, 1000, 1000 };
			int[] mAmplitudes = { 0, 255, 0, 255, 0, 255, 0, 255 };
			//Create a waveform vibration with different vibration amplitudes
			AGVibrator.Cancel();
			AGVibrator.Vibrate(VibrationEffect.CreateWaveForm(mVibratePattern, mAmplitudes, 0));
		}

		[UsedImplicitly]
		public void OnVibrateCancel()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			//Cancels the current vibration pattern
			AGVibrator.Cancel();
		}

		[UsedImplicitly]
		public void OnVibrateWithAudioAttributes()
		{
			if (!AGVibrator.HasVibrator())
			{
				Debug.LogWarning("This device does not have vibrator");
			}

			if (!AGVibrator.AreVibrationEffectsSupported)
			{
				Debug.LogWarning("This device does not support vibration effects API!");
				return;
			}

			AGVibrator.Cancel();
			//Creating new audio attributes with custom parameters
			var audioAttributes = new AudioAttributes.Builder()
				.SetContentType(AudioAttributes.ContentType.Music)
				.SetFlags(AudioAttributes.Flags.FlagAll)
				.SetUsage(AudioAttributes.Usage.Alarm)
				.Build();
			//Create a one shot vibration for 1000 ms at default amplitude with audio attributes
			AGVibrator.Vibrate(VibrationEffect.CreateOneShot(800, VibrationEffect.DEFAULT_AMPLITUDE), audioAttributes);
		}

		#region screenshot

		[UsedImplicitly]
		public void OnSaveScreenshotToGallery()
		{
			StartCoroutine(TakeScreenshot(Screen.width, Screen.height));
		}

		IEnumerator TakeScreenshot(int width, int height)
		{
			yield return new WaitForEndOfFrame();

			var texture = new Texture2D(width, height, TextureFormat.RGB24, true);
			texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			texture.Apply();
			_lastTakenScreenshot = texture;
			var imageTitle = "Screenshot-" + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss");
			const string description = "Goodies";
			AGFileUtils.InsertImageToGallery(_lastTakenScreenshot, imageTitle, description);
			AGUIMisc.ShowToast(imageTitle + " saved to gallery");
		}

		#endregion

		[UsedImplicitly]
		public void OnFlashlightOn()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.CAMERA, AGFlashLight.Enable,
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.CAMERA));
		}

		[UsedImplicitly]
		public void OnFlashlightOff()
		{
			AGFlashLight.Disable();
		}

		#region push_notifications

		const int NotificationId = 42;

		[UsedImplicitly]
		public void SendLocalPushNotification()
		{
			var when = DateTime.Now.AddSeconds(3);
			var title = "Title";
			var message = "Awesome message";
			var sound = true;
			var vibrate = true;
			var lights = true;
			var bigIcon = "app_icon";
#pragma warning disable 0618
			AGLocalNotifications.ShowNotification(NotificationId, when, title, message, Color.red,
				sound, vibrate, lights, bigIcon);
#pragma warning restore 0618
		}

		[UsedImplicitly]
		public void SendLocalPushNotificationRepeating()
		{
			var when = DateTime.Now.AddSeconds(3); // when to show the first one
			var intervalMillis = 10 * 1000L; // interval between subsequent
			var title = "Title";
			var message = "Awesome message";
			var sound = true;
			var vibrate = true;
			var lights = true;
			var bigIcon = "app_icon";
#pragma warning disable 0618
			AGLocalNotifications.ShowNotificationRepeating(NotificationId, when, intervalMillis, title, message,
				Color.red,
				sound, vibrate, lights, bigIcon);
#pragma warning restore 0618
		}

		[UsedImplicitly]
		public void CancelNotification()
		{
#pragma warning disable 0618
			AGLocalNotifications.CancelNotification(NotificationId);
#pragma warning restore 0618
		}

		#endregion

		[UsedImplicitly]
		public void WatchYoutubeVideo()
		{
			const string videoId = "mZqjmyyJkQc";
			AGApps.WatchYoutubeVideo(videoId);
		}

		[UsedImplicitly]
		public void OpenInstagramProfile()
		{
			const string profileId = "tarasleskivlviv";
			AGApps.OpenInstagramProfile(profileId);
		}

		[UsedImplicitly]
		public void OpenTwitterProfile()
		{
			const string profileId = "Taras_Leskiv";
			AGApps.OpenTwitterProfile(profileId);
		}

		[UsedImplicitly]
		public void OpenFacebookProfile()
		{
			const string profileId = "4"; // Mark Zuckerberg
			AGApps.OpenFacebookProfile(profileId);
		}

		[UsedImplicitly]
		public void OpenOtherApp()
		{
			var package = input.text;
			AGApps.OpenOtherAppOnDevice(package, () => AGUIMisc.ShowToast("Could not launch " + package));
		}

		[UsedImplicitly]
		public void UninstallOtherApp()
		{
			var package = input.text;
			AGApps.UninstallApp(package);
		}

		[UsedImplicitly]
		public void InstallApkFromSdCard()
		{
			// NOTE: In order to test this method apk file must exist on file system.
			// To test this method you put 'test.apk' file into your downloads folder
			const string apkFileName = "/test.apk";
			var filePath = Path.Combine(AGEnvironment.ExternalStorageDirectoryPath, AGEnvironment.DirectoryDownloads) +
			               apkFileName;
			Debug.Log("Installing APK: " + filePath + ", file exists?: " + File.Exists(filePath));

			AGApps.InstallApkFileFromSDCard(filePath);
		}

		[UsedImplicitly]
		public void HideApplicationIcon()
		{
			AGUIMisc.ChangeApplicationIconState(false);
		}

		[UsedImplicitly]
		public void OpenPdfFromFile()
		{
			var filePath = Path.Combine(Application.persistentDataPath, "esimple-flyer-eng.pdf");

			AGApps.OpenPdf(filePath);
		}

		[UsedImplicitly]
		public void OnPickContactFromAddressBook()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.READ_CONTACTS, PickContact,
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.READ_CONTACTS));
		}

		void PickContact()
		{
			AGContacts.PickContact(
				pickedContact =>
				{
					var msg =
						$"Picked contact: {pickedContact.DisplayName}, photo URI: {pickedContact.PhotoUri}, emails: {string.Join(",", pickedContact.Emails.ToArray())}, phones: {string.Join(",", pickedContact.Phones.ToArray())}";

					Debug.Log(msg);
					AGUIMisc.ShowToast(msg);

					if (!string.IsNullOrEmpty(pickedContact.PhotoUri)) // Not all contacts have image
					{
						var contactPicture = AGFileUtils.ImageUriToTexture2D(pickedContact.PhotoUri);
						image.sprite = SpriteFromTex2D(contactPicture);
					}
				},
				failureReason => { AGUIMisc.ShowToast("Picking contact failed: " + failureReason); });
		}

		[UsedImplicitly]
		public void OnPickGalleryMultipleImages()
		{
			// Whether to generate thumbnails
			var shouldGenerateThumbnails = true;

			// if image is larger it will be downscaled to the max size proportionally
			var imageResultSize = ImageResultSize.Max512;
			AGGallery.PickMultipleImagesFromGallery(
				selectedImages =>
				{
					selectedImages.ForEach(img => Debug.Log("Picked: " + img.OriginalPath));
					ProcessImage(selectedImages.First());
				},
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage),
				imageResultSize, shouldGenerateThumbnails);
		}

		[UsedImplicitly]
		public void OnPickGalleryImage()
		{
			// Whether to generate thumbnails
			var shouldGenerateThumbnails = true;

			// if image is larger it will be downscaled to the max size proportionally
			var imageResultSize = ImageResultSize.Max512;
			AGGallery.PickImageFromGallery(ProcessImage,
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage),
				imageResultSize, shouldGenerateThumbnails);
		}

		void ProcessImage(ImagePickResult imagePickResult)
		{
			var imageTexture2D = imagePickResult.LoadTexture2D();

			var msg = $"{imagePickResult.OriginalPath} was loaded from gallery with size {imageTexture2D.width}x{imageTexture2D.height}";
			AGUIMisc.ShowToast(msg);
			Debug.Log(msg);
			image.sprite = SpriteFromTex2D(imageTexture2D);

			// Clean up
			Resources.UnloadUnusedAssets();
		}

		[UsedImplicitly]
		public void OnTakePhoto()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.CAMERA, TakePhoto,
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.CAMERA));
		}

		void TakePhoto()
		{
			// Whether to generate thumbnails
			const bool shouldGenerateThumbnails = false;

			// if image is larger it will be downscaled to the max size proportionally
			const ImageResultSize imageResultSize = ImageResultSize.Max1024;

			AGCamera.TakePhoto(
				selectedImage =>
				{
					// Load received image into Texture2D
					var imageTexture2D = selectedImage.LoadTexture2D();
					var msg = $"{selectedImage.DisplayName} was taken from camera with size {imageTexture2D.width}x{imageTexture2D.height}";
					AGUIMisc.ShowToast(msg);
					Debug.Log(msg);
					image.sprite = SpriteFromTex2D(imageTexture2D);

					// Clean up
					File.Delete(selectedImage.OriginalPath);
					Resources.UnloadUnusedAssets();
				},
				error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), imageResultSize,
				shouldGenerateThumbnails);
		}

		[UsedImplicitly]
		public void OnPickAudioFile()
		{
			AGFilePicker.PickAudio(audioFile =>
				{
					StartCoroutine(LoadAudioFile(audioFile));
					_demoController.audioPanel.SetActive(true);
					var msg = "Audio file was picked: " + audioFile;
					Debug.Log(msg);
					AGUIMisc.ShowToast(msg);
				},
				error => AGUIMisc.ShowToast("Cancelled picking audio file"));
		}

		IEnumerator LoadAudioFile(AudioPickResult audioFile)
		{
			var musicFolder = new DirectoryInfo(audioFile.OriginalPath);
			using (var audioRequest = UnityWebRequestMultimedia.GetAudioClip("file:///" + musicFolder, AudioType.MPEG))
			{
				yield return audioRequest.SendWebRequest();
				if (audioRequest.isHttpError || audioRequest.isNetworkError)
				{
					Debug.Log(audioRequest.error);
				}
				else
				{
					_audioSource.clip = DownloadHandlerAudioClip.GetContent(audioRequest);
				}
			}
		}

		[UsedImplicitly]
		public void OnPlayAudio()
		{
			_audioSource.Play();
			togglePauseText = true;
			PauseBtnTextCheck();
		}

		[UsedImplicitly]
		public void OnPauseAudio()
		{
			if (togglePauseText)
			{
				_audioSource.Pause();
			}
			else
			{
				_audioSource.UnPause();
			}

			togglePauseText = !togglePauseText;
			PauseBtnTextCheck();
		}

		void PauseBtnTextCheck()
		{
			_demoController.pauseButtonText.text = togglePauseText ? "Pause" : "Unpause";
		}

		public void OnStopAudio()
		{
			_audioSource.Stop();
		}

		[UsedImplicitly]
		public void OnPickVideoFileDevice()
		{
			bool generatePreviewImages;
			generatePreviewImages = true;
			AGFilePicker.PickVideo(videoFile =>
				{
					var msg = "Video file was picked: " + videoFile;
					Debug.Log(msg);
					AGUIMisc.ShowToast(msg);

					if (videoFile.PreviewImagePath != null)
					{
						image.sprite = SpriteFromTex2D(videoFile.LoadPreviewImage());
						PlayVideo(videoFile);
					}
				},
				error => AGUIMisc.ShowToast("Cancelled picking video file: " + error), generatePreviewImages);
		}

		void PlayVideo(VideoPickResult videoFile)
		{
			videoPlayer.url = videoFile.OriginalPath;
			videoPlayer.isLooping = true;
			videoPlayer.Play();
		}

		[UsedImplicitly]
		public void OnPickVideoFileCamera()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.CAMERA, RecordVideo,
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.CAMERA));
		}

		void RecordVideo()
		{
			AGCamera.RecordVideo(videoFile =>
				{
					var msg = "Video file was recorded: " + videoFile;
					Debug.Log(msg);
					AGUIMisc.ShowToast(msg);
					image.sprite = SpriteFromTex2D(videoFile.LoadPreviewImage());
					PlayVideo(videoFile);
				},
				error => AGUIMisc.ShowToast("Cancelled recording video file: " + error), true);
		}

		[UsedImplicitly]
		public void OnPickPdfFile()
		{
			const string mimeType = "application/pdf"; // pick only pdfs
			AGFilePicker.PickFile(file =>
			{
				var msg = "Picked file: " + file;
				Debug.Log(msg);
				AGUIMisc.ShowToast(msg);
				AGApps.OpenPdf(file.OriginalPath);
			}, error => AGUIMisc.ShowToast("Picking file: " + error), mimeType);
		}

		/// <summary>
		/// Example how to request for runtime permissions
		/// </summary>
		[UsedImplicitly]
		public void OnRequestPermissions()
		{
			// Don't forget to also add the permissions you need to manifest!
			var permissions = new[]
			{
				AGPermissions.WRITE_CONTACTS,
				AGPermissions.CALL_PHONE,
				AGPermissions.ACCESS_FINE_LOCATION,
				AGPermissions.READ_CALENDAR
			};

			// Filter permissions so we don't request already granted permissions,
			// otherwise if the user denies already granted permission the app will be killed
			var nonGrantedPermissions = permissions.ToList()
				.Where(x => !AGPermissions.IsPermissionGranted(x))
				.ToArray();

			if (nonGrantedPermissions.Length == 0)
			{
				var message = "User already granted all these permissions: " + string.Join(",", permissions);
				Debug.Log(message);
				AGUIMisc.ShowToast(message);
				return;
			}

			// Finally request permissions user has not granted yet and log the results
			AGPermissions.RequestPermissions(permissions, results =>
			{
				// Process results of requested permissions
				foreach (var result in results)
				{
					var msg = $"Permission [{result.Permission}] is [{result.Status}], should show explanation?: {result.ShouldShowRequestPermissionRationale}";
					Debug.Log(msg);
					AGUIMisc.ShowToast(msg);
					if (result.Status == AGPermissions.PermissionStatus.Denied)
					{
						// User denied permission, now we need to find out if he clicked "Do not show again" checkbox
						if (result.ShouldShowRequestPermissionRationale)
						{
							// User just denied permission, we can show explanation here and request permissions again
							// or send user to settings to do so
						}
						else
						{
							// User checked "Do not show again" checkbox or permission can't be granted.
							// We should continue with this permission denied
						}
					}
				}
			});
		}

		#region wallpapers

		[UsedImplicitly]
		public void OnSetWallpaperFromTexture()
		{
			AGWallpaperManager.SetWallpaper(wallpaperTexture, null, true, AGWallpaperManager.WallpaperType.System);
		}

		[UsedImplicitly]
		public void OnSetLockScreenWallpaperFromTexture()
		{
			var rect = new AndroidRect(0, 0, 400, 400);
			AGWallpaperManager.SetWallpaper(wallpaperTexture, rect, true, AGWallpaperManager.WallpaperType.Lock);
		}

		[UsedImplicitly]
		public void OnSetWallpaperFromFilePath()
		{
			AGGallery.PickImageFromGallery(
				selectedImage =>
				{
					var imageTexture2D = selectedImage.LoadTexture2D();

					var msg = $"{selectedImage.OriginalPath} was loaded from gallery with size {imageTexture2D.width}x{imageTexture2D.height}";
					AGUIMisc.ShowToast(msg);
					Debug.Log(msg);

					AGWallpaperManager.SetWallpaper(selectedImage.OriginalPath, null, true,
						AGWallpaperManager.WallpaperType.System);

					// Clean up
					Resources.UnloadUnusedAssets();
				},
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage));
		}

		[UsedImplicitly]
		public void OnSetLockScreenWallpaperFromFilePath()
		{
			AGGallery.PickImageFromGallery(
				selectedImage =>
				{
					var imageTexture2D = selectedImage.LoadTexture2D();

					var msg = $"{selectedImage.OriginalPath} was loaded from gallery with size {imageTexture2D.width}x{imageTexture2D.height}";
					AGUIMisc.ShowToast(msg);
					Debug.Log(msg);

					var rect = new AndroidRect(0, 0, imageTexture2D.width, imageTexture2D.height);
					AGWallpaperManager.SetWallpaper(selectedImage.OriginalPath, rect, true,
						AGWallpaperManager.WallpaperType.Lock);

					// Clean up
					Resources.UnloadUnusedAssets();
				},
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage));
		}

		[UsedImplicitly]
		public void OnShowLiveWallpaperChooser()
		{
			AGWallpaperManager.ShowLiveWallpaperChooser();
		}

		[UsedImplicitly]
		public void OnResetWallpaperToDefault()
		{
			AGWallpaperManager.Clear();
			AGUIMisc.ShowToast("Wallpaper was reset to default");
		}

		[UsedImplicitly]
		public void OnIsWallpaperSupportedAndAllowed()
		{
			var isSupported = AGWallpaperManager.IsWallpaperSupported();
			var isAllowed = AGWallpaperManager.IsSetWallpaperAllowed();
			AGUIMisc.ShowToast($"Wallpaper supported? - {isSupported}, set allowed? - {isAllowed}");
		}

		[UsedImplicitly]
		public void OnCropAndSetWallpaperFromTexture()
		{
			var wallpaperType = AGWallpaperManager.WallpaperType.Lock | AGWallpaperManager.WallpaperType.System;
			AGWallpaperManager.ShowCropAndSetWallpaperChooser(wallpaperTexture, which: wallpaperType);
		}

		[UsedImplicitly]
		public void OnCropAndSetWallpaperFromFile()
		{
			AGGallery.PickImageFromGallery(
				selectedImage =>
				{
					var imageTexture2D = selectedImage.LoadTexture2D();

					var msg = $"{selectedImage.OriginalPath} was loaded from gallery with size {imageTexture2D.width}x{imageTexture2D.height}";
					AGWallpaperManager.ShowCropAndSetWallpaperChooser(selectedImage.OriginalPath, null, true,
						AGWallpaperManager.WallpaperType.Lock);

					// Clean up
					Resources.UnloadUnusedAssets();

					AGUIMisc.ShowToast(msg);
					Debug.Log(msg);
				},
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage));
		}

		#endregion

		#region audioRecorder

		bool _isRecording;
		string _lastRecordingPath;

		[UsedImplicitly]
		public void OnStartRecording()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.RECORD_AUDIO, StartRecording,
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.RECORD_AUDIO));
		}

		void StartRecording()
		{
			if (_isRecording)
			{
				AGUIMisc.ShowToast("Already recording, please stop previous first");
			}

			_isRecording = true;
			_lastRecordingPath = CreatePathForRecording();
			AGMediaRecorder.StartRecording(_lastRecordingPath);
			AGUIMisc.ShowToast("Start recording to file: " + _lastRecordingPath);
		}

		static string CreatePathForRecording()
		{
			var downloadsDir =
				Path.Combine(AGEnvironment.ExternalStorageDirectoryPath, AGEnvironment.DirectoryDownloads);
			var fullFilePath = Path.Combine(downloadsDir, "my_voice_recording_" + Guid.NewGuid() + ".3gp");
			return fullFilePath;
		}

		[UsedImplicitly]
		public void OnStopRecording()
		{
			if (_isRecording)
			{
				var recordingWasStopped = AGMediaRecorder.StopRecording();
				AGUIMisc.ShowToast(recordingWasStopped ? "Stopped recording" : "Failed to stop recording");
			}
			else
			{
				AGUIMisc.ShowToast("Can't stop recording, recording was not started");
			}
		}

		#endregion

		[UsedImplicitly]
		public void OnSetClipboardText()
		{
			AGClipboard.SetClipBoardText("label", "This text is now saved in clipboard");
		}

		[UsedImplicitly]
		public void OnGetImageExifTags()
		{
			if (_imageFilePath == null)
			{
				AGGallery.PickImageFromGallery(
					selectedImage =>
					{
						GetImageTags(selectedImage.OriginalPath);
						Resources.UnloadUnusedAssets();
					},
					errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage));
			}
			else
			{
				GetImageTags(_imageFilePath);
			}
		}

		[UsedImplicitly]
		public void OnSetImageExifTags()
		{
			//Note, that PickImageFromGallery creates a duplicate of the selected image in the application folder
			//This is why you need to store the path to it, so you can later get it or its attributes
			AGGallery.PickImageFromGallery(
				selectedImage =>
				{
					_imageFilePath = selectedImage.OriginalPath;

					var exif = new AGExifInterface(_imageFilePath) { Artist = "Osiris" };

					Debug.Log("Artist: " + exif.Artist);

					exif.SaveAttributes();
					Resources.UnloadUnusedAssets();
				},
				errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage));
		}

		static void GetImageTags(string imagePath)
		{
			var exif = new AGExifInterface(imagePath);

			var tags = exif.ToString().Split(',');
			foreach (var exifTag in tags)
			{
				Debug.Log(exifTag);
			}
		}

		static Sprite SpriteFromTex2D(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
	}
}