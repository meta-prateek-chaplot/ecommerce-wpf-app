// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: pubSub.proto
// </auto-generated>
#pragma warning disable 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace PubSub {
  public static partial class Notifier
  {
    static readonly string __ServiceName = "pubSub.Notifier";

    static readonly grpc::Marshaller<global::PubSub.DataRequest> __Marshaller_DataRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::PubSub.DataRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::PubSub.DataReply> __Marshaller_DataReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::PubSub.DataReply.Parser.ParseFrom);

    static readonly grpc::Method<global::PubSub.DataRequest, global::PubSub.DataReply> __Method_Data = new grpc::Method<global::PubSub.DataRequest, global::PubSub.DataReply>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Data",
        __Marshaller_DataRequest,
        __Marshaller_DataReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::PubSub.PubSubReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Notifier</summary>
    public abstract partial class NotifierBase
    {
      public virtual global::System.Threading.Tasks.Task Data(global::PubSub.DataRequest request, grpc::IServerStreamWriter<global::PubSub.DataReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Notifier</summary>
    public partial class NotifierClient : grpc::ClientBase<NotifierClient>
    {
      /// <summary>Creates a new client for Notifier</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public NotifierClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Notifier that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public NotifierClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected NotifierClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected NotifierClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::PubSub.DataReply> Data(global::PubSub.DataRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Data(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::PubSub.DataReply> Data(global::PubSub.DataRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Data, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override NotifierClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new NotifierClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(NotifierBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Data, serviceImpl.Data).Build();
    }

  }
}
#endregion
