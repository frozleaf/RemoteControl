using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;

namespace RemoteControl.Client
{
    class RequestViewRegistryKeyHandler:IRequestHandler
    {
        public void Handle(SocketSession session, object reqObj)
        {
            RequestViewRegistryKey req = reqObj as RequestViewRegistryKey;

            ResponseViewRegistryKey resp = new ResponseViewRegistryKey();
            resp.KeyRoot = req.KeyRoot;
            resp.KeyPath = req.KeyPath;

            try
            {
                RegistryKey rootKey = RegistryKey.OpenBaseKey(
                                                    (RegistryHive)req.KeyRoot,
                                                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                RegistryKey subKey = rootKey;
                if (!string.IsNullOrWhiteSpace(req.KeyPath))
                {
                    subKey = rootKey.OpenSubKey(req.KeyPath);
                }
                resp.KeyNames = subKey.GetSubKeyNames();
                resp.ValueNames = subKey.GetValueNames();
                int valueNamesLength = resp.ValueNames.Length;
                resp.ValueKinds = new eRegistryValueKind[valueNamesLength];
                resp.Values = new object[valueNamesLength];
                for (int i = 0; i < valueNamesLength; i++)
                {
                    string valueName = resp.ValueNames[i];
                    resp.ValueKinds[i] = (eRegistryValueKind)subKey.GetValueKind(valueName);
                    resp.Values[i] = subKey.GetValue(valueName);
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_VIEW_REGISTRY_KEY_RESPONSE, resp);
        }
    }
}
