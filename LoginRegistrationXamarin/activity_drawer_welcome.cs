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
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Content.Res;


namespace MyFirstApp
{
	[Activity (Label = "activity_drawer_welcome",Name="com.companyname.myfirstapp.activity_drawer_welcome")]			
	public class activity_drawer_welcome : Android.Support.V4.App.FragmentActivity
	{
		DrawerLayout mDrawerLayout;
		List<string> mLeftitem = new List<String>();
		ListView mLeftDrawer;
		ActionBarDrawerToggle mDrawerToggle;

		//public List<DrawerItem> items;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.activity_drawer_welcomeLayout);
			try
			{
				mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.myDrawer_layout);
				mLeftDrawer = FindViewById<ListView> (Resource.Id.leftListView);

				mDrawerToggle = new myActionBarDrawerToggle (this, mDrawerLayout, Resource.Drawable.ic_drawer, Resource.String.open_drawer, Resource.String.close_drawer);

				mLeftDrawer.Adapter=new MenuListAdapter(this);
				this.mLeftDrawer.ItemClick += (sender, e) => SelectItem(e.Position);

				mDrawerLayout.SetDrawerListener (mDrawerToggle);

				ActionBar.SetDisplayHomeAsUpEnabled (true);
				ActionBar.SetHomeButtonEnabled (true);
				ActionBar.SetDisplayShowTitleEnabled (true);
			}
			catch (Exception ex) {	
				var toast = Toast.MakeText (this, ex.Message ,ToastLength.Short);
				toast.Show ();
			}
		}

		private void SelectItem(int position)
		{
			var toast = Toast.MakeText (this, mLeftDrawer.GetItemAtPosition (position).ToString () ,ToastLength.Short);
			toast.Show ();

			this.mLeftDrawer.SetItemChecked(position, true);
			this.mDrawerLayout.CloseDrawer(this.mLeftDrawer);

			Android.Support.V4.App.Fragment fragment = null;

			switch (position) {
			case 0:
				//fragment = new BrowseFragment ();
				break;
			case 1:
				//fragment = new FriendsFragment ();
				break;
			case 2:
				//fragment = new ProfileFragment ();
				break;
			}

			this.SupportFragmentManager.BeginTransaction ()
				.Replace (Resource.Id.Content_frame, fragment)
				.Commit ();

			/*
			if (position == this.GetNavigationDrawerItemPosition(title))            //GetNavigationDrawerItemPosition checks the list of navigationDrawerItems and compares the newly selected position and the currently selected title
			{
				this.mDrawerLayout.CloseDrawer(this.mLeftDrawer);
				return;
			}

			this.mLeftDrawer.SetItemChecked(position, true);
			ActionBar.Title = this.navigationDrawerItems[position].ItemName; //setting the title to match the newly selected item
			this.mDrawerLayout.CloseDrawer(this.mLeftDrawer);

			switch (navigationDrawerItems[position].ItemName)
			{

			}*/
		}



		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			this.mDrawerToggle.SyncState();
		}
		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			this.mDrawerToggle.OnConfigurationChanged (newConfig);
		}
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (this.mDrawerToggle.OnOptionsItemSelected (item)) {
					//var toast = Toast.MakeText (this,item.ToString() ,ToastLength.Short);
					//toast.Show ();
									
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}

