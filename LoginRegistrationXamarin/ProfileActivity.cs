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
	[Activity (Name="com.companyname.myfirstapp.ProfileActivity",Label = "Profile")]

	public class ProfileActivity:Activity
	{		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Create your application here
			SetContentView (Resource.Layout.ProfileActivityLayout);


			// Get a reference to the AutoCompleteTextView in the layout
			AutoCompleteTextView textView =  FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_edittext_city);

			// Get the string array
			//String[] countries =  getResources().getStringArray(Resource.Array.city_array);

			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.city_array, Android.Resource.Layout.SimpleListItem1);
			
			textView.Adapter= adapter;

		}
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);
			MenuInflater inflater = this.MenuInflater;
			inflater.Inflate (Resource.Menu.check_submit, menu);
			return true;
		}
	}
}

