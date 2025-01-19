namespace TaskUserManager.DTOs
{
    public class UpdateEvidence
    {
        public int UserTaskId { get; set; }

        public int UserId { get; set; }

        public IFormFile? vlf_image { get; set; }

        public DateTime? EvidenceUploadDate { get; set; }
    }
}
