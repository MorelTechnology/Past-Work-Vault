using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentService.Models
{
    public class NodesResult
    {
        public class Data
        {
        }

        public class Definitions
        {
        }

        public class CellMetadata
        {
            public Data data { get; set; }
            public Definitions definitions { get; set; }
        }

        public class Action
        {
            public string name { get; set; }
            public string url { get; set; }
            public List<string> children { get; set; }
            public string signature { get; set; }
        }

        public class Datum
        {
            public int volume_id { get; set; }
            public int id { get; set; }
            public int parent_id { get; set; }
            public string name { get; set; }
            public int type { get; set; }
            public string description { get; set; }
            public string create_date { get; set; }
            public string modify_date { get; set; }
            public bool reserved { get; set; }
            public int reserved_user_id { get; set; }
            public string reserved_date { get; set; }
            public string icon { get; set; }
            public string mime_type { get; set; }
            public int original_id { get; set; }
            public string type_name { get; set; }
            public bool container { get; set; }
            public int size { get; set; }
            public bool perm_see { get; set; }
            public bool perm_see_contents { get; set; }
            public bool perm_modify { get; set; }
            public bool perm_modify_attributes { get; set; }
            public bool perm_modify_permissions { get; set; }
            public bool perm_create { get; set; }
            public bool perm_delete { get; set; }
            public bool perm_delete_versions { get; set; }
            public bool perm_reserve { get; set; }
            public bool perm_add_major_version { get; set; }
            public CellMetadata cell_metadata { get; set; }
            public string menu { get; set; }
            public bool favorite { get; set; }
            public string size_formatted { get; set; }
            public string reserved_user_login { get; set; }
            public string action_url { get; set; }
            public string parent_id_url { get; set; }
            public List<Action> actions { get; set; }
        }

        public class Definitions2
        {
        }

        public class DefinitionsMap
        {
        }

        public class RootObject
        {
            public List<Datum> data { get; set; }
            public Definitions2 definitions { get; set; }
            public DefinitionsMap definitions_map { get; set; }
            public List<object> definitions_order { get; set; }
            public int limit { get; set; }
            public int page { get; set; }
            public int page_total { get; set; }
            public int range_max { get; set; }
            public int range_min { get; set; }
            public string sort { get; set; }
            public int total_count { get; set; }
            public List<string> where_facet { get; set; }
            public int where_name { get; set; }
            public List<int> where_type { get; set; }
        }
    }
}