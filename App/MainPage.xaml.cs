using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RegisterBackgroundTask();
        }

        /// <summary>
        /// 用TaskBuilder注册BackgroundTask，此外还需要解决未文档的bug：
        ///     If it is not an audio background task, 
        ///     it is not allowed to have EntryPoint="BackgroundTask.MyBackgroundTask" 
        ///     without ActivatableClassId in windows.activatableClass.inProcessServer.
        /// 参考：https://developercommunity.visualstudio.com/content/problem/606866/uwp-solution-with-timer-backgroundtask-no-longer-b.html
        /// 另外，除了在AppManifest中声明，用TaskBuilder注册外，还需要在主项目中添加对后台组件的Reference，
        /// 这似乎是文档中忽略未讲的内容。
        /// </summary>
        private async void RegisterBackgroundTask()
        {
            const string taskName = "MyBackgroundTask";
            const string taskEntryPoint = "BackgroundTask.MyBackgroundTask";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }
    }
}
