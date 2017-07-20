using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using PS_project.Models;
using PS_project.Models.Exceptions;
using PS_project.Models.NotificationModel;

namespace PS_project.Utils
{
    public class FcmHandler
    {
        private const string API_KEY = "AAAAF8zfOn0:APA91bHGncBxTbdD3Sad9hroIPuXJbBBgqhYx7gxRBthTYunUzrN_wSUx31R7HBw3MJnvR1qcrtKiFTMH4krMeKo3l3cwFD9nB2MflNN4-HLaidff9H7zg6B2_uu-ktunBJ04-Z3ayIG";
        private const string SENDER_ID = "102221429373";
		
        private static List<String> GetDevicesIdList(List<DeviceModel> devices)
        {
            List<String> devices_id = new List<String>();
            devices.ForEach(d => devices_id.Add(d.device_id));
            return devices_id;
        }

		public static void PushNotice(List<DeviceModel> devices, int service_id, string service_name, int notice_id, string notice_text) {
            var send = new
            {
                registration_ids = GetDevicesIdList(devices),
                notification = new
                {
                    body = notice_text,
                    title = "Notícia do Serviço " + service_name + " criada.",   
                    },
                    data = new
                    {
                        service_id = service_id,
                        service_name = service_name,
                        notice_id = notice_id,
                        notice_text = notice_text
                    }
                };
			PushNotification(send);
		}
		
		public static void PushCreatedEvent(List<DeviceModel> devices, int service_id, string service_name, string event_text, long event_begin) {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(event_begin);
            var str = dtDateTime.ToString();

            var send = new
            {
                registration_ids = GetDevicesIdList(devices),
                notification = new
                {
                    body = event_text+" no dia "+str,
                    title = "Evento do serviço " + service_name + " foi criado.",
                },
                data = new
                {
                    service_id = service_id,
                    service_name = service_name,
                    event_text = event_text,
                    event_begin = event_begin
                }
            };
            PushNotification(send);		
		}

        public static void PushDeletedEvent(List<DeviceModel> devices, int service_id, string service_name, string event_text, long event_begin)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(event_begin);
            var str = dtDateTime.ToString();

            var send = new
            {
                registration_ids = GetDevicesIdList(devices),
                notification = new
                {
                    body = event_text + " no dia " + str,
                    title = "Evento do serviço " + service_name + " foi cancelado.",
                },
                data = new
                {
                    service_id = service_id,
                    service_name = service_name,
                    event_text = event_text,
                    event_begin = event_begin
                }
            };
            PushNotification(send);
        }

        private static void PushNotification(object send)
        {
            try
            {                
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";                

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