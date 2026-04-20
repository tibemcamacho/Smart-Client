
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Elipgo.SmartClient.Common.Enum
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AlarmTypeOf : Attribute
    {
        public AlarmTypeOf(GroupAlarmType alt)
        {
            AlarmType = alt;
        }
        public GroupAlarmType AlarmType { get; private set; }
    }

    public enum GroupAlarmType
    {
        [Description("auth.app.alarms.GroupAlarmTypeAVL")]
        AVL,
        [Description("auth.app.alarms.GroupAlarmTypeKpi")]
        KPI,
        [Description("auth.app.alarms.GroupAlarmTypeAnalytics")]
        ANALYTICS,
        [Description("auth.app.alarms.GroupAlarmTypeMotion")]
        MOTION,
        [Description("auth.app.alarms.GroupAlarmTypeLpr")]
        LPR,
        [Description("auth.app.alarms.GroupAlarmTypeFace")]
        FACE,
        [Description("auth.app.alarms.GroupAlarmTypeCustom")]
        CUSTOM,
        [Description("auth.app.alarms.GroupAlarmTypeGPS")]
        GPS,
        [Description("auth.app.alarms.GroupAlarmTypeAccessControl")]
        ACCESS_CONTROL,
        [Description("auth.app.alarms.GroupAlarmTypePanicButton")]
        PANIC_BUTTON,
        [Description("auth.app.alarms.GroupAlarmTypeAVL")]
        OCR
    }

    public enum PanicButtonType
    {
        [Description("Físico")]
        F = 1,
        [Description("Virtual")]
        V = 2
    }
    public enum ElementType
    {
        None,
        [Display(Name = "Camera")]
        Camera,
        Iot,
        [Display(Name = "Kpi")]
        Kpi,
        [Display(Name = "Face")]
        Face,
        [Display(Name = "Lpr")]
        Lpr,
        Geomap,
        [Display(Name = "Carousel")]
        Carousel,
        Blueprint,
        [Display(Name = "Iot in")]
        Iot_In,
        [Display(Name = "Iot out")]
        Iot_Out,
        [Display(Name = "Location")]
        Location,
        [Display(Name = "Alarms Map")]
        AlarmsMap,
        [Display(Name = "Geolocation Alarm")]
        Geolocation_Alarm
    }

    public enum AlarmType
    {

        [AlarmTypeOf(GroupAlarmType.ACCESS_CONTROL)]
        [Display(Name = "ACCESS_CONTROL")]
        [Translation("ACCESS_CONTROL", typeof(Resources))]
        ACCESS_CONTROL,

        [AlarmTypeOf(GroupAlarmType.ACCESS_CONTROL)]
        [Display(Name = "Evento Suprema")]
        [Translation("Evento_Suprema", typeof(Resources))]
        Evento_Suprema,

        [AlarmTypeOf(GroupAlarmType.ACCESS_CONTROL)]
        [Display(Name = "Zkteco")]
        [Translation("Zkteco", typeof(Resources))]
        Zkteco,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Abandoned Object")]
        [Translation("ABANDONED_OBJECT", typeof(Resources))]
        ABANDONED_OBJECT,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Crowd Density")]
        [Translation("CROWD_DENSITY", typeof(Resources))]
        CROWD_DENSITY,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Crowd Gathering Estimation")]
        [Translation("CROWD_GATHERING_ESTIMATION", typeof(Resources))]
        CROWD_GATHERING_ESTIMATION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Direction")]
        [Translation("DIRECTION", typeof(Resources))]
        DIRECTION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Enter Exit")]
        [Translation("ENTER_EXIT", typeof(Resources))]
        ENTER_EXIT,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Face")]
        [Translation("FACE_ANALYTICS", typeof(Resources))]
        FACE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Humidity")]
        [Translation("HUMIDITY", typeof(Resources))]
        HUMIDITY,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Intrusion")]
        [Translation("INTRUSION", typeof(Resources))]
        INTRUSION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Left Object")]
        [Translation("LEFT_OBJECT", typeof(Resources))]
        LEFT_OBJECT,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Loitering")]
        [Translation("LOITERING", typeof(Resources))]
        LOITERING,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Loitering Detection")]
        [Translation("LOITERING_DETECTION", typeof(Resources))]
        LOITERING_DETECTION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Missing Object")]
        [Translation("MISSING_OBJECT", typeof(Resources))]
        MISSING_OBJECT,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Object Taken")]
        [Translation("OBJECT_TAKEN", typeof(Resources))]
        OBJECT_TAKEN,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Dwell")]
        [Translation("OCCUPATION_TIME", typeof(Resources))]
        OCCUPATION_TIME,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Proximity")]
        [Translation("PROXIMITY", typeof(Resources))]
        PROXIMITY,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Queue")]
        [Translation("QUEUE", typeof(Resources))]
        QUEUE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Speed Detection")]
        [Translation("SPEED_DETECTION", typeof(Resources))]
        SPEED_DETECTION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Tampering")]
        [Translation("TAMPERING", typeof(Resources))]
        TAMPERING,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Temperature")]
        [Translation("TEMPERATURE", typeof(Resources))]
        TEMPERATURE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Tripwire")]
        [Translation("TRIPWIRE", typeof(Resources))]
        TRIPWIRE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Video Tampering")]
        [Translation("VIDEO_TAMPERING", typeof(Resources))]
        VIDEO_TAMPERING,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Water Detection")]
        [Translation("WATER_DETECTION", typeof(Resources))]
        WATER_DETECTION,

        [AlarmTypeOf(GroupAlarmType.AVL)]
        [Display(Name = "AVL")]
        [Translation("AVL", typeof(Resources))]
        AVL,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Alerta")]
        [Translation("Generic_Alert", typeof(Resources))]
        Generic,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Impact")]
        [Translation("IMPACT", typeof(Resources))]
        IMPACT,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Local Alarm, Local Alarm Start")]
        [Translation("INPUT_LOCAL_ALARM", typeof(Resources))]
        INPUT,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Local Alarm Clear, INPUT_RESTORE")]
        [Translation("INPUT_RESTORE", typeof(Resources))]
        INPUT_RESTORE,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "INPUT")]
        [Translation("INPUTS", typeof(Resources))]
        INPUTS,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Manufacture")]
        [Translation("MANUFACTURE", typeof(Resources))]
        MANUFACTURE,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "OFFLINE")]
        [Translation("OFFLINE", typeof(Resources))]
        OFFLINE,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "OPEN")]
        [Translation("OPEN_CUSTOM", typeof(Resources))]
        OPEN,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Parking Detection")]
        [Translation("PARKING_DETECTION", typeof(Resources))]
        PARKING_DETECTION,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Video Call")]
        [Translation("VIDEO_CALL", typeof(Resources))]
        VIDEO_CALL,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "Video Guard")]
        [Translation("VIDEO_GUARD", typeof(Resources))]
        VIDEO_GUARD,

        [AlarmTypeOf(GroupAlarmType.FACE)]
        [Display(Name = "Face Detection, Stranger Alarm, Face Recognition(Stranger Alarm)")]
        [Translation("FACE_DETECTION", typeof(Resources))]
        FACE_DETECTION,

        [AlarmTypeOf(GroupAlarmType.FACE)]
        [Display(Name = "Face Recognition, General Alarm, Face Recognition(General Alarm)")]
        [Translation("FACE_RECOGNITION", typeof(Resources))]
        FACE_RECOGNITION,

        [AlarmTypeOf(GroupAlarmType.GPS)]
        [Display(Name = "GPS")]
        [Translation("GPS", typeof(Resources))]
        GPS,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "AIR_QUALITY")]
        [Translation("AIR_QUALITY", typeof(Resources))]
        AIR_QUALITY,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Air Quality")]
        [Translation("KPI_AIR_QUALITY", typeof(Resources))]
        KPI_AIR_QUALITY,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Air Quality Instant")]
        [Translation("KPI_AIR_QUALITY_INSTANT", typeof(Resources))]
        KPI_AIR_QUALITY_INSTANT,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Air Quality Peak")]
        [Translation("KPI_AIR_QUALITY_PEAK", typeof(Resources))]
        KPI_AIR_QUALITY_PEAK,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Humidity")]
        [Translation("KPI_HUMIDITY", typeof(Resources))]
        KPI_HUMIDITY,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Occupancy")]
        [Translation("KPI_OCCUPANCY", typeof(Resources))]
        KPI_OCCUPANCY,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Queue")]
        [Translation("KPI_QUEUE", typeof(Resources))]
        KPI_QUEUE,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Rate")]
        [Translation("KPI_RATE", typeof(Resources))]
        KPI_RATE,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Temperature")]
        [Translation("KPI_TEMPERATURE", typeof(Resources))]
        KPI_TEMPERATURE,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Temperature Instant")]
        [Translation("KPI_TEMPERATURE_INSTANT", typeof(Resources))]
        KPI_TEMPERATURE_INSTANT,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Temperature Peak")]
        [Translation("KPI_TEMPERATURE_PEAK", typeof(Resources))]
        KPI_TEMPERATURE_PEAK,

        [AlarmTypeOf(GroupAlarmType.KPI)]
        [Display(Name = "KPI Visitors")]
        [Translation("KPI_VISITORS", typeof(Resources))]
        KPI_VISITORS,

        [AlarmTypeOf(GroupAlarmType.LPR)]
        [Display(Name = "LPR")]
        [Translation("LPR_ALARM", typeof(Resources))]
        LPR,

        [AlarmTypeOf(GroupAlarmType.MOTION)]
        [Display(Name = "Motion, Motion Detect, Motion Detection, MOTION_ALERT, SMD(Human) end")]
        [Translation("MOTION", typeof(Resources))]
        MOTION,

        [AlarmTypeOf(GroupAlarmType.MOTION)]
        [Display(Name = "SMD(Vehicle)")]
        [Translation("MOTION_DETECTION_HUMAN_VEHICLE", typeof(Resources))]
        MOTION_DETECTION_HUMAN_VEHICLE,
        [AlarmTypeOf(GroupAlarmType.MOTION)]
        [Display(Name = "SMD(Human)")]
        [Translation("MOTION_DETECTION_HUMAN", typeof(Resources))]
        MOTION_DETECTION_HUMAN,

        [AlarmTypeOf(GroupAlarmType.OCR)]
        [Display(Name = "OCR")]
        [Translation("OCR", typeof(Resources))]
        OCR,

        [AlarmTypeOf(GroupAlarmType.PANIC_BUTTON)]
        [Display(Name = "Panic Button")]
        [Translation("PANIC_BUTTON", typeof(Resources))]
        PANIC_BUTTON,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Open Door")]
        [Translation("OPEN_DOOR", typeof(Resources))]
        OPEN_DOOR,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "People Counting")]
        [Translation("PEOPLE_COUNTING", typeof(Resources))]
        PEOPLE_COUNTING,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Permanence People")]
        [Translation("PERMANENCE_PEOPLE", typeof(Resources))]
        PERMANENCE_PEOPLE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "BODY_POSITION")]
        [Translation("BODY_POSITION", typeof(Resources))]
        BODY_POSITION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "CROSSING_VIRTUAL_FENCE")]
        [Translation("CROSSING_VIRTUAL_FENCE", typeof(Resources))]
        CROSSING_VIRTUAL_FENCE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "Crowd Gathering")]
        [Translation("CROWD_GATHERING", typeof(Resources))]
        CROWD_GATHERING,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "WEAPON_DETECTION")]
        [Translation("WEAPON_DETECTION", typeof(Resources))]
        WEAPON_DETECTION,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "CHANGE_SCENE")]
        [Translation("CHANGE_SCENE", typeof(Resources))]
        CHANGE_SCENE,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "OBJECT_ON_FACE")]
        [Translation("OBJECT_ON_FACE", typeof(Resources))]
        OBJECT_ON_FACE,

        [AlarmTypeOf(GroupAlarmType.CUSTOM)]
        [Display(Name = "VIN")]
        [Translation("VIN", typeof(Resources))]
        VIN,

        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "FIRE")]
        [Translation("FIRE", typeof(Resources))]
        FIRE,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "EPP")]
        [Translation("EPP", typeof(Resources))]
        EPP,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "PHONE DETECTION")]
        [Translation("PHONE_DETECTION", typeof(Resources))]
        PHONE_DETECTION,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "CROUCH DETECTION")]
        [Translation("CROUCH_DETECTION", typeof(Resources))]
        CROUCH_DETECTION,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "BROKEN GLASS")]
        [Translation("BROKEN_GLASS", typeof(Resources))]
        BROKEN_GLASS,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "VANDALIZED ATM")]
        [Translation("VANDALIZED_ATM", typeof(Resources))]
        VANDALIZED_ATM,
        [AlarmTypeOf(GroupAlarmType.ANALYTICS)]
        [Display(Name = "VANDALIZED CAMERA")]
        [Translation("VANDALIZED_CAMERA", typeof(Resources))]
        VANDALIZED_CAMERA

    }

    public enum GroupType
    {
        None,

        [Display(Name = "Live")]
        LIVE,

        [Display(Name = "Carrousel")]
        CARROUSEL,

        [Display(Name = "Mobile")]
        MOBILE,

        [Display(Name = "Playback")]
        PLAYBACK
    }

    public static class TypeAlarms
    {
        public static readonly Dictionary<string, string> EnumAlarmsCluster = new Dictionary<string, string>
        {
            { "INPUT",  "INPUT, DIGITAL_INPUT"},
            { "INPUT_RESTORE",  "INPUT_RESTORE, INPUT_RESTORES"},
            { "MOTION",  "MOTION, MOTION_DETECT, MOTION_DETECTION, MOTION_DETECTED, MOTION_DETECTION_HUMAN, MOTION_DETECTION_HUMAN_END, MOTION_DETECTION_HUMAN_VEHICLE"},
         };

        public static List<string> GetDisplayNames()
        {
            List<string> result = new List<string>();
            foreach (var item in System.Enum.GetValues(typeof(AlarmType)))
            {
                var eType = (AlarmType)item;
                var itemEnum = eType.GetAttribute<DisplayAttribute>().Name;

                // Si hay más de un nombre separado por coma, solo toma el primero
                var displayName = itemEnum.Contains(",")
                    ? itemEnum.Split(',')[0].Trim()
                    : itemEnum.Trim();

                result.Add(displayName);
            }
            return result;
        }

        public enum SidebarElement
        {
            all,
            devices,
            Analytics,
            Carousels,
            Locations,
            GeoAlarms,
            Groups,
            Playback

        }
        public static string GetDisplayName(AlarmType value)
        {
            var itemEnum = value.GetAttribute<DisplayAttribute>().Name;
            var displayName = itemEnum.Contains(",")
                    ? itemEnum.Split(',')[0].Trim()
                    : itemEnum.Trim();

            return displayName;
        }

        public static AlarmType ConvertEnumAlarmType(string enumAlarmType)
        {
            if (string.IsNullOrWhiteSpace(enumAlarmType))
                return AlarmType.Generic;

            foreach (var kvp in EnumAlarmsCluster)
            {
                var values = kvp.Value.Split(',').Select(x => x.Trim()).ToHashSet();
                if (values.Contains(enumAlarmType))
                {
                    enumAlarmType = kvp.Key;
                    break;
                }
            }

            if (System.Enum.TryParse<AlarmType>(enumAlarmType, out var result))
            {
                return result;
            }

            return AlarmType.Generic;
        }
    }

}
