using System;

namespace Auditor.Models
{
    public class Activity
    {
        public Int32 ID { get; set; }
        public Int32 ActionType { get; set; }
        public Decimal TimeSpend { get; set; }
        public String ObjectName { get; set; }
        public DateTime ActionTime { get; set; }
        public String ToolName { get; set; }
        public Int32 IsUploaded { get; set; }
        public Int32 ProjectID { get; set; }
        public Int32 OldActionType { get; set; }
        public Int32 IsReadOnly { get; set; }
        public String Version { get; set; }
        public Int32 ServerID { get; set; }
        public String MachineName { get; set; }
        public DateTime UpdateTime { get; set; }
        public String MacID { get; set; }
        public String ExeName { get; set; }
        public Int32 TeamID { get; set; }
        public Int32 ArtifactType { get; set; }
        public Int32 IsUTC { get; set; }
        public Int32 IsShift { get; set; }
        public Int32 AppID { get; set; }
        public Int32 ObjID { get; set; }
        public Int32 MachineID { get; set; }

    }
}