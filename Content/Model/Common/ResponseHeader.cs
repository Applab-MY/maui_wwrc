using System.Collections;

namespace wwrc_maui.Content.Model.Common
{
    public class ResponseHeader
    {
        public string SystemCode { get; set; } = "";
        public string SystemMessage { get; set; } = "";
        public string SystemDebugMessage { get; set; } = "";
        public ArrayList Items { get; set; } = new ArrayList();
    }
}
