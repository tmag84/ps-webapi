using Drum;
using WebApi.Hal;
using PS_project.Models;
using PS_project.Controllers;
using System.Collections.Generic;

namespace PS_project.Utils
{
    public class HrefBuilders
    {
        public static void BuildServiceHrefs(UriMaker<ProviderController> uriMaker, ProviderResponseModel pr)
        {
            ServiceModel service = pr.service;
            service.Href = uriMaker.UriFor(c => c.GetService()).AbsolutePath;
            service.Links.Add(new Link("create-event",uriMaker.UriFor(c => c.CreateEvent(null)).AbsolutePath));
            service.Links.Add(new Link("create-notice", uriMaker.UriFor(c => c.CreateNotice(null)).AbsolutePath));

            foreach (var ev in service.service_events)
            {
                ev.Links.Add(new Link("delete-event", uriMaker.UriFor(c => c.DeleteEvent(ev)).AbsolutePath));
            }

            foreach (var notice in service.service_notices)
            {
                notice.Links.Add(new Link("delete-notice", uriMaker.UriFor(c => c.DeleteNotice(notice)).AbsolutePath));
            }
        }

        public static void BuildSubscriptionsHrefs(UriMaker<UserController> uriMaker, UserResponseModel user_info)
        {
            foreach (ServiceModel service in user_info.services)
            {
                service.subscribed = true;
                SubscriptionModel sub = new SubscriptionModel();
                sub.id = service.id;
                service.Links.Add(new Link("remove-subscription",uriMaker.UriFor(c => c.RemoveSubscription(sub)).AbsoluteUri));
                service.Links.Add(new Link("add-rank", uriMaker.UriFor(c => c.CreateRanking(null)).AbsolutePath));
            }
        }

        public static void BuildSearchServicesHrefs(UriMaker<UserController> uriMaker, List<ServiceModel> services, UserResponseModel user_info)
        {
            foreach (ServiceModel service in services)
            {
                foreach(ServiceModel serv in user_info.services)
                {
                    if (serv.id == service.id)
                    {
                        service.subscribed = true;
                    }
                }

                SubscriptionModel sub = new SubscriptionModel();
                sub.id = service.id;
                service.Links.Add(new Link("add-subscription", uriMaker.UriFor(c => c.AddSubscription(sub)).AbsoluteUri));
                service.Links.Add(new Link("remove-subscription", uriMaker.UriFor(c => c.RemoveSubscription(sub)).AbsoluteUri));
            }
        }
    }
}