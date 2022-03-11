using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallTools.model
{
    //如果实体类名称和表名不一致可以加上SugarTable特性指定表名
    [SugarTable("map_floor")]
    class map_floor
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string id { set; get; }
        public string group_id { set; get; }
        public int order_num { set; get; }
        public string floor_name { set; get; }
        public string build_id { set; get; }
        ///public string model_url { set; get; }
        //public string center_position { set; get; }
        //public bool is_underground { set; get; }
        //public int rise_height { set; get; }
        //public string remark { set; get; }
    }
}
