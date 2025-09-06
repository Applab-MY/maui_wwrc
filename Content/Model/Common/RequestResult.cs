namespace wwrc_maui.Content.Model.Common
{
    public class RequestResult<T>
    {
        public RequestResult() { }

        public int SystemCode;
        public string SystemMessage = "";
        public string SystemDebugMessage = "";
        public T? items = default;
    }
}