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
using Android.Preferences;

namespace MyFirstApp
{
	[Activity (Name="com.companyname.myfirstapp.WelcomeActivity",Label = "Welcome")]
	public class WelcomeActivity : Activity
	{
		private string strId;
		private string strLoginUser;
		private string strLoginPassword;
		SettingsService settingservice = new SettingsService ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.WelcomeActivityLayout);



			strId= Intent.GetStringExtra("LOGIN_ID");
			strLoginUser= Intent.GetStringExtra("LOGIN_USERNAME");
			strLoginPassword=Intent.GetStringExtra("LOGIN_PASSWORD");

			/*show Username ,Password ,id*/
			var toast = Toast.MakeText(this,strLoginUser,ToastLength.Short);
			//toast.Show();

			toast = Toast.MakeText(this,strLoginPassword,ToastLength.Short);
			//toast.Show();

			toast = Toast.MakeText(this,strId,ToastLength.Short);
			//toast.Show();


			ImageView showPopupMenu = FindViewById<ImageView> (Resource.Id.popupbuttonmenuimageView);
			showPopupMenu.Click += (s, arg) => {
				PopupMenu menu = new PopupMenu (this, showPopupMenu);
				// with Android 3 need to use MenuInfater to inflate the menu
				//In Android 3, the code to inflate the menu from an XML resource required that you 
				//first get a reference to a MenuInflator, and then call its Inflate method with the 
				//resource ID of the XML that contained the menu and the menu instance to inflate into. 
				//Such an approach still works in Android 4 as the code below shows:
				//menu.MenuInflater.Inflate (Resource.Menu.popup_menu, menu.Menu);

				// with Android 4 Inflate can be called directly on the menu
				//As of Android 4 however, you can now call Inflate directly on the instance of the PopupMenu. 
				//This makes the code more concise as shown here:
				menu.Inflate (Resource.Menu.popup_List_menu);
				menu.Show();

				menu.MenuItemClick += (s1, arg1) => {					
					//Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString().Trim()=="UserList")
					{
						toast = Toast.MakeText(this,arg1.Item.TitleFormatted,ToastLength.Short);	
					}
					else if(arg1.Item.TitleFormatted.ToString().Trim()=="Profile")
					{
						toast = Toast.MakeText(this,arg1.Item.TitleFormatted,ToastLength.Short);	
					}
					else if (arg1.Item.TitleFormatted.ToString().Trim()=="Logout")
					{
						logout();
						toast = Toast.MakeText(this,arg1.Item.TitleFormatted,ToastLength.Short);	

					}
					else
					{
						toast = Toast.MakeText(this,"Item Not Selected",ToastLength.Short);	
					}

					toast.Show();
				};
				// Android 4 now has the DismissEvent
				menu.DismissEvent += (s2, arg2) => {
					//Console.WriteLine ("menu dismissed"); 
					toast = Toast.MakeText(this,"menu dismissed",ToastLength.Short);
					toast.Show();
				};
				menu.Show ();
			};
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);

			MenuInflater inflater = this.MenuInflater;
			inflater.Inflate (Resource.Menu.popup_menu, menu);


			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			base.OnOptionsItemSelected (item);

			string strActionbartext = this.GetString (Resource.String.Welcome_label);
				if (item.TitleFormatted.ToString () == strActionbartext) {
				goToLoginScreen ();
			}

			var toast=Toast.MakeText(this,"Menu Initiallised",ToastLength.Short);

			switch (item.ItemId)
			{
			case Resource.Id.itemUserList:
				{
					
					toast = Toast.MakeText(this,item.TitleFormatted,ToastLength.Short);
					break;
				}
			case Resource.Id.itemProfile:
				{
					goToProfileScreen ();

					toast = Toast.MakeText(this,item.TitleFormatted,ToastLength.Short);

					break;
				}
			case Resource.Id.itemLogout:
				{					
					logout ();
					break;
				}
			default:
				break;
			}

			toast.Show();

			return true;
		}

		private void goToLoginScreen()
		{				
			var myIntent = new Intent (this, typeof(LoginScreen));
			StartActivityForResult (myIntent, 0);
		}

		private void goToProfileScreen()
		{				
			//var myIntent = new Intent (this, typeof(ProfileActivity));

			var myIntent = new Intent (this, typeof(activity_drawer_welcome));

			StartActivityForResult (myIntent, 0);
		}

		private void logout()
		{	
			ISharedPreferences sharedPreferences; 
			sharedPreferences = Application.Context.GetSharedPreferences("MyFirstApp", FileCreationMode.Private);

			var editor = sharedPreferences.Edit ();
			settingservice.AddOrUpdateSetting("RemeberCredentials", false);
			//editor.PutBoolean("RemeberCredentials",false);
			editor.Remove("username");
			editor.Remove("password");
			editor.Commit();
			var myIntent = new Intent (this, typeof(LoginScreen));
			StartActivityForResult (myIntent, 0);
			Finish();
		}
	}
}