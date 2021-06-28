// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace RequestLog.Serialize
{
    /// <summary>
    /// Json 序列化基础类库（后期可通过自定义实现更改默认序列化类库）
    /// </summary>
    public interface IJsonProvider
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="format"></param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        string Serializer(object o, bool format = false, Func<System.Exception, string> action = null);

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        object Deserialize(string s, Type type, Func<System.Exception, object> action = null);

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="defaultResult">反序列化异常</param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        T Deserialize<T>(string s, T defaultResult = default(T), Action<System.Exception> action = null)
            where T : class, new();
    }
}
