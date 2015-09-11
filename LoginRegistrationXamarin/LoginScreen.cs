using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using System.Threading;

namespace MyFirstApp
{
	[Activity (Label = "LoginScreen" , Name="com.companyname.myfirstapp.LoginScreen")]			
	public class LoginScreen : Activity, View.IOnClickListener
	{
		Button bSignIn;
		EditText etUsername, etpwd ;
		TextView tvregister,tvforget,id,name,flat,age;
		Drawable errorIcon;
		Drawable correctIcon;
		my_SqliteDatabaseHelper myDb;
		Boolean blnchkRemember;
		Boolean isLogin;
		SettingsService settingservice = new SettingsService ();

		//ListView object for displaying data from database
		ListView listItems;

		//Message TextView object for displaying data
		TextView shMsg;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_Login);
			try
			{
				blnchkRemember=true;				
				isLogin=false;
				var remeberCredentials = settingservice.GetSetting<bool>("RemeberCredentials");
				blnchkRemember=remeberCredentials ;


				hideControls(true);

				/*create or open database*/
				myDb = new my_SqliteDatabaseHelper(this);
				/*end create or open database*/

				errorIcon = Resources.GetDrawable(Resource.Drawable.correcticon);
				correctIcon = Resources.GetDrawable(Resource.Drawable.erroricon);

				// Create your application here
				bSignIn=FindViewById<Button>(Resource.Id.bSignIn);
				tvregister = FindViewById<TextView> (Resource.Id.tvregister);
				tvforget = FindViewById<TextView> (Resource.Id.tvforget);
				etUsername=FindViewById<EditText>(Resource.Id.etUsername);
				etpwd=FindViewById<EditText>(Resource.Id.etpwd);

				if (remeberCredentials==true)
				{
					var savedusername = settingservice.GetSetting<string>("username");
					var savedpassword = settingservice.GetSetting<string>("password");					
					if (savedusername != null)
					{
						etUsername.Text=savedusername.ToString();
						etpwd.Text=savedpassword.ToString();
					}
				}

				CheckBox checkbox = FindViewById<CheckBox>(Resource.Id.checkbox_remember_me);
				checkbox.Checked=blnchkRemember;

				//Gets TextView object instances
				shMsg = FindViewById<TextView> (Resource.Id.shMsg);

				//Gets ListView object instance
				listItems = FindViewById<ListView> (Resource.Id.listItems);

				//Add ItemClick event handler to ListView instance
				listItems.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs> (item_Clicked);

				//Sets Database class message property to shMsg TextView instance
				shMsg.Text = myDb.sqldb_message;

				bSignIn.SetOnClickListener (this);
				tvregister.SetOnClickListener (this);
				tvforget.SetOnClickListener (this);

				checkbox.Click += (o, e) => {
					if (checkbox.Checked)
						//Toast.MakeText (this, "Selected", ToastLength.Short).Show ();
						blnchkRemember=true;
					else
						//Toast.MakeText (this, "Not selected", ToastLength.Short).Show ();
						blnchkRemember=false;
				};

			}
			catch (Exception ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
		}

		//Launched when a ListView item is clicked
		void item_Clicked (object sender, AdapterView.ItemClickEventArgs e)
		{
			//Gets TextView object instance from record_view layout
			TextView shId = e.View.FindViewById<TextView> (Resource.Id.Id_row);
			TextView shName = e.View.FindViewById<TextView> (Resource.Id.Name_row);
			TextView shFlatNo = e.View.FindViewById<TextView> (Resource.Id.Flat_row);
			TextView shAge = e.View.FindViewById<TextView> (Resource.Id.Age_row);

			//Displays messages for CRUD operations
			shMsg.Text = shId.Text;

			var myIntent = new Intent (this, typeof(WelcomeActivity));
			//Assuming you're using a ListActivity as the basis for your listview you can use the following code to bind the ItemClick event of the listview. 
			//Within the listview you can then finish the current activity and start the next one,passing in the ID or any other property of the selected item.
			// Start the second activity and finish this activity.
			// NOTICE: Finishing the activity removes it from the backstack.
			// The user will not be able to navigate back to the list!

			myIntent.PutExtra ("LOGIN_ID", shId.Text);
			myIntent.PutExtra ("LOGIN_USERNAME", settingservice.LoginUserName);
			myIntent.PutExtra ("LOGIN_PASSWORD", settingservice.LoginPassword);


			StartActivityForResult (myIntent, 0);
			Finish();
		}


		private void hideControls(Boolean blnHd)
		{
			try
			{
				id = FindViewById<TextView> (Resource.Id.id);
				name = FindViewById<TextView> (Resource.Id.name);
				flat = FindViewById<TextView> (Resource.Id.flat);
				age = FindViewById<TextView> (Resource.Id.age);

				if (blnHd==true)
				{
					id.Visibility=ViewStates.Gone;	
					name.Visibility=ViewStates.Gone;	
					flat.Visibility=ViewStates.Gone;	
					age.Visibility=ViewStates.Gone;	
				}
				if (blnHd==false)
				{
					id.Visibility=ViewStates.Visible;	
					name.Visibility=ViewStates.Visible;	
					flat.Visibility=ViewStates.Visible;	
					age.Visibility=ViewStates.Visible;	
				}
			}
			catch (Exception ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
		}

		public void OnClick(View V)
		{ 	
			ProgressDialog progress = new ProgressDialog(this);
			try
			{
				switch(V.Id)
				{
				case Resource.Id.bSignIn :
					{
						isLogin=false;
						Boolean blnValidate=  FieldValidation();
						if (blnValidate==true)
						{	
							progress.Indeterminate = true;
							progress.SetProgressStyle(ProgressDialogStyle.Spinner);
							progress.SetMessage("Contacting server. Please wait...");
							progress.SetCancelable(true);
							progress.Show();

							var progressDialog = ProgressDialog.Show(this, "Please wait...", "Checking Login info...", true);

							new Thread(new ThreadStart(delegate
							{

								RunOnUiThread(() => ValidateSqluserpwd(etUsername.Text.ToString() , etpwd.Text.ToString()));
								if (isLogin== true){
									RunOnUiThread(() => Toast.MakeText(this, "Login detail found in system...", ToastLength.Short).Show());
								}
								RunOnUiThread(() => progressDialog.Hide());
							})).Start();
						}
						break;
					}
				case Resource.Id.tvforget :
					{
						var toast = Toast.MakeText(this,"Forget link clicked",ToastLength.Short);
						toast.Show();
						break;
					}
				case Resource.Id.tvregister :
					{
						var myIntent = new Intent (this, typeof(MainActivity));
						StartActivityForResult (myIntent, 0);
						break;
					}
				}
			}
			catch (NullReferenceException ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
			finally
			{
				// Now hide the progress dialog
				progress.Dismiss();
			}
		}

		public Boolean FieldValidation()
		{
			string Validation_msg="";

			if (etUsername.Text == "") {
				Validation_msg = "Please provide the username..";
				etUsername.SetError("Please provide the username..", errorIcon);
				etUsername.SetSelectAllOnFocus (true);
			}
			else if (etpwd.Text == "") {
				Validation_msg = "Please provide the password..";
				etpwd.SetError("Please provide the password..", errorIcon);
				etpwd.SetSelectAllOnFocus (true);
			}
			if (Validation_msg != "") {
				var toast = Toast.MakeText (this, Validation_msg,ToastLength.Long);
				toast.Show ();
				return false;
			}
			return true;
		}

		public Boolean ValidateSqluserpwd(string sUsername, string sPwd)
		{
			try
			{
				isLogin=false;
				settingservice.LoginUserName=string.Empty;
				settingservice.LoginPassword= string.Empty;

				GetValidUserCursorView (sUsername, sPwd);		
				if  (isLogin==true)
				{
					settingservice.LoginUserName = sUsername;
					settingservice.LoginPassword = sPwd;

					// check user name and password in database
					//Boolean blnValidUserPwd = ValidateSqluserpwd(etUsername.Text.ToString() , etpwd.Text.ToString());
					if (blnchkRemember==true)
					{	
						if (isLogin==true)
						{
							settingservice.AddOrUpdateSetting("username", settingservice.LoginUserName);
							settingservice.AddOrUpdateSetting("password", settingservice.LoginPassword);
							settingservice.AddOrUpdateSetting("RemeberCredentials", blnchkRemember);	
						}
					}
					etUsername.Text=string.Empty;
					etpwd.Text=string.Empty;	
				}
			}
			catch (Exception ex)
			{	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
				return false;
			}
			return isLogin;
		}

		//Gets the cursor view to show records according to search criteria
		void GetValidUserCursorView (string sUserName, string sPwd)
		{
			try 
			{
				Android.Database.ICursor sqldb_cursor = myDb.getDatabaseUser_RecordCursor(sUserName, sPwd);
				if (sqldb_cursor != null)
				{
					isLogin=true;
					//Login successful
					sqldb_cursor.MoveToFirst ();

					//Identity column should be named "_id" in Android SQLite
					string[] from = new string[] {"_id","NAME","FLATNO","AGE" };
					int[] to = new int[]
					{
						Resource.Id.Id_row,
						Resource.Id.Name_row,
						Resource.Id.Flat_row,
						Resource.Id.Age_row
					};

					shMsg.Text = myDb.sqldb_message; // Welcome Message to the User is Observed here.
					SimpleCursorAdapter sqldb_adapter = new SimpleCursorAdapter (this, Resource.Layout.record_view, sqldb_cursor, from, to);
					listItems.Adapter = sqldb_adapter;
					hideControls(false);
				}
				else
				{
					//Login failed
					isLogin=false;
					listItems.Adapter=null;
					shMsg.Text = myDb.sqldb_message;
					hideControls(true);
				}
			}
			catch (Exception ex) {
				isLogin=false;	
				listItems.Adapter=null;
				hideControls(true);
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
		}
	}
}