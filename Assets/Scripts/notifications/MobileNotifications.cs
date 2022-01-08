using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

[System.Serializable]
public struct WorkoutNotificationStruct
{
    public string NotificationTitle, NotificationDescription;
}

public class MobileNotifications : MonoBehaviour
{
    [SerializeField]
    WorkoutNotificationStruct [] workoutNotifications;

    int notificationIndex;

    // Start is called before the first frame update
    void Start()
    {
        //remove notifications that have already been displayed
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        //create the android notification channel to send message through
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.High,
            Description = "Reminder notification",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        /*
          "Ready for a quick and easy workout?";
        notification.Text = "Get your workout clothes on and let the dice decide your workout!";
         */
        //setup of notifications that is going to be sent
        var notification = new AndroidNotification();
        notificationIndex = Random.Range(0,workoutNotifications.Length);
        notification.Title = workoutNotifications[notificationIndex].NotificationTitle;
        notification.Text = workoutNotifications[notificationIndex].NotificationDescription;
        notification.ShowTimestamp = true;
        notification.FireTime = System.DateTime.Now.AddHours(12);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        //if the script has run and a message is already scheduled, cancel it and re-schedule another message
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
