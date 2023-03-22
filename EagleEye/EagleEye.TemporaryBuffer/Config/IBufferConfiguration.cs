namespace EagleEye.TemporaryBuffer.Config
{
    internal interface IBufferConfiguration
    {
        int SizeOfBuffer { get; set; }
        string CollectionNameIndex { get; set; }
    }
}