// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: FriendsAppListMsg.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels {

  /// <summary>Holder for reflection information generated from FriendsAppListMsg.proto</summary>
  public static partial class FriendsAppListMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for FriendsAppListMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FriendsAppListMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdGcmllbmRzQXBwTGlzdE1zZy5wcm90bxJGVnJMaWZlU2hhcmVkLkNvcmUu",
            "QXBwbGljYXRpb25zLkRlZmF1bHRBcHBzLkZyaWVuZHNBcHAuTmV0d29ya2lu",
            "Z01vZGVscxoXRnJpZW5kc0FwcFVzZXJNc2cucHJvdG8igwEKEUZyaWVuZHNB",
            "cHBMaXN0TXNnEm4KC2ZyaWVuZHNMaXN0GAEgAygLMlkuVnJMaWZlU2hhcmVk",
            "LkNvcmUuQXBwbGljYXRpb25zLkRlZmF1bHRBcHBzLkZyaWVuZHNBcHAuTmV0",
            "d29ya2luZ01vZGVscy5GcmllbmRzQXBwVXNlck1zZ2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsgReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg), global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg.Parser, new[]{ "FriendsList" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class FriendsAppListMsg : pb::IMessage<FriendsAppListMsg> {
    private static readonly pb::MessageParser<FriendsAppListMsg> _parser = new pb::MessageParser<FriendsAppListMsg>(() => new FriendsAppListMsg());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FriendsAppListMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppListMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppListMsg(FriendsAppListMsg other) : this() {
      friendsList_ = other.friendsList_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppListMsg Clone() {
      return new FriendsAppListMsg(this);
    }

    /// <summary>Field number for the "friendsList" field.</summary>
    public const int FriendsListFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg> _repeated_friendsList_codec
        = pb::FieldCodec.ForMessage(10, global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg.Parser);
    private readonly pbc::RepeatedField<global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg> friendsList_ = new pbc::RepeatedField<global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg> FriendsList {
      get { return friendsList_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FriendsAppListMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FriendsAppListMsg other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!friendsList_.Equals(other.friendsList_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= friendsList_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      friendsList_.WriteTo(output, _repeated_friendsList_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += friendsList_.CalculateSize(_repeated_friendsList_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FriendsAppListMsg other) {
      if (other == null) {
        return;
      }
      friendsList_.Add(other.friendsList_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            friendsList_.AddEntriesFrom(input, _repeated_friendsList_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code