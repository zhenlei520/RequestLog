// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace RequestLog.Extension
{
    /// <summary>
    /// 工具类
    /// </summary>
    internal static class Utils
    {
        #region 得到Ticks

        /// <summary>
        /// 得到Ticks
        /// </summary>
        /// <returns></returns>
        public static long ToTicks(this DateTime target)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(target).Ticks - TimeZoneInfo
                .ConvertTimeToUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).Ticks);
        }

        #endregion

        #region 得到13位时间戳

        /// <summary>
        /// 得到13位时间戳
        /// </summary>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime target)
        {
            return ToTicks(target) / 10000;
        }

        #endregion

        #region 返回枚举项的描述信息

        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    if (Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }

        #endregion

        #region 是否是控制器

        /// <summary>
        /// 是否是控制器
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (!typeInfo.IsPublic)
            {
                return false;
            }

            return !typeInfo.ContainsGenericParameters
                   && typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 根据KeyValuePair集合得到对象

        /// <summary>
        /// 根据KeyValuePair集合得到对象
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        internal static object GetData(List<KeyValuePair<string, string>> keyValuePairs)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            foreach (var item in keyValuePairs)
            {
                ((IDictionary<string, object>) data).Add(item.Key, item.Value);
            }

            return data;
        }

        #endregion
    }
}