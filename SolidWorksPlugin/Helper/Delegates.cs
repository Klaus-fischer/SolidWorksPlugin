// <copyright file="Delegates.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    public delegate int DocumentEventHandler<TArgs>(SwDocument document, TArgs args);
}
