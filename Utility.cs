﻿using MyCustomDiscordBot.MyCustomDiscordBot;
using System;
using VMProtect;

namespace MyCustomDiscordBot
{

    //public static class Config
    //{
    //    public static string token = "OTA3NTkwMTUxOTIyOTEzMjgw.YYpZMg.1uSiJjFfnY1ijIw5boYUqB2X4ZI";
    //    public static string DBConnectionString = @"mongodb+srv://admin:01032021020aA#@botpug.g0mkk.mongodb.net/botPug?retryWrites=true&w=majority";
    //    public static ulong id = 909023733547671632;
    //    public static char Prfix = '!';
    //}
    public static class Config
    {

        public static Microsoft.Win32.RegistryKey XXXXX2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("DiscordBOTValo");

        //   public static string token = "OTA3NTkwMTUxOTIyOTEzMjgw.YYpZMg.1uSiJjFfnY1ijIw5boYUqB2X4ZI";
        //    public static ulong id = 909023733547671632;
        //public static char Prfix = '!';
        public static string token = (string)XXXXX2.GetValue(@"Token2");
        // public static string DBConnectionString = @"mongodb+srv://admin:01032021020aA#@botpug.g0mkk.mongodb.net/botPug?retryWrites=true&w=majority";
        public static string DBConnectionString = @"mongodb+srv://admin:#Data2811#2911@cluster0.nfisl.mongodb.net/cluster0?retryWrites=true&w=majority";//Valo

        public static ulong id = Convert.ToUInt64(XXXXX2.GetValue(@"serverid").ToString());
        public static char Prfix = char.Parse(XXXXX2.GetValue(@"perfix").ToString());
      //  public static string dbname = char.Parse(XXXXX2.GetValue(@"perfix").ToString());
    }
}
