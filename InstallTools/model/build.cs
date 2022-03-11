using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InstallTools.model
{
    class build
    {
    }

    public enum AuditEnum
    {
        [Description("未送审")]
        Holding = 0,

        [Description("审核中")]
        Auditing = 1,

        [Description("审核通过")]
        Pass = 2,

        [Description("驳回")]
        Reject = 3,
        
        
    }

    public class EnumService
    {
        public static string GetDescription(Enum obj)
        {
            string objName = obj.ToString();
            Type t = obj.GetType();
            FieldInfo fi = t.GetField(objName);

            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return arrDesc[0].Description;
        }
    }
}
