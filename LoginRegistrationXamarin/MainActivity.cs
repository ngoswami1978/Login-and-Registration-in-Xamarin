/*
	Xamarin are a California based software company who created Mono, 
	MonoTouch and Mono for Android which are all cross-platform implementations of the Common Language 
	Infrastructure (CLI) and Common Language Specifications (which are often referred to as Microsoft.NET)
	Extracted from Visual Studio Magazine
	"Monodroid is a plug-in to Visual Studio 2010. This plug-in allows developers to deploy apps to the Android 
	Emulator running locally, as well as to a physical device connected over a USB cable and Wi-Fi. 
	The plug-in allows MonoDroid to actively work with the rest of the Visual Studio ecosystem and 
	integrate with the tools that developers are already using."
	Using the C# language, developers are able to build native apps that can target iOS, Android and Windows Phone. 
	They can use either Xamarin Studio (which is an IDE that is downloaded as part of the Xamarin.Android SDK) or they 
	can use Visual Studio. Personally, I use Visual Studio as that is what I am most familiar with. For the purposes 
	of this article, I will use Visual Studio.
	This article assumes the reader is familiar with C#, Visual Studio and the .NET Framework. It also 
	assumes that you have downloaded the Xamarin.Android SDK from the Xamarin web site. if you haven't then download it now.

	Expand the Properties folder.

	AndroidManifest.xml - This file defines essential information about the application to the Android system. 
	Every Android application has one of these. This file MUST be present for the application to run as it defines 
	such things as the name of the application to the Android system, describes the components of the application, 
	declares permissions and defines the minimum Android API level for example.
	
	Expand the Resources folder.

	Drawable - This folder contains the images for your application. If your application requires different sized images 
	for the different screen sizes, then create a sub-folder underneath here for each of the different resolutions.
	
	Layout - This folder contains the Android layout (.AXML) files for your project. 
	An Android layout file is similar to an HTML or ASPX file. It defines a single Android screen using XML syntax. 
	Create a new layout for each screen. If your application requires different layouts for landscape or portrait, 
	then you need to create layout-land and layout-port sub-folders respectively. By default, there is a layout 
	created automatically for you called Main.axml. This is the UI layout that is invoked by the default Activity 
	class mentioned earlier - Activity1.cs.
	
	Values - This folder contains resource definitions that your application uses such as string values. 
	For example, you could store the name of the application or dropdown list items in the Strings.xml file
	
	Resource.Designer.cs - This file contains the class definitions for the generated IDs contained within the application. 
	This allows application resources to be retrieved by name so the developer does not have to use the ID. 
	The file therefore maps the application resource name to its generated ID. The file is updated each time the project is built. 
	This file should NOT be updated by the developer.

	Other files you could have underneath the Values folder include:

	Arrays.xml
	Colors.xml
	Dimensions.xml
	Styles.xml

*/

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;
using Java;
using Dalvik;
using Android.Database.Sqlite;
using System.IO;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Globalization;
using System.Text.RegularExpressions;
using Android.Net;



namespace MyFirstApp
{
	
	[Activity (Name="com.companyname.myfirstapp.MainActivity", Label = "Galaxy Apartment", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, View.IOnClickListener
	{		
		Button bSignUp;
		//Button btSave;
		EditText etname,etAge,etUsername,etpwd,etEmail;
		TextView tvLoginLink;
		ImageView imageview_profile;
		my_SqliteDatabaseHelper myDb;
		string sflat;
		bool invalid = false;
		Drawable errorIcon;
		Drawable correctIcon;

		public static readonly int PickImageId = 1000;
		private BitmapDrawable bitmap;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Thread.Sleep(10000); // Simulate a long loading process on app startup.
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.activity_register_main);

		
			/*Setting flat name in Spinner control */
			Spinner spinner = FindViewById<Spinner> (Resource.Id.flatspinner);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.Flats_array, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
			/*end*/

			/*create or open database*/
			myDb = new my_SqliteDatabaseHelper(this);
			/*end create or open database*/

			errorIcon = Resources.GetDrawable(Resource.Drawable.correcticon);
			correctIcon = Resources.GetDrawable(Resource.Drawable.erroricon);

			imageview_profile= FindViewById<ImageView> (Resource.Id.imageview_profile);

			etname = FindViewById<EditText> (Resource.Id.etname);
			etAge = FindViewById<EditText>(Resource.Id.etAge);
			etUsername  = FindViewById<EditText>(Resource.Id.etUsername);

			//etFlatNo  = FindViewById<EditText>(Resource.Id.etFlatNo);
			etpwd  = FindViewById<EditText>(Resource.Id.etpwd);
			etEmail  = FindViewById<EditText>(Resource.Id.etEmail);

			etEmail.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				if (IsValidEmail(e.Text.ToString()))
				{
					//change icon to green
					etEmail.SetError("right", errorIcon);
					//emailEditText.SetError(
				}
				else
				{
					//emailEditText.set
					etEmail.SetError("wrong", correctIcon);
					//change icon to red
				}
			};

			bSignUp= FindViewById <Button>(Resource.Id.bSignUp);
			tvLoginLink = FindViewById<TextView> (Resource.Id.tvLoginLink);

			bSignUp.SetOnClickListener(this);
			tvLoginLink.SetOnClickListener (this);
			imageview_profile.SetOnClickListener (this);
			//bLogout.Click += DoClick ();
			//btSave.Click +=  DoClick ();
		}

		/*Email Validation method */
		public bool IsValidEmail(string strIn)
		{
			invalid = false;
			if (String.IsNullOrEmpty(strIn))
				return false;

			// Use IdnMapping class to convert Unicode domain names. 
			try {
				strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
					RegexOptions.None, TimeSpan.FromMilliseconds(200));
			}
			catch (RegexMatchTimeoutException) {
				return false;
			}

			if (invalid)
				return false;

			// Return true if strIn is in valid e-mail format. 
			try {
				return Regex.IsMatch(strIn,
					@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
					@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException) {
				return false;
			}
		}

		private string DomainMapper(Match match)
		{
			// IdnMapping class with default property values.
			IdnMapping idn = new IdnMapping();

			string domainName = match.Groups[2].Value;
			try {
				domainName = idn.GetAscii(domainName);
			}
			catch (ArgumentException) {
				invalid = true;
			}
			return match.Groups[1].Value + domainName;
		}
		/*end-- Email Validation method */


		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			string toast = string.Format ("The Selected Flat is {0}", spinner.GetItemAtPosition (e.Position));
			sflat = spinner.GetItemAtPosition (e.Position).ToString();
			Toast.MakeText (this, sflat, ToastLength.Long).Show ();
		}

		public void OnClick(View V)
		{ 			
			try
			{			
				switch(V.Id)
				{
				case Resource.Id.bSignUp :
					{
						//CHECK VALIDATION - FOR 'RIGHT DATA/BLANK FIELD' ENTERD BY USER
						//STATEMENT TO VALIDATE THE EACH RECORDS
						Boolean blnValidate =FieldValidation();

						if (blnValidate==true)
						{
							//AADING REGISTRATION DETAILS IN DATABASE.
							myDb.AddRecord (etname.Text, sflat, etUsername.Text, etpwd.Text, etEmail.Text, etAge.Text);
							etname.Text = etUsername.Text = etpwd.Text = etEmail.Text = etAge.Text = "";

							var toast = Toast.MakeText (this, myDb.sqldb_message ,ToastLength.Long);
							toast.Show ();

							//WHEN SAVING SUCESSFULLY DONE CALL TO LOGIN ACTIVITY
							var myIntent = new Intent (this, typeof(LoginScreen));
							StartActivityForResult (myIntent, 0);								
						}
						break;
					}
				case Resource.Id.tvLoginLink :
					{
						//tvLoginLink.SetBackgroundColor(Color.PowderBlue);
						//tvLoginLink.SetShadowLayer(30, 0, 0, Color.Red);
						var myIntent = new Intent (this, typeof(LoginScreen));
						StartActivityForResult (myIntent, 0);
						break;
					}
				case Resource.Id.imageview_profile :
					{
						//var myIntent = new Intent (Intent.ActionPick ,Android.Provider.MediaStore.Images.Media.ExternalContentUri);
						//StartActivityForResult (myIntent, SELECT_PICTURE);

						Intent = new Intent();
						Intent.SetType("image/*");
						Intent.SetAction(Intent.ActionGetContent);
						StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);

						break;
					}
				}
			}
			catch (NullReferenceException ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
			finally {
				//myDb = null;
			}

		}

		protected override void OnActivityResult(int requestCode, Result  resultCode, Intent data)
		{
			try
			{
				if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
				{
					Android.Net.Uri uri = data.Data;
					imageview_profile.SetImageURI(uri);

					string path = GetPathToImage(uri);
					Toast.MakeText(this, path, ToastLength.Long);
				}				
			}
			catch (Exception ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
		}

		private string GetPathToImage(Android.Net.Uri uri)
		{
			string path = null;
			try
			{
				// The projection contains the columns we want to return in our query.
				string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
				using (Android.Database.ICursor cursor = ManagedQuery(uri, projection, null, null, null))
				{
					if (cursor != null)
					{
						int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
						cursor.MoveToFirst();
						path = cursor.GetString(columnIndex);
					}
				}
			}
			catch (Exception ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
			return path;
		}

		public Boolean FieldValidation()
		{
			string Validation_msg="";

			if (etname.Text == "") {
				Validation_msg = "Please enter your name..";
				etname.SetError("Please enter your name..", errorIcon);
				etname.SetSelectAllOnFocus (true);
			}
			else if (sflat == "Select Your Flat") {
				Validation_msg = "Please select flat..";
			}
			else if (etUsername.Text == "") {
				Validation_msg = "Please enter username..";
				etUsername.SetError("Please enter username..", errorIcon);
				etUsername.SetSelectAllOnFocus (true);
			}
			else if (etpwd.Text == "") {
				Validation_msg = "Please enter password..";
				etpwd.SetError("Please enter password..", errorIcon);
				etname.SetSelectAllOnFocus (true);
			}
			else if (etEmail.Text == "") {
				Validation_msg = "Please enter email..";
				etname.SetSelectAllOnFocus (true);
			}
			else if (etAge.Text == "") {
				Validation_msg = "Please enter your age..";
				etAge.SetError("Please enter your age..", errorIcon);
				etname.SetSelectAllOnFocus (true);
			}

			if (IsValidEmail(etEmail.Text.ToString()))
			{
				//change icon to green
				etEmail.SetError("right", errorIcon);
				//emailEditText.SetError(
			}
			else
			{
				//emailEditText.set
				etEmail.SetError("wrong", correctIcon);
				Validation_msg = "Wrong email..";
				//change icon to red
			}

			if (Validation_msg != "") {
				var toast = Toast.MakeText (this, Validation_msg,ToastLength.Long);
				toast.Show ();

				return false;
					
			}

			return true;

		}

		/*We can use thDoClick Function also same it is OnClick function*/
		/*
		public void DoClick (object sender, EventArgs e)
		{
			View view = (View)sender;
			switch (view.Id) {
			case Resource.Id.bLogout :
				{
					var toast = Toast.MakeText (this, "Button Logout Clicked",ToastLength.Short);
					toast.Show ();
					break;
				}

			case Resource.Id.btSave :
				{
					var toast = Toast.MakeText (this, "Button Save Clicked",ToastLength.Short);
					toast.Show ();

					Log.Error("Save Button", "Clicked");
					break;
				}			
			}

		}*/

	}
}


