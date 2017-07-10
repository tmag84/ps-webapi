namespace PS_project.Models
{
    public class DeviceModel
    {
        public string device_id { get; set; }
        public string email { get; set; }
        public int last_used { get; set; }

        public override string ToString()
        {
            return device_id;
        }
    }
}