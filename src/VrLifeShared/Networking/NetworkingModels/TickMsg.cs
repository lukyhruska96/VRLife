// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: TickMsg.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VrLifeShared.Networking.NetworkingModels {

  /// <summary>Holder for reflection information generated from TickMsg.proto</summary>
  public static partial class TickMsgReflection {

    #region Descriptor
    /// <summary>File descriptor for TickMsg.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TickMsgReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1UaWNrTXNnLnByb3RvEihWckxpZmVTaGFyZWQuTmV0d29ya2luZy5OZXR3",
            "b3JraW5nTW9kZWxzIkwKB1RpY2tNc2cSQQoHcGVyc29ucxgBIAMoCzIwLlZy",
            "TGlmZVNoYXJlZC5OZXR3b3JraW5nLk5ldHdvcmtpbmdNb2RlbHMuUGVyc29u",
            "IvQFCgZQZXJzb24SDgoGdXNlcklkGAEgASgEEj0KBGhlYWQYAiABKAsyLy5W",
            "ckxpZmVTaGFyZWQuTmV0d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLkNvb3Jk",
            "Ej0KBG5lY2sYAyABKAsyLy5WckxpZmVTaGFyZWQuTmV0d29ya2luZy5OZXR3",
            "b3JraW5nTW9kZWxzLkNvb3JkEjwKA2hpcBgEIAEoCzIvLlZyTGlmZVNoYXJl",
            "ZC5OZXR3b3JraW5nLk5ldHdvcmtpbmdNb2RlbHMuQ29vcmQSQgoJbGVmdEhh",
            "bmQxGAUgASgLMi8uVnJMaWZlU2hhcmVkLk5ldHdvcmtpbmcuTmV0d29ya2lu",
            "Z01vZGVscy5Db29yZBJCCglsZWZ0SGFuZDIYBiABKAsyLy5WckxpZmVTaGFy",
            "ZWQuTmV0d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLkNvb3JkEkMKCnJpZ2h0",
            "SGFuZDEYByABKAsyLy5WckxpZmVTaGFyZWQuTmV0d29ya2luZy5OZXR3b3Jr",
            "aW5nTW9kZWxzLkNvb3JkEkMKCnJpZ2h0SGFuZDIYCCABKAsyLy5WckxpZmVT",
            "aGFyZWQuTmV0d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLkNvb3JkEkEKCGxl",
            "ZnRMZWcxGAkgASgLMi8uVnJMaWZlU2hhcmVkLk5ldHdvcmtpbmcuTmV0d29y",
            "a2luZ01vZGVscy5Db29yZBJBCghsZWZ0TGVnMhgKIAEoCzIvLlZyTGlmZVNo",
            "YXJlZC5OZXR3b3JraW5nLk5ldHdvcmtpbmdNb2RlbHMuQ29vcmQSQgoJcmln",
            "aHRMZWcxGAsgASgLMi8uVnJMaWZlU2hhcmVkLk5ldHdvcmtpbmcuTmV0d29y",
            "a2luZ01vZGVscy5Db29yZBJCCglyaWdodExlZzIYDCABKAsyLy5WckxpZmVT",
            "aGFyZWQuTmV0d29ya2luZy5OZXR3b3JraW5nTW9kZWxzLkNvb3JkIigKBUNv",
            "b3JkEgkKAXgYASABKAISCQoBeRgCIAEoAhIJCgF6GAMgASgCYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeShared.Networking.NetworkingModels.TickMsg), global::VrLifeShared.Networking.NetworkingModels.TickMsg.Parser, new[]{ "Persons" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeShared.Networking.NetworkingModels.Person), global::VrLifeShared.Networking.NetworkingModels.Person.Parser, new[]{ "UserId", "Head", "Neck", "Hip", "LeftHand1", "LeftHand2", "RightHand1", "RightHand2", "LeftLeg1", "LeftLeg2", "RightLeg1", "RightLeg2" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VrLifeShared.Networking.NetworkingModels.Coord), global::VrLifeShared.Networking.NetworkingModels.Coord.Parser, new[]{ "X", "Y", "Z" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///  Tick information for client
  /// </summary>
  public sealed partial class TickMsg : pb::IMessage<TickMsg> {
    private static readonly pb::MessageParser<TickMsg> _parser = new pb::MessageParser<TickMsg>(() => new TickMsg());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TickMsg> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeShared.Networking.NetworkingModels.TickMsgReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TickMsg() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TickMsg(TickMsg other) : this() {
      persons_ = other.persons_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TickMsg Clone() {
      return new TickMsg(this);
    }

    /// <summary>Field number for the "persons" field.</summary>
    public const int PersonsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VrLifeShared.Networking.NetworkingModels.Person> _repeated_persons_codec
        = pb::FieldCodec.ForMessage(10, global::VrLifeShared.Networking.NetworkingModels.Person.Parser);
    private readonly pbc::RepeatedField<global::VrLifeShared.Networking.NetworkingModels.Person> persons_ = new pbc::RepeatedField<global::VrLifeShared.Networking.NetworkingModels.Person>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VrLifeShared.Networking.NetworkingModels.Person> Persons {
      get { return persons_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TickMsg);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TickMsg other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!persons_.Equals(other.persons_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= persons_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      persons_.WriteTo(output, _repeated_persons_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += persons_.CalculateSize(_repeated_persons_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TickMsg other) {
      if (other == null) {
        return;
      }
      persons_.Add(other.persons_);
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
            persons_.AddEntriesFrom(input, _repeated_persons_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Person : pb::IMessage<Person> {
    private static readonly pb::MessageParser<Person> _parser = new pb::MessageParser<Person>(() => new Person());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Person> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeShared.Networking.NetworkingModels.TickMsgReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Person() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Person(Person other) : this() {
      userId_ = other.userId_;
      Head = other.head_ != null ? other.Head.Clone() : null;
      Neck = other.neck_ != null ? other.Neck.Clone() : null;
      Hip = other.hip_ != null ? other.Hip.Clone() : null;
      LeftHand1 = other.leftHand1_ != null ? other.LeftHand1.Clone() : null;
      LeftHand2 = other.leftHand2_ != null ? other.LeftHand2.Clone() : null;
      RightHand1 = other.rightHand1_ != null ? other.RightHand1.Clone() : null;
      RightHand2 = other.rightHand2_ != null ? other.RightHand2.Clone() : null;
      LeftLeg1 = other.leftLeg1_ != null ? other.LeftLeg1.Clone() : null;
      LeftLeg2 = other.leftLeg2_ != null ? other.LeftLeg2.Clone() : null;
      RightLeg1 = other.rightLeg1_ != null ? other.RightLeg1.Clone() : null;
      RightLeg2 = other.rightLeg2_ != null ? other.RightLeg2.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Person Clone() {
      return new Person(this);
    }

    /// <summary>Field number for the "userId" field.</summary>
    public const int UserIdFieldNumber = 1;
    private ulong userId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "head" field.</summary>
    public const int HeadFieldNumber = 2;
    private global::VrLifeShared.Networking.NetworkingModels.Coord head_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord Head {
      get { return head_; }
      set {
        head_ = value;
      }
    }

    /// <summary>Field number for the "neck" field.</summary>
    public const int NeckFieldNumber = 3;
    private global::VrLifeShared.Networking.NetworkingModels.Coord neck_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord Neck {
      get { return neck_; }
      set {
        neck_ = value;
      }
    }

    /// <summary>Field number for the "hip" field.</summary>
    public const int HipFieldNumber = 4;
    private global::VrLifeShared.Networking.NetworkingModels.Coord hip_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord Hip {
      get { return hip_; }
      set {
        hip_ = value;
      }
    }

    /// <summary>Field number for the "leftHand1" field.</summary>
    public const int LeftHand1FieldNumber = 5;
    private global::VrLifeShared.Networking.NetworkingModels.Coord leftHand1_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord LeftHand1 {
      get { return leftHand1_; }
      set {
        leftHand1_ = value;
      }
    }

    /// <summary>Field number for the "leftHand2" field.</summary>
    public const int LeftHand2FieldNumber = 6;
    private global::VrLifeShared.Networking.NetworkingModels.Coord leftHand2_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord LeftHand2 {
      get { return leftHand2_; }
      set {
        leftHand2_ = value;
      }
    }

    /// <summary>Field number for the "rightHand1" field.</summary>
    public const int RightHand1FieldNumber = 7;
    private global::VrLifeShared.Networking.NetworkingModels.Coord rightHand1_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord RightHand1 {
      get { return rightHand1_; }
      set {
        rightHand1_ = value;
      }
    }

    /// <summary>Field number for the "rightHand2" field.</summary>
    public const int RightHand2FieldNumber = 8;
    private global::VrLifeShared.Networking.NetworkingModels.Coord rightHand2_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord RightHand2 {
      get { return rightHand2_; }
      set {
        rightHand2_ = value;
      }
    }

    /// <summary>Field number for the "leftLeg1" field.</summary>
    public const int LeftLeg1FieldNumber = 9;
    private global::VrLifeShared.Networking.NetworkingModels.Coord leftLeg1_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord LeftLeg1 {
      get { return leftLeg1_; }
      set {
        leftLeg1_ = value;
      }
    }

    /// <summary>Field number for the "leftLeg2" field.</summary>
    public const int LeftLeg2FieldNumber = 10;
    private global::VrLifeShared.Networking.NetworkingModels.Coord leftLeg2_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord LeftLeg2 {
      get { return leftLeg2_; }
      set {
        leftLeg2_ = value;
      }
    }

    /// <summary>Field number for the "rightLeg1" field.</summary>
    public const int RightLeg1FieldNumber = 11;
    private global::VrLifeShared.Networking.NetworkingModels.Coord rightLeg1_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord RightLeg1 {
      get { return rightLeg1_; }
      set {
        rightLeg1_ = value;
      }
    }

    /// <summary>Field number for the "rightLeg2" field.</summary>
    public const int RightLeg2FieldNumber = 12;
    private global::VrLifeShared.Networking.NetworkingModels.Coord rightLeg2_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::VrLifeShared.Networking.NetworkingModels.Coord RightLeg2 {
      get { return rightLeg2_; }
      set {
        rightLeg2_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Person);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Person other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (!object.Equals(Head, other.Head)) return false;
      if (!object.Equals(Neck, other.Neck)) return false;
      if (!object.Equals(Hip, other.Hip)) return false;
      if (!object.Equals(LeftHand1, other.LeftHand1)) return false;
      if (!object.Equals(LeftHand2, other.LeftHand2)) return false;
      if (!object.Equals(RightHand1, other.RightHand1)) return false;
      if (!object.Equals(RightHand2, other.RightHand2)) return false;
      if (!object.Equals(LeftLeg1, other.LeftLeg1)) return false;
      if (!object.Equals(LeftLeg2, other.LeftLeg2)) return false;
      if (!object.Equals(RightLeg1, other.RightLeg1)) return false;
      if (!object.Equals(RightLeg2, other.RightLeg2)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0UL) hash ^= UserId.GetHashCode();
      if (head_ != null) hash ^= Head.GetHashCode();
      if (neck_ != null) hash ^= Neck.GetHashCode();
      if (hip_ != null) hash ^= Hip.GetHashCode();
      if (leftHand1_ != null) hash ^= LeftHand1.GetHashCode();
      if (leftHand2_ != null) hash ^= LeftHand2.GetHashCode();
      if (rightHand1_ != null) hash ^= RightHand1.GetHashCode();
      if (rightHand2_ != null) hash ^= RightHand2.GetHashCode();
      if (leftLeg1_ != null) hash ^= LeftLeg1.GetHashCode();
      if (leftLeg2_ != null) hash ^= LeftLeg2.GetHashCode();
      if (rightLeg1_ != null) hash ^= RightLeg1.GetHashCode();
      if (rightLeg2_ != null) hash ^= RightLeg2.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserId != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(UserId);
      }
      if (head_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Head);
      }
      if (neck_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Neck);
      }
      if (hip_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(Hip);
      }
      if (leftHand1_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(LeftHand1);
      }
      if (leftHand2_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(LeftHand2);
      }
      if (rightHand1_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(RightHand1);
      }
      if (rightHand2_ != null) {
        output.WriteRawTag(66);
        output.WriteMessage(RightHand2);
      }
      if (leftLeg1_ != null) {
        output.WriteRawTag(74);
        output.WriteMessage(LeftLeg1);
      }
      if (leftLeg2_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(LeftLeg2);
      }
      if (rightLeg1_ != null) {
        output.WriteRawTag(90);
        output.WriteMessage(RightLeg1);
      }
      if (rightLeg2_ != null) {
        output.WriteRawTag(98);
        output.WriteMessage(RightLeg2);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(UserId);
      }
      if (head_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Head);
      }
      if (neck_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Neck);
      }
      if (hip_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Hip);
      }
      if (leftHand1_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LeftHand1);
      }
      if (leftHand2_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LeftHand2);
      }
      if (rightHand1_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RightHand1);
      }
      if (rightHand2_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RightHand2);
      }
      if (leftLeg1_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LeftLeg1);
      }
      if (leftLeg2_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LeftLeg2);
      }
      if (rightLeg1_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RightLeg1);
      }
      if (rightLeg2_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RightLeg2);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Person other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0UL) {
        UserId = other.UserId;
      }
      if (other.head_ != null) {
        if (head_ == null) {
          head_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        Head.MergeFrom(other.Head);
      }
      if (other.neck_ != null) {
        if (neck_ == null) {
          neck_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        Neck.MergeFrom(other.Neck);
      }
      if (other.hip_ != null) {
        if (hip_ == null) {
          hip_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        Hip.MergeFrom(other.Hip);
      }
      if (other.leftHand1_ != null) {
        if (leftHand1_ == null) {
          leftHand1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        LeftHand1.MergeFrom(other.LeftHand1);
      }
      if (other.leftHand2_ != null) {
        if (leftHand2_ == null) {
          leftHand2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        LeftHand2.MergeFrom(other.LeftHand2);
      }
      if (other.rightHand1_ != null) {
        if (rightHand1_ == null) {
          rightHand1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        RightHand1.MergeFrom(other.RightHand1);
      }
      if (other.rightHand2_ != null) {
        if (rightHand2_ == null) {
          rightHand2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        RightHand2.MergeFrom(other.RightHand2);
      }
      if (other.leftLeg1_ != null) {
        if (leftLeg1_ == null) {
          leftLeg1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        LeftLeg1.MergeFrom(other.LeftLeg1);
      }
      if (other.leftLeg2_ != null) {
        if (leftLeg2_ == null) {
          leftLeg2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        LeftLeg2.MergeFrom(other.LeftLeg2);
      }
      if (other.rightLeg1_ != null) {
        if (rightLeg1_ == null) {
          rightLeg1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        RightLeg1.MergeFrom(other.RightLeg1);
      }
      if (other.rightLeg2_ != null) {
        if (rightLeg2_ == null) {
          rightLeg2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
        }
        RightLeg2.MergeFrom(other.RightLeg2);
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
            UserId = input.ReadUInt64();
            break;
          }
          case 18: {
            if (head_ == null) {
              head_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(head_);
            break;
          }
          case 26: {
            if (neck_ == null) {
              neck_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(neck_);
            break;
          }
          case 34: {
            if (hip_ == null) {
              hip_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(hip_);
            break;
          }
          case 42: {
            if (leftHand1_ == null) {
              leftHand1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(leftHand1_);
            break;
          }
          case 50: {
            if (leftHand2_ == null) {
              leftHand2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(leftHand2_);
            break;
          }
          case 58: {
            if (rightHand1_ == null) {
              rightHand1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(rightHand1_);
            break;
          }
          case 66: {
            if (rightHand2_ == null) {
              rightHand2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(rightHand2_);
            break;
          }
          case 74: {
            if (leftLeg1_ == null) {
              leftLeg1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(leftLeg1_);
            break;
          }
          case 82: {
            if (leftLeg2_ == null) {
              leftLeg2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(leftLeg2_);
            break;
          }
          case 90: {
            if (rightLeg1_ == null) {
              rightLeg1_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(rightLeg1_);
            break;
          }
          case 98: {
            if (rightLeg2_ == null) {
              rightLeg2_ = new global::VrLifeShared.Networking.NetworkingModels.Coord();
            }
            input.ReadMessage(rightLeg2_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Coord : pb::IMessage<Coord> {
    private static readonly pb::MessageParser<Coord> _parser = new pb::MessageParser<Coord>(() => new Coord());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Coord> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VrLifeShared.Networking.NetworkingModels.TickMsgReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Coord() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Coord(Coord other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Coord Clone() {
      return new Coord(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private float x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private float y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 3;
    private float z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Coord);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Coord other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      if (Z != other.Z) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0F) hash ^= X.GetHashCode();
      if (Y != 0F) hash ^= Y.GetHashCode();
      if (Z != 0F) hash ^= Z.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Z);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      if (Z != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Coord other) {
      if (other == null) {
        return;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
      if (other.Z != 0F) {
        Z = other.Z;
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
          case 13: {
            X = input.ReadFloat();
            break;
          }
          case 21: {
            Y = input.ReadFloat();
            break;
          }
          case 29: {
            Z = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
