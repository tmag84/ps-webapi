using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using PS_project.Models.Exceptions;
using PS_project.Models.NotificationModel;

namespace PS_project.Utils
{
    public class FcmHandler
    {
        private const string API_KEY = "AIzaSyDkMgJTO6njl3oLf1FNFVJGs-ZpP5FWPyw";
        private const string SENDER_ID = "102221429373";      

        public static void PushNotification(List<string> devices, PushObjectModel obj)
        {
            try
            {                
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var registered_devices = String.Join(",", devices);

                var send = new
                {
                    registration_ids = devices.ToArray(),
                    notification = new
                    {
                        body = obj.body,
                        title = obj.title   
                    },
                    data = new
                    {
                        service_id = obj.service_id,
                        service_name = obj.service_name
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(send);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", API_KEY));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidNotificationPushException(ex.Message);
            }
        }
    }
}