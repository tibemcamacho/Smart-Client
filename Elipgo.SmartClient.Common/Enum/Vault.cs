using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.Enum
{
    
    public enum Watermark
    {
        /// Warning: Make sure if the table users.ApplicationEntities CssImage saves dimensions as % or px
        [Description("top: 1%; right: 50%;")]
        TopLeft,
        [Description("top: 71%; right: 50%;")]
        BottomLeft,
        [Description("top: 1%; right: -20%;")]
        TopRight,
        [Description("top: 71%; right: -20%;")]
        BottomRight,
        [Description("top: 35%; right: 15%;")]
        Center
    }

}
