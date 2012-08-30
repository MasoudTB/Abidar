using System;
using System.Reflection;
using System.Threading;
using System.Timers;

namespace Abidar
{
    public partial class Task
    {
        private void Initialize()
        {
            this.Stopped = false;
            this.Enabled = true;

            timer = new System.Timers.Timer(this.Interval);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        private void StartTask()
        {
            if (!this.Stopped)
            {
                Thread thread = new Thread(new ThreadStart(Execute));
                thread.Start();
            }
        }

        private void Execute()
        {
            try
            {
                this.IsRunning = true;

                this.LastRunTime = DateTime.Now;

                //We are sure that TaskType is ITask so why we need to call using Invoke?
                ITask targetTask = (ITask) Activator.CreateInstance(this.TaskType);
                targetTask.Execute(this.ConfigurationNode);

                #region Commented
                //MethodInfo method = this.TaskType.GetMethod("Execute");
                //object[] arguments = { this.ConfigurationNode };
                //object obj = Activator.CreateInstance(this.TaskType);
                //method.Invoke(obj, new object[] { this.ConfigurationNode });
                #endregion

                this.IsLastRunSuccessful = true;
            }
            catch
            {
                this.IsLastRunSuccessful = false;
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!this.IsRunning)
                StartTask();
        }
    }
}
