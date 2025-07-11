using System;
using System.Threading.Tasks;

namespace EFK2.Extensions.Tasks
{
    public static class TaskSugar
    {
        public static Task Delay(float time)
        {
            return Task.Delay(TimeSpan.FromSeconds(time));
        }
        
        public static Task Seconds(this float time)
        {
            return Task.Delay(TimeSpan.FromSeconds(time));
        }
    }
}