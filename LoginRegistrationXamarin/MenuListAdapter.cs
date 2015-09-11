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


namespace MyFirstApp
{
	public class MenuListAdapter : BaseAdapter
	{
		Activity context;

		public List<myMenuItem> items;

		public MenuListAdapter(Activity context) : base()
		{
			this.context = context;

			this.items = new List<myMenuItem>() 
			{
				//new myMenuItem() { Name = "Customer", Img =Resource.Drawable.checkicon1 },
				//new myMenuItem() { Name = "Products", Img = Resource.Drawable.checkicon2 },
				//new myMenuItem() { Name = "Orders", Img = Resource.Drawable.checkicon3 }

				new myMenuItem() { Name = "UserList", Img =Resource.Drawable.Icon }, 
				new myMenuItem() { Name = "Maintainance", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "DailyExpenses", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "Cashflow", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "Events", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "Gallary", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "Renters", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "Rules", Img =Resource.Drawable.Icon },
				new myMenuItem() { Name = "AboutUs", Img =Resource.Drawable.Icon }
			};
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return position;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{	
			var item = items[position];
			var view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.NavigationDrawerItem,parent,false)) as LinearLayout;

			var menuimg = view.FindViewById(Resource.Id.drawer_photo) as ImageView;
			var menutxt = view.FindViewById(Resource.Id.drawer_itemName) as TextView;

			menuimg.SetImageResource(item.Img);
			menutxt.SetText(item.Name, TextView.BufferType.Normal);
			menutxt.Gravity = GravityFlags.Left;

			((LinearLayout)view.FindViewById<LinearLayout>(Resource.Id.NavigationDrawerItem)).SetGravity(GravityFlags.Left);

			return view;
		}
	}

	public class myMenuItem
	{
		public myMenuItem()
		{

		}

		public myMenuItem(string name, int img)
		{
			this.Name = name;
			this.Img = img;
		}

		public string Name { get; set; }

		public int Img { get; set; }
	}

}

