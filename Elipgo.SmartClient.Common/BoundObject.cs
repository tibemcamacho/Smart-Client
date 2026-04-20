

namespace Elipgo.SmartClient.Common
{
    public class BoundObject
    {
        public delegate void OnClickObjectEventHandler(object sender, string json);
        public event OnClickObjectEventHandler OnClickObject;

        public delegate void OnClickDocumentEventHandler(object sender);
        public event OnClickDocumentEventHandler OnClickDocument;

        public delegate void DragEventHandler(object sender);
        public event DragEventHandler OnDrag;

        public delegate void OnDoubleClickObjectEventHandler(object sender, string json);
        public event OnDoubleClickObjectEventHandler OnDoubleClickObject;

        public delegate void OnClickTabBarEventHandler(object sender, bool show);
        public event OnClickTabBarEventHandler OnClickTabBar;

        public delegate void OnGotoPreviewEventHandler(object sender, string json);
        public event OnGotoPreviewEventHandler GotoPreview;

        public void sendData(string json)
        {
            OnClickObject?.Invoke(this, json);
        }

        public void hideTabBar(bool show)
        {
            OnClickTabBar?.Invoke(this, show);
        }

        public void onDocumentClicked(string data)
        {
            OnClickDocument?.Invoke(this);
        }

        public void onDrag(string data)
        {
            OnDrag?.Invoke(this);
        }

        public void onDoubleClickObject(string json)
        {
            OnDoubleClickObject?.Invoke(this, json);
        }

        public void gotoPreview(string json)
        {
            GotoPreview?.Invoke(this, json);
        }
    }
}
