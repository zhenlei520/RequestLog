// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RequestLog.Internal
{
    /// <summary>
    /// 批处理帮助类
    /// </summary>
    internal class BatchCommon<T> : IDisposable
    {
        private Queue<T> _buffer;
        private DateTime _lastSaveTime;
        private readonly int _batchRowNum;

        /// <summary>
        /// 时间间隔，默认1000ms，0为不执行定时任务上传日志
        /// </summary>
        public readonly int _timeInterval;

        private readonly object _lockbuffer;

        private readonly Func<List<T>,Task> _action;

        /// <summary>
        ///
        /// </summary>
        internal BatchCommon()
        {
            this._buffer = new Queue<T>();
            this._lastSaveTime = DateTime.Now;
            this._batchRowNum = 300;
            this._timeInterval = 1000;
            this._lockbuffer = new object();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="batchRowNum">批处理数量</param>
        /// <param name="timeInterval">时间间隔，默认1000ms，0为不执行定时任务上传日志</param>
        /// <param name="action"></param>
        internal BatchCommon(int batchRowNum, int timeInterval, Func<List<T>,Task> action) : this()
        {
            this._batchRowNum = batchRowNum;
            this._timeInterval = timeInterval;
            this._action = action;
            if (this._timeInterval > 0)
            {
                Thread myThread = new Thread(this.ExecuteJob)
                {
                    IsBackground = true
                };
                myThread.Start();
            }
            else if (this._timeInterval < 0)
            {
                throw new Exception("时间间隔配置错误");
            }
        }

        #region 添加任务

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="t"></param>
        internal void AddJob(T t)
        {
            if (t == null)
            {
                return;
            }

            lock (this._lockbuffer)
            {
                this._buffer.Enqueue(t);
            }

            if (this._buffer.Count > this._batchRowNum)
            {
                lock (this._lockbuffer)
                {
                    if (this._buffer.Count > this._batchRowNum)
                    {
                        this.Execute(this._batchRowNum);
                    }
                }
            }
        }

        #endregion

        #region 执行任务

        /// <summary>
        /// 执行任务
        /// </summary>
        private void ExecuteJob()
        {
            while (true)
            {
                if (this._buffer.Count > 0 && DateTime.Now.AddMilliseconds(-this._timeInterval) > this._lastSaveTime)
                {
                    lock (this._lockbuffer)
                    {
                        if (this._buffer.Count > 0 && DateTime.Now.AddMilliseconds(-this._timeInterval) > this._lastSaveTime)
                        {
                            this.Execute(this._buffer.Count);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(this._timeInterval);
                }
            }
        }

        #endregion

        #region 执行

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="count">执行的任务数量</param>
        private void Execute(int count)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (this._buffer.Count > 0)
                    {
                        list.Add(this._buffer.Dequeue());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("执行任务失败：" + ex.Message);
                }
            }

            this._action.Invoke(list);
            this._lastSaveTime = DateTime.Now;
        }

        #endregion

        #region 释放

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            lock (this._lockbuffer)
            {
                this.Execute(this._buffer.Count);
            }
        }

        #endregion
    }
}
