using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.BTD6.util
{
    public static class BloonsUtils
    {
        public static Dictionary<string, string> BloonsName = new() {
            {"red", "红气球"},
            {"blue", "蓝气球"},
            {"green", "绿气球"},
            {"yellow", "黄气球"},
            {"pink", "粉气球"},
            {"black", "黑气球"},
            {"white", "白气球"},
            {"purple", "紫气球"},
            {"lead", "铅气球"},
            {"zebra", "斑马气球"},
            {"rainbow", "彩虹气球"},
            {"ceramic", "陶瓷气球"},
            {"moab", "MOAB气球"},
            {"bfb", "BFB气球"},
            {"zomg", "ZOMG气球"},
            {"ddt", "DDT气球"},
            {"c-ddt", "DDT气球"},
            {"bad", "BAD气球"},
        };
        public static Dictionary<string, string> BloonsAttribute = new() {
            {"c", "迷彩"},
            {"r", "重生"},
            {"f", "加固"},
        };
        public static string Translate(string bloon)
        {
            if (bloon.Contains("-") && bloon != "c-ddt")
            {
                string[] tmp = bloon.Split('-');
                string Attribute = tmp[0];
                string Name = tmp[1];
                return TranslateAttribute(Attribute) + TranslateName(Name);
            }
            else
            {
                return TranslateName(bloon);
            }
        }
        public static string TranslateName(string bloon)
        {
            return BloonsName[bloon.ToLower()];
        }
        public static string TranslateAttribute(string attributes)
        {
            string tmp = string.Empty;
            foreach(char c in attributes)
            {
                tmp += BloonsAttribute[c.ToString()];
            }
            return tmp;
        }
    }
}
