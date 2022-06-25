namespace BlazorApp.Data
{
    public class commodityItem
    {
        public int commodity_id { get; set; }

        public int user_id { get; set; }

        public string commodity_name { get; set; } = null!;

        public int item_price { get; set; }

        public int total_count { get; set; }

        public string start_time { get; set; }

        public string end_time { get; set; }
    }
}
