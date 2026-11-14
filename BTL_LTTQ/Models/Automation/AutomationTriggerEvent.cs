using System;

namespace BTL_LTTQ.Models.Automation
{
    /// <summary>
    /// Sự kiện nghiệp vụ được phát sau khi transaction đã commit.
    /// EventToken dùng để nhận diện một lần phát duy nhất (chống xử lý trùng).
    /// </summary>
    public class AutomationTriggerEvent
    {
        public string TriggerType { get; set; } = "";
        public string EntityType { get; set; } = "CLASS";
        public string EntityID { get; set; } = "";
        public string EventToken { get; set; } = "";

        public static AutomationTriggerEvent Tao(string triggerType, string entityType, string entityID)
        {
            return new AutomationTriggerEvent
            {
                TriggerType = triggerType,
                EntityType = entityType,
                EntityID = entityID,
                EventToken = Guid.NewGuid().ToString("N")
            };
        }
    }
}
