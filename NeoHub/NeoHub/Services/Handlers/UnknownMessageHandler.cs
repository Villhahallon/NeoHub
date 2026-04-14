using DSC.TLink.ITv2.MediatR;
using DSC.TLink.ITv2.Messages;
using DSC.TLink.ITv2.Enumerations;
using MediatR;

namespace NeoHub.Services.Handlers
{
    /// <summary>
    /// Handles unknown messages, specifically logging camera image transfer messages.
    /// </summary>
    public class UnknownMessageHandler : INotificationHandler<SessionNotification<DefaultMessage>>
    {
        private readonly ILogger<UnknownMessageHandler> _logger;

        public UnknownMessageHandler(ILogger<UnknownMessageHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SessionNotification<DefaultMessage> notification, CancellationToken cancellationToken)
        {
            var msg = notification.MessageData;
            var sessionId = notification.SessionId;

            // Check for camera image transfer commands
            if (msg.Command == (ITv2Command)3074) // ImageTransfer_Image_File_Header_Command
            {
                _logger.LogInformation(
                    "Received camera image file header for session {SessionId}. Data length: {Length}, Data: {Data}",
                    sessionId, msg.Data.Length, BitConverter.ToString(msg.Data));
            }
            else if (msg.Command == (ITv2Command)3075) // ImageTransfer_File_Transfer_Data_Blocks
            {
                _logger.LogInformation(
                    "Received camera image data block for session {SessionId}. Data length: {Length}, Data: {Data}",
                    sessionId, msg.Data.Length, BitConverter.ToString(msg.Data));
            }
            else
            {
                _logger.LogDebug(
                    "Received unknown message for session {SessionId}. Command: {Command} (0x{Command:X4}), Data length: {Length}",
                    sessionId, (ushort)msg.Command, (ushort)msg.Command, msg.Data.Length);
            }

            return Task.CompletedTask;
        }
    }
}