using FileStorageNetCore.Models;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data
{
	public class FileTemplate : AttachedFile, IEntity
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
