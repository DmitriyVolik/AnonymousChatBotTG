using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGbot
{
    class Handler
    {
        static string GetEntityText(MessageEventArgs m, MessageEntity messageEntity)
        {
            int offset = messageEntity.Offset;
            int len = messageEntity.Length;
            var text = m.Message.Text.Substring(offset, len);
            return text;
        }

        static MessageEntity GetCommand(MessageEventArgs m)
        {
            if (m.Message.Entities == null) return null;

            foreach (var item in m.Message.Entities)
            {
                if (item.Type == Telegram.Bot.Types.Enums.MessageEntityType.BotCommand)
                {
                    return item;
                }
            }
            return null;
        }



        public static async void Handle(object sender, MessageEventArgs m)
        {
            try
            {

                var bot = Program.Bot;
                var chatId = m.Message.Chat.Id;
                if (m.Message == null) return;

                using (var db = new DbBotContext())
                {
                    var user = db.Users.Where(u => u.ChatId == chatId.ToString()).FirstOrDefault();
                    if (user == null)
                    {
                        user = new Models.User()
                        {
                            ChatId = chatId.ToString(),
                        };
                        db.Users.Add(user);
                        Console.WriteLine("New user");
                    }
                    else
                    {
                        Console.WriteLine("user exist");
                    }
                    user.FirstName = m.Message.From.FirstName;
                    user.LastName = m.Message.From.LastName;

                    //Log
                    var date = m.Message.Date;
                    var message = new Models.Message()
                    {
                        Text = m.Message.Text,
                        User = user,
                        Time = m.Message.Date.ToUniversalTime()
                    };
                    db.Messages.Add(message);

                    Console.WriteLine(chatId);
                    Console.WriteLine(m.Message.Text);

                    bool relay = true;

                    if (user.State == "addnickname")
                    {
                        if (m.Message.ReplyToMessage != null &&
                            m.Message.ReplyToMessage.MessageId ==
                            user.WaitMessageId)
                        {
                            relay = false;
                            user.State = null;
                            user.NickName = m.Message.Text;
                            await bot.SendTextMessageAsync(m.Message.Chat, "Nickname set: " + user.NickName);
                            Console.WriteLine("New nickname: " + user.NickName);
                        }
                        else
                        {
                            relay = true;
                            user.State = null;
                        }

                    }

                    user.WaitMessageId = null;

                    var command = GetCommand(m);
                    if (command != null)
                    {
                        relay = false;
                        var commandText = GetEntityText(m, command);
                        if (commandText == "/addnickname")
                        {
                            var forceReply = new ForceReplyMarkup();
                            var askNickname = await bot.SendTextMessageAsync(m.Message.Chat, "Send me new nickname",
                                replyMarkup: forceReply
                                );
                            user.State = "addnickname";
                            user.WaitMessageId = askNickname.MessageId;
                            //Обработка команд
                        }
                    }

                    if (relay)
                    {
                        foreach (var recipient in db.Users.Where(x => x.ChatId != user.ChatId))
                        {
                            var nick = user.NickName ?? "Anonymous";
                            await bot.SendTextMessageAsync(recipient.ChatId, "👤 " + nick + ":");
                            CopyMessage(m.Message, recipient.ChatId);
                        }
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
            }
        }



        static async void CopyMessage(Message m, string chatId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.telegram.org/bot1755940122:AAHKfdr-G4sQNGzQq585mLeQuDAVw2lOMME/copyMessage");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var from_chat_id = m.From.Id;
                var message_id = m.MessageId;

                string json = "{\"chat_id\":\"" + chatId + "\"," +
                              "\"from_chat_id\":\"" + from_chat_id.ToString() + "\"," +
                              "\"message_id\":\"" + message_id.ToString() + "\"}";

                streamWriter.Write(json);
            }
            await httpWebRequest.GetResponseAsync();

            //var httpResponse = await httpWebRequest.GetResponseAsync();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    //Console.WriteLine(result);
            //}
        }

    }


}

