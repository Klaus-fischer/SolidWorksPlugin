// <copyright file="Demo2Class.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.DemoAddin
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using SIM.SolidWorksPlugin;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swpublished;
    using SolidWorks.Interop.swconst;

    [Guid("217A321E-E54D-4658-ABEB-EDE9E15A86CF")]
    [ComVisible(true)]
    [Description("Demo 2 description")]
    [DisplayName("Demo 2")]
    public class Demo2Class : ISwAddin
    {
        [ComRegisterFunction]
        public static void RegisterFcuntion(Type t) => SwComInterop.RegisterFunction(t);

        [ComUnregisterFunction]
        public static void Unregisterfunction(Type t) => SwComInterop.UnregisterFunction(t);

        public bool ConnectToSW(object swApp, int cookie)
        {
            SldWorks swApplication = (SldWorks)swApp;

            try
            {
                swApplication.SendMsgToUser2("Hallo Welt", 0, 0);

                var swCommandManager = swApplication.GetCommandManager(cookie);

                // Setup callbacks
                swApplication.SetAddinCallbackInfo(0, this, cookie);
            }
            catch (Exception ex)
            {
                swApplication.SendMsgToUser2(
                    $"Loading add-in failed.\n{ex.Message}\n{ex.StackTrace}",
                    (int)swMessageBoxIcon_e.swMbStop,
                    (int)swMessageBoxBtn_e.swMbOk);
            }

            return true;
        }

        public bool DisconnectFromSW()
        {
            return true;
        }
    }
}
