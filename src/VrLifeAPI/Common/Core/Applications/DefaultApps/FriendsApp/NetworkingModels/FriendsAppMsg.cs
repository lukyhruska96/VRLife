// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: FriendsAppMsg.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels {

  /// <summary>Holder for reflection information generated from FriendsAppMsg.proto</summary>
  public static partial class FriendsAppMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for FriendsAppMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FriendsAppMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNGcmllbmRzQXBwTXNnLnByb3RvEkVWckxpZmVBUEkuQ29tbW9uLkFwcGxp",
            "Y2F0aW9ucy5EZWZhdWx0QXBwcy5GcmllbmRzQXBwLk5ldHdvcmtpbmdNb2Rl",
            "bHMaF0ZyaWVuZHNBcHBVc2VyTXNnLnByb3RvGhdGcmllbmRzQXBwTGlzdE1z",
            "Zy5wcm90bxobRnJpZW5kc0FwcFJlcXVlc3RzTXNnLnByb3RvIvUCCg1Gcmll",
            "bmRzQXBwTXNnEm8KC2ZyaWVuZHNMaXN0GAEgASgLMlguVnJMaWZlQVBJLkNv",
            "bW1vbi5BcHBsaWNhdGlvbnMuRGVmYXVsdEFwcHMuRnJpZW5kc0FwcC5OZXR3",
            "b3JraW5nTW9kZWxzLkZyaWVuZHNBcHBMaXN0TXNnSAAScAoMZnJpZW5kRGV0",
            "YWlsGAIgASgLMlguVnJMaWZlQVBJLkNvbW1vbi5BcHBsaWNhdGlvbnMuRGVm",
            "YXVsdEFwcHMuRnJpZW5kc0FwcC5OZXR3b3JraW5nTW9kZWxzLkZyaWVuZHNB",
            "cHBVc2VyTXNnSAASdgoOZnJpZW5kUmVxdWVzdHMYAyABKAsyXC5WckxpZmVB",
            "UEkuQ29tbW9uLkFwcGxpY2F0aW9ucy5EZWZhdWx0QXBwcy5GcmllbmRzQXBw",
            "Lk5ldHdvcmtpbmdNb2RlbHMuRnJpZW5kc0FwcFJlcXVlc3RzTXNnSABCCQoH",
            "TXNnVHlwZWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsgReflection.Descriptor, global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsgReflection.Descriptor, global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppRequestsMsgReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppMsg), global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppMsg.Parser, new[]{ "FriendsList", "FriendDetail", "FriendRequests" }, new[]{ "MsgType" }, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class FriendsAppMsg : pb::IMessage<FriendsAppMsg> {
    private static readonly pb::MessageParser<FriendsAppMsg> _parser = new pb::MessageParser<FriendsAppMsg>(() => new FriendsAppMsg());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FriendsAppMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppMsg(FriendsAppMsg other) : this() {
      switch (other.MsgTypeCase) {
        case MsgTypeOneofCase.FriendsList:
          FriendsList = other.FriendsList.Clone();
          break;
        case MsgTypeOneofCase.FriendDetail:
          FriendDetail = other.FriendDetail.Clone();
          break;
        case MsgTypeOneofCase.FriendRequests:
          FriendRequests = other.FriendRequests.Clone();
          break;
      }

    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FriendsAppMsg Clone() {
      return new FriendsAppMsg(this);
    }

    /// <summary>Field number for the "friendsList" field.</summary>
    public const int FriendsListFieldNumber = 1;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg FriendsList {
      get { return msgTypeCase_ == MsgTypeOneofCase.FriendsList ? (global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg) msgType_ : null; }
      set {
        msgType_ = value;
        msgTypeCase_ = value == null ? MsgTypeOneofCase.None : MsgTypeOneofCase.FriendsList;
      }
    }

    /// <summary>Field number for the "friendDetail" field.</summary>
    public const int FriendDetailFieldNumber = 2;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg FriendDetail {
      get { return msgTypeCase_ == MsgTypeOneofCase.FriendDetail ? (global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg) msgType_ : null; }
      set {
        msgType_ = value;
        msgTypeCase_ = value == null ? MsgTypeOneofCase.None : MsgTypeOneofCase.FriendDetail;
      }
    }

    /// <summary>Field number for the "friendRequests" field.</summary>
    public const int FriendRequestsFieldNumber = 3;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppRequestsMsg FriendRequests {
      get { return msgTypeCase_ == MsgTypeOneofCase.FriendRequests ? (global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppRequestsMsg) msgType_ : null; }
      set {
        msgType_ = value;
        msgTypeCase_ = value == null ? MsgTypeOneofCase.None : MsgTypeOneofCase.FriendRequests;
      }
    }

    private object msgType_;
    /// <summary>Enum of possible cases for the "MsgType" oneof.</summary>
    public enum MsgTypeOneofCase {
      None = 0,
      FriendsList = 1,
      FriendDetail = 2,
      FriendRequests = 3,
    }
    private MsgTypeOneofCase msgTypeCase_ = MsgTypeOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MsgTypeOneofCase MsgTypeCase {
      get { return msgTypeCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMsgType() {
      msgTypeCase_ = MsgTypeOneofCase.None;
      msgType_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FriendsAppMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FriendsAppMsg other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(FriendsList, other.FriendsList)) return false;
      if (!object.Equals(FriendDetail, other.FriendDetail)) return false;
      if (!object.Equals(FriendRequests, other.FriendRequests)) return false;
      if (MsgTypeCase != other.MsgTypeCase) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (msgTypeCase_ == MsgTypeOneofCase.FriendsList) hash ^= FriendsList.GetHashCode();
      if (msgTypeCase_ == MsgTypeOneofCase.FriendDetail) hash ^= FriendDetail.GetHashCode();
      if (msgTypeCase_ == MsgTypeOneofCase.FriendRequests) hash ^= FriendRequests.GetHashCode();
      hash ^= (int) msgTypeCase_;
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (msgTypeCase_ == MsgTypeOneofCase.FriendsList) {
        output.WriteRawTag(10);
        output.WriteMessage(FriendsList);
      }
      if (msgTypeCase_ == MsgTypeOneofCase.FriendDetail) {
        output.WriteRawTag(18);
        output.WriteMessage(FriendDetail);
      }
      if (msgTypeCase_ == MsgTypeOneofCase.FriendRequests) {
        output.WriteRawTag(26);
        output.WriteMessage(FriendRequests);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (msgTypeCase_ == MsgTypeOneofCase.FriendsList) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(FriendsList);
      }
      if (msgTypeCase_ == MsgTypeOneofCase.FriendDetail) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(FriendDetail);
      }
      if (msgTypeCase_ == MsgTypeOneofCase.FriendRequests) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(FriendRequests);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FriendsAppMsg other) {
      if (other == null) {
        return;
      }
      switch (other.MsgTypeCase) {
        case MsgTypeOneofCase.FriendsList:
          FriendsList = other.FriendsList;
          break;
        case MsgTypeOneofCase.FriendDetail:
          FriendDetail = other.FriendDetail;
          break;
        case MsgTypeOneofCase.FriendRequests:
          FriendRequests = other.FriendRequests;
          break;
      }

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
            global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg subBuilder = new global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppListMsg();
            if (msgTypeCase_ == MsgTypeOneofCase.FriendsList) {
              subBuilder.MergeFrom(FriendsList);
            }
            input.ReadMessage(subBuilder);
            FriendsList = subBuilder;
            break;
          }
          case 18: {
            global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg subBuilder = new global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppUserMsg();
            if (msgTypeCase_ == MsgTypeOneofCase.FriendDetail) {
              subBuilder.MergeFrom(FriendDetail);
            }
            input.ReadMessage(subBuilder);
            FriendDetail = subBuilder;
            break;
          }
          case 26: {
            global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppRequestsMsg subBuilder = new global::VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels.FriendsAppRequestsMsg();
            if (msgTypeCase_ == MsgTypeOneofCase.FriendRequests) {
              subBuilder.MergeFrom(FriendRequests);
            }
            input.ReadMessage(subBuilder);
            FriendRequests = subBuilder;
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code