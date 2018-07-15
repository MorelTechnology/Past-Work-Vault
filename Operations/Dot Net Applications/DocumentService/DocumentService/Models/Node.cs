using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentService.Models
{
    public class Node
    {
        public class Column
        {
            public int data_type { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string sort_key { get; set; }
        }

        public class DescriptionMultilingual
        {
            public string en { get; set; }
            public string de { get; set; }
        }

        public class NameMultilingual
        {
            public string en { get; set; }
            public string de { get; set; }
        }

        public class Properties
        {
            public bool container { get; set; }
            public int container_size { get; set; }
            public string create_date { get; set; }
            public int create_user_id { get; set; }
            public string description { get; set; }
            public DescriptionMultilingual description_multilingual { get; set; }
            public string external_create_date { get; set; }
            public string external_identity { get; set; }
            public string external_identity_type { get; set; }
            public string external_modify_date { get; set; }
            public string external_source { get; set; }
            public bool favorite { get; set; }
            public string guid { get; set; }
            public string icon { get; set; }
            public string icon_large { get; set; }
            public int id { get; set; }
            public string modify_date { get; set; }
            public int modify_user_id { get; set; }
            public string name { get; set; }
            public NameMultilingual name_multilingual { get; set; }
            public string owner { get; set; }
            public int owner_group_id { get; set; }
            public int owner_user_id { get; set; }
            public int parent_id { get; set; }
            public bool reserved { get; set; }
            public string reserved_date { get; set; }
            public int reserved_user_id { get; set; }
            public int type { get; set; }
            public string type_name { get; set; }
            public bool versions_control_advanced { get; set; }
            public int volume_id { get; set; }
        }

        public class Data
        {
            public List<Column> columns { get; set; }
            public Properties properties { get; set; }
        }

        public class RootObject
        {
            public Data data { get; set; }
        }
    }
}