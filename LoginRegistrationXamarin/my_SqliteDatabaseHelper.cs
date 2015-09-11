//Identity column should be named "_id" in Android SQLite

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
using Android.Database.Sqlite;
using System.IO;


namespace MyFirstApp
{
	[Activity (Label = "my_SqliteDatabaseHelper")]			
	public class my_SqliteDatabaseHelper : Android.Database.Sqlite.SQLiteOpenHelper
	{
		public static readonly string DATABASE_NAME ="Apartment.db";
		public static readonly string TABLE_NAME ="Owner_tbl";
		public static readonly int DATABASEVERSION=1;

		public static readonly string Col_1 ="ID";
		public static readonly string Col_2 ="NAME";
		public static readonly string Col_3 ="FLATNO";
		public static readonly string Col_4 ="USERNAME";
		public static readonly string Col_5 ="PASSWORD";
		public static readonly string Col_6 ="EMAIL";
		public static readonly string Col_7 ="AGE";

		public string sqldb_location;
		public string sqldb_path;
		public static String DbPath;
		private SQLiteDatabase sqldb;
		public string sqldb_message;
		private string sqldb_query;

		public my_SqliteDatabaseHelper(Context context):base(context, DATABASE_NAME, null, DATABASEVERSION)
		{
			try
			{
				sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				sqldb_path = Path.Combine(sqldb_location, DATABASE_NAME);
				bool sqldb_exists = File.Exists(sqldb_path);

				if (!sqldb_exists) {
					sqldb = SQLiteDatabase.OpenOrCreateDatabase (sqldb_path, null);
					sqldb.ExecSQL (
						"create table " + TABLE_NAME + "(ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
						"NAME VARCHAR(50) , " +
						"FLATNO  VARCHAR(25), " +
						"USERNAME  VARCHAR(25), " +
						"PASSWORD  VARCHAR(25), " +
						"EMAIL  VARCHAR(100), " +
						"AGE INTEGER )"
					);
					sqldb_message = "Database: " + DATABASE_NAME + " created";
				} else 
				{
					sqldb=SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
					//sqldb_message = "Database: " + DATABASE_NAME + " opened";
					sqldb_message = "System is ready to login please proceed...";

				}

			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		public override void  OnCreate (Android.Database.Sqlite.SQLiteDatabase db )
		{		
			sqldb_message = "OnCreate method is calling - in which Table is created ";

			sqldb.ExecSQL (
				"create table " + TABLE_NAME + "(ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
				"NAME VARCHAR(50) , " +
				"FLATNO  VARCHAR(25), " +
				"USERNAME  VARCHAR(25), " +
				"PASSWORD  VARCHAR(25), " +
				"EMAIL  VARCHAR(100), " +
				"AGE INTEGER )"
			);			
		}

		public override void OnUpgrade(Android.Database.Sqlite.SQLiteDatabase db, int oldVersion, int newVersion)
		{
			sqldb_message = "OnUpgrade method is calling ";
			db.ExecSQL ("DROP TABLE IF EXISTS " + TABLE_NAME);

			if (oldVersion < 2)
			{
				//perform any database upgrade tasks for versions prior to  version 2              
			}
			if (oldVersion < 3)
			{
				//perform any database upgrade tasks for versions prior to  version 3
			}

			// create fresh items table
			this.OnCreate(sqldb);
		}

		//Adds a new record with the given parameters
		public void AddRecord(string sName ,string sFlat,string sUsername,string sPassword,string sEmail,string iAge)
		{
			try
			{
				sqldb_query = "INSERT INTO "+ TABLE_NAME +"  (NAME,FLATNO,USERNAME,PASSWORD,EMAIL,AGE) VALUES ('" + sName + "' , '" + sFlat + "' ,'"+ sUsername +"','"+ sPassword  +"','"+ sEmail +"' , " + int.Parse(iAge) + ");";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		//Updates an existing record with the given parameters depending on id parameter
		public void UpdateRecord(int iId, string sName ,string sFlat,string sUsername,string sPassword,string sEmail,int iAge)
		{
			try
			{
				sqldb_query=" UPDATE " + TABLE_NAME + " SET NAME ='" + sName + "', FLATNO ='" + sFlat + "', USERNAME ='" + sUsername + "' , PASSWORD='"+ sPassword + "' , EMAIL ='"+ sEmail +"',AGE='"+ iAge +"'  WHERE ID ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " updated";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		//Deletes the record associated to id parameter
		public void DeleteRecord(int iId)
		{
			try
			{
				sqldb_query = "DELETE FROM " + TABLE_NAME  + " WHERE ID ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//Searches a record and returns an Android.Database.ICursor cursor
		//Shows all the records from the table
		public Android.Database.ICursor GetRecordCursor()
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				//Identity column should be named "_id" in Android SQLite
				sqldb_query = "SELECT ID as _id,NAME,FLATNO,USERNAME,PASSWORD,EMAIL,AGE FROM " + TABLE_NAME  + ";";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}

		//Searches a record and returns an Android.Database.ICursor cursor
		//Shows records according to search criteria
		public Android.Database.ICursor GetRecordCursor(string sColumn, string sValue)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				//Identity column should be named "_id" in Android SQLite
				sqldb_query = "SELECT ID  as _id ,NAME,FLATNO,USERNAME,PASSWORD,EMAIL,AGE FROM "  + TABLE_NAME  +  " WHERE " + sColumn + " LIKE '" + sValue + "%';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					//sqldb_message = "Record not found";
					sqldb_message = "Invalid user! pls try again";
				}
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}

		//Searches a record and returns an Android.Database.ICursor cursor
		//Shows records according to search criteria
		public Android.Database.ICursor getDatabaseUser_RecordCursor(string sUsername, string sPwd)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				//Identity column should be named "_id" in Android SQLite
				sqldb_query = "SELECT ID as _id ,NAME,FLATNO,USERNAME,PASSWORD,EMAIL,AGE FROM "  + TABLE_NAME  +  " WHERE " + Col_4 + "= '" + sUsername + "' AND " + Col_5 +"= '" + sPwd + "';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if( (!(sqldb_cursor != null)) || (sqldb_cursor.Count==0))
				{
					//sqldb_message = "Record not found";
					sqldb_message = "Invalid user! pls try again";
					sqldb_cursor = null;
				}
				else if ((sqldb_cursor != null) && (sqldb_cursor.Count>0))
				{
					//sqldb_message = "Record found";
					sqldb_message = "Welcome to phonex apartment";
				}					
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}
	}
}