using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? keyword {get; set;} //ค้นหาห้อง
        public bool IsDecsending { get; set; } = false;//การจัดเรียงข้อมูล
        public int PageNumber { get; set; } = 1;//เลขหน้า
        public int PageSize { get; set; } = 20;//ชนาดข้อมูลต่อหน้า
    }
}