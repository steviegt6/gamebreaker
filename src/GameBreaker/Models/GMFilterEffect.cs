﻿namespace GameBreaker.Models
{
    public class GMFilterEffect : IGMNamedSerializable
    {
        public GMString Name { get; set; }
        public GMString Value; // Unsure how this is determined just yet

        public void Serialize(GMDataWriter writer)
        {
            writer.WritePointerString(Name);
            writer.WritePointerString(Value);
        }

        public void Deserialize(GMDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            Value = reader.ReadStringPointerObject();
        }

        public override string ToString()
        {
            return $"Filter Effect: \"{Name.Content}\"";
        }
    }
}
