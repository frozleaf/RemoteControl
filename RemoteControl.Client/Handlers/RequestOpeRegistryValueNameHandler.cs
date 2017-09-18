using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;

namespace RemoteControl.Client
{
    class RequestOpeRegistryValueNameHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            RequestOpeRegistryValueName req = reqObj as RequestOpeRegistryValueName;

            ResponseOpeRegistryValueName resp = new ResponseOpeRegistryValueName();
            try
            {
                RegistryKey rootKey = RegistryKey.OpenBaseKey(
                                                    (RegistryHive)req.KeyRoot,
                                                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

                RegistryKey subKey = rootKey.OpenSubKey(req.KeyPath, true);
                bool valueNameExists = subKey.GetValueNames().ToList().Contains(req.ValueName);
                if (req.Operation == OpeType.Delete)
                {
                    if (valueNameExists)
                    {
                        subKey.DeleteValue(req.ValueName);
                    }
                }
                else if (req.Operation == OpeType.New || req.Operation == OpeType.Edit)
                {
                    subKey.SetValue(req.ValueName, req.Value, (RegistryValueKind)req.ValueKind);
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_RESPONSE, resp);
        }
    }
}
