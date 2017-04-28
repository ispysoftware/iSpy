using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public static class Mailer
    {
        public static bool IsValidEmail(string email)
        {
            var message = new MailMessage();
            bool f = false;
            try
            {
                message.To.Add(email);//use built in validator
            }
            catch
            {
                f = true;
            }
            message.Dispose();
            return !f;
        }


        public static bool Send(string to, string subject, string body)
        {
            return Send(to, subject, body, null);
        }

        public static bool Send(string to, string subject, string body, byte[] attach)
        {
            bool success = true;
            try
            {
                to = to.Replace(",", ";");

                string[] addrs = to.Split(';');

                var sendFrom = new MailAddress(MainForm.Conf.SMTPFromAddress.Trim());
                var sendTo = new MailAddress(addrs[0].Trim());
                var myMessage = new MailMessage(sendFrom, sendTo)
                {
                    Subject = subject.Replace(Environment.NewLine, " ").Trim(),
                    Body = body,
                    IsBodyHtml = true
                };

                if (addrs.Length > 1)
                {
                    for (int i = 1; i < addrs.Length && i < 5; i++)
                    {
                        if (IsValidEmail(addrs[i].Trim()))
                            myMessage.Bcc.Add(new MailAddress(addrs[i].Trim()));
                    }
                }

                MemoryStream stream = null;
                if (attach != null && attach.Length>0)
                {
                    stream = new MemoryStream(attach) {Position = 0};

                    var attachFile = new Attachment(stream, "Screenshot.jpg",
                        System.Net.Mime.MediaTypeNames.Image.Jpeg);

                    myMessage.Attachments.Add(attachFile);
                    
                }

                var emailClient = new SmtpClient(MainForm.Conf.SMTPServer, MainForm.Conf.SMTPPort)
                                  {
                                      UseDefaultCredentials = false,
                                      Credentials =
                                          new NetworkCredential(MainForm.Conf.SMTPUsername, MainForm.Conf.SMTPPassword),
                                      EnableSsl = MainForm.Conf.SMTPSSL
                                  };

                emailClient.Send(myMessage);

                stream?.Dispose();
                myMessage.Dispose();
                myMessage = null;
                emailClient.Dispose();
                emailClient = null;

            }
            catch (Exception ex)
            {
                success = false;
                Logger.LogException(ex);
            }
            return success;
        }
    }
}
