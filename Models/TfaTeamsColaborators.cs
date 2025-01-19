namespace TaskUserManager.Models
{
    public class TfaTeamsColaborators
    {
        public int ColaboratorTeamID { get; set; } // Foreign Key to TFA_TEAMS
        public TfaTeam Team { get; set; } = null!;

        public int ColaboratorUsersID { get; set; } // Foreign Key to TFA_USERS
        public TfaUser User { get; set; } = null!;
    }
}
