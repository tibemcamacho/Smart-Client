using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.PlaybackControls
{
    public delegate void ObjectClickScale(object sender, string element);
    public partial class TimeLineControl : UserControl
    {
        //public event ObjectClickScale ObjectClickScale;

        private ChromiumWebBrowser _browser;

        private string _htmlPath;

        private List<AlarmDTO> _alarmsTimeline;
        //private DateTime? _dateAlarms;
        public int startHourScale { get; set; }

        public TimeLineControl()
        {
            InitializeComponent();
            this.AutoScroll = false;
        }

        public enum AppName
        {
            Playback = 0,
            InstantPlayback = 1,
            InstantVault = 2,
            FullScreenPlayback = 3
        }
        //
        public void LoadTimeLineHTML(AppName appName, DateTime? start, DateTime? end, int hourLine, double minutesLineInterval, bool showSideBar, double _totalDurationInMinutes, List<AlarmDTO> alarmsTimeLine = null)
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                _alarmsTimeline = alarmsTimeLine;
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                string html = GenerateHTML_Timeline(appName, start, start, end, hourLine, minutesLineInterval, showSideBar, _totalDurationInMinutes, main.Width);

                if (!string.IsNullOrEmpty(_htmlPath) && appName != AppName.Playback)
                {
                    PathUtils.DeleteFile(_htmlPath);
                }

                _htmlPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.html");

                File.WriteAllText(_htmlPath, html);

                if (this.Controls.Count > 0)
                {
                    ChromiumWebBrowser web = Controls[0] as ChromiumWebBrowser;
                    web.Load(_htmlPath);
                }
                else
                {
                    CefSettings settings = new CefSettings()
                    { RemoteDebuggingPort = 8088 };
                    if (!Cef.IsInitialized)
                    {
                        Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                    }
                    CefSharpSettings.LegacyJavascriptBindingEnabled = true;

                    _browser = new ChromiumWebBrowser(_htmlPath)
                    {
                        RequestContext = new RequestContext(),
                        BrowserSettings = new BrowserSettings
                        {
                            FileAccessFromFileUrls = CefState.Enabled,
                            UniversalAccessFromFileUrls = CefState.Enabled,
                            BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34)
                        },
                        MenuHandler = new CustomCefMenuHandler()
                    };
                    this.AutoScroll = false;
                    _browser.AllowDrop = true;

                    this.Controls.Add(_browser);
                }
            }
        }

        // Metodo anterior a resolución 1024*768
        private string GenerateHTML_Timeline_(AppName appName, DateTime? selectedDateTime, DateTime? start, DateTime? end, int hourLine, double intervalMinutes, bool showSideBar, double _totalDurationInMinutes, int mainWidth = 0)
        {
            var html = new StringBuilder();
            html.Append(@"<!DOCTYPE html>
                <html style='overflow: hidden;'>
                <head>
                <style type='text/css'>
                *{
                    margin: 0;
                    padding: 0;
                    box-sizing: border-box;
                    font-family: 'Roboto';
                }

                section{
                    height: 100vh;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    align-items: center;
                }

                #mainContainer{
                    background-color: #191919;
                    height: " + (appName == AppName.InstantPlayback ? "45px" : "35px") + @";
                    font-size: 68px;
                    width: " + (mainWidth != 0 && mainWidth < 1500
                        ? (showSideBar ? "1159px" : "1362px")
                        : (showSideBar ? "1630px" : "1930px")) + @";
                }

                #mainContainer h1{
                    font-size: 68px;
                    letter-spacing: 5px;
                    position: absolute;
                    left: calc(50% - 1485px/2 + 5.5px);
                    top: 16px;
                    width: 1485px;
                    height: 29.9px;
                }

                section h2{
                    font-size: 50px;
                    letter-spacing: 5px;
                    text-align: center;
                }

                .time-container{
                    position: fixed;
                    top: 20px;
                    left: 50%;
                    transform: translateX(-50%);
                    display: flex;
                    justify-content: center;
                }

                .time-container .line{
                    width: 900px;
                    position: absolute;
                    top: 30px;
                    height: 2px;
                    background-color: #FFFFFF;
                    z-index: -2;
                }

              .large-hour-bar{
                    width: " + ((appName == AppName.InstantPlayback || appName == AppName.InstantVault)
                        ? "21.08px"
                        : (mainWidth != 0 && mainWidth < 1500
                            ? (showSideBar ? "19.64px" : "19.54px")
                            : (showSideBar ? "18.54px" : "21.54px"))) + @";
                    height: 0;
                    border: 0.975615px solid #FFFFFF;
                    transform: rotate(90deg);
                    flex: none;
                    flex-grow: 0;
                }

                .medium-hour-bar{
                    width: " + ((appName == AppName.InstantPlayback || appName == AppName.InstantVault)
                        ? "8.59px"
                        : (mainWidth != 0 && mainWidth < 1500
                            ? (showSideBar ? "6.05px" : "5.85px")
                            : (showSideBar ? "5.85px" : "8.85px"))) + @";
                    height: 0;
                    border: 0.975615px solid #FFFFFF;
                    transform: rotate(90deg);
                    flex: none;
                    flex-grow: 0;
                }

                .small-minute-bar{
                    width: " + ((appName == AppName.InstantPlayback || appName == AppName.InstantVault)
                        ? "12.30px"
                        : (mainWidth != 0 && mainWidth < 1500
                            ? (showSideBar ? "9.76px" : "9.76px")
                            : (showSideBar ? "9.76px" : "12.76px"))) + @";
                    height: 0;
                    border: 0.975615px solid #FFFFFF;
                    transform: rotate(90deg);
                    flex: none;
                    flex-grow: 0;
                }

                .time{
                    display: flex;
                    flex-direction: row;
                    align-items: flex-start;
                    padding: 0px;
                    gap: 6.91px;
                    position: absolute;
                    width: 1474.42px;
                    height: 18.54px;
                    left: 5.59px;
                    top: 0px;
                }

                .timeText{
                    position: absolute;
                    width: 22px;
                    height: 6px;
                    left: 0px;
                    top: 23.55px;
                    font-family: 'Roboto';
                    font-weight: 500;
                    font-size: 11.46574px;
                    line-height: 10px;
                    text-align: center;
                    color: #FFFFFF;
                    display:inline-block;
                }

                .timeline{
                    position: absolute;
                    width: 1485px;
                    height: 29.9px;
                    left: calc(50% - 1485px/2 + 5.5px);
                    top: 16px;
                }

                .sectionTime{
                    display: flex;
                    flex-direction: row;
                    align-items: flex-start;
                    padding: 0px;
                    gap: 15.61px;
                    width: 47.81px;
                    height: 18.54px;
                    flex: none;
                    flex-grow: 0;
                }

                .selected{
                    transform: scale(1.2);
                    background-color: #EA5455;
                    color: #000;
                    font-weight: bold;
                }

                .red-mark {
                    border-color: #c0392b !important;
                    color: #c0392b !important;
                    cursor: pointer;
                }

                .instant-plbk-red-mark {
                    border: 1px solid #c0392b !important;
                    cursor: pointer;
                }

                .instant-plbk-large-bar, .instant-plbk-medium-bar, .instant-plbk-small-bar {
                    width: 1px;
                    background-color: #FFFFFF;
                    position: absolute;
                    top: 0; /* Cambié bottom a top para que crezca desde arriba */
                }

                .instant-plbk-large-bar {
                    height: 21px; /* El largo de la barra */
                }

                .instant-plbk-medium-bar {
                    height: 12px;
                }

                .instant-plbk-small-bar {
                    height: 8px;
                }

                </style>
                </head>
                <body>
                <section id='mainContainer'> 
            ");

            double positionPX;

            if ((appName == AppName.InstantPlayback || appName == AppName.InstantVault) && start.HasValue && end.HasValue)
            {
                html.Append(@"<div class='time' style='position: relative; width: 99.3%; height: 50px;'>");
                positionPX = 78.88;
                double totalDurationInMinutes = (end.Value - start.Value).TotalMinutes;

                var specialTimes = GetTimesAlarms();
                for (int i = 0; i <= hourLine; i++)
                {
                    double minutesToAdd;

                    if (appName == AppName.InstantVault || appName == AppName.InstantPlayback) //escala para vault
                    {
                        totalDurationInMinutes = (end.Value - start.Value).TotalMinutes;
                        double intervaloMinutos = totalDurationInMinutes / hourLine;

                        if (appName == AppName.InstantPlayback)
                        {
                            minutesToAdd = i * intervalMinutes;
                        }
                        else
                        {
                            if (i < hourLine)
                            {
                                minutesToAdd = i * intervaloMinutos;
                            }
                            else
                            {
                                minutesToAdd = totalDurationInMinutes;
                            }
                        }
                    }
                    else
                    {
                        minutesToAdd = (i < hourLine) ? (i * totalDurationInMinutes / hourLine) : totalDurationInMinutes;
                    }

                    DateTime hourToShow = start.Value.AddMinutes(minutesToAdd);
                    hourToShow = new DateTime(hourToShow.Year, hourToShow.Month, hourToShow.Day, hourToShow.Hour, hourToShow.Minute, 0);
                    string timeDisplay;
                    double basePosition = i * positionPX; // Desplazamiento general para esta hora/bloque
                    double[] positions = SetBarPositions(i, intervalMinutes, hourLine, ref positionPX, appName, mainWidth);

                    var quartTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes / 4);
                    var middleTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes / 2);
                    var quartThreeTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes((intervalMinutes / 4) * 3);
                    var nextTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes);

                    var specialOne = specialTimes.Where(time => time.Key >= hourToShow.TimeOfDay && time.Key < quartTime).FirstOrDefault();
                    var specialClassOne_text = specialOne.Key != default ? "red-mark" : "";
                    var specialClassOne = specialOne.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipOne = specialOne.Key != default ? $"{Resources.AlarmDetected}: {specialOne.Value} {specialOne.Key}" : string.Empty;

                    var specialQuart = specialTimes.Where(time => time.Key >= quartTime && time.Key < middleTime).FirstOrDefault();
                    var specialClassQuart = specialQuart.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipQuart = specialQuart.Key != default ? $"{Resources.AlarmDetected}: {specialQuart.Value} {specialQuart.Key}" : string.Empty;

                    var specialMiddle = specialTimes.Where(time => time.Key >= middleTime && time.Key < quartThreeTime).FirstOrDefault();
                    var specialClassMiddle = specialMiddle.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipMiddle = specialMiddle.Key != default ? $"{Resources.AlarmDetected}: {specialMiddle.Value} {specialMiddle.Key}" : string.Empty;

                    var specialQuartThree = specialTimes.Where(time => time.Key >= quartThreeTime && time.Key < nextTime).FirstOrDefault();
                    var specialClassQuartThree = specialQuartThree.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipQuartThree = specialQuartThree.Key != default ? $"{Resources.AlarmDetected}: {specialQuartThree.Value} {specialQuartThree.Key}" : string.Empty;

                    if (_totalDurationInMinutes < 360 && appName == AppName.InstantVault)
                    {
                        string hour = hourToShow.ToString("HH");
                        string minute = hourToShow.Minute.ToString("D2");
                        string second = hourToShow.Second.ToString("D2");

                        timeDisplay = $"{hour}:{minute}:{second}";
                        html.Append($@"
                        <a class='timeText {specialClassOne_text}' style='position:absolute; left:calc({(i * positionPX).ToString("F2", CultureInfo.InvariantCulture)}px - 15px)' title='{tooltipOne}'>{timeDisplay}</a>
                        <a class='instant-plbk-large-bar {specialClassOne}' style='position:absolute; left:{(basePosition + positions[0]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipOne}'></a>
                        <a class='instant-plbk-small-bar {specialClassMiddle}' style='position:absolute; left:{(basePosition + positions[2]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipMiddle}'></a>
                        ");
                    }
                    else
                    {
                        string hour = hourToShow.ToString("HH");
                        string minute = hourToShow.Minute.ToString("D2");
                        timeDisplay = $"{hour}:{minute}";
                        html.Append($@"
                            <a class='timeText {specialClassOne_text}' style='position:absolute; left:calc({(i * positionPX).ToString("F2", CultureInfo.InvariantCulture)}px - 12px)' title='{tooltipOne}'>{timeDisplay}</a>
                            <a class='instant-plbk-large-bar {specialClassOne}' style='position:absolute; left:{(basePosition + positions[0]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipOne}'></a>
                            <a class='instant-plbk-medium-bar {specialClassQuart}' style='position:absolute; left:{(basePosition + positions[1]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipQuart}'></a>
                            <a class='instant-plbk-small-bar {specialClassMiddle}' style='position:absolute; left:{(basePosition + positions[2]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipMiddle}'></a>
                            <a class='instant-plbk-medium-bar {specialClassQuartThree}' style='position:absolute; left:{(basePosition + positions[4]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipQuartThree}'></a>
                        ");
                    }
                }
            }
            else//playback
            {
                html.Append(@"<div class='time'>");
                DateTime currentTimeline = selectedDateTime ?? DateTime.Now;
                int currentHour = (hourLine == 24) ? 0 : currentTimeline.Hour + (hourLine / 2);
                int hoursTotal = hourLine;
                int startHourScale = (currentHour >= hoursTotal) ? (currentHour - hoursTotal) : 0;
                this.startHourScale = startHourScale;
                int minutes = (int)intervalMinutes;
                if (mainWidth != 0 && mainWidth < 1500)
                {
                    positionPX = (showSideBar ? 23.86 : 28.20);
                }
                else if (mainWidth != 0 && mainWidth > 1500 && mainWidth < 1800)
                {
                    positionPX = (showSideBar ? 28.32 : 33.46);
                }
                else
                    positionPX = (showSideBar ? 33.74 : 39.74);

                double position = 0;

                var specialTimes = GetTimesAlarms();
                List<string> times = new List<string>();
                Dictionary<TimeSpan, double> timePosition = new Dictionary<TimeSpan, double>();
                for (int hour = startHourScale, indexHours = 0; indexHours < hoursTotal; hour++, indexHours++)
                {
                    if (hour == 24)
                    {
                        hour = 0;
                    }

                    for (int minute = 0; minute < 60; minute += minutes)
                    {
                        TimeSpan currentTime = new TimeSpan(hour, minute, 0);
                        timePosition.Add(currentTime, position);

                        position += positionPX;
                    }
                }

                int indexT = 0;
                int count = timePosition.Count;
                foreach (var hourPlace in timePosition)
                {
                    var timeSpecial = specialTimes.Where(time => time.Key >= hourPlace.Key && time.Key < (hourPlace.Key + TimeSpan.FromMinutes(minutes / 2))).FirstOrDefault();
                    string hourBarClass = timeSpecial.Key != default ? "red-mark" : "";
                    string tooltipText = timeSpecial.Key != default ? $"{Resources.AlarmDetected}: {timeSpecial.Value}" : string.Empty;

                    if ((minutes == 30 && hourPlace.Key.Minutes != 30) ||
                       (minutes == 15 && hourPlace.Key.Minutes != 15 && hourPlace.Key.Minutes != 45) ||
                       (minutes == 10 && hourPlace.Key.Minutes != 10 && hourPlace.Key.Minutes != 30 && hourPlace.Key.Minutes != 50) ||
                       (minutes == 5 && hourPlace.Key.Minutes != 5 && hourPlace.Key.Minutes != 15 && hourPlace.Key.Minutes != 25 && hourPlace.Key.Minutes != 35 && hourPlace.Key.Minutes != 45 && hourPlace.Key.Minutes != 55))
                    {
                        html.Append($@"<a class='timeText {hourBarClass}' style='margin-left:{hourPlace.Value.ToString().Replace(',', '.')}px' title='{tooltipText}'>{hourPlace.Key.Hours:D2}:{hourPlace.Key.Minutes:D2}</a>");
                    }

                    string largeMarginLeft, mediumMarginLeft1, mediumMarginLeft2, smallMarginLeft;

                    if (mainWidth > 1500 && mainWidth < 1800)
                    {
                        largeMarginLeft = showSideBar ? "-2.7px" : "-4.5px";
                        mediumMarginLeft1 = showSideBar ? "-3.4px" : "-4.5px";
                        mediumMarginLeft2 = showSideBar ? "-1.9px" : "-1.8px";
                        smallMarginLeft = showSideBar ? "-2.8px" : "-1.6px";
                    }
                    else if (mainWidth < 1500)
                    {
                        largeMarginLeft = showSideBar ? "-9px" : "-3.7px";
                        mediumMarginLeft1 = showSideBar ? "-7px" : "-5.7px";
                        mediumMarginLeft2 = showSideBar ? "-2.5px" : "-1.9px";
                        smallMarginLeft = showSideBar ? "-2.5px" : "-0.9px";
                    }

                    else
                    {
                        largeMarginLeft = "-1px";
                        mediumMarginLeft1 = "-2px";
                        mediumMarginLeft2 = "1.0px";
                        smallMarginLeft = "2.0px";
                    }

                    bool isEven = indexT % 2 == 0;

                    if (isEven)
                    {
                        html.Append($@"<a class='large-hour-bar {hourBarClass}' style='margin-left:{largeMarginLeft}' title='{tooltipText}'></a>");
                    }
                    else
                    {
                        html.Append($@"<a class='small-minute-bar {hourBarClass}' style='margin-left:{smallMarginLeft}' title='{tooltipText}'></a>");
                    }

                    var specialQuart = specialTimes.FirstOrDefault(time =>
                        time.Key >= (hourPlace.Key + TimeSpan.FromMinutes(minutes / 2)) &&
                        time.Key < (hourPlace.Key + TimeSpan.FromMinutes(minutes)));

                    string quartBarClass = specialQuart.Key != default ? "red-mark" : "";
                    tooltipText = specialQuart.Key != default ? $"{Resources.AlarmDetected}: {specialQuart.Value}" : string.Empty;

                    string finalMediumMarginLeft = isEven ? mediumMarginLeft1 : mediumMarginLeft2;
                    html.Append($@"<a class='medium-hour-bar {quartBarClass}' style='margin-left:{finalMediumMarginLeft}' title='{tooltipText}'></a>");
                    indexT++;
                }
            }

            html.Append(@"</div>
                </section>
                </body>
                </html>"
            );
            return html.ToString();
        }
        
        private string GenerateHTML_Timeline(AppName appName, DateTime? selectedDateTime, DateTime? start, DateTime? end, int hourLine, double intervalMinutes, bool showSideBar, double _totalDurationInMinutes, int mainWidth = 0)
        {
            string containerWidth, timelineWidth, timeWidth;

            if (mainWidth == 1024)
            {
                if (showSideBar)
                {
                    containerWidth = "790px";
                    timelineWidth = "780px";
                    timeWidth = "770px";
                }
                else
                {
                    containerWidth = "1024px";
                    timelineWidth = "1014px";
                    timeWidth = "1000px";
                }
            }
            else if (mainWidth != 0 && mainWidth < 1500)
            {
                containerWidth = showSideBar ? "1159px" : "1362px";
                timelineWidth = "1485px";
                timeWidth = "1474.42px";
            }
            else
            {
                containerWidth = showSideBar ? "1630px" : "1930px";
                timelineWidth = "1485px";
                timeWidth = "1474.42px";
            }

            string largeBarWidth, mediumBarWidth, smallBarWidth;

            if (appName == AppName.InstantPlayback || appName == AppName.InstantVault)
            {
                largeBarWidth = "21.08px";
                mediumBarWidth = "8.59px";
                smallBarWidth = "12.30px";
            }
            else if (mainWidth == 1024)
            {
                largeBarWidth = showSideBar ? "15.0px" : "17.0px";
                mediumBarWidth = showSideBar ? "4.5px" : "5.0px";
                smallBarWidth = showSideBar ? "7.5px" : "8.0px";
            }
            else if (mainWidth != 0 && mainWidth < 1500)
            {
                largeBarWidth = showSideBar ? "19.64px" : "19.54px";
                mediumBarWidth = showSideBar ? "6.05px" : "5.85px";
                smallBarWidth = showSideBar ? "9.76px" : "9.76px";
            }
            else
            {
                largeBarWidth = showSideBar ? "18.54px" : "21.54px";
                mediumBarWidth = showSideBar ? "5.85px" : "8.85px";
                smallBarWidth = showSideBar ? "9.76px" : "12.76px";
            }

            var html = new StringBuilder();
            html.Append($@"<!DOCTYPE html>
        <html style='overflow: hidden;'>
        <head>
        <style type='text/css'>
        *{{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Roboto';
        }}

        section{{
            height: 100vh;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }}

        #mainContainer{{
            background-color: #191919;
            height: {(appName == AppName.InstantPlayback ? "45px" : "35px")};
            font-size: 68px;
            width: {containerWidth};
        }}

        #mainContainer h1{{
            font-size: 68px;
            letter-spacing: 5px;
            position: absolute;
            left: calc(50% - {timelineWidth}/2 + 5.5px);
            top: 16px;
            width: {timelineWidth};
            height: 29.9px;
        }}

        section h2{{
            font-size: 50px;
            letter-spacing: 5px;
            text-align: center;
        }}

        .time-container{{
            position: fixed;
            top: 20px;
            left: 50%;
            transform: translateX(-50%);
            display: flex;
            justify-content: center;
        }}

        .time-container .line{{
            width: 900px;
            position: absolute;
            top: 30px;
            height: 2px;
            background-color: #FFFFFF;
            z-index: -2;
        }}

        .large-hour-bar{{
            width: {largeBarWidth};
            height: 0;
            border: 0.975615px solid #FFFFFF;
            transform: rotate(90deg);
            flex: none;
            flex-grow: 0;
        }}

        .medium-hour-bar{{
            width: {mediumBarWidth};
            height: 0;
            border: 0.975615px solid #FFFFFF;
            transform: rotate(90deg);
            flex: none;
            flex-grow: 0;
        }}

        .small-minute-bar{{
            width: {smallBarWidth};
            height: 0;
            border: 0.975615px solid #FFFFFF;
            transform: rotate(90deg);
            flex: none;
            flex-grow: 0;
        }}

        .time{{
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 0px;
            gap: 6.91px;
            position: absolute;
            width: {timeWidth};
            height: 18.54px;
            left: 5.59px;
            top: 0px;
        }}

        .timeText{{
            position: absolute;
            width: 22px;
            height: 6px;
            left: 0px;
            top: 23.55px;
            font-family: 'Roboto';
            font-weight: 500;
            font-size: 11.46574px;
            line-height: 10px;
            text-align: center;
            color: #FFFFFF;
            display:inline-block;
        }}

        .timeline{{
            position: absolute;
            width: {timelineWidth};
            height: 29.9px;
            left: calc(50% - {timelineWidth}/2 + 5.5px);
            top: 16px;
        }}

        .sectionTime{{
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 0px;
            gap: 15.61px;
            width: 47.81px;
            height: 18.54px;
            flex: none;
            flex-grow: 0;
        }}

        .selected{{
            transform: scale(1.2);
            background-color: #EA5455;
            color: #000;
            font-weight: bold;
        }}

        .red-mark {{
            border-color: #c0392b !important;
            color: #c0392b !important;
            cursor: pointer;
        }}

        .instant-plbk-red-mark {{
            border: 1px solid #c0392b !important;
            cursor: pointer;
        }}

        .instant-plbk-large-bar, .instant-plbk-medium-bar, .instant-plbk-small-bar {{
            width: 1px;
            background-color: #FFFFFF;
            position: absolute;
            top: 0; 
        }}

        .instant-plbk-large-bar {{ height: 21px; }}
        .instant-plbk-medium-bar {{ height: 12px; }}
        .instant-plbk-small-bar {{ height: 8px; }}

        </style>
        </head>
        <body>
        <section id='mainContainer'> 
    ");

            double positionPX;

            if ((appName == AppName.InstantPlayback || appName == AppName.InstantVault) && start.HasValue && end.HasValue)
            {
                html.Append(@"<div class='time' style='position: relative; width: 99.3%; height: 50px;'>");

                if (mainWidth == 1024)
                    positionPX = showSideBar ? 54.0 : 69.7;
                else
                    positionPX = 78.88;

                double totalDurationInMinutes = (end.Value - start.Value).TotalMinutes;
                var specialTimes = GetTimesAlarms();

                for (int i = 0; i <= hourLine; i++)
                {
                    double minutesToAdd;

                    if (appName == AppName.InstantVault || appName == AppName.InstantPlayback)
                    {
                        totalDurationInMinutes = (end.Value - start.Value).TotalMinutes;
                        double intervaloMinutos = totalDurationInMinutes / hourLine;

                        if (appName == AppName.InstantPlayback)
                            minutesToAdd = i * intervalMinutes;
                        else
                            minutesToAdd = (i < hourLine) ? (i * intervaloMinutos) : totalDurationInMinutes;
                    }
                    else
                    {
                        minutesToAdd = (i < hourLine) ? (i * totalDurationInMinutes / hourLine) : totalDurationInMinutes;
                    }

                    DateTime hourToShow = start.Value.AddMinutes(minutesToAdd);
                    hourToShow = new DateTime(hourToShow.Year, hourToShow.Month, hourToShow.Day, hourToShow.Hour, hourToShow.Minute, 0);
                    string timeDisplay;
                    double basePosition = i * positionPX;
                    double[] positions = SetBarPositions(i, intervalMinutes, hourLine, ref positionPX, appName, mainWidth);

                    var quartTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes / 4);
                    var middleTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes / 2);
                    var quartThreeTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes((intervalMinutes / 4) * 3);
                    var nextTime = hourToShow.TimeOfDay + TimeSpan.FromMinutes(intervalMinutes);

                    var specialOne = specialTimes.Where(time => time.Key >= hourToShow.TimeOfDay && time.Key < quartTime).FirstOrDefault();
                    var specialClassOne_text = specialOne.Key != default ? "red-mark" : "";
                    var specialClassOne = specialOne.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipOne = specialOne.Key != default ? $"{Resources.AlarmDetected}: {specialOne.Value} {specialOne.Key}" : string.Empty;

                    var specialQuart = specialTimes.Where(time => time.Key >= quartTime && time.Key < middleTime).FirstOrDefault();
                    var specialClassQuart = specialQuart.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipQuart = specialQuart.Key != default ? $"{Resources.AlarmDetected}: {specialQuart.Value} {specialQuart.Key}" : string.Empty;

                    var specialMiddle = specialTimes.Where(time => time.Key >= middleTime && time.Key < quartThreeTime).FirstOrDefault();
                    var specialClassMiddle = specialMiddle.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipMiddle = specialMiddle.Key != default ? $"{Resources.AlarmDetected}: {specialMiddle.Value} {specialMiddle.Key}" : string.Empty;

                    var specialQuartThree = specialTimes.Where(time => time.Key >= quartThreeTime && time.Key < nextTime).FirstOrDefault();
                    var specialClassQuartThree = specialQuartThree.Key != default ? "instant-plbk-red-mark" : "";
                    var tooltipQuartThree = specialQuartThree.Key != default ? $"{Resources.AlarmDetected}: {specialQuartThree.Value} {specialQuartThree.Key}" : string.Empty;

                    if (_totalDurationInMinutes < 360 && appName == AppName.InstantVault)
                    {
                        string hour = hourToShow.ToString("HH");
                        string minute = hourToShow.Minute.ToString("D2");
                        string second = hourToShow.Second.ToString("D2");

                        timeDisplay = $"{hour}:{minute}:{second}";
                        html.Append($@"
                <a class='timeText {specialClassOne_text}' style='position:absolute; left:calc({(i * positionPX).ToString("F2", CultureInfo.InvariantCulture)}px - 15px)' title='{tooltipOne}'>{timeDisplay}</a>
                <a class='instant-plbk-large-bar {specialClassOne}' style='position:absolute; left:{(basePosition + positions[0]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipOne}'></a>
                <a class='instant-plbk-small-bar {specialClassMiddle}' style='position:absolute; left:{(basePosition + positions[2]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipMiddle}'></a>
                ");
                    }
                    else
                    {
                        string hour = hourToShow.ToString("HH");
                        string minute = hourToShow.Minute.ToString("D2");
                        timeDisplay = $"{hour}:{minute}";

                        // Si es 1024, omitimos imprimir los cuartos de hora (medium-bar) para no saturar la barra
                        if (mainWidth == 1024)
                        {
                            html.Append($@"
                        <a class='timeText {specialClassOne_text}' style='position:absolute; left:calc({(i * positionPX).ToString("F2", CultureInfo.InvariantCulture)}px - 12px)' title='{tooltipOne}'>{timeDisplay}</a>
                        <a class='instant-plbk-large-bar {specialClassOne}' style='position:absolute; left:{(basePosition + positions[0]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipOne}'></a>
                        <a class='instant-plbk-small-bar {specialClassMiddle}' style='position:absolute; left:{(basePosition + positions[2]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipMiddle}'></a>
                    ");
                        }
                        else
                        {
                            html.Append($@"
                        <a class='timeText {specialClassOne_text}' style='position:absolute; left:calc({(i * positionPX).ToString("F2", CultureInfo.InvariantCulture)}px - 12px)' title='{tooltipOne}'>{timeDisplay}</a>
                        <a class='instant-plbk-large-bar {specialClassOne}' style='position:absolute; left:{(basePosition + positions[0]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipOne}'></a>
                        <a class='instant-plbk-medium-bar {specialClassQuart}' style='position:absolute; left:{(basePosition + positions[1]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipQuart}'></a>
                        <a class='instant-plbk-small-bar {specialClassMiddle}' style='position:absolute; left:{(basePosition + positions[2]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipMiddle}'></a>
                        <a class='instant-plbk-medium-bar {specialClassQuartThree}' style='position:absolute; left:{(basePosition + positions[4]).ToString("F2", CultureInfo.InvariantCulture)}px' title='{tooltipQuartThree}'></a>
                    ");
                        }
                    }
                }
            }
            else // Playback normal
            {
                html.Append(@"<div class='time'>");
                DateTime currentTimeline = selectedDateTime ?? DateTime.Now;
                int currentHour = (hourLine == 24) ? 0 : currentTimeline.Hour + (hourLine / 2);
                int hoursTotal = hourLine;
                int startHourScale = (currentHour >= hoursTotal) ? (currentHour - hoursTotal) : 0;
                this.startHourScale = startHourScale;
                int minutes = (int)intervalMinutes;

                if (mainWidth == 1024)
                {
                    positionPX = showSideBar ? 15.8 : 20.9;
                }
                else if (mainWidth != 0 && mainWidth < 1500)
                {
                    positionPX = (showSideBar ? 23.86 : 28.20);
                }
                else if (mainWidth != 0 && mainWidth > 1500 && mainWidth < 1800)
                {
                    positionPX = (showSideBar ? 28.32 : 33.46);
                }
                else
                {
                    positionPX = (showSideBar ? 33.74 : 39.74);
                }

                double position = 0;
                var specialTimes = GetTimesAlarms();
                Dictionary<TimeSpan, double> timePosition = new Dictionary<TimeSpan, double>();

                for (int hour = startHourScale, indexHours = 0; indexHours < hoursTotal; hour++, indexHours++)
                {
                    if (hour == 24) hour = 0;

                    for (int minute = 0; minute < 60; minute += minutes)
                    {
                        TimeSpan currentTime = new TimeSpan(hour, minute, 0);
                        timePosition.Add(currentTime, position);
                        position += positionPX;
                    }
                }

                int indexT = 0;
                foreach (var hourPlace in timePosition)
                {
                    var timeSpecial = specialTimes.Where(time => time.Key >= hourPlace.Key && time.Key < (hourPlace.Key + TimeSpan.FromMinutes(minutes / 2))).FirstOrDefault();
                    string hourBarClass = timeSpecial.Key != default ? "red-mark" : "";
                    string tooltipText = timeSpecial.Key != default ? $"{Resources.AlarmDetected}: {timeSpecial.Value}" : string.Empty;

                    if ((minutes == 30 && hourPlace.Key.Minutes != 30) ||
                       (minutes == 15 && hourPlace.Key.Minutes != 15 && hourPlace.Key.Minutes != 45) ||
                       (minutes == 10 && hourPlace.Key.Minutes != 10 && hourPlace.Key.Minutes != 30 && hourPlace.Key.Minutes != 50) ||
                       (minutes == 5 && hourPlace.Key.Minutes != 5 && hourPlace.Key.Minutes != 15 && hourPlace.Key.Minutes != 25 && hourPlace.Key.Minutes != 35 && hourPlace.Key.Minutes != 45 && hourPlace.Key.Minutes != 55))
                    {
                        html.Append($@"<a class='timeText {hourBarClass}' style='margin-left:{hourPlace.Value.ToString(CultureInfo.InvariantCulture)}px' title='{tooltipText}'>{hourPlace.Key.Hours:D2}:{hourPlace.Key.Minutes:D2}</a>");
                    }

                    string largeMarginLeft, mediumMarginLeft1, mediumMarginLeft2, smallMarginLeft;

                    // Para 1024px recalculamos los espacios
                    if (mainWidth == 1024)
                    {
                        if (showSideBar)
                        {
                            largeMarginLeft = "1.5px";
                            smallMarginLeft = "-6px";
                        }
                        else
                        {
                            largeMarginLeft = "6px";
                            smallMarginLeft = "-3px";
                        }
                        mediumMarginLeft1 = "0px";
                        mediumMarginLeft2 = "0px";
                    }
                    else if (mainWidth > 1500 && mainWidth < 1800)
                    {
                        largeMarginLeft = showSideBar ? "-2.7px" : "-4.5px";
                        mediumMarginLeft1 = showSideBar ? "-3.4px" : "-4.5px";
                        mediumMarginLeft2 = showSideBar ? "-1.9px" : "-1.8px";
                        smallMarginLeft = showSideBar ? "-2.8px" : "-1.6px";
                    }
                    else if (mainWidth != 0 && mainWidth < 1500)
                    {
                        largeMarginLeft = showSideBar ? "-9px" : "-3.7px";
                        mediumMarginLeft1 = showSideBar ? "-7px" : "-5.7px";
                        mediumMarginLeft2 = showSideBar ? "-2.5px" : "-1.9px";
                        smallMarginLeft = showSideBar ? "-2.5px" : "-0.9px";
                    }
                    else
                    {
                        largeMarginLeft = "-1px";
                        mediumMarginLeft1 = "-2px";
                        mediumMarginLeft2 = "1.0px";
                        smallMarginLeft = "2.0px";
                    }

                    bool isEven = indexT % 2 == 0;

                    if (isEven)
                    {
                        html.Append($@"<a class='large-hour-bar {hourBarClass}' style='margin-left:{largeMarginLeft}' title='{tooltipText}'></a>");
                    }
                    else
                    {
                        html.Append($@"<a class='small-minute-bar {hourBarClass}' style='margin-left:{smallMarginLeft}' title='{tooltipText}'></a>");
                    }

                    // Si es 1024px, no imprimimos la barra "medium" de los 15/45 minutos.
                    if (mainWidth != 1024)
                    {
                        var specialQuart = specialTimes.FirstOrDefault(time =>
                            time.Key >= (hourPlace.Key + TimeSpan.FromMinutes(minutes / 2)) &&
                            time.Key < (hourPlace.Key + TimeSpan.FromMinutes(minutes)));

                        string quartBarClass = specialQuart.Key != default ? "red-mark" : "";
                        tooltipText = specialQuart.Key != default ? $"{Resources.AlarmDetected}: {specialQuart.Value}" : string.Empty;

                        string finalMediumMarginLeft = isEven ? mediumMarginLeft1 : mediumMarginLeft2;
                        html.Append($@"<a class='medium-hour-bar {quartBarClass}' style='margin-left:{finalMediumMarginLeft}' title='{tooltipText}'></a>");
                    }

                    indexT++;
                }
            }

            html.Append(@"</div>
        </section>
        </body>
        </html>"
            );
            return html.ToString();
        }
        private double[] SetBarPositions(int i, double minutesLineInterval, int hourLine, ref double textPositions, AppName appName, int mainWidth)
        {

            if (hourLine == 6)
            {
                double largeHourBar = (i == 0) ? 0.00 : 60.68;
                double mediumHourBar = 52.68;
                double smallMinuteBar = 57.68;
                double mediumHourBarExtra = 57.68;
                textPositions = (minutesLineInterval == 30) ? 153.45 : 306.85;

                return new double[] { largeHourBar, mediumHourBar, smallMinuteBar, smallMinuteBar, mediumHourBarExtra };
            }

            if (appName == AppName.InstantVault)
            {
                //double largeHourBar = (i == 0) ? 0.00 : 18.68;
                //double mediumHourBar = (i == 0) ? 15.88 : 16.68;
                //double smallMinuteBar = (i == 0) ? 19.88 : 20.68;
                //double mediumHourBarExtra = (i == 0) ? 18.88 : 19.68;
                //textPositions = (minutesLineInterval == 30) ? 153.45
                double largeHourBar = 0.00;
                double mediumHourBar = 19.21;
                double smallMinuteBar = 38.42;
                double mediumHourBarExtra = 57.63;
                textPositions = 76.85;

                return new double[] { largeHourBar, mediumHourBar, smallMinuteBar, smallMinuteBar, mediumHourBarExtra };
            }
            else
            {
                //double largeHourBar = (i == 0) ? 0.00 : 18.30;
                //double mediumHourBar = (i == 0) ? 15.88 : 16.80;
                //double smallMinuteBar = (i == 0) ? 19.88 : 20.80;
                //double mediumHourBarExtra = (i == 0) ? 18.88 : 19.80;

                double largeHourBar = 0.00;
                double mediumHourBar = 38.36;
                double smallMinuteBar = 76.72;
                double mediumHourBarExtra = 115.08;
                textPositions = 153.45;

                if (mainWidth <= 1400)
                {
                    mediumHourBar = 29.39;
                    smallMinuteBar = 58.78;
                    mediumHourBarExtra = 85.17;
                    textPositions = (minutesLineInterval == 30) ? 107.83 : 153.45;
                }

                return new double[] { largeHourBar, mediumHourBar, smallMinuteBar, smallMinuteBar, mediumHourBarExtra };
            }

        }

        private List<KeyValuePair<TimeSpan, string>> GetTimesAlarms()
        {
            if (_alarmsTimeline == null)
            {
                return new List<KeyValuePair<TimeSpan, string>>();
            }

            return _alarmsTimeline
                .Select(alarm => new KeyValuePair<TimeSpan, string>(
                    DateTime.ParseExact(alarm.Time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TimeOfDay,
                    alarm.Type))
                .ToList();
        }
    }
}
