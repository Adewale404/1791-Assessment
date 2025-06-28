using LoadExpressApi.Application.Common.Interface;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace LoadExpressApi.Application.Auditing
{
    public class AuditTrail(EntityEntry entry, ISerializerService serializer)
    {
        private readonly ISerializerService _serializer = serializer;

        public EntityEntry Entry { get; } = entry;
        public string UserId { get; set; }
        public string? TableName { get; set; }
        public string? IPAddress { get; set; }
        public Dictionary<string, object?> KeyValues { get; } = [];
        public Dictionary<string, object?> OldValues { get; } = [];
        public Dictionary<string, object?> NewValues { get; } = [];
        public List<PropertyEntry> TemporaryProperties { get; } = [];
        public TrailType TrailType { get; set; }
        public List<string> ChangedColumns { get; } = [];
        public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

        public Trail ToAuditTrail() =>
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = UserId,
                Type = TrailType.ToString(),
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = _serializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : _serializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : _serializer.Serialize(NewValues),
                AffectedColumns = ChangedColumns.Count == 0 ? null : _serializer.Serialize(ChangedColumns),
                IPAddress = IPAddress,
            };
    }
}
