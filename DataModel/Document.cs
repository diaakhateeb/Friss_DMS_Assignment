using System;

namespace DataModel
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public long? Size { get; set; }
        public string Extension { get; set; }
        public int DownloadCounter { get; set; }
        public string UserId { get; set; }
        //public virtual User User { get; set; }
    }
}
