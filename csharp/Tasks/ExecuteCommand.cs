using System;

namespace Tasks
{
    public class ExecuteCommand
    {
        public ExecuteCommand(string commandLine)
        {
            var strings = commandLine.Split(" ".ToCharArray(), 2);

            ActionType = ConvertToActionType(strings[0]);
            Command = strings[1];
        }

        private ActionTypeEnum ConvertToActionType(string s)
        {
            return Enum.Parse<ActionTypeEnum>(s, true);
        }

        public string Command { get; set; }

        public ActionTypeEnum ActionType { get; set; }
    }
}
