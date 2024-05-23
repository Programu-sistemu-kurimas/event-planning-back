

namespace Event_planning_back.DataAccess.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string TaskName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string TaskState { get; set; } = string.Empty;

        public Guid ProjectId { get; set; } = Guid.Empty;

        public ProjectEntity Project { get; set; } = new ProjectEntity();

        public ICollection<UserEntity> AssignedUsers { get; set; } = new List<UserEntity>();
    }
}