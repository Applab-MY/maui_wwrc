using CommunityToolkit.Mvvm.Messaging.Messages;

namespace wwrc_maui.Content.Helper
{
    public class ReferenceMessenger
    {
        public class ClassObjectNotify(KeyClassObject value) : ValueChangedMessage<KeyClassObject>(value) { }
        public class StringNotify(string value) : ValueChangedMessage<string>(value) { }
        public class KeyValueNotify(KeyValue value) : ValueChangedMessage<KeyValue>(value) { }

        public class KeyValue()
        {
            public string Key { get; set; } = "";
            public string Value { get; set; } = "";
            public string Description { get; set; } = "";
        }

        public class KeyClassObject()
        {
            public string Key { get; set; } = "";
            public object? Value { get; set; } = null;
            public string Description { get; set; } = "";
        }
    }
}
