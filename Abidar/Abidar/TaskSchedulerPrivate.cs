using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

namespace Abidar
{
    public partial class TaskScheduler
    {
        private void Initialize()
        {
            _tasks = new List<Task>();

            foreach (XmlNode node in this._nodes)
            {
                if (node.Name == "Task")
                {
                    try
                    {
                        XmlAttributeCollection attributes = node.Attributes;

                        if (bool.Parse(attributes["enabled"].Value))
                        {
                            Task task = new Task(double.Parse(attributes["interval"].Value));

                            task.Name = attributes["name"].Value;
                            // In some cases user might use an assembly in different path
                            // Check if it has "assemblyPath" attributes
                            if (attributes["assemblyPath"] != null)
                            {
                                Assembly targetAssembly = Assembly.LoadFrom(attributes["assemblyPath"].Value);
                                task.TaskType = targetAssembly.GetType(attributes["type"].Value, true);
                            }
                            else
                            {
                                //Load assembly in current path or global assembly cache
                                task.TaskType = Type.GetType(attributes["type"].Value, true);
                            }
                            task.Enabled = bool.Parse(attributes["enabled"].Value);
                            task.Priority = (Priority)Convert.ToInt16(attributes["priority"].Value);
                            task.ConfigurationNode = node;

                            _tasks.Add(task);
                        }
                    }
                    catch
                    {
                        // Handle the exception
                        // Usually log a warning in event log
                    }
                }
            }
        }
    }
}
