﻿// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json;

namespace RequestLog.Serialize
{
    /// <summary>
    ///
    /// </summary>
    internal class NewtonsoftJsonProvider : IJsonProvider
    {
        /// <summary>
        ///
        /// </summary>
        public NewtonsoftJsonProvider()
        {
        }

        #region json序列化

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="format"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string Serializer(object o, bool format = false, Func<Exception, string> action = null)
        {
            try
            {
                if (o == null)
                {
                    return string.Empty;
                }

                using (StringWriter sw = new StringWriter())
                {
                    JsonSerializer serializer = JsonSerializer.Create(
                        new JsonSerializerSettings
                        {
                            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }
                    );
                    serializer.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    JsonWriter jsonWriter;
                    if (format)
                    {
                        jsonWriter = new JsonTextWriter(sw)
                        {
                            Formatting = Formatting.Indented,
                            Indentation = 4,
                            IndentChar = ' '
                        };
                    }
                    else
                    {
                        jsonWriter = new JsonTextWriter(sw);
                    }

                    using (jsonWriter)
                    {
                        serializer.Serialize(jsonWriter, o);
                    }

                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                if (action != null)
                {
                    return action.Invoke(ex);
                }

                throw new System.Exception($"json序列化出错,序列化类型：{o.GetType().FullName}");
            }
        }

        #endregion

        #region json反序列化

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <param name="func">委托方法</param>
        /// <returns></returns>
        public object Deserialize(string str, Type type, Func<Exception, object> func = null)
        {
            try
            {
                return JsonConvert.DeserializeObject(str, type);
            }

            catch (System.Exception ex)
            {
                if (func != null)
                {
                    return func.Invoke(ex);
                }

                throw;
            }
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="defaultResult">反序列化异常</param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        public T Deserialize<T>(string str, T defaultResult = default(T), Action<Exception> action = null)
            where T : class, new()
        {
            try
            {
                object obj = Deserialize(str, typeof(T), (exception =>
                {
                    action?.Invoke(exception);
                    return defaultResult;
                }));
                return (T) obj;
            }
            catch (Exception ex)
            {
                action?.Invoke(ex);
                return defaultResult;
            }
        }

        #endregion

        #region 返回权重

        /// <summary>
        /// 返回权重
        /// </summary>
        /// <returns></returns>
        public int GetWeights()
        {
            return 99;
        }

        #endregion
    }
}
