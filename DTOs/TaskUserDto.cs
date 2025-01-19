namespace TaskUserManager.DTOs
{
    public class TaskUserDto
    {
        public int UserTaskId { get; set; }

        public int UserId { get; set; }

        public bool? StatusTask { get; set; }

        public string? EvidencePath { get; set; }

        public DateTime? EvidenceUploadDate { get; set; }

        // Propiedades relacionadas (opcional, dependiendo de tus requerimientos)
        public string? UserName { get; set; } // Nombre del usuario (de TfaUser)
        public string? TaskName { get; set; } // Nombre de la tarea (de TfaTask)
    }
}
