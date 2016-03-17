using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BingWallPaper
{
    public class BackGroundTaskHelper
    {
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(string taskEntryPoint, string name,
            IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            
            UnregisterExistTask(name);

            await BackgroundExecutionManager.RequestAccessAsync();

            var builder = new BackgroundTaskBuilder
            {
                Name = name,
                TaskEntryPoint = taskEntryPoint,
            };
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);

                //
                // If the condition changes while the background task is executing then it will
                // be canceled.
                //
                builder.CancelOnConditionLoss = true;

            }

            var task = builder.Register();

           
            return task;
        }

        public static void UnregisterExistTask(string taskName)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == taskName)
                {
                    cur.Value.Unregister(true);

                }
            }
        }


    }

}
