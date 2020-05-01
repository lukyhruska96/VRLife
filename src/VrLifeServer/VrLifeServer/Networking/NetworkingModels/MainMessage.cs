// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: MainMessage.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrLifeServer.Networking.NetworkingModels {

  /// <summary>Holder for reflection information generated from MainMessage.proto</summary>
  public static partial class MainMessageReflection {

    #region Descriptor
    /// <summary>File descriptor for MainMessage.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MainMessageReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFNYWluTWVzc2FnZS5wcm90bxIoVnJMaWZlU2VydmVyLk5ldHdvcmtpbmcu",
            "TmV0d29ya2luZ01vZGVscxoPU3lzdGVtTXNnLnByb3RvGg1UaWNrTXNnLnBy",
            "b3RvGg5FdmVudE1zZy5wcm90bxoNUm9vbU1zZy5wcm90bxoNVXNlck1zZy5w",
            "cm90bxoMQXBwTXNnLnByb3RvIo0ECgtNYWluTWVzc2FnZRINCgVtc2dJZBgB",
            "IAEoBBISCghzZXJ2ZXJJZBgCIAEoDUgAEhIKCGNsaWVudElkGAMgASgNSAAS",
            "SAoJc3lzdGVtTXNnGAQgASgLMjMuVnJMaWZlU2VydmVyLk5ldHdvcmtpbmcu",
            "TmV0d29ya2luZ01vZGVscy5TeXN0ZW1Nc2dIARJECgd0aWNrTXNnGAUgASgL",
            "MjEuVnJMaWZlU2VydmVyLk5ldHdvcmtpbmcuTmV0d29ya2luZ01vZGVscy5U",
            "aWNrTXNnSAESRgoIZXZlbnRNc2cYBiABKAsyMi5WckxpZmVTZXJ2ZXIuTmV0",
            "d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLkV2ZW50TXNnSAESRAoHcm9vbU1z",
            "ZxgHIAEoCzIxLlZyTGlmZVNlcnZlci5OZXR3b3JraW5nLk5ldHdvcmtpbmdN",
            "b2RlbHMuUm9vbU1zZ0gBEkoKCnVzZXJNbmdNc2cYCCABKAsyNC5WckxpZmVT",
            "ZXJ2ZXIuTmV0d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLlVzZXJNbmdNc2dI",
            "ARJCCgZhcHBNc2cYCSABKAsyMC5WckxpZmVTZXJ2ZXIuTmV0d29ya2luZy5O",
            "ZXR3b3JraW5nTW9kZWxzLkFwcE1zZ0gBQgoKCFNlbmRlcklkQg0KC01lc3Nh",
            "Z2VUeXBlYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::VrLifeServer.Networking.NetworkingModels.SystemMsgReflection.Descriptor, global::VrLifeServer.Networking.NetworkingModels.TickMsgReflection.Descriptor, global::VrLifeServer.Networking.NetworkingModels.EventMsgReflection.Descriptor, global::VrLifeServer.Networking.NetworkingModels.RoomMsgReflection.Descriptor, global::VrLifeServer.Networking.NetworkingModels.UserMsgReflection.Descriptor, global::VrLifeServer.Networking.NetworkingModels.AppMsgReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeServer.Networking.NetworkingModels.MainMessage), global::VrLifeServer.Networking.NetworkingModels.MainMessage.Parser, new[]{ "MsgId", "ServerId", "ClientId", "SystemMsg", "TickMsg", "EventMsg", "RoomMsg", "UserMngMsg", "AppMsg" }, new[]{ "SenderId", "MessageType" }, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class MainMessage : pb::IMessage<MainMessage> {
    private static readonly pb::MessageParser<MainMessage> _parser = new pb::MessageParser<MainMessage>(() => new MainMessage());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MainMessage> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeServer.Networking.NetworkingModels.MainMessageReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainMessage() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainMessage(MainMessage other) : this() {
      msgId_ = other.msgId_;
      switch (other.SenderIdCase) {
        case SenderIdOneofCase.ServerId:
          ServerId = other.ServerId;
          break;
        case SenderIdOneofCase.ClientId:
          ClientId = other.ClientId;
          break;
      }

      switch (other.MessageTypeCase) {
        case MessageTypeOneofCase.SystemMsg:
          SystemMsg = other.SystemMsg.Clone();
          break;
        case MessageTypeOneofCase.TickMsg:
          TickMsg = other.TickMsg.Clone();
          break;
        case MessageTypeOneofCase.EventMsg:
          EventMsg = other.EventMsg.Clone();
          break;
        case MessageTypeOneofCase.RoomMsg:
          RoomMsg = other.RoomMsg.Clone();
          break;
        case MessageTypeOneofCase.UserMngMsg:
          UserMngMsg = other.UserMngMsg.Clone();
          break;
        case MessageTypeOneofCase.AppMsg:
          AppMsg = other.AppMsg.Clone();
          break;
      }

    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MainMessage Clone() {
      return new MainMessage(this);
    }

    /// <summary>Field number for the "msgId" field.</summary>
    public const int MsgIdFieldNumber = 1;
    private ulong msgId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong MsgId {
      get { return msgId_; }
      set {
        msgId_ = value;
      }
    }

    /// <summary>Field number for the "serverId" field.</summary>
    public const int ServerIdFieldNumber = 2;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint ServerId {
      get { return senderIdCase_ == SenderIdOneofCase.ServerId ? (uint) senderId_ : 0; }
      set {
        senderId_ = value;
        senderIdCase_ = SenderIdOneofCase.ServerId;
      }
    }

    /// <summary>Field number for the "clientId" field.</summary>
    public const int ClientIdFieldNumber = 3;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint ClientId {
      get { return senderIdCase_ == SenderIdOneofCase.ClientId ? (uint) senderId_ : 0; }
      set {
        senderId_ = value;
        senderIdCase_ = SenderIdOneofCase.ClientId;
      }
    }

    /// <summary>Field number for the "systemMsg" field.</summary>
    public const int SystemMsgFieldNumber = 4;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.SystemMsg SystemMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.SystemMsg ? (global::VrLifeServer.Networking.NetworkingModels.SystemMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.SystemMsg;
      }
    }

    /// <summary>Field number for the "tickMsg" field.</summary>
    public const int TickMsgFieldNumber = 5;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.TickMsg TickMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.TickMsg ? (global::VrLifeServer.Networking.NetworkingModels.TickMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.TickMsg;
      }
    }

    /// <summary>Field number for the "eventMsg" field.</summary>
    public const int EventMsgFieldNumber = 6;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.EventMsg EventMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.EventMsg ? (global::VrLifeServer.Networking.NetworkingModels.EventMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.EventMsg;
      }
    }

    /// <summary>Field number for the "roomMsg" field.</summary>
    public const int RoomMsgFieldNumber = 7;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.RoomMsg RoomMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.RoomMsg ? (global::VrLifeServer.Networking.NetworkingModels.RoomMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.RoomMsg;
      }
    }

    /// <summary>Field number for the "userMngMsg" field.</summary>
    public const int UserMngMsgFieldNumber = 8;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.UserMngMsg UserMngMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.UserMngMsg ? (global::VrLifeServer.Networking.NetworkingModels.UserMngMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.UserMngMsg;
      }
    }

    /// <summary>Field number for the "appMsg" field.</summary>
    public const int AppMsgFieldNumber = 9;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeServer.Networking.NetworkingModels.AppMsg AppMsg {
      get { return messageTypeCase_ == MessageTypeOneofCase.AppMsg ? (global::VrLifeServer.Networking.NetworkingModels.AppMsg) messageType_ : null; }
      set {
        messageType_ = value;
        messageTypeCase_ = value == null ? MessageTypeOneofCase.None : MessageTypeOneofCase.AppMsg;
      }
    }

    private object senderId_;
    /// <summary>Enum of possible cases for the "SenderId" oneof.</summary>
    public enum SenderIdOneofCase {
      None = 0,
      ServerId = 2,
      ClientId = 3,
    }
    private SenderIdOneofCase senderIdCase_ = SenderIdOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SenderIdOneofCase SenderIdCase {
      get { return senderIdCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearSenderId() {
      senderIdCase_ = SenderIdOneofCase.None;
      senderId_ = null;
    }

    private object messageType_;
    /// <summary>Enum of possible cases for the "MessageType" oneof.</summary>
    public enum MessageTypeOneofCase {
      None = 0,
      SystemMsg = 4,
      TickMsg = 5,
      EventMsg = 6,
      RoomMsg = 7,
      UserMngMsg = 8,
      AppMsg = 9,
    }
    private MessageTypeOneofCase messageTypeCase_ = MessageTypeOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MessageTypeOneofCase MessageTypeCase {
      get { return messageTypeCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMessageType() {
      messageTypeCase_ = MessageTypeOneofCase.None;
      messageType_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MainMessage);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MainMessage other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MsgId != other.MsgId) return false;
      if (ServerId != other.ServerId) return false;
      if (ClientId != other.ClientId) return false;
      if (!object.Equals(SystemMsg, other.SystemMsg)) return false;
      if (!object.Equals(TickMsg, other.TickMsg)) return false;
      if (!object.Equals(EventMsg, other.EventMsg)) return false;
      if (!object.Equals(RoomMsg, other.RoomMsg)) return false;
      if (!object.Equals(UserMngMsg, other.UserMngMsg)) return false;
      if (!object.Equals(AppMsg, other.AppMsg)) return false;
      if (SenderIdCase != other.SenderIdCase) return false;
      if (MessageTypeCase != other.MessageTypeCase) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MsgId != 0UL) hash ^= MsgId.GetHashCode();
      if (senderIdCase_ == SenderIdOneofCase.ServerId) hash ^= ServerId.GetHashCode();
      if (senderIdCase_ == SenderIdOneofCase.ClientId) hash ^= ClientId.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.SystemMsg) hash ^= SystemMsg.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.TickMsg) hash ^= TickMsg.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.EventMsg) hash ^= EventMsg.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.RoomMsg) hash ^= RoomMsg.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.UserMngMsg) hash ^= UserMngMsg.GetHashCode();
      if (messageTypeCase_ == MessageTypeOneofCase.AppMsg) hash ^= AppMsg.GetHashCode();
      hash ^= (int) senderIdCase_;
      hash ^= (int) messageTypeCase_;
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (MsgId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(MsgId);
      }
      if (senderIdCase_ == SenderIdOneofCase.ServerId) {
        output.WriteRawTag(16);
        output.WriteUInt32(ServerId);
      }
      if (senderIdCase_ == SenderIdOneofCase.ClientId) {
        output.WriteRawTag(24);
        output.WriteUInt32(ClientId);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.SystemMsg) {
        output.WriteRawTag(34);
        output.WriteMessage(SystemMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.TickMsg) {
        output.WriteRawTag(42);
        output.WriteMessage(TickMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.EventMsg) {
        output.WriteRawTag(50);
        output.WriteMessage(EventMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.RoomMsg) {
        output.WriteRawTag(58);
        output.WriteMessage(RoomMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.UserMngMsg) {
        output.WriteRawTag(66);
        output.WriteMessage(UserMngMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.AppMsg) {
        output.WriteRawTag(74);
        output.WriteMessage(AppMsg);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (MsgId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(MsgId);
      }
      if (senderIdCase_ == SenderIdOneofCase.ServerId) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(ServerId);
      }
      if (senderIdCase_ == SenderIdOneofCase.ClientId) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(ClientId);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.SystemMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SystemMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.TickMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(TickMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.EventMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EventMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.RoomMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RoomMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.UserMngMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(UserMngMsg);
      }
      if (messageTypeCase_ == MessageTypeOneofCase.AppMsg) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(AppMsg);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MainMessage other) {
      if (other == null) {
        return;
      }
      if (other.MsgId != 0UL) {
        MsgId = other.MsgId;
      }
      switch (other.SenderIdCase) {
        case SenderIdOneofCase.ServerId:
          ServerId = other.ServerId;
          break;
        case SenderIdOneofCase.ClientId:
          ClientId = other.ClientId;
          break;
      }

      switch (other.MessageTypeCase) {
        case MessageTypeOneofCase.SystemMsg:
          SystemMsg = other.SystemMsg;
          break;
        case MessageTypeOneofCase.TickMsg:
          TickMsg = other.TickMsg;
          break;
        case MessageTypeOneofCase.EventMsg:
          EventMsg = other.EventMsg;
          break;
        case MessageTypeOneofCase.RoomMsg:
          RoomMsg = other.RoomMsg;
          break;
        case MessageTypeOneofCase.UserMngMsg:
          UserMngMsg = other.UserMngMsg;
          break;
        case MessageTypeOneofCase.AppMsg:
          AppMsg = other.AppMsg;
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
          case 8: {
            MsgId = input.ReadUInt64();
            break;
          }
          case 16: {
            ServerId = input.ReadUInt32();
            break;
          }
          case 24: {
            ClientId = input.ReadUInt32();
            break;
          }
          case 34: {
            global::VrLifeServer.Networking.NetworkingModels.SystemMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.SystemMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.SystemMsg) {
              subBuilder.MergeFrom(SystemMsg);
            }
            input.ReadMessage(subBuilder);
            SystemMsg = subBuilder;
            break;
          }
          case 42: {
            global::VrLifeServer.Networking.NetworkingModels.TickMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.TickMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.TickMsg) {
              subBuilder.MergeFrom(TickMsg);
            }
            input.ReadMessage(subBuilder);
            TickMsg = subBuilder;
            break;
          }
          case 50: {
            global::VrLifeServer.Networking.NetworkingModels.EventMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.EventMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.EventMsg) {
              subBuilder.MergeFrom(EventMsg);
            }
            input.ReadMessage(subBuilder);
            EventMsg = subBuilder;
            break;
          }
          case 58: {
            global::VrLifeServer.Networking.NetworkingModels.RoomMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.RoomMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.RoomMsg) {
              subBuilder.MergeFrom(RoomMsg);
            }
            input.ReadMessage(subBuilder);
            RoomMsg = subBuilder;
            break;
          }
          case 66: {
            global::VrLifeServer.Networking.NetworkingModels.UserMngMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.UserMngMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.UserMngMsg) {
              subBuilder.MergeFrom(UserMngMsg);
            }
            input.ReadMessage(subBuilder);
            UserMngMsg = subBuilder;
            break;
          }
          case 74: {
            global::VrLifeServer.Networking.NetworkingModels.AppMsg subBuilder = new global::VrLifeServer.Networking.NetworkingModels.AppMsg();
            if (messageTypeCase_ == MessageTypeOneofCase.AppMsg) {
              subBuilder.MergeFrom(AppMsg);
            }
            input.ReadMessage(subBuilder);
            AppMsg = subBuilder;
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
