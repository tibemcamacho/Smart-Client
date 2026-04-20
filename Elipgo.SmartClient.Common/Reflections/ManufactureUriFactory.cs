using Elipgo.SmartClient.Common.Enum;
using System;
using System.Linq;

namespace Elipgo.SmartClient.Common.Reflections
{
    public class ManufactureUriFactory
    {
        private const string NAMESPACE = "Elipgo.SmartClient.Common.Reflections.Manufactures.";
        private const string PROTOCOL = "Uri";

        public static IManufactureUri Instance(DTOs.CameraDTO camera, int recorderId = 0)
        {
            string manufacture = camera.ManufactureCode.ToString();
            if (camera.Recorders != null && camera.Recorders.Count > 0 && recorderId != 0)
            {
                var recorder = camera.Recorders.Where(r => r.Id == recorderId).First();

                //if (recorder.RecorderType == RecorderType.VREC)
                //if (recorder != null && (recorder.Driver == "RTSP_STREAMCODERS" || recorder.Driver == "RTSP_VREC5_CODER"))
                switch (recorder.Driver)
                {
                    case "RTSP_STREAMCODERS":
                        manufacture = "Vrec";
                        break;
                    case "RTSP_VREC5_CODER":
                        manufacture = "Vrec5";
                        break;
                    case "GenericDriver":
                    case "NETSDK_351":
                    case "NETSDK_351v2":
                    case "NETSDK_351v3":
                    case "NETSDK_351v4":
                    case "NETSDK_351v5":
                    case "NETSDK_351v6":
                    case "NETSDK_351v7":
                    case "NETSDK_351v8":
                    case "NETSDK_351v9":
                    case "NETSDK_351v10":
                    case "NETSDK_351v11":
                    case "NETSDK_351v12":
                    case "NETSDK_352":
                        manufacture = "Dahua";
                        break;
                    case "AMC_741_ENCODER":
                    case "AMC_741":
                        manufacture = "Axis";
                        break;
                    case "HCNetSDK_619":
                    case "HCNetSDK_616":
                        manufacture = "Hikvision";
                        break;
                    case "UNVNetSDK_231":
                        manufacture = "Uniview";
                        break;
                    default:
                        break;


                }

                //manufacture = (recorder.Driver == "RTSP_VREC5_CODER") ? "Vrec5" : "Vrec";
            }

            if (camera.Driver == Enum.Drivers.NETSDK_351GENERIC)
            {
                manufacture = "DahuaGeneric";
            }

            Type classType = Type.GetType(NAMESPACE + manufacture + PROTOCOL);
            if (classType == null)
                return null;
            Type[] cparamTypes = new Type[1];
            cparamTypes[0] = typeof(DTOs.CameraDTO);
            System.Reflection.ConstructorInfo cinfo = classType.GetConstructor(cparamTypes);
            if (cinfo == null)
                throw new Exception("ManufactureUriFactory.Instance: appropiate constructor not found");
            object[] cparams = new object[1];
            cparams[0] = camera;
            return (IManufactureUri)cinfo.Invoke(cparams);
        }

        public static IManufactureUri Instance(DTOs.CameraDTO camera, Enum.Profile profile)
        {
            string manufacture = camera.ManufactureCode.ToString();
            if (!camera.EdgeEnabled && camera.Recorders != null && camera.Recorders.Count > 0 && camera.RecorderId != 0 && camera.Recorders[0].RecorderType == RecorderType.VREC)
            {
                manufacture = (camera.Recorders[0].Driver == "RTSP_VREC5_CODER") ? "Vrec5" : "Vrec";
            }

            Type classType = Type.GetType(NAMESPACE + (camera.Driver == Enum.Drivers.RTSPGENERIC? "Generic" : manufacture)    + PROTOCOL);
            if (classType == null)
                return null;
            Type[] cparamTypes = new Type[2];
            cparamTypes[0] = typeof(DTOs.CameraDTO);
            cparamTypes[1] = typeof(Enum.Profile);
            System.Reflection.ConstructorInfo cinfo = classType.GetConstructor(cparamTypes);
            if (cinfo == null)
                throw new Exception("ManufactureUriFactory.Instance: appropiate constructor not found");
            object[] cparams = new object[2];
            cparams[0] = camera;
            cparams[1] = profile;
            return (IManufactureUri)cinfo.Invoke(cparams);
        }
    }
}
