﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;


namespace MyFirstApp
{
	public class BrowseFragment : Fragment
	{
		public BrowseFragment()
		{
			RetainInstance = true;
		}

		List<FriendViewModel> friends;
		public override View OnCreateView(LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			HasOptionsMenu = true;
			var view = inflater.Inflate(Resource.Layout.fragment_browse, null);

			var grid = view.FindViewById<GridView>(Resource.Id.grid);
			friends = Util.GenerateFriends();

			//grid.Adapter = new MonkeyAdapter(Activity, friends);
			//grid.ItemClick += GridOnItemClick;

			return view;
		}

		void GridOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{
			//var intent = new Intent(Activity, typeof(FriendActivity));
			//intent.PutExtra("Title", friends[itemClickEventArgs.Position].Title);
			//intent.PutExtra("Image", friends[itemClickEventArgs.Position].Image);
			//StartActivity(intent);
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//inflater.Inflate(Resource.Menu.refresh, menu);
		}

	}
}

