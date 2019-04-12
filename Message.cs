using Dapper.Contrib.Extensions;

namespace voxie_message_seeder {

    [Table ("messages")]
    public class Message {
        public int id { get; set; }
        public int team_id { get; set; } = 42;
        public string vendor_identity { get; set; }
        public string vendor { get; set; }
        public string type { get; set; } = "Inbound";
        public string status { get; set; } = "sent";
        public int source_id { get; set; } = 1;
        public string source_type { get; set; } = "source";
        public int origin_id { get; set; } = 1;
        public string origin_type { get; set; } = "origin";
        public int contact_id { get; set; }
        public string from { get; set; } = "123";
        public string to { get; set; } = "456";
        public string body { get; set; } = "Hi there";
        public string media_url { get; set; }
        public object error_code { get; set; }
        public object error_message { get; set; }
        public string direction { get; set; } = "inbound";
        public int read { get; set; } = 1;
        public int archived { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int sent_on_trial { get; set; }
        public int ignore_origin { get; set; }
        public object num_segments { get; set; }
    }
}