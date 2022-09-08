using System;

namespace DBOT.BotCallbacks
{
    /// <summary>
    /// Represents callback data in format: |callback key word|:|viewing date||message id key word|:|message id|.
    /// </summary>
    public class BotCallbackInfo
    {
        private readonly DateTime _dateTimeInfo;
        private readonly int _editingMessageId;

        public BotCallbackInfo(DateTime dateTimeInfo, int editingMessageId)
        {
            _dateTimeInfo = dateTimeInfo;
            _editingMessageId = editingMessageId;
        }

        public DateTime DateTimeInfo => _dateTimeInfo;
        public int EditingMessageId => _editingMessageId;
    }
}
