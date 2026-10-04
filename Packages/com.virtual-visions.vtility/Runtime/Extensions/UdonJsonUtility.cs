using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class UdonJsonUtility
    {
        public static string ToJson(this DataDictionary dictionary, JsonExportType exportType = JsonExportType.Beautify)
        {
            VRCJson.TrySerializeToJson(dictionary, exportType, out DataToken result);
            return result.String;
        }
        
        public static string ToJson(this DataList list, JsonExportType exportType = JsonExportType.Beautify)
        {
            VRCJson.TrySerializeToJson(list, exportType, out DataToken result);
            return result.String;
        }
    }
}