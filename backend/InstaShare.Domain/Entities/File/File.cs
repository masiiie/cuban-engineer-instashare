using InstaShare.Domain.Entities.Common;

namespace InstaShare.Domain.Entities.Files
{
    public class File : AuditableBase
    {
        public string Name {get;set;}
        public FileStatus Status {get;set;}
        public long Size {get;set;}
        public string BlobUrl {get;set;}
    }

    public enum FileStatus
    {
        Uploading,
        Zipping,
        Zipped
    }
}