namespace PS_project.Utils.DB
{
    public class DB_QueryStrings
    {
        public const string GET_USER = "select * from Users where email=@email";
        public const string GET_SERVICE_PROVIDER = "select * from ServiceProvider where email=@provider_email";
        public const string GET_SERVICE_USER = "select * from ServiceUser where email=@user_email";
        public const string GET_SERVICE_WITH_ID = "select * from service where id=@service_id";
        public const string GET_SERVICE_WITH_EMAIL = "select * from service where provider_email=@email";
        public const string GET_TOTAL_SERVICE_SUBSCRIBERS = "select count(*) from Subscriber where service_id=@serv_id";
        public const string GET_SERVICE_AVERAGE_RANKING = "select avg(value) from Ranking where service_id=@serv_id";
        public const string GET_SERVICE_EVENTS = "select * from event where service_id=@id and event_date>=GETUTCDATE()";
        public const string GET_SERVICE_NOTICES = "select * from notice where service_id=@id";
        public const string GET_SERVICE_RANKINGS = "select * from ranking where service_id=@id";
        public const string GET_SERVICE_TYPES = "select * from ServiceType";
        public const string GET_SUBSCRIBED_SERVICES = "select * from Service inner join Subscriber on Service.id=Subscriber.service_id where Subscriber.user_email=@email";
        public const string GET_SERVICES_BY_TYPE = "select * from Service where service_type=@type";
        public const string GET_USER_REGISTRED_DEVICES = "select device_id from RegistredDevices where email=@email";
        public const string GET_SERVICE_SUBSCRIBED_DEVICES = "select device_id from Subscriber left join RegistredDevices on Subscriber.user_email=RegistredDevices.email where Subscriber.service_id=@id";
        
        public const string SELECT_SCOPE_IDENTITY = "SELECT SCOPE_IDENTITY()";

        public const string INSERT_USER = "insert into Users values(@email,@password,@salt)";
        public const string INSERT_SERVICE_PROVIDER = "insert into ServiceProvider values(@email)";
        public const string INSERT_SERVICE_USER = "insert into ServiceUser values(@email,@name)";
        public const string INSERT_DEVICE_REGISTRATION = "insert into RegistredDevices values(@device_id,@email)";
        public const string INSERT_SERVICE = "insert into Service(provider_email,name,contact_number,contact_name,contact_location,service_type) values(@provider_email,@name,@contact_number,@contact_name,@contact_location,@service_type)";
        public const string INSERT_NOTICE = "insert into Notice(service_id,text,creation_date) values(@serv_id,@notice_text,GETUTCDATE())";
        public const string INSERT_EVENT = "insert into Event(service_id,text,creation_date,event_date) values(@serv_id,@event_text,GETUTCDATE(),@event_date)";
        public const string INSERT_RANKING = "insert into Ranking(user_email,service_id,value,text,creation_date) values(@user_email,@serv_id,@value,@text,GETUTCDATE())";
        public const string INSERT_SUBSCRIPTION = "insert into Subscriber(user_email,service_id) values(@user_email,@serv_id)";

        public const string UPDATE_USER_PASSWORD = "update Users set password=@pass where email=@provider_email";
        public const string UPDATE_SERVICE_INFO = "update service set name=@serv_name,contact_number=@c_num,contact_name=@c_name,contact_location=@c_loc where id=@serv_id";
        public const string UPDATE_USER_INFO = "update ServiceUsers set name=@user_name where email=@user_email";

        public const string DELETE_NOTICE = "delete from Notice where service_id=@serv_id and id=@notice_id";
        public const string DELETE_EVENT = "delete from Event where service_id=@serv_id and id=@event_id";
        public const string DELETE_SUBSCRIPTION = "delete from Subscriber where service_id=@serv_id and user_email=@email";
        public const string DELETE_RANKING = "delete from Ranking where service_id=@serv_id and user_email=@email";
    }
}