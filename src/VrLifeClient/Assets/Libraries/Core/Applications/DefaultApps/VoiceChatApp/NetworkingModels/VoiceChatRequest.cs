// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: VoiceChatRequest.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels {

  /// <summary>Holder for reflection information generated from VoiceChatRequest.proto</summary>
  public static partial class VoiceChatRequestReflection {

    #region Descriptor
    /// <summary>File descriptor for VoiceChatRequest.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static VoiceChatRequestReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZWb2ljZUNoYXRSZXF1ZXN0LnByb3RvEkhWckxpZmVTaGFyZWQuQ29yZS5B",
            "cHBsaWNhdGlvbnMuRGVmYXVsdEFwcHMuVm9pY2VDaGF0QXBwLk5ldHdvcmtp",
            "bmdNb2RlbHMiMgoQVm9pY2VDaGF0UmVxdWVzdBIQCghzYW1wbGVJZBgBIAEo",
            "BBIMCgRkYXRhGAIgAygCYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels.VoiceChatRequest), global::VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels.VoiceChatRequest.Parser, new[]{ "SampleId", "Data" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class VoiceChatRequest : pb::IMessage<VoiceChatRequest> {
    private static readonly pb::MessageParser<VoiceChatRequest> _parser = new pb::MessageParser<VoiceChatRequest>(() => new VoiceChatRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<VoiceChatRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels.VoiceChatRequestReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VoiceChatRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VoiceChatRequest(VoiceChatRequest other) : this() {
      sampleId_ = other.sampleId_;
      data_ = other.data_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public VoiceChatRequest Clone() {
      return new VoiceChatRequest(this);
    }

    /// <summary>Field number for the "sampleId" field.</summary>
    public const int SampleIdFieldNumber = 1;
    private ulong sampleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong SampleId {
      get { return sampleId_; }
      set {
        sampleId_ = value;
      }
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 2;
    private static readonly pb::FieldCodec<float> _repeated_data_codec
        = pb::FieldCodec.ForFloat(18);
    private readonly pbc::RepeatedField<float> data_ = new pbc::RepeatedField<float>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<float> Data {
      get { return data_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as VoiceChatRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(VoiceChatRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SampleId != other.SampleId) return false;
      if(!data_.Equals(other.data_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SampleId != 0UL) hash ^= SampleId.GetHashCode();
      hash ^= data_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (SampleId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(SampleId);
      }
      data_.WriteTo(output, _repeated_data_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SampleId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(SampleId);
      }
      size += data_.CalculateSize(_repeated_data_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(VoiceChatRequest other) {
      if (other == null) {
        return;
      }
      if (other.SampleId != 0UL) {
        SampleId = other.SampleId;
      }
      data_.Add(other.data_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            SampleId = input.ReadUInt64();
            break;
          }
          case 18:
          case 21: {
            data_.AddEntriesFrom(input, _repeated_data_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
