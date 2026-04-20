using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.EasyTabs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Common
{
    public interface IAppContainer
    {
        ListWithEvents<TitleBarTab> Tabs { get; }
        int SelectedTabIndex { get; set; }
        Dictionary<Guid, Form> Forms { get; set; }
        Dictionary<Guid, Apps> AppsActives { get; set; }
        TitleBarTab CreateTab();
        Guid FirstTabMainViewId { get; }
        void JumpToApp(Apps app, SidebarElementDTO selectSidebarElementDTO = null, DateTime? selectedDateTime = null, CardDto card = null, int groupId = 0);
        void CloseAllTab();
        bool JumpToHome(Guid mainViewId);
        bool IsSelectedTab(Guid mainViewId);
        void UpdateUserPreferences();
        void ShowToolbarHidenButton(Guid mainViewId);
        void HideButtonsTitlebar(Guid mainViewId, bool jump);
        bool CloseOtherTabs(Guid mainViewId, string tokenUser);
        void SaveCatalog(object catalog);
    }
}
