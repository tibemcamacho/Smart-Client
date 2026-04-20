using Elipgo.SmartClient.Common.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Elipgo.SmartClient.Common.Enum
{
    public enum LprTags
    {
        [Display(Name = Constants.NAME_LPR_LISTNAME)]
        [Description(Constants.DESCRIPTION_LPR_LISTNAME)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_LISTNAME_EN)]
        ListName,
        [Display(Name = Constants.NAME_LPR_NOPLATE)]
        [Description(Constants.DESCRIPTION_LPR_NOPLATE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_NOPLATE_EN)]
        NoPlate,
        [Display(Name = Constants.NAME_LPR_PLATENUMBER)]
        [Description(Constants.DESCRIPTION_LPR_PLATENUMBER)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_PLATENUMBER_EN)]
        PlateNumber,
        [Display(Name = Constants.NAME_LPR_SNAPTIME)]
        [Description(Constants.DESCRIPTION_LPR_SNAPTIME)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_SNAPTIME_EN)]
        SnapTime,
        [Display(Name = Constants.NAME_LPR_VEHICLEOWNER)]
        [Description(Constants.DESCRIPTION_LPR_VEHICLEOWNER)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_VEHICLEOWNER_EN)]
        VehicleOwner,
        [Display(Name = Constants.NAME_LPR_VEHICLECOLOR)]
        [Description(Constants.DESCRIPTION_LPR_VEHICLECOLOR)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_VEHICLECOLOR_EN)]
        VehicleColor,
        [Display(Name = Constants.NAME_LPR_VEHICLETYPE)]
        [Description(Constants.DESCRIPTION_LPR_VEHICLETYPE)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_VEHICLETYPE_EN)]
        VehicleType,
        [Display(Name = Constants.NAME_LPR_DEVICESN)]
        [Description(Constants.DESCRIPTION_LPR_DEVICESN)]
        [DescriptionEN(Descripcion = Constants.DESCRIPTION_LPR_DEVICESN_EN)]
        DeviceSN,
    }
}
