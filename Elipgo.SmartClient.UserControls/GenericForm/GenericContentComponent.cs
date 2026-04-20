using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class GenericContentComponent : UserControl
    {
        public GenericContentComponent()
        {
            InitializeComponent();
        }


        public GenericFormItemCard SelectedItem { get; set; }

        public ContentFormDTO SelectedItemOption { get; set; }

        public virtual ConfigGenericForm Configuration { get; internal set; }

        public virtual Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }
        public virtual void GoBack()
        {
            //throw new NotImplementedException();
        }

        public object ViewModel { get; set; }

        public virtual Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<ContentFormDTO>> callback)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Edit()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> EditSelected()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual async  Task<bool> Execute()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> SwitchChange(bool value, ContentFormDTO element)
        {
            //throw new NotImplementedException();
            return false;
        }

        public virtual void DobleClick(ContentFormDTO element)
        {

        }
        private Apps _parentApp = Apps.None;
        public Apps ParentApps
        {
            get { return this._parentApp; }
            set { this._parentApp = value; }

        }

        public Manufacturer mancode;
    }
}
