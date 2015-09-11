using System;
using Android.Content;
using Java.IO;


namespace MyFirstApp
{
		public class FileCache
		{
			private File m_CacheDir;

			public FileCache(Context context)
			{
				string stuff = Android.OS.Environment.ExternalStorageState;
				if (Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted))
				{
					m_CacheDir = new File(Android.OS.Environment.ExternalStorageDirectory, "Android/data/" + context.ApplicationContext.PackageName);
					m_CacheDir = context.ExternalCacheDir;
				}
				else
				{
					m_CacheDir = context.CacheDir;
				}

				if(m_CacheDir == null)
					m_CacheDir = context.CacheDir;




				if (!m_CacheDir.Exists())
				{
					var success = m_CacheDir.Mkdirs();
				}
			}

			public File GetFile(string url)
			{
				var fileName = url.GetHashCode().ToString();
				var file = new File(m_CacheDir, fileName);
				return file;
			}

			public void Clear()
			{
				File[] files = m_CacheDir.ListFiles();
				if (files == null)
					return;

				foreach (var file in files)
				{
					try
					{
						file.Delete();
					}
					catch (Exception)
					{
						//TODO log exception
					}
				}
			}
	}
}


