using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface INotificationService
    {
        void ShowLocalNotification(string title, string text, DateTime time);
    }
}
