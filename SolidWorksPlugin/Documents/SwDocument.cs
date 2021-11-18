// <copyright file="IDocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    public class SwDocument
    {
        public SwDocument(IModelDoc2 model)
        {
            this.Model = model;
        }

        public IModelDoc2 Model { get; }

        public event DocumentEventHandler<swDestroyNotifyType_e>? OnDestroy;

        protected void InvokeOnDestroy(int destroyType) => OnDestroy?.Invoke(this, (swDestroyNotifyType_e)destroyType);
    }
}
