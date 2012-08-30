using System.Collections.Generic;
using System.Xml;
using System.Net;

namespace Abidar
{
    public partial class TaskScheduler
    {
        public static List<Task> _tasks = null;
        private XmlNodeList _nodes = null;

        public TaskScheduler(XmlNodeList nodes)
        {
            this._nodes = nodes;
            Initialize();
        }

        public void StartTasks()
        {
            foreach (Task task in _tasks)
            {
                if (!task.IsRunning)
                    task.Start();
            }
        }

        public void StopTasks()
        {
            foreach (Task task in _tasks)
            {
                task.Stop();
            }
        }

        /// <summary>
        /// Use this method in your Application_End event to prevent recycling your application
        /// </summary>
        /// <param name="keepAliveUrl">Url to keep alive your application</param>
        public void PingBack(string keepAliveUrl)
        {
            try
            {
                WebClient http = new WebClient();
                string Result = http.DownloadString(keepAliveUrl);
            }
            catch (System.Exception ex)
            {
                string Message = ex.Message;
            }
        }
    }
}
