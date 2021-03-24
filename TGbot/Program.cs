using System;
using Telegram.Bot;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TGbot
{
    class Program
    {
        public static ITelegramBotClient Bot;

        static void Main(string[] args)
        {
            //using (var db = new DbBotContext())
            //{
            //    var messages = db.Messages.Include(x => x.User);
            //    foreach (var item in messages)
            //    {
            //        Console.WriteLine("{0} {1}: {2}", item.Time.ToLongTimeString(), item.User.ChatId, item.Text);
            //    }
            //}
            //return;

            try
            {
                Bot = new TelegramBotClient("1755940122:AAHKfdr-G4sQNGzQq585mLeQuDAVw2lOMME");
                Bot.OnMessage += Handler.Handle;
                Bot.StartReceiving();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            while (true) { }
            

        }
    }
}
