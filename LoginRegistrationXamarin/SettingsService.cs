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
using System.IO;
using System.Security.Cryptography;
using Android.Preferences;

namespace MyFirstApp
{
	public class SettingsService
	{
		ISharedPreferences preferences;
		//=====DON'T MODIFY THESE VALUES AFTER PUBLISHING APP! =========================|
		private const string initVector = "random_code_here";
		private const string passPhrase = "long_random_code_here_like_guid_for_eg";
		private const int keysize = 256;
		public string strLoginName;
		public string strLoginPassword;
		public string _message;
		//==============================================================================|

		public SettingsService ()
		{			
			try
			{
				preferences = Application.Context.GetSharedPreferences("MyFirstApp", FileCreationMode.Private);	
			}
			catch (Exception ex) {	
				_message = ex.Message;
			}
		}

		public string LoginUserName
		{
			get
			{
				return this.strLoginName;
			}
			set
			{
				this.strLoginName = value;
			}
			
		}

		public string LoginPassword
		{
			get
			{
				return this.strLoginPassword;
			}
			set
			{
				this.strLoginPassword = value;
			}
		}

		public void AddOrUpdateSetting<T>(string key, T value)
		{
			try
			{
				if (string.IsNullOrEmpty(key))
					throw new ArgumentNullException("key");

				var editor = preferences.Edit();

				var encryptedValue = EncryptString(value.ToString());

				editor.PutString(key, encryptedValue);

				editor.Apply();	
			}
			catch (Exception ex) {	
				_message = ex.Message;
			}
		}

		public T GetSetting<T>(string key, T defaultValue = default(T))
		{
			try
			{
				if (string.IsNullOrEmpty(key))
					throw new ArgumentNullException("key");

				if (!preferences.Contains(key))
				{
					if (defaultValue != null)
						AddOrUpdateSetting<T>(key, defaultValue);

					return defaultValue;
				}	
			}
			catch (Exception ex) {	
				_message = ex.Message;
			}
			return (T)Convert.ChangeType(DecryptString(preferences.GetString(key, string.Empty)), typeof(T));
		}

		private string EncryptString(string plainText)
		{
			if (string.IsNullOrEmpty(plainText))
				return string.Empty;

			byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] cipherTextBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(cipherTextBytes);
		}

		private string DecryptString(string cipherText)
		{
			if (string.IsNullOrEmpty(cipherText))
				return string.Empty;

			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
		}

	}
}