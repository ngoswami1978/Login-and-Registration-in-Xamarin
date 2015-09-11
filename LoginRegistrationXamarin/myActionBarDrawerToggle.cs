using System;
using Android.Support.V4.App;
using Android.App;
using Android.Support.V4.Widget;

namespace MyFirstApp
{
	public class myActionBarDrawerToggle:ActionBarDrawerToggle
	{
		Activity mActivity;

		public myActionBarDrawerToggle (Activity activity,DrawerLayout drawerlayout,int imageResource,int openDrawerDesc, int closeDrawerDesc)
			: base (activity, drawerlayout,imageResource,openDrawerDesc,closeDrawerDesc)
		{			
			mActivity = activity;
		}

		public override void OnDrawerOpened (Android.Views.View drawerView)
		{
			base.OnDrawerOpened (drawerView);
			mActivity.ActionBar.Title = "Please select from List";
		}
		public override void OnDrawerClosed (Android.Views.View drawerView)
		{
			base.OnDrawerClosed (drawerView);
			mActivity.ActionBar.Title = "Drawer layout App";
		}
		public override void OnDrawerSlide (Android.Views.View drawerView, float slideOffset)
		{
			base.OnDrawerSlide (drawerView, slideOffset);
		}
	}
}

