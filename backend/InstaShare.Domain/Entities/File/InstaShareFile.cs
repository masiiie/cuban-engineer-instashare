using InstaShare.Domain.Entities.Common;

namespace InstaShare.Domain.Entities.Files
{
    public class InstaShareFile : AuditableBase
    {
        public long Id { get; private set; }
        public string Name {get; private set;}
        public FileStatus Status {get;private set;}
        public long Size {get;private set;}
        public string BlobUrl {get;private set;}

        protected InstaShareFile() {} 
        public InstaShareFile(string name, FileStatus status, long size, string bloburl = null)
        {
            // blob url is set after uploading
            Name = name;
            Status = status;
            Size = size;

            if(bloburl != null)
            {
                BlobUrl = bloburl;
            }
        }
    
        public void SetName(string name)
        {
            Name = name;
        }

        public void SetStatus(FileStatus status)
        {
            Status = status;
        }

        public void SetSize(long size)
        {
            Size = size;
        }

        public void SetBlobUrl(string url)
        {
            BlobUrl = url;
        }
    }

    public enum FileStatus
    {
        OnlyInDbNoContent,
        Uploading,
        Zipping,
        Zipped
    }
}